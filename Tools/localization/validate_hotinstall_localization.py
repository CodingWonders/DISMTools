#!/usr/bin/env python3
from pathlib import Path
from zipfile import ZipFile
import configparser
import sys

ROOT = Path(__file__).resolve().parents[2]
SOURCE_DIR = ROOT / 'Helpers' / 'extps1' / 'PE_Helper' / 'tools' / 'HotInstall' / 'Installer' / 'Languages'
PARSER_FILE = ROOT / 'Helpers' / 'extps1' / 'PE_Helper' / 'tools' / 'HotInstall' / 'Installer' / 'Util' / 'LanguageFileParser.vb'
PACKAGE_FILE = ROOT / 'Helpers' / 'extps1' / 'PE_Helper' / 'files' / 'HotInstall.zip'
DEFAULT_CODE = 'en-US'


def parse_language_bytes(data: bytes, label: str):
    text = data.decode('utf-8-sig')
    parser = configparser.ConfigParser(interpolation=None, strict=False)
    parser.optionxform = str
    parser.read_string(text)
    if 'LanguageFileInformation' not in parser:
        raise ValueError(f'{label}: missing [LanguageFileInformation]')
    metadata = parser['LanguageFileInformation']
    code = metadata.get('LanguageCode', '').strip().strip('"')
    name = metadata.get('LanguageName', '').strip().strip('"')
    if not code:
        raise ValueError(f'{label}: missing LanguageCode')
    if not name:
        raise ValueError(f'{label}: missing LanguageName')
    keys = {
        f'{section}.{item}'
        for section in parser.sections()
        if section != 'LanguageFileInformation'
        for item in parser[section]
    }
    return code, name, keys


def main():
    errors = []
    source_by_name = {}
    source_codes = {}
    reference_keys = None

    for path in sorted(SOURCE_DIR.glob('*.ini')):
        try:
            code, name, keys = parse_language_bytes(path.read_bytes(), path.as_posix())
        except Exception as exc:
            errors.append(str(exc))
            continue
        if code in source_codes:
            errors.append(f'duplicate HotInstall LanguageCode {code}: {source_codes[code]} and {path.name}')
        source_codes[code] = path.name
        source_by_name[path.name] = path.read_bytes()
        if reference_keys is None:
            reference_keys = keys
        elif keys != reference_keys:
            missing = sorted(reference_keys - keys)
            extra = sorted(keys - reference_keys)
            errors.append(f'{path.name}: key mismatch, missing={missing}, extra={extra}')

    if DEFAULT_CODE not in source_codes:
        errors.append(f'HotInstall default language {DEFAULT_CODE} is missing')

    parser_text = PARSER_FILE.read_text(encoding='utf-8-sig')
    required_markers = [
        'LanguageCode',
        'Directory.GetFiles(languageDirectory, "*.ini"',
        'DefaultLanguageCode As String = "en-US"',
    ]
    for marker in required_markers:
        if marker not in parser_text:
            errors.append(f'HotInstall dynamic language parser marker missing: {marker}')
    forbidden_markers = ['lang_en.ini', 'lang_es.ini', 'lang_fr.ini', 'lang_it.ini', 'lang_pt.ini', 'supportedLanguages As New Dictionary']
    for marker in forbidden_markers:
        if marker in parser_text:
            errors.append(f'HotInstall parser still contains a fixed language mapping: {marker}')

    if not PACKAGE_FILE.exists():
        errors.append('HotInstall.zip is missing')
    else:
        with ZipFile(PACKAGE_FILE) as package:
            packaged = {
                Path(name).name: package.read(name)
                for name in package.namelist()
                if name.startswith('Languages/') and name.lower().endswith('.ini')
            }
        if set(packaged) != set(source_by_name):
            errors.append(f'HotInstall.zip language set differs from source: source={sorted(source_by_name)}, package={sorted(packaged)}')
        for name, source_data in source_by_name.items():
            packaged_data = packaged.get(name)
            if packaged_data is not None and packaged_data != source_data:
                errors.append(f'HotInstall.zip contains an outdated language file: Languages/{name}')

    if errors:
        print('HotInstall localization validation failed.')
        for error in errors:
            print(error)
        sys.exit(1)

    key_count = len(reference_keys or set())
    print(f'HotInstall languages checked: {len(source_codes)}')
    print(f'HotInstall localization keys per language: {key_count}')
    print('HotInstall dynamic LanguageCode validation passed.')


if __name__ == '__main__':
    main()
