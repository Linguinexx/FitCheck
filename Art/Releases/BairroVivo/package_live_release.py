"""Package the current players without development labels or local saves."""
from pathlib import Path
from zipfile import ZipFile, ZIP_DEFLATED
import hashlib
import json
import shutil

root = Path(__file__).resolve().parent.parent
project = root / 'FitCheck'
source = root / 'tools/.web05-build-project/Builds'
notes = project / 'Art/Releases/BairroVivo'
notes.mkdir(parents=True, exist_ok=True)
reports = []
for platform in ['Windows', 'Web']:
    src = source / platform
    dst = project / 'Builds' / platform
    assert src.is_dir(), src
    assert not dst.exists(), f'Existing delivery must be preserved before packaging: {dst}'
    if platform == 'Windows':
        assert (src / 'FitCheck.exe').is_file()
        dst.mkdir(parents=True)
        for item in src.iterdir():
            if item.name in ('FitCheck_Data', 'MonoBleedingEdge', 'D3D12'):
                shutil.copytree(item, dst / item.name)
            elif item.suffix.lower() in ('.exe', '.dll'):
                shutil.copy2(item, dst / item.name)
    else:
        assert (src / 'index.html').is_file()
        shutil.copytree(src, dst)
        for name in ['serve.py', 'ABRIR-WEB.bat']:
            shutil.copy2(project / 'Art/Releases/Web09' / name, dst / name)
    instruction = f'FIT CHECK — {platform.upper()}\nAtualização de 13/11/2026\n\n'
    instruction += ('Extraia o ZIP inteiro e abra FitCheck.exe. Mantenha as pastas e DLLs junto do executável.\n' if platform == 'Windows' else 'itch.io: envie FitCheck-Web.zip como HTML e habilite jogar no navegador. O index.html está na raiz. Não é necessário descompactar para enviar.\nLocal: use ABRIR-WEB.bat ou python serve.py. Não abra index.html diretamente pelo Explorador.\n')
    instruction += '\nWASD: andar; Shift: correr; mouse: olhar; E: interagir; F: personalizar pelo catálogo; P: pausar; Tab: soltar/capturar cursor. Emotes 1–5, 0 encerra.\n'
    instruction += ('Esc também abre a pausa no Windows.\n' if platform == 'Windows' else 'Na Web, Esc é reservado ao navegador; use P para pausar.\n')
    instruction += '\nComece em casa. Caminhe até a loja e abra o expediente no balcão. Até três saves independentes, sete dias e 21 encontros. Seus saves anteriores são preservados; esta entrega não inclui progresso pronto.\n'
    instruction += '\nNovo: comentários no FitFeed, reações aos emotes, bairro com preparativos por dia, pets com olhos/orelhas animados e gato Sphynx, pedidos especiais e foto coletiva final.\n'
    instruction += '\nEm save já concluído, olhe para o porta-retrato na mesa do quarto e pressione E para gerar a lembrança.\n'
    (dst / 'LEIA-ME.txt').write_text(instruction, encoding='utf-8')
    (notes / f'LEIA-ME-{platform}.txt').write_text(instruction, encoding='utf-8')
    for name in ['RELATO-DE-BUG.txt', 'ROTEIRO.md']:
        shutil.copy2(notes / name, dst / name)
    files = sorted(item for item in dst.rglob('*') if item.is_file())
    assert not any(any(word in part.lower() for word in ['donotship', 'dontship', 'session', '__pycache__', 'credential']) for item in files for part in item.relative_to(dst).parts)
    size = sum(item.stat().st_size for item in files)
    assert size < 200_000_000, (platform, size)
    archive = project / 'Builds' / f'FitCheck-{platform}.zip'
    assert not archive.exists(), archive
    with ZipFile(archive, 'w', ZIP_DEFLATED, compresslevel=6) as zip_file:
        for item in files:
            zip_file.write(item, item.relative_to(dst).as_posix())
    with ZipFile(archive) as zip_file:
        assert zip_file.testzip() is None
        assert ('FitCheck.exe' if platform == 'Windows' else 'index.html') in zip_file.namelist()
    assert archive.stat().st_size < 200_000_000
    sha = hashlib.sha256(archive.read_bytes()).hexdigest()
    archive.with_suffix('.zip.sha256').write_text(sha + '  ' + archive.name + '\n')
    reports.append(dict(platform=platform, unity='6000.6.7f1', folder_bytes=size,
                        zip_bytes=archive.stat().st_size, sha256=sha, archive=str(archive.relative_to(project)),
                        files=[dict(path=str(item.relative_to(dst)), bytes=item.stat().st_size) for item in files]))
(notes / 'delivery.json').write_text(json.dumps(reports, indent=2) + '\n', encoding='utf-8')
print(json.dumps([{key: value for key, value in report.items() if key != 'files'} for report in reports], indent=2))
