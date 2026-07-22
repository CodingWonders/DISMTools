from pathlib import Path
import re, csv
ROOT=Path(__file__).resolve().parents[2]
LANG_DIR=ROOT/'language'
CALL_RE=re.compile(r'LocalizationService\.(?:T|TUpper)\((?:[^"\r\n]*,\s*)?"([^"]+)"')
EXCLUDED={'.git','bin','obj'}
used=set()
for p in ROOT.rglob('*.vb'):
    if any(part in EXCLUDED for part in p.parts): continue
    used.update(CALL_RE.findall(p.read_text(encoding='utf-8-sig',errors='ignore')))
removed=[]
for path in sorted(LANG_DIR.glob('*.ini')):
    lines=path.read_text(encoding='utf-8-sig').splitlines()
    out=[]; section=None
    for raw in lines:
        m=re.match(r'^\[([^\]]+)\]$',raw.strip())
        if m:
            section=m.group(1); out.append(raw); continue
        if section and section!='LanguageFileInformation' and '=' in raw and not raw.strip().startswith(';'):
            k=raw.split('=',1)[0].strip(); full=f'{section}.{k}'
            if full not in used:
                removed.append({'culture':path.stem,'key':full})
                continue
        out.append(raw)
    # Remove repeated blank lines caused by pruning.
    compact=[]; prev_blank=False
    for line in out:
        blank=(line.strip()=='')
        if blank and prev_blank: continue
        compact.append(line); prev_blank=blank
    path.write_text('\n'.join(compact).rstrip()+'\n',encoding='utf-8-sig')
with (ROOT/'docs/localization/stage5_pruned_unused_keys.csv').open('w',encoding='utf-8-sig',newline='') as f:
    w=csv.DictWriter(f,fieldnames=['culture','key']); w.writeheader(); w.writerows(removed)
print('Pruned unused key rows:',len(removed))
print('Unique keys pruned:',len({r['key'] for r in removed}))
