from pathlib import Path
import csv
import re

ROOT = Path(__file__).resolve().parents[2]
OUTPUT_DIR = ROOT / "docs" / "localization"
EXCLUDED_PARTS = {".git", "bin", "obj"}

TEXT_ASSIGNMENT_PATTERN = re.compile(r'^\s*(?P<target>(?:Me\.)?[A-Za-z_][\w]*(?:\.[A-Za-z_][\w]*)*\.Text)\s*=\s*"(?P<text>(?:""|[^"])*)"', re.MULTILINE)
MESSAGE_PATTERN = re.compile(r'(?P<call>MsgBox|MessageBox\.Show)\s*\(\s*"(?P<text>(?:""|[^"])*)"')
LOCALIZATION_CALL_PATTERN = re.compile(r'LocalizationService\.(?:ForSection|T|TUpper)\(')
SELECT_LANGUAGE_PATTERN = re.compile(r"Select Case (?:Language|LangCode|MainForm\.Language)")
INSTALLED_UI_CULTURE_PATTERN = re.compile(r"InstalledUICulture")
ADDRANGE_START_PATTERN = re.compile(r'Me\.(?P<target>[A-Za-z_]\w*)\.Items\.AddRange\(New Object\(\) \{')
STRING_RE = re.compile(r'"(?P<value>(?:""|[^"])*)"')

COMPONENT_NAME_RE = re.compile(r'^(?:MenuStrip|ToolStrip|StatusStrip|DarkToolStrip)\d*$')
NUMBER_RE = re.compile(r'^\d+$')


def is_source_file(path: Path) -> bool:
    if path.suffix.lower() != ".vb":
        return False
    return not any(part in EXCLUDED_PARTS for part in path.parts)


def decode_vb_string(value: str) -> str:
    return value.replace('""', '"')


def split_top_level_items(body: str):
    items = []
    current = []
    in_string = False
    index = 0
    while index < len(body):
        char = body[index]
        if char == '"':
            current.append(char)
            if in_string and index + 1 < len(body) and body[index + 1] == '"':
                current.append('"')
                index += 2
                continue
            in_string = not in_string
            index += 1
            continue
        if not in_string and char == ',':
            items.append(''.join(current).strip())
            current = []
            index += 1
            continue
        current.append(char)
        index += 1
    if ''.join(current).strip():
        items.append(''.join(current).strip())
    return items


def parse_vb_string_expr(expr: str) -> str:
    return ''.join(decode_vb_string(match.group('value')) for match in STRING_RE.finditer(expr))


def find_item_add_ranges(text: str):
    start = 0
    while True:
        match = ADDRANGE_START_PATTERN.search(text, start)
        if not match:
            break
        body_start = match.end()
        index = body_start
        in_string = False
        while index < len(text) - 1:
            char = text[index]
            if char == '"':
                if in_string and index + 1 < len(text) and text[index + 1] == '"':
                    index += 2
                    continue
                in_string = not in_string
            elif not in_string and text[index:index + 2] == '})':
                body = text[body_start:index]
                yield match.group('target'), match.start(), body
                index += 2
                break
            index += 1
        start = index


def classify_text_assignment(relative_path: str, target: str, value: str):
    if not value.strip():
        return "empty_runtime_value"
    if COMPONENT_NAME_RE.match(value) or value in {"Status"}:
        return "component_identifier"
    if NUMBER_RE.match(value):
        return "numeric_default"
    if value in {"Consolas"}:
        return "font_name"
    if value in {"amd64", "x86", "arm64", "WIM", "ESD"}:
        return "technical_token"
    if value in {"Machine", "User"} and "EnvironmentVariables" in relative_path:
        return "internal_scope_token"
    if value in {"Windows 10", "Windows 11"} and relative_path.endswith("ProjProperties.vb"):
        return "windows_product_name"
    if value == "N/A" and relative_path == "MainForm.vb":
        return "project_file_placeholder"
    if value.startswith("C:\\Windows\\Logs\\DISM"):
        return "default_log_path"
    if value.startswith("\\\\.\\"):
        return "device_path_prefix"
    if value.startswith("HKEY_LOCAL_MACHINE"):
        return "registry_path"
    if value.startswith("return 'DESKTOP-"):
        return "powershell_template"
    if value.startswith("<?xml"):
        return "xml_template"
    if value in {"Admin", "Administrators", "Users"} and "NewUnattendWiz" in relative_path:
        return "windows_account_or_group_seed"
    if value in {"PowerShell", "Batch", "VBScript", "JScript", "VBScript (obsolete)", "JScript (obsolete)"}:
        return "script_language_token"
    if value in {"United States", "US", "English (United States)", "English"} and "NewUnattendWiz" in relative_path:
        return "locale_seed_value"
    if value in {"DISMTools", "VirtIO Guest Tools", "VirtualBox Guest Additions", "VMware Tools", "Parallels Tools"}:
        return "product_or_tool_name"
    return ""


def classify_item_add_range(relative_path: str, target: str, values):
    value_set = set(values)
    if all(NUMBER_RE.match(value or "") for value in values):
        return "numeric_choice_values"
    if value_set <= {"x86", "amd64", "arm64"}:
        return "architecture_tokens_used_as_command_values"
    if value_set <= {"WIM", "ESD"}:
        return "image_format_tokens_used_as_file_extensions"
    if any("Keyboard" in value for value in values) and "SetLayeredDriver" in relative_path:
        return "keyboard_layout_names_used_by_dism_index"
    if value_set <= {"PowerShell", "Batch", "VBScript", "JScript", "VBScript (obsolete)", "JScript (obsolete)"}:
        return "script_language_tokens_used_as_model_values"
    if value_set <= {"Administrators", "Users"}:
        return "windows_group_names_used_as_account_values"
    if {"Education", "Pro", "Enterprise"}.intersection(value_set) and "NewUnattendWiz" in relative_path:
        return "windows_edition_names_used_as_unattend_values"
    return ""


def candidate_row(kind, relative_path, line_number, target, text, suggested_section, suggested_key):
    return {
        "type": kind,
        "file": relative_path,
        "line": line_number,
        "target": target,
        "text": text,
        "suggested_section": suggested_section,
        "suggested_key": suggested_key,
    }


def main():
    OUTPUT_DIR.mkdir(parents=True, exist_ok=True)
    candidates_path = OUTPUT_DIR / "localization_candidates.csv"
    ignored_path = OUTPUT_DIR / "localization_ignored_literals.csv"
    inventory_path = OUTPUT_DIR / "source_localization_inventory.csv"

    candidates = []
    ignored = []
    inventory = []
    checked_count = 0

    for path in sorted(ROOT.rglob("*.vb")):
        if not is_source_file(path):
            continue

        checked_count += 1
        relative_path = path.relative_to(ROOT).as_posix()
        text = path.read_text(encoding="utf-8-sig", errors="ignore")
        lines = text.splitlines()

        select_case_count = len(SELECT_LANGUAGE_PATTERN.findall(text))
        installed_ui_culture_count = len(INSTALLED_UI_CULTURE_PATTERN.findall(text))
        raw_direct_text_count = 0
        raw_item_string_count = 0
        message_count = len(MESSAGE_PATTERN.findall(text))
        localization_call_count = len(LOCALIZATION_CALL_PATTERN.findall(text))
        unclassified_count = 0

        for line_number, line in enumerate(lines, start=1):
            for match in TEXT_ASSIGNMENT_PATTERN.finditer(line):
                value = decode_vb_string(match.group("text"))
                if value.strip() == "":
                    continue
                raw_direct_text_count += 1
                target = match.group("target")
                reason = classify_text_assignment(relative_path, target, value)
                row = candidate_row(
                    "text_assignment",
                    relative_path,
                    line_number,
                    target,
                    value,
                    path.stem,
                    f"{path.stem}.{target.replace('.', '_')}",
                )
                if reason:
                    row["reason"] = reason
                    ignored.append(row)
                else:
                    candidates.append(row)
                    unclassified_count += 1

            for match in MESSAGE_PATTERN.finditer(line):
                value = decode_vb_string(match.group("text"))
                candidates.append(candidate_row(
                    "message",
                    relative_path,
                    line_number,
                    match.group("call"),
                    value,
                    path.stem,
                    f"{path.stem}.Message.Line{line_number}",
                ))
                unclassified_count += 1

        for target, start_position, body in find_item_add_ranges(text):
            item_exprs = split_top_level_items(body)
            if item_exprs and all("LocalizationService" in expr for expr in item_exprs):
                continue
            values = [parse_vb_string_expr(expr) for expr in item_exprs]
            if not values:
                continue
            raw_item_string_count += len(values)
            line_number = text.count("\n", 0, start_position) + 1
            reason = classify_item_add_range(relative_path, target, values)
            for index, value in enumerate(values):
                if not value.strip():
                    continue
                row = candidate_row(
                    "items_add_range",
                    relative_path,
                    line_number,
                    f"Me.{target}.Items[{index}]",
                    value,
                    path.stem,
                    f"{path.stem}.{target}_Items_{index}",
                )
                if reason:
                    row["reason"] = reason
                    ignored.append(row)
                else:
                    candidates.append(row)
                    unclassified_count += 1

        if select_case_count or installed_ui_culture_count or raw_direct_text_count or raw_item_string_count or message_count or localization_call_count or unclassified_count:
            inventory.append({
                "file": relative_path,
                "select_case_language": select_case_count,
                "installed_ui_culture": installed_ui_culture_count,
                "raw_direct_text_assignments": raw_direct_text_count,
                "raw_item_add_range_strings": raw_item_string_count,
                "message_calls": message_count,
                "localization_calls": localization_call_count,
                "unclassified_localization_candidates": unclassified_count,
            })

    with candidates_path.open("w", encoding="utf-8-sig", newline="") as output:
        writer = csv.DictWriter(output, fieldnames=["type", "file", "line", "target", "text", "suggested_section", "suggested_key"])
        writer.writeheader()
        writer.writerows(candidates)

    with ignored_path.open("w", encoding="utf-8-sig", newline="") as output:
        writer = csv.DictWriter(output, fieldnames=["type", "file", "line", "target", "text", "suggested_section", "suggested_key", "reason"])
        writer.writeheader()
        writer.writerows(ignored)

    with inventory_path.open("w", encoding="utf-8-sig", newline="") as output:
        writer = csv.DictWriter(output, fieldnames=["file", "select_case_language", "installed_ui_culture", "raw_direct_text_assignments", "raw_item_add_range_strings", "message_calls", "localization_calls", "unclassified_localization_candidates"])
        writer.writeheader()
        writer.writerows(inventory)

    print(f"Source files checked: {checked_count}")
    print(f"Unclassified localization candidates: {len(candidates)}")
    print(f"Classified non-localization literals: {len(ignored)}")
    print(f"Candidates: {candidates_path.relative_to(ROOT).as_posix()}")
    print(f"Ignored literals: {ignored_path.relative_to(ROOT).as_posix()}")
    print(f"Inventory: {inventory_path.relative_to(ROOT).as_posix()}")


if __name__ == "__main__":
    main()
