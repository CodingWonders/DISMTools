from pathlib import Path
import csv
import hashlib
import re
import validate_localization

ROOT = Path(__file__).resolve().parents[2]
LANGUAGE_DIR = ROOT / "language"
OUTPUT = ROOT / "docs" / "localization" / "localization_key_index.csv"


def parse_ini(path: Path):
    values = {}
    line_map = {}
    section_map = {}
    item_map = {}
    section = None
    for line_number, raw in enumerate(path.read_text(encoding="utf-8-sig").splitlines(), start=1):
        line = raw.strip()
        section_match = re.match(r"^\[([^\]]+)\]$", line)
        if section_match:
            section = section_match.group(1).strip()
            continue
        if section and section != "LanguageFileInformation" and "=" in raw and not line.startswith(";"):
            key, value = raw.split("=", 1)
            item = key.strip()
            full_key = f"{section}.{item}"
            cleaned_value = value.strip()
            if len(cleaned_value) >= 2 and cleaned_value[0] == '"' and cleaned_value[-1] == '"':
                cleaned_value = cleaned_value[1:-1]
            values[full_key] = cleaned_value.replace("{quot;}", '"').replace("{crlf;}", "\n")
            line_map[full_key] = line_number
            section_map[full_key] = section
            item_map[full_key] = item
    return values, line_map, section_map, item_map


languages = {}
language_lines = {}
sections = {}
items = {}
for language_file in sorted(LANGUAGE_DIR.glob("*.ini")):
    values, line_map, section_map, item_map = parse_ini(language_file)
    languages[language_file.stem] = values
    language_lines[language_file.stem] = line_map
    sections.update(section_map)
    items.update(item_map)

usage = {}
source_calls = validate_localization.collect_source_calls()
for call in source_calls:
    usage.setdefault(call["key"], []).append(f"{call['source_file']}:{call['source_line']}")

all_keys = sorted(set().union(*(set(values.keys()) for values in languages.values())))
fields = ["key", "section", "item", "used", "usage", "stable_id"]
fields.extend(f"text_{culture}" for culture in sorted(languages))
fields.extend(f"line_{culture}" for culture in sorted(languages))

OUTPUT.parent.mkdir(parents=True, exist_ok=True)
with OUTPUT.open("w", encoding="utf-8-sig", newline="") as output:
    writer = csv.DictWriter(output, fieldnames=fields)
    writer.writeheader()
    for key in all_keys:
        row = {
            "key": key,
            "section": sections.get(key, ""),
            "item": items.get(key, ""),
            "used": "yes" if key in usage else "no",
            "usage": " | ".join(usage.get(key, [])),
            "stable_id": hashlib.sha1(key.encode("utf-8")).hexdigest()[:12],
        }
        for culture in sorted(languages):
            row[f"text_{culture}"] = languages[culture].get(key, "")
            row[f"line_{culture}"] = language_lines[culture].get(key, "")
        writer.writerow(row)

print(f"Wrote {OUTPUT.relative_to(ROOT).as_posix()} with {len(all_keys)} keys")
