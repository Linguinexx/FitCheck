"""Bake textile detail and painted folds while retaining each garment's artwork."""
import hashlib
import json
from pathlib import Path
import tempfile
import zipfile

import bpy
import numpy as np

project = Path(__file__).resolve().parents[3]
art = Path(__file__).resolve().parent
resources = project / "Assets/Resources/Garments"
archive_path = art / "OriginalTextures.zip"
manifest_path = art / "manifest.json"
previous = json.loads(manifest_path.read_text()) if manifest_path.exists() else []
previous_hashes = {row["texture"]: row["sha256"] for row in previous}
originals = {}
if archive_path.exists():
    with zipfile.ZipFile(archive_path) as archive:
        originals = {name: archive.read(name) for name in archive.namelist()}

paths = [p for folder in ("Streetwear", "Player") for p in (resources / folder).glob("*.png")
         if "_TOP_" in p.stem or "_BOTTOM" in p.stem or p.stem.endswith("Sleeve")]
paths += [resources / (name + ".png") for name in ("BaggyDenim01", "PlayerBlueDenim", "PlayerWashedDenim")]
paths += list((resources / "Finishes").glob("*.png"))
for path in paths:
    key = path.relative_to(resources).as_posix()
    data = path.read_bytes()
    if key not in originals and key.startswith("Finishes/") and previous_hashes.get(key) == hashlib.sha256(data).hexdigest():
        continue
    if key not in originals or hashlib.sha256(data).hexdigest() not in (previous_hashes.get(key), hashlib.sha256(originals[key]).hexdigest()):
        originals[key] = data
with zipfile.ZipFile(archive_path, "w", zipfile.ZIP_DEFLATED) as archive:
    for name, data in sorted(originals.items()):
        archive.writestr(name, data)

def load_pixels(path, size=None):
    image = bpy.data.images.load(str(path), check_existing=False)
    if size is not None:
        image.scale(size, size)
    width, height = image.size
    pixels = np.array(image.pixels[:], dtype=np.float32).reshape(height, width, 4)
    bpy.data.images.remove(image)
    return pixels

cloth = load_pixels(project / "Assets/Resources/Surfaces/Fabric030.jpg", 512)[..., :3].mean(axis=2)
felt = load_pixels(project / "Assets/Resources/Surfaces/Fabric084.jpg", 512)[..., :3].mean(axis=2)

def fabric_kind(key):
    name = Path(key).stem
    if key.startswith("Finishes/"):
        return name
    if name == "AH_TOP_03":
        return "Leather"
    if name in ("CG_TOP_01", "CG_TOP_04", "CG_BOTTOM_02", "CG_BOTTOM_03", "PLAYER_PINK_TOP_01", "PinkSleeve"):
        return "Jersey"
    if name in ("AH_TOP_04", "AH_BOTTOM_02", "CG_TOP_02", "CG_TOP_03", "PLAYER_RED_TOP_03", "PLAYER_STANDARD_TOP_03", "PLAYER_PINK_BOTTOM", "RedSleeve", "StandardSleeve"):
        return "Nylon"
    if name == "SB_TOP_03":
        return "Fleece"
    if "Denim" in name or name in ("CG_BOTTOM_04", "PLAYER_BLUE_BOTTOM"):
        return "Denim"
    if "BOTTOM" in name or name in ("SB_TOP_02", "SB_TOP_04"):
        return "Canvas"
    return "Cotton"

def geometry_path(path):
    if path.stem == "AH_TOP_02":
        return path.with_name("AH_TOP_01.json")
    if path.stem == "PlayerBlueDenim":
        return path.with_name("PlayerWashedDenim.json")
    return path.with_suffix(".json")

def uv_positions(path, width, height):
    yy, xx = np.mgrid[0:height, 0:width]
    positions = np.stack(((xx / width - .5) * .8, 1.4 + yy / height * .8, np.full_like(xx, -.25, dtype=float)), axis=2).astype(np.float32)
    model_path = geometry_path(path)
    if not model_path.exists():
        return positions, False
    model = json.loads(model_path.read_text(encoding="utf-8-sig"))
    parts = [p for p in model["parts"] if p.get("textured") or p.get("denim")]
    if not parts:
        return positions, False
    # The main cloth surface supplies the location of folds, rather than labels or hardware.
    part = max(parts, key=lambda p: len(p["triangles"]))
    vertices = np.array([[p["x"], p["y"], p["z"]] for p in part["vertices"]], dtype=np.float32)
    uv = np.array([[p["x"], p["y"]] for p in part["uv"]], dtype=np.float32)
    triangles = np.array(part["triangles"], dtype=np.int32).reshape(-1, 3)
    for triangle in triangles:
        coords = uv[triangle]
        for shift_y in range(int(np.floor(coords[:, 1].min())), int(np.floor(coords[:, 1].max()))+1):
            for shift_x in range(int(np.floor(coords[:, 0].min())), int(np.floor(coords[:, 0].max()))+1):
                q = (coords - (shift_x, shift_y)) * (width, height)
                lo = np.maximum(np.floor(q.min(axis=0)).astype(int), 0)
                hi = np.minimum(np.ceil(q.max(axis=0)).astype(int), (width-1, height-1))
                if np.any(lo > hi):
                    continue
                a, b, c = q
                denominator = (b[1]-c[1])*(a[0]-c[0])+(c[0]-b[0])*(a[1]-c[1])
                if abs(denominator) < 1e-7:
                    continue
                y, x = np.mgrid[lo[1]:hi[1]+1, lo[0]:hi[0]+1]
                x, y = x+.5, y+.5
                wa = ((b[1]-c[1])*(x-c[0])+(c[0]-b[0])*(y-c[1])) / denominator
                wb = ((c[1]-a[1])*(x-c[0])+(a[0]-c[0])*(y-c[1])) / denominator
                wc = 1-wa-wb
                inside = (wa >= -1e-5) & (wb >= -1e-5) & (wc >= -1e-5)
                points = wa[..., None]*vertices[triangle[0]]+wb[..., None]*vertices[triangle[1]]+wc[..., None]*vertices[triangle[2]]
                section = positions[lo[1]:hi[1]+1, lo[0]:hi[0]+1]
                section[inside] = points[inside]
    return positions, True

def gaussian(value, center, width):
    return np.exp(-((value-center)/width)**2)

def detail_map(kind, height, width):
    y, x = np.mgrid[0:height, 0:width]
    sample = (felt if kind in ("Fleece", "Knit") else cloth)[y*512//height, x*512//width]
    detail = np.clip((sample-sample.mean())*1.8, -.08, .08)
    if kind == "Denim":
        detail += np.where((x+2*y)%7 < 2, .025, -.010)
    elif kind == "Jersey":
        detail += np.where((x%5 < 2) & (y%5 < 2), -.085, .012)
    elif kind == "Knit":
        detail += np.sin((x*.9+np.sin(y*.3)))*.025
    elif kind == "Nylon":
        detail *= .32
    elif kind == "Leather":
        detail *= .55
        detail += np.sin(x*1.7+np.sin(y*1.3))*np.cos(y*1.8)*.016
    return detail

report = []
keys = sorted(set(originals) | {"Finishes/"+name+".png" for name in ("Jersey", "Nylon", "Leather", "Fleece", "Knit")})
with tempfile.TemporaryDirectory(prefix="fitcheck-cloth-") as temp:
    for key in keys:
        path = resources / key
        if key in originals:
            temporary = Path(temp) / "source.png"
            temporary.write_bytes(originals[key])
            base = load_pixels(temporary)
        else:
            base = np.ones((256, 256, 4), dtype=np.float32)
            base[..., :3] = .95
        height, width = base.shape[:2]
        kind = fabric_kind(key)
        fine = detail_map(kind, height, width)
        macro = np.zeros((height, width), dtype=np.float32)
        if not key.startswith("Finishes/"):
            p, mapped = uv_positions(path, width, height)
            x, y, z = p[..., 0], p[..., 1], p[..., 2]
            bottom = "BOTTOM" in path.stem or "Denim" in path.stem
            if bottom:
                macro += gaussian(y, .76, .19) * np.sin(x*38+z*13+y*24) * .075
                macro += gaussian(y, 1.06, .15) * np.cos(x*22-z*9) * .055
                macro -= gaussian(y, float(y.min())+.025, .035) * .10
            else:
                macro += gaussian(y, 1.52, .15) * np.sin(x*40+z*9+y*10) * .085
                macro -= gaussian(np.abs(x), .34, .085) * gaussian(y, 1.93, .16) * .13
                macro -= gaussian(y, 2.14, .035) * .07
                macro += gaussian(y, 1.77, .25) * np.sin(x*25+z*11) * .025
            if path.stem in ("BaggyDenim01", "CG_BOTTOM_04", "CG_BOTTOM_05", "PlayerBlueDenim", "PlayerWashedDenim"):
                macro *= .45
                fine *= .65
        else:
            mapped = False
        result = base.copy()
        result[..., :3] = np.clip(base[..., :3]*(1+fine[..., None]+macro[..., None]), 0, 1)
        if base[..., :3].mean() < .30:
            highlight = np.maximum(macro, 0)*.18
            result[..., :3] = np.clip(result[..., :3]+highlight[..., None], 0, 1)
        assert abs(float(result[..., :3].mean()-base[..., :3].mean())) < .10, key
        image = bpy.data.images.new(path.stem+" — textile finish", width, height)
        image.pixels[:] = result.ravel()
        image.file_format = "PNG"
        image.filepath_raw = str(path)
        image.save()
        bpy.data.images.remove(image)
        report.append(dict(texture=key, fabric=kind, width=width, height=height, geometry_mapped=mapped, sha256=hashlib.sha256(path.read_bytes()).hexdigest()))
        print("CLOTH_REFINED", key, kind, flush=True)
manifest_path.write_text(json.dumps(report, indent=2)+"\n")
print("CLOTH_FINISH_READY", len(report), flush=True)
