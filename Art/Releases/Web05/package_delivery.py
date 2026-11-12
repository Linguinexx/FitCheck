"""Package only the browser player; never include browser profiles or saves."""
import hashlib
import json
from pathlib import Path
from zipfile import ZipFile, ZIP_DEFLATED

release = Path(__file__).resolve().parent
project = release.parents[2]
player = project / "Builds" / "Web-Test05"
assert (player / "index.html").is_file()
assert (player / "Build").is_dir()
for name in ("serve.py", "ABRIR-WEB.bat", "LEIA-ME.txt", "RELATO-DE-BUG.txt", "ROTEIRO-DE-TESTE.md"):
    (player / name).write_bytes((release / name).read_bytes())
files = sorted(p for p in player.rglob("*") if p.is_file())
assert not any("__pycache__" in p.parts for p in files)
archive = project / "Builds" / "FitCheck-Web-Test05.zip"
with ZipFile(archive, "w", ZIP_DEFLATED, compresslevel=6) as z:
    for p in files:
        z.write(p, p.relative_to(player).as_posix())
with ZipFile(archive) as z:
    assert z.testzip() is None
    assert "index.html" in z.namelist()
report = {
    "platform": "WebGL", "unity": "6000.6.7f1",
    "folder_bytes": sum(p.stat().st_size for p in files),
    "zip_bytes": archive.stat().st_size,
    "zip_sha256": hashlib.sha256(archive.read_bytes()).hexdigest(),
    "files": [{"path": p.relative_to(player).as_posix(),
               "bytes": p.stat().st_size,
               "sha256": hashlib.sha256(p.read_bytes()).hexdigest()} for p in files]
}
assert report["folder_bytes"] < 200_000_000
assert report["zip_bytes"] < 200_000_000
(release / "delivery.json").write_text(json.dumps(report, indent=2), encoding="utf-8")
print(json.dumps({k: v for k, v in report.items() if k != "files"}, indent=2))
