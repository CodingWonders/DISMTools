#!/usr/bin/env python3
import csv
import re
import xml.etree.ElementTree as ET
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
OUT_DIR = ROOT / 'docs' / 'localization'
OUT_DIR.mkdir(parents=True, exist_ok=True)
STRING_PATTERN = re.compile(r'"(?:""|[^"])*"')
DIRECT_UI_ASSIGNMENT = re.compile(r'\.(Text|Filter|Title|Description|ToolTipText|BalloonTipText|BalloonTipTitle)\s*=\s*"')
UI_CALLS = ('MsgBox', 'MessageBox.Show', 'WindowHelper.DisplayToolTip', '.SetToolTip', 'GetHeader', 'GetParagraph', 'GetListItems', 'GetTableHeader')
DIRECT_RESOURCE_USAGE = re.compile(r'My\.Resources\.([A-Za-z0-9_]+)')
LOCALIZED_PREFIXES = ('LocalizationService.T(', 'LocalizationService.ForSection(', 'LocalizationService.TUpper(')
LOCALIZED_CALL_PATTERN = re.compile(r'LocalizationService\.(?:T|TUpper|ForSection)\("(?:""|[^"])*"\)(?:\.(?:Format)\("(?:""|[^"])*"\)|\("(?:""|[^"])*"\))?')

TECHNICAL_PATTERNS = [
    re.compile(r'\\'),
    re.compile(r'^\s*[/-]'),
    re.compile(r'^HKLM|^HKEY_|^LDAP://|^\(objectCategory='),
    re.compile(r'^%[A-Z0-9_]+%$'),
    re.compile(r'^<[^>]+>$'),
    re.compile(r'^\*\.'),
    re.compile(r'^SELECT .* FROM ', re.I),
    re.compile(r'^[A-Z0-9_]+$'),
    re.compile(r'^(x86|amd64|arm64|WIM|ESD|FFU|ISO|XML|PowerShell|Batch|VBScript|JScript|WindowsPE|neutral|Ireland|United States|English|English \(United States\))$', re.I),
    re.compile(r'^(MenuStrip1|ToolStrip1|ToolStrip2|StatusStrip1|DarkToolStrip1|DISMTools|Consolas)$'),
    re.compile(r'^[A-Za-z0-9_]+(\.[A-Za-z0-9_]+)+$'),
    re.compile(r'^(Windows 10|Windows 11|Admin|Users|Administrators)$'),
    re.compile(r'^yyMMdd-HHmm$'),
    re.compile(r'^INI File Parser:'),
    re.compile(r'^(InstRoot|WinPECacheThreshold)$'),
    re.compile(r'^return \'DESKTOP-'),
    re.compile(r'^<unattend\s'),
    re.compile(r'^microsoft windows \{0\}\{1\}\{0\}$'),
]
RESOURCE_UI_NAMES = {'LicenseOverview', 'WhatsNew'}
RESOURCE_TECHNICAL_NAMES = {'DefaultUnattended_AuditMode', 'HI_UninstallScript'}

def decode_vb_string(token: str) -> str:
    return token[1:-1].replace('""', '"')

def looks_technical(value: str) -> bool:
    if not value or value.isspace():
        return True
    if not re.search(r'[A-Za-zА-Яа-я]{2,}', value):
        return True
    return any(p.search(value.strip()) for p in TECHNICAL_PATTERNS)

def write_csv(filename, rows, fields):
    path = OUT_DIR / filename
    with path.open('w', encoding='utf-8-sig', newline='') as f:
        writer = csv.DictWriter(f, fieldnames=fields)
        writer.writeheader()
        writer.writerows(rows)
    return path

candidates = []
ignored = []

for path in sorted(ROOT.rglob('*.vb')):
    if any(part in {'.git', 'bin', 'obj'} for part in path.parts):
        continue
    rel = path.relative_to(ROOT).as_posix()
    text = path.read_text(encoding='utf-8-sig', errors='ignore')
    for lineno, line in enumerate(text.splitlines(), 1):
        stripped = line.strip()
        if not stripped or stripped.startswith("'"):
            continue
        scan_line = LOCALIZED_CALL_PATTERN.sub('LocalizationService.LocalizedValue', line)
        is_ui_context = bool(DIRECT_UI_ASSIGNMENT.search(scan_line)) or any(call in scan_line for call in UI_CALLS)
        for match in STRING_PATTERN.finditer(scan_line):
            value = decode_vb_string(match.group(0))
            if looks_technical(value):
                ignored.append({'type':'technical_literal', 'file':rel, 'line':lineno, 'details':value})
                continue
            if is_ui_context:
                candidates.append({'type':'direct_ui_literal', 'file':rel, 'line':lineno, 'details':value})
            else:
                ignored.append({'type':'data_or_internal_literal', 'file':rel, 'line':lineno, 'details':value})
        for res_match in DIRECT_RESOURCE_USAGE.finditer(line):
            name = res_match.group(1)
            if name in RESOURCE_UI_NAMES:
                candidates.append({'type':'ui_resource_usage', 'file':rel, 'line':lineno, 'details':name})
            elif name in RESOURCE_TECHNICAL_NAMES:
                ignored.append({'type':'technical_resource_usage', 'file':rel, 'line':lineno, 'details':name})

# Check resx text resources that are still referenced directly by source.
referenced_resources = set()
for path in ROOT.rglob('*.vb'):
    if any(part in {'.git', 'bin', 'obj'} for part in path.parts):
        continue
    for m in DIRECT_RESOURCE_USAGE.finditer(path.read_text(encoding='utf-8-sig', errors='ignore')):
        referenced_resources.add(m.group(1))
for path in sorted(ROOT.rglob('*.resx')):
    if any(part in {'.git', 'bin', 'obj'} for part in path.parts):
        continue
    rel = path.relative_to(ROOT).as_posix()
    try:
        tree = ET.parse(path)
    except Exception:
        continue
    for data in tree.findall('data'):
        name = data.attrib.get('name', '')
        value_node = data.find('value')
        value = value_node.text if value_node is not None and value_node.text is not None else ''
        if name in referenced_resources and name in RESOURCE_UI_NAMES:
            candidates.append({'type':'referenced_ui_resx_text', 'file':rel, 'line':'', 'details':name})
        elif name in referenced_resources:
            ignored.append({'type':'referenced_non_ui_resx', 'file':rel, 'line':'', 'details':name})

remaining_path = write_csv('localization_deep_audit_remaining.csv', candidates, ['type','file','line','details'])
ignored_path = write_csv('localization_deep_audit_ignored.csv', ignored, ['type','file','line','details'])
summary_rows = [
    {'metric':'direct_ui_candidates', 'value':len(candidates)},
    {'metric':'classified_non_ui_literals', 'value':len(ignored)},
]
summary_path = write_csv('localization_deep_audit_summary.csv', summary_rows, ['metric','value'])
print(f'Deep UI localization candidates: {len(candidates)}')
print(f'Classified non UI literals: {len(ignored)}')
print(f'Remaining: {remaining_path}')
print(f'Ignored: {ignored_path}')
print(f'Summary: {summary_path}')
raise SystemExit(1 if candidates else 0)
