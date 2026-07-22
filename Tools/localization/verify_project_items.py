from pathlib import Path
import xml.etree.ElementTree as ET
import sys

ROOT = Path(__file__).resolve().parents[2]
errors = []
checked_projects = 0
checked_items = 0

for project_file in ROOT.rglob('*.vbproj'):
    if any(part.lower() in {'bin', 'obj'} for part in project_file.parts):
        continue
    checked_projects += 1
    try:
        root = ET.parse(project_file).getroot()
    except Exception as exc:
        errors.append(f'{project_file.relative_to(ROOT)}: cannot parse project file: {exc}')
        continue
    ns = {'m': 'http://schemas.microsoft.com/developer/msbuild/2003'} if root.tag.startswith('{') else {}

    def find_all(name):
        return root.findall(f'.//m:{name}', ns) if ns else root.findall(f'.//{name}')

    for item_name in ('Compile', 'EmbeddedResource'):
        for item in find_all(item_name):
            include = item.get('Include')
            if not include:
                continue
            checked_items += 1
            item_path = project_file.parent / include.replace('\\', '/')
            if not item_path.exists():
                errors.append(f'{project_file.relative_to(ROOT)}: missing {item_name}: {include}')
            if item_name == 'EmbeddedResource':
                dep = item.find('m:DependentUpon', ns) if ns else item.find('DependentUpon')
                if dep is not None and dep.text:
                    dep_path = item_path.parent / dep.text
                    if not dep_path.exists():
                        errors.append(
                            f'{project_file.relative_to(ROOT)}: bad resource dependency: {include} depends on {dep.text}'
                        )


# Lightweight VB declaration sanity checks.
# These checks catch broken identifiers introduced by file or key rename passes,
# for example `Public Class EnvironmentVariables.ManagementForm`.
import re

declaration_re = re.compile(
    r"^(?:(?:Public|Private|Protected|Friend|Partial|Shared|Overloads|Overrides|Static|Default|ReadOnly|WriteOnly)\s+)*"
    r"(Class|Module|Structure|Interface|Enum|Sub|Function|Property)\s+([^\s\(]+)",
    re.IGNORECASE,
)

for vb_file in ROOT.rglob('*.vb'):
    if any(part.lower() in {'bin', 'obj'} for part in vb_file.parts):
        continue
    try:
        lines = vb_file.read_text(encoding='utf-8-sig').splitlines()
    except UnicodeDecodeError:
        lines = vb_file.read_text(errors='ignore').splitlines()
    for line_number, line in enumerate(lines, 1):
        stripped = line.strip()
        match = declaration_re.match(stripped)
        if not match:
            continue
        identifier = match.group(2)
        if '.' in identifier:
            errors.append(
                f'{vb_file.relative_to(ROOT)}:{line_number}: invalid dotted VB declaration: {identifier}'
            )



# References that are known to fail compilation after semantic form renames.
# Keep these checks close to the project item audit because they validate
# source level links between renamed forms and their callers.
forbidden_source_patterns = {
    'Casters.CastDismSignatureStatus(': 'use Casters.SignatureStatus(',
    'Casters.CastDismApplicabilityStatus(': 'use Casters.Applicability(',
    'Casters.CastDismFullyOfflineInstallationType(': 'use Casters.OfflineInstallType(',
    'Casters.ApplicabilityStatus(': 'use Casters.Applicability(',
    'Casters.OfflineInstallationType(': 'use Casters.OfflineInstallType(',
    'PEScratch.ShowDialog(': 'use SetPEScratchSpace.ShowDialog(',
    'ApplyUnattend.ShowDialog(': 'use ApplyUnattendFile.ShowDialog(',
}

# Some WinForms controls have names that can collide with renamed form classes.
# Calling ShowDialog on those controls is a compile error, so flag it here.
control_declarations = {}
control_decl_re = re.compile(
    r'^\s*Friend\s+WithEvents\s+([A-Za-z_][A-Za-z0-9_]*)\s+As\s+([A-Za-z0-9_.]+)',
    re.IGNORECASE,
)
show_dialog_re = re.compile(r'\b([A-Za-z_][A-Za-z0-9_]*)\.ShowDialog\s*\(')

for vb_file in ROOT.rglob('*.vb'):
    if any(part.lower() in {'bin', 'obj'} for part in vb_file.parts):
        continue
    try:
        text = vb_file.read_text(encoding='utf-8-sig')
    except UnicodeDecodeError:
        text = vb_file.read_text(errors='ignore')
    for line in text.splitlines():
        match = control_decl_re.match(line)
        if match:
            control_declarations[match.group(1)] = match.group(2)

for vb_file in ROOT.rglob('*.vb'):
    if any(part.lower() in {'bin', 'obj'} for part in vb_file.parts):
        continue
    try:
        lines = vb_file.read_text(encoding='utf-8-sig').splitlines()
    except UnicodeDecodeError:
        lines = vb_file.read_text(errors='ignore').splitlines()
    for line_number, line in enumerate(lines, 1):
        for pattern, fix in forbidden_source_patterns.items():
            if pattern in line:
                errors.append(
                    f'{vb_file.relative_to(ROOT)}:{line_number}: forbidden stale reference {pattern}. {fix}'
                )
        for match in show_dialog_re.finditer(line):
            target = match.group(1)
            target_type = control_declarations.get(target)
            if target_type and target_type.endswith('ToolStripMenuItem'):
                errors.append(
                    f'{vb_file.relative_to(ROOT)}:{line_number}: ShowDialog called on menu item {target}. Use the form class instead.'
                )

if errors:
    print('Project item validation failed.')
    for error in errors:
        print(error)
    sys.exit(1)

print(f'Project item validation passed. Projects checked: {checked_projects}. Items checked: {checked_items}.')
