"""Package the validated Test08 executable; never include player saves or caches."""
from pathlib import Path
import hashlib
import json
import shutil
import subprocess
import zipfile

project=Path(__file__).resolve().parents[3]
workspace=project.parent
output=workspace/'tools/.city-build-project/Builds/Windows-Test08'
proof=project/'Captures/Test08'
assert output.resolve().is_relative_to(workspace.resolve())
assert (output/'FitCheck.exe').is_file()
assert (proof/'FlowFinal/checks.txt').read_text(encoding='utf-8').startswith('TEST_BUILD_01_PASSED:')
performance=json.loads((proof/'DropShelves/drop-shelf-performance.json').read_text(encoding='utf-8'))
assert performance['refreshes']==0 and performance['steady_lookup_allocated_bytes']==0
assert (proof/'Gameplay/shop-gameplay-performance.json').is_file()
assert (proof/'Entries/entry-performance.json').is_file()
for name in ('LEIA-ME.txt','ROTEIRO-DE-TESTE.md','RELATO-DE-BUG.txt'):
    shutil.copy2(Path(__file__).parent/name,output/name)
credits=output/'CREDITOS';credits.mkdir(exist_ok=True)
for source in ('Art/Audio/CREDITS.md','Art/AmbientCG/CREDITS.md','Art/Fonts/CREDITS.md','Art/UserArtwork/CREDITS.md'):
    path=project/source;shutil.copy2(path,credits/(path.parent.name+'.md'))
fingerprints={p.relative_to(project).as_posix():hashlib.sha256(p.read_bytes()).hexdigest()
              for p in sorted((project/'Assets').rglob('*')) if p.is_file()}
build_report=(output.parent/'Windows-Test08-build-report.txt').read_text(encoding='utf-8').strip()
version={'edition':'Fit Check — Teste 08','date':'2026-11-11','platform':'Windows x64',
         'unity':(project/'ProjectSettings/ProjectVersion.txt').read_text().splitlines()[0],
         'base_commit':subprocess.check_output(['git','-C',str(project),'rev-parse','HEAD'],text=True).strip(),
         'uncommitted_changes':True,'assets_sha256':hashlib.sha256(json.dumps(fingerprints,sort_keys=True).encode()).hexdigest(),
         'asset_files':len(fingerprints),'build_report':build_report,'compression':'LZ4HC',
         'validation':['seven-day campaign and historical saves','native fitting shelf colors and cache invalidation',
                       'home/neighborhood/shop routes, service, loadings, save slots','shop gameplay and entry performance'],
         'wardrobe_cache':performance}
(output/'VERSAO.json').write_text(json.dumps(version,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
for p in output.rglob('*'):
    if not p.is_file():continue
    assert p.name.lower() not in ('day-one.json','appearance.json','shop-style.json','player.log'),p
    assert not any(part.lower() in ('captures','library','temp','saves','.git','packages') for part in p.relative_to(output).parts),p
folder_bytes=sum(p.stat().st_size for p in output.rglob('*') if p.is_file())
assert folder_bytes<=200_000_000,folder_bytes
destination=project/'Builds/Windows-Test08'
assert destination.resolve().is_relative_to((project/'Builds').resolve())
previous=project/'Builds/FitCheck-Windows-Test07.zip'
with zipfile.ZipFile(previous) as zipped:
    assert zipped.testzip() is None,'Previous delivery must remain recoverable'
    executable=next(name for name in zipped.namelist() if name.endswith('/FitCheck.exe') or name=='FitCheck.exe')
    assert hashlib.sha256(zipped.read(executable)).digest()==hashlib.sha256((project/'Builds/Windows-Test07/FitCheck.exe').read_bytes()).digest()
assert not destination.exists(),'Never overwrite an existing delivery silently'
shutil.move(str(output),str(destination))
shutil.copy2(workspace/'tools/.city-build-project/Builds/Windows-Test08-build-report.txt',destination.parent/'Windows-Test08-build-report.txt')
archive=destination.parent/'FitCheck-Windows-Test08.zip'
assert not archive.exists()
with zipfile.ZipFile(archive,'w',zipfile.ZIP_DEFLATED,compresslevel=9) as zipped:
    for p in sorted(destination.rglob('*')):
        if p.is_file():zipped.write(p,Path(destination.name)/p.relative_to(destination))
with zipfile.ZipFile(archive) as zipped:
    assert zipped.testzip() is None
assert archive.stat().st_size<=200_000_000
digest=hashlib.sha256(archive.read_bytes()).hexdigest()
archive.with_suffix(archive.suffix+'.sha256').write_text(digest+'  '+archive.name+'\n',encoding='ascii')
report={'folder':destination.relative_to(project).as_posix(),'folder_bytes':folder_bytes,
        'archive':archive.relative_to(project).as_posix(),'archive_bytes':archive.stat().st_size,'sha256':digest,
        'build_report':build_report,'wardrobe_cache':performance,'previous_archive_crc_verified':True}
(Path(__file__).parent/'delivery.json').write_text(json.dumps(report,indent=2)+'\n',encoding='utf-8')
print(json.dumps(report,ensure_ascii=False))
