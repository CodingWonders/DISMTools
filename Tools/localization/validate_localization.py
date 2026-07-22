from pathlib import Path
import csv
import re
import sys

ROOT = Path(__file__).resolve().parents[2]
LANGUAGE_DIR = ROOT / "language"
EXCLUDED_PARTS = {".git", "bin", "obj"}
PLACEHOLDER_PATTERN = re.compile(r"\{(\d+)(?:[^}]*)\}")

MAX_LOCALIZATION_SECTION_LENGTH = 34
MAX_LOCALIZATION_ITEM_LENGTH = 28
MAX_LOCALIZATION_COMPONENT_LENGTH = 18
MAX_LOCALIZATION_FULL_KEY_LENGTH = 60
BAD_LONG_COMPONENTS = ("Dismconfiguration", "PackageWantAddAlready", "DetectMultiSelectionCommonProperties", "PrimaryDomainSuffixMust", "toolsPEHelperMainMenu", "PEHelperMainMenuPEHelper", "BranchParameterNecessaryAble", "CurrentVersionDISMTools", "ClickHereGetInfo", "SpecifyCustomNameDestination", "ThereConfigurableOptionsTask", "Armarchitecture", "CommitImageAddingDrivers", "PleaseSpecifyDriversAdd", "ThereDriverPackagesGiven", "ChooseWhichPackagesAdd", "NoteAccommodateLargeFile", "PickImageFile", "ThereNewVersionAvailable", "CurrentVersionDISMTools", "PrimaryDomainSuffixMust", "BranchParameterNecessaryAble")


BAD_SECTION_PATTERN = re.compile(
    r"(^|\.)(?:Button|Label|LinkLabel|RadioButton|CheckBox|ComboBox|TextBox|ListBox|TreeView|ListView|ToolStrip|ColumnHeader|TrackBar|Scintilla|ProgressBW|BackgroundWorker|MsgBox|MessageBoxShow|FolderBrowserDialog|OpenFileDialog|SaveFileDialog)[A-Za-z0-9]*(\.|$)|(^|\.)(?:Click|LinkClicked|MouseHover|MouseLeave|MouseEnter|DoWork|RunWorkerCompleted|FormClosing|ChangeLangs|LoadDTProj|UnloadDTProj|PanelsISOFiles|PanelsImgOps|ImgOps|GetOps|ToolsStarterScriptEditor|ToolsDynaViewer|ToolsDTThemeDesigner|UpdaterDISMToolsUCS|Helpersextps1|toolsPEHelperMainMenu)(\.|$)|(^|\.)Line\d+(\.|$)",
    re.IGNORECASE,
)

BAD_KEY_PATTERN = re.compile(
    r"(^|\.)(?:Button|Label|LinkLabel|RadioButton|CheckBox|ComboBox|TextBox|ListBox|TreeView|ListView|ColumnHeader|TrackBar|AccentedLabel)\d+(\.|$)|(^|\.)\d|(^|\.)(?:Items\d+|Text|msg|titleMsg|String\d*|MsgBox|MessageBoxShow|Variant\d*|Alternate(?:Second|Third)?|Line\d+|Placeholder(?:Second|Third|Fourth)?|SecondPlaceholder|ThirdPlaceholder|FourthPlaceholder|FifthPlaceholder|Message\d+)(\.|$)|(^|\.)[A-Z](\.|$)",
    re.IGNORECASE,
)

BAD_PHRASE_COMPONENT_PATTERN = re.compile(r"^(?:There[A-Z]|PleaseSpecify|CommitImageAdding|ClickHere|NoteAccommodate|PickImageFile)", re.IGNORECASE)


def parse_language_file(path: Path):
    sections = {}
    current_section = None
    duplicates = []
    values = {}
    style_findings = []

    for line_number, raw_line in enumerate(path.read_text(encoding="utf-8-sig").splitlines(), start=1):
        line = raw_line.strip()
        if not line or line.startswith(";"):
            continue

        section_match = re.match(r"^\[([^\]]+)\]$", line)
        if section_match:
            current_section = section_match.group(1).strip()
            sections.setdefault(current_section, {})
            continue

        if current_section is None or "=" not in raw_line:
            continue

        name, value = raw_line.split("=", 1)
        key = name.strip()
        cleaned_value = value.strip()
        if len(cleaned_value) >= 2 and cleaned_value[0] == '"' and cleaned_value[-1] == '"':
            cleaned_value = cleaned_value[1:-1]

        full_key = f"{current_section}.{key}"
        if current_section != "LanguageFileInformation":
            if BAD_SECTION_PATTERN.search(current_section):
                style_findings.append(("localization_section_style", full_key, line_number, "Section contains a control name, event name, generated line marker, glued legacy path, or another old technical path."))
            if BAD_KEY_PATTERN.search(key):
                style_findings.append(("localization_item_style", full_key, line_number, "Item key contains an old control name, generated number, Placeholder, Line marker, Text/msg/String alias, numbered message alias, or message box path."))
            if len(current_section) > MAX_LOCALIZATION_SECTION_LENGTH:
                style_findings.append(("localization_section_style", full_key, line_number, "Section is too long. Split it into a shorter semantic section."))
            if len(key) > MAX_LOCALIZATION_ITEM_LENGTH:
                style_findings.append(("localization_item_style", full_key, line_number, "Item key is too long. Keep the key short inside its section."))
            if len(full_key) > MAX_LOCALIZATION_FULL_KEY_LENGTH:
                style_findings.append(("localization_full_key_style", full_key, line_number, "Full localization path is too long."))
            for component in current_section.split('.') + key.split('.'):
                if len(component) > MAX_LOCALIZATION_COMPONENT_LENGTH:
                    style_findings.append(("localization_component_style", full_key, line_number, "A section or key component is too long."))
                if any(bad_part.lower() in component.lower() for bad_part in BAD_LONG_COMPONENTS):
                    style_findings.append(("localization_component_style", full_key, line_number, "A component still contains a long legacy code name."))
                if BAD_PHRASE_COMPONENT_PATTERN.search(component):
                    style_findings.append(("localization_component_style", full_key, line_number, "A component still looks like a sentence copied from old UI text."))
        if key in sections[current_section]:
            duplicates.append((full_key, line_number))
        sections[current_section][key] = cleaned_value
        values[full_key] = cleaned_value

    keys = set()
    for section_name, section_values in sections.items():
        if section_name == "LanguageFileInformation":
            continue
        for item_name in section_values:
            keys.add(f"{section_name}.{item_name}")

    return keys, values, duplicates, style_findings


def source_files():
    for path in ROOT.rglob("*.vb"):
        if any(part in EXCLUDED_PARTS for part in path.parts):
            continue
        yield path


def split_arguments(argument_text: str):
    result = []
    buffer = []
    in_string = False
    paren_depth = 0
    index = 0

    while index < len(argument_text):
        char = argument_text[index]
        if char == '"':
            buffer.append(char)
            if in_string and index + 1 < len(argument_text) and argument_text[index + 1] == '"':
                buffer.append('"')
                index += 2
                continue
            in_string = not in_string
            index += 1
            continue

        if not in_string:
            if char == "(":
                paren_depth += 1
            elif char == ")" and paren_depth > 0:
                paren_depth -= 1
            elif char == "," and paren_depth == 0:
                result.append("".join(buffer).strip())
                buffer = []
                index += 1
                continue

        buffer.append(char)
        index += 1

    if buffer or argument_text.strip():
        result.append("".join(buffer).strip())

    return result


def parse_parenthesized(text: str, open_paren_index: int):
    index = open_paren_index + 1
    in_string = False
    paren_depth = 1

    while index < len(text):
        char = text[index]
        if char == '"':
            if in_string and index + 1 < len(text) and text[index + 1] == '"':
                index += 2
                continue
            in_string = not in_string
        elif not in_string:
            if char == "(":
                paren_depth += 1
            elif char == ")":
                paren_depth -= 1
                if paren_depth == 0:
                    return text[open_paren_index + 1:index], index + 1
        index += 1

    return None, None


def string_literal_value(argument: str):
    match = re.match(r'^"([^"]*)"$', argument.strip())
    if not match:
        return None
    return match.group(1)


def collect_forsection_calls(text: str, relative_path: str):
    calls = []
    marker = 'LocalizationService.ForSection('
    start = 0

    while True:
        call_start = text.find(marker, start)
        if call_start < 0:
            break

        section_arguments, after_section = parse_parenthesized(text, call_start + len('LocalizationService.ForSection'))
        if section_arguments is None:
            start = call_start + len(marker)
            continue

        section_args = split_arguments(section_arguments)
        section = string_literal_value(section_args[0]) if section_args else None
        if not section:
            start = after_section
            continue

        index = after_section
        while index < len(text) and text[index].isspace():
            index += 1

        call_name = None
        argument_text = None
        call_end = index

        if index < len(text) and text[index] == '(':
            call_name = 'Item'
            argument_text, call_end = parse_parenthesized(text, index)
        elif text.startswith('.Format(', index):
            call_name = 'Format'
            argument_text, call_end = parse_parenthesized(text, index + len('.Format'))
        elif text.startswith('.Upper(', index):
            call_name = 'Upper'
            argument_text, call_end = parse_parenthesized(text, index + len('.Upper'))

        if call_name and argument_text is not None:
            args = split_arguments(argument_text)
            key_index = 0
            first_value_index = {'Item': 1, 'Format': 1, 'Upper': 2}[call_name]

            if len(args) > key_index:
                key = string_literal_value(args[key_index])
                if key:
                    supplied_arguments = max(0, len(args) - first_value_index)
                    calls.append({
                        'key': f'{section}.{key}',
                        'source_file': relative_path,
                        'source_line': text.count('\n', 0, call_start) + 1,
                        'call_name': call_name,
                        'supplied_arguments': supplied_arguments,
                    })
        start = call_start + len(marker)

    return calls


def find_direct_localization_calls(text: str):
    call_names = ["TUpper", "T"]
    for call_name in call_names:
        marker = "LocalizationService." + call_name + "("
        start = 0
        while True:
            call_start = text.find(marker, start)
            if call_start < 0:
                break

            argument_text, call_end = parse_parenthesized(text, call_start + len("LocalizationService." + call_name))
            if argument_text is None:
                start = call_start + len(marker)
                continue

            yield call_name, call_start, argument_text
            start = call_end



def find_forbidden_localization_apis():
    forbidden_markers = [
        "LocalizationService.TryGetText",
        ".TryGetText(",
        "LocalizationService.ApplyToControl",
        "LocalizationService.ApplyToToolStrip",
        "Public Function TryGetText",
        "Public Sub ApplyToControl",
        "Public Sub ApplyToToolStrip",
        "LocalizationService.TForLegacyLanguage",
        ".ForLegacyLanguage(",
        "GetLegacyLanguageSetting",
        "ResolveLegacyLanguage",
    ]
    rows = []
    for path in source_files():
        text = path.read_text(encoding="utf-8-sig", errors="ignore")
        relative_path = path.relative_to(ROOT).as_posix()
        for line_number, line in enumerate(text.splitlines(), start=1):
            if re.search(r"\bMainForm\.Language\b", line):
                rows.append({
                    "type": "forbidden_localization_api",
                    "culture": "",
                    "key": "",
                    "source_file": relative_path,
                    "source_line": line_number,
                    "details": "MainForm.Language",
                })
            for marker in forbidden_markers:
                if marker in line:
                    rows.append({
                        "type": "forbidden_localization_api",
                        "culture": "",
                        "key": "",
                        "source_file": relative_path,
                        "source_line": line_number,
                        "details": marker,
                    })
    return rows

def collect_source_calls():
    result = []

    for path in source_files():
        text = path.read_text(encoding="utf-8-sig", errors="ignore")
        relative_path = path.relative_to(ROOT).as_posix()
        result.extend(collect_forsection_calls(text, relative_path))

        for call_name, call_start, argument_text in find_direct_localization_calls(text):
            args = split_arguments(argument_text)
            key_index = 0
            first_value_index = 1 if call_name == "T" else 2

            if len(args) <= key_index:
                continue

            key = string_literal_value(args[key_index])
            if not key:
                continue

            supplied_arguments = max(0, len(args) - first_value_index)
            result.append({
                'key': key,
                'source_file': relative_path,
                'source_line': text.count('\n', 0, call_start) + 1,
                'call_name': call_name,
                'supplied_arguments': supplied_arguments,
            })

    return result



def validate_pe_helper_export_map(source_calls):
    script_path = ROOT / "Helpers" / "extps1" / "PE_Helper" / "PE_Helper.ps1"
    if not script_path.exists():
        return [{
            "type": "pe_helper_export_script_missing",
            "culture": "",
            "key": "",
            "source_file": script_path.relative_to(ROOT).as_posix(),
            "source_line": "",
            "details": "",
        }]

    script_text = script_path.read_text(encoding="utf-8-sig", errors="ignore")
    marker = "$requiredKeys = [ordered]@{"
    start = script_text.find(marker)
    end_marker = "\n    }\n\n    $sourceLines"
    end = script_text.find(end_marker, start + len(marker)) if start >= 0 else -1
    if start < 0 or end < 0:
        return [{
            "type": "pe_helper_export_map_missing",
            "culture": "",
            "key": "",
            "source_file": script_path.relative_to(ROOT).as_posix(),
            "source_line": "",
            "details": marker,
        }]

    export_block = script_text[start:end]
    exported_keys = set()
    for section, key_list in re.findall(r"(?m)^\s*'([^']+)'\s*=\s*@\(([^\n]*)\)", export_block):
        for key in re.findall(r"'([^']+)'", key_list):
            exported_keys.add(f"{section}.{key}")

    used_keys = {
        call["key"]
        for call in source_calls
        if call["source_file"].startswith("Helpers/extps1/PE_Helper/tools/PEHelperMainMenu/")
    }

    rows = []
    for key in sorted(used_keys - exported_keys):
        call = next(call for call in source_calls if call["key"] == key and call["source_file"].startswith("Helpers/extps1/PE_Helper/tools/PEHelperMainMenu/"))
        rows.append({
            "type": "pe_helper_key_missing_from_iso_export",
            "culture": "",
            "key": key,
            "source_file": call["source_file"],
            "source_line": call["source_line"],
            "details": script_path.relative_to(ROOT).as_posix(),
        })

    for key in sorted(exported_keys - used_keys):
        rows.append({
            "type": "unused_pe_helper_iso_export_key",
            "culture": "",
            "key": key,
            "source_file": script_path.relative_to(ROOT).as_posix(),
            "source_line": "",
            "details": "",
        })

    return rows

def placeholder_signature(value: str):
    return tuple(sorted({int(match.group(1)) for match in PLACEHOLDER_PATTERN.finditer(value)}))


def decoded_value(value: str):
    return (value or "").replace("{quot;}", "\"").replace("{lbrace;}", "{").replace("{rbrace;}", "}").replace("{crlf;}", "\r\n")


def is_valid_dialog_filter(value: str):
    decoded = decoded_value(value).strip()
    if not decoded:
        return True
    parts = decoded.split("|")
    if len(parts) < 2 or len(parts) % 2 != 0:
        return False
    for index, part in enumerate(parts):
        if not part.strip():
            return False
        if index % 2 == 1 and not any(char in part for char in ["*", "."]):
            return False
    return True


def main():
    language_files = sorted(LANGUAGE_DIR.glob("*.ini"))
    if not language_files:
        print("No language files were found.")
        return 1

    language_keys = {}
    language_values = {}
    duplicate_rows = []
    for language_file in language_files:
        culture = language_file.stem
        keys, values, duplicates, style_findings = parse_language_file(language_file)
        language_keys[culture] = keys
        language_values[culture] = values
        for key, line_number in duplicates:
            duplicate_rows.append((culture, key, line_number))
        if culture == "en-US":
            for finding_type, key, line_number, details in style_findings:
                duplicate_rows.append(("__STYLE__" + finding_type, key, line_number, details))

    all_language_keys = set().union(*language_keys.values())
    source_calls = collect_source_calls()
    used_keys = {call['key'] for call in source_calls}
    source_locations = {}
    for call in source_calls:
        source_locations.setdefault(call['key'], []).append(call)

    rows = []
    exit_code = 0

    for culture, keys in sorted(language_keys.items()):
        for key in sorted(all_language_keys - keys):
            rows.append({"type": "missing_in_language", "culture": culture, "key": key, "source_file": "", "source_line": "", "details": ""})
            exit_code = 1

    for key in sorted(used_keys):
        for culture, keys in sorted(language_keys.items()):
            if key not in keys:
                for call in source_locations[key]:
                    rows.append({"type": "used_key_missing_in_language", "culture": culture, "key": key, "source_file": call['source_file'], "source_line": call['source_line'], "details": call['call_name']})
                    exit_code = 1

    for key in sorted(all_language_keys - used_keys):
        rows.append({"type": "unused_key", "culture": "", "key": key, "source_file": "", "source_line": "", "details": ""})

    for key in sorted(all_language_keys):
        signatures = {}
        for culture, values in sorted(language_values.items()):
            if key in values:
                signatures[culture] = placeholder_signature(values[key])
        unique_signatures = set(signatures.values())
        if len(unique_signatures) > 1:
            detail = "; ".join(f"{culture}:{signature}" for culture, signature in signatures.items())
            rows.append({"type": "placeholder_mismatch", "culture": "", "key": key, "source_file": "", "source_line": "", "details": detail})
            exit_code = 1

    for duplicate_row in duplicate_rows:
        if len(duplicate_row) == 4 and str(duplicate_row[0]).startswith("__STYLE__"):
            finding_type, key, line_number, details = duplicate_row
            rows.append({"type": finding_type.replace("__STYLE__", ""), "culture": "", "key": key, "source_file": "", "source_line": line_number, "details": details})
            exit_code = 1
        else:
            culture, key, line_number = duplicate_row
            rows.append({"type": "duplicate_key", "culture": culture, "key": key, "source_file": "", "source_line": line_number, "details": ""})
            exit_code = 1

    for culture, values in sorted(language_values.items()):
        for key, value in sorted(values.items()):
            if key.lower().endswith(".filter") and not is_valid_dialog_filter(value):
                rows.append({"type": "invalid_dialog_filter", "culture": culture, "key": key, "source_file": "", "source_line": "", "details": decoded_value(value)})
                exit_code = 1

    english_values = language_values.get("en-US", {})
    for call in source_calls:
        value = english_values.get(call['key'], "")
        placeholders = sorted({int(match.group(1)) for match in PLACEHOLDER_PATTERN.finditer(value)})
        required_arguments = max(placeholders) + 1 if placeholders else 0
        if call['supplied_arguments'] < required_arguments:
            rows.append({
                "type": "not_enough_format_arguments",
                "culture": "",
                "key": call['key'],
                "source_file": call['source_file'],
                "source_line": call['source_line'],
                "details": f"supplied:{call['supplied_arguments']}; required:{required_arguments}",
            })
            exit_code = 1

    forbidden_rows = find_forbidden_localization_apis()
    if forbidden_rows:
        rows.extend(forbidden_rows)
        exit_code = 1

    pe_helper_export_rows = validate_pe_helper_export_map(source_calls)
    if pe_helper_export_rows:
        rows.extend(pe_helper_export_rows)
        exit_code = 1

    report_path = ROOT / "docs" / "localization" / "localization_validation_report.csv"
    report_path.parent.mkdir(parents=True, exist_ok=True)
    with report_path.open("w", encoding="utf-8-sig", newline="") as output:
        writer = csv.DictWriter(output, fieldnames=["type", "culture", "key", "source_file", "source_line", "details"])
        writer.writeheader()
        writer.writerows(rows)

    print(f"Language files checked: {len(language_files)}")
    print(f"Localization keys in language files: {len(all_language_keys)}")
    print(f"Localization keys used by source code: {len(used_keys)}")
    print(f"Report: {report_path.relative_to(ROOT).as_posix()}")

    if exit_code == 0:
        print("Localization validation passed.")
    else:
        print("Localization validation failed.")

    return exit_code


if __name__ == "__main__":
    sys.exit(main())
