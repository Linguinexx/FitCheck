"""Add portable playtest instructions after native validation; no player saves."""
from pathlib import Path
import hashlib,json,shutil,subprocess

project=Path(__file__).resolve().parents[3]
workspace=project.parent
output=workspace/'tools/.city-build-project/Builds/Windows-Test01'
assert output.resolve().is_relative_to(workspace.resolve())
assert (output/'FitCheck.exe').is_file()
proof=(project/'Captures/Test01/checks.txt').read_text(encoding='utf-8')
assert proof.startswith('TEST_BUILD_01_PASSED:'), 'Native review must complete first'
for name in ('LEIA-ME.txt','ROTEIRO-DE-TESTE.md','RELATO-DE-BUG.txt'):
    shutil.copy2(Path(__file__).parent/name,output/name)
fingerprints={p.relative_to(project).as_posix():hashlib.sha256(p.read_bytes()).hexdigest()
              for p in sorted((project/'Assets').rglob('*')) if p.is_file() and p.suffix!='.meta'}
version={'edition':'Fit Check — Teste 01','date':'2026-11-05','platform':'Windows x64',
         'unity':(project/'ProjectSettings/ProjectVersion.txt').read_text().splitlines()[0],
         'base_commit':subprocess.check_output(['git','-C',str(project),'rev-parse','HEAD'],text=True).strip(),
         'assets_sha256':hashlib.sha256(json.dumps(fingerprints,sort_keys=True).encode()).hexdigest(),
         'asset_files':len(fingerprints),'validation':proof.strip(),
         'build_report':(output.parent/'Windows-Test01-build-report.txt').read_text(encoding='utf-8').strip()}
(output/'VERSAO.json').write_text(json.dumps(version,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
for p in output.rglob('*'):
    if not p.is_file():continue
    assert p.name.lower() not in ('day-one.json','appearance.json','shop-style.json','player.log'),p
    assert not any(part.lower() in ('captures','library','temp','saves','.git') for part in p.relative_to(output).parts),p
print('TEST01_DELIVERY_READY:',len(fingerprints),'source assets; native review passed; portable instructions added')
