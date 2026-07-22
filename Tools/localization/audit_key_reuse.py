#!/usr/bin/env python3
import csv
from collections import defaultdict
from pathlib import Path

import validate_localization

ROOT = Path(__file__).resolve().parents[2]
OUTPUT = ROOT / 'docs' / 'localization' / 'localization_key_reuse_audit.csv'


def logical_owner(source_file: str) -> str:
    if source_file.endswith('.Designer.vb'):
        return source_file[:-len('.Designer.vb')] + '.vb'
    return source_file


def main():
    uses = defaultdict(list)
    for call in validate_localization.collect_source_calls():
        uses[call['key']].append({
            'owner': logical_owner(call['source_file']),
            'source_file': call['source_file'],
            'source_line': call['source_line'],
            'call_name': call['call_name'],
        })

    cross_owner = {
        key: entries
        for key, entries in uses.items()
        if len({entry['owner'] for entry in entries}) > 1
    }

    OUTPUT.parent.mkdir(parents=True, exist_ok=True)
    with OUTPUT.open('w', newline='', encoding='utf-8-sig') as handle:
        writer = csv.writer(handle)
        writer.writerow(['key', 'owner_count', 'usage_count', 'logical_owner', 'source_file', 'source_line', 'call_name'])
        for key in sorted(cross_owner):
            entries = cross_owner[key]
            owner_count = len({entry['owner'] for entry in entries})
            for entry in entries:
                writer.writerow([
                    key,
                    owner_count,
                    len(entries),
                    entry['owner'],
                    entry['source_file'],
                    entry['source_line'],
                    entry['call_name'],
                ])

    print(f'Localization keys used by more than one logical window or component: {len(cross_owner)}')
    print(f'Report: {OUTPUT}')


if __name__ == '__main__':
    main()
