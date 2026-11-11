"""Prepare copies of user models. The model inbox is read only."""

import hashlib
import json
import math
from pathlib import Path

import bmesh
import bpy
from mathutils import Matrix, Vector


project = Path(__file__).resolve().parents[3]
art = Path(__file__).resolve().parent
inbox = project.parent / "refs roupas/modelos tenis"
output = project / "Assets/Resources/Garments/Streetwear"


def fit_clog(point):
    return Vector((-point.x * .82, -point.y * .82 + .07, .095 + point.z * .82))


def glasses_frame(points):
    front = [p for p in points if p.y < -.20]
    back = [p for p in points if p.y > .20]
    def center(region):
        return Vector(tuple((min(p[i] for p in region) + max(p[i] for p in region)) * .5 for i in range(3)))
    a, b = center(front), center(back)
    angle = math.atan2(a.z - b.z, b.y - a.y)
    c, s = math.cos(angle), math.sin(angle)
    return c, s, a.y * c - a.z * s, a.y * s + a.z * c


def fit_glasses(point, frame):
    # A rigid rotation and uniform scale preserve the original lenses and arms.
    c, s, front_depth, front_height = frame
    depth = (point.y * c - point.z * s - front_depth) * 1.12 - .64
    height = (point.y * s + point.z * c - front_height) * 1.12 + .10
    return Vector((-point.x * 1.12, -depth, height))


def unity(vector):
    return dict(x=round(vector.x, 6), y=round(vector.z, 6), z=round(-vector.y, 6))


bpy.ops.wm.read_factory_settings(use_empty=True)
report = []
models = (
    ("red clog 3d model", "AH_SHOES_03", 2, 2200, "Clog Brasa"),
    ("futuristic sunglasses 3d model", "SB_HEAD_03", 3, 1800, "Óculos Prisma"),
)
for index, (folder, identifier, slot, budget, label) in enumerate(models):
    bpy.ops.object.select_all(action="DESELECT")
    source = next((inbox / folder).glob("*.fbx"))
    basecolor = next((inbox / folder).rglob("*basecolor*"))
    bpy.ops.import_scene.fbx(filepath=str(source))
    obj, = (item for item in bpy.context.selected_objects if item.type == "MESH")
    bpy.context.view_layer.objects.active = obj
    obj.name = label
    matrix = obj.matrix_world.copy()
    points = [matrix @ vertex.co for vertex in obj.data.vertices]
    frame = glasses_frame(points) if slot == 3 else None
    for vertex, point in zip(obj.data.vertices, points):
        if slot == 2:
            vertex.co = fit_clog(point)
        else:
            vertex.co = fit_glasses(point, frame)
    obj.matrix_world = Matrix.Identity(4)
    before = sum(len(face.vertices) - 2 for face in obj.data.polygons)
    reduction = obj.modifiers.new("Orçamento de jogo", "DECIMATE")
    reduction.ratio = budget / before
    bpy.ops.object.modifier_apply(modifier=reduction.name)
    mesh = bmesh.new()
    mesh.from_mesh(obj.data)
    bmesh.ops.triangulate(mesh, faces=list(mesh.faces))
    bmesh.ops.recalc_face_normals(mesh, faces=list(mesh.faces))
    mesh.to_mesh(obj.data)
    mesh.free()
    for face in obj.data.polygons:
        face.use_smooth = True

    image = bpy.data.images.load(str(basecolor), check_existing=False)
    image.scale(512, 512)
    image.name = label + " — 512"
    image.file_format = "PNG"
    image.filepath_raw = str(output / (identifier + ".png"))
    image.save()
    image.pack()
    material = bpy.data.materials.new(label)
    material.use_nodes = True
    bsdf = next(node for node in material.node_tree.nodes if node.type == "BSDF_PRINCIPLED")
    bsdf.inputs["Roughness"].default_value = .75
    texture = material.node_tree.nodes.new("ShaderNodeTexImage")
    texture.image = image
    material.node_tree.links.new(texture.outputs["Color"], bsdf.inputs["Base Color"])
    obj.data.materials.clear()
    obj.data.materials.append(material)
    for face in obj.data.polygons:
        face.material_index = 0
    part = dict(name=label, side=0, vertices=[], normals=[], uv=[], triangles=[],
                color=dict(r=1, g=1, b=1, a=1), textured=True)
    uv_layer = obj.data.uv_layers.active.data
    for face in obj.data.polygons:
        for loop_index in face.loop_indices:
            vertex = obj.data.vertices[obj.data.loops[loop_index].vertex_index]
            uv = uv_layer[loop_index].uv
            part["triangles"].append(len(part["vertices"]))
            part["vertices"].append(unity(vertex.co))
            part["normals"].append(unity(vertex.normal))
            part["uv"].append(dict(x=round(uv.x, 6), y=round(uv.y, 6)))
    asset = dict(id=identifier, slot=slot, source="Blender — SuppliedWardrobe.blend", parts=[part])
    (output / (identifier + ".json")).write_text(json.dumps(asset, separators=(",", ":")), encoding="utf-8")
    triangles = len(part["triangles"]) // 3
    assert triangles <= budget, (identifier, triangles)
    report.append(dict(id=identifier, input=str(source.relative_to(project.parent)),
                       sha256=hashlib.sha256(source.read_bytes()).hexdigest(),
                       source_triangles=before, runtime_triangles=triangles, texture=512))
    obj.location.x = index * 3.4
    if slot == 3:
        obj.location.z = 2.8
    print("WARDROBE_MODEL_READY", identifier, triangles, flush=True)

# The pink option shares every vertex, UV and detail with the approved blue baggy.
baggy_path = project / "Assets/Resources/Garments/BaggyDenim01.json"
baggy = json.loads(baggy_path.read_text(encoding="utf-8-sig"))
baggy.update(id="CG_BOTTOM_04", slot=1, source="Blender — BaggyDenim01.blend, pink denim variant")
image = bpy.data.images.load(str(project / "Assets/Resources/Garments/BaggyDenim01.png"), check_existing=False)
pixels = list(image.pixels[:])
for i in range(0, len(pixels), 4):
    shade = (.299 * pixels[i] + .587 * pixels[i+1] + .114 * pixels[i+2]) / .25
    pixels[i:i+3] = [min(1, shade * channel) for channel in (.82, .36, .53)]
image.pixels[:] = pixels
image.name = "Jeans Rosa — mesmo denim da baggy azul"
image.file_format = "PNG"
image.filepath_raw = str(output / "CG_BOTTOM_04.png")
image.save()
image.pack()
pink = bpy.data.materials.new("Denim rosa")
pink.use_nodes = True
shader = next(n for n in pink.node_tree.nodes if n.type == "BSDF_PRINCIPLED")
shader.inputs["Roughness"].default_value = .8
texture = pink.node_tree.nodes.new("ShaderNodeTexImage")
texture.image = image
pink.node_tree.links.new(texture.outputs["Color"], shader.inputs["Base Color"])
details = {}
for part in baggy["parts"]:
    mesh = bpy.data.meshes.new(part["name"])
    vertices = [(p["x"], -p["z"], p["y"]) for p in part["vertices"]]
    triangles = part["triangles"]
    mesh.from_pydata(vertices, [], [triangles[i:i+3] for i in range(0, len(triangles), 3)])
    uv = mesh.uv_layers.new()
    for loop in mesh.loops:
        tex = part["uv"][loop.vertex_index]
        uv.data[loop.index].uv = tex["x"], tex["y"]
    obj = bpy.data.objects.new("Jeans Rosa — " + part["name"], mesh)
    bpy.context.collection.objects.link(obj)
    obj.location.x = 1.7
    if part.get("denim") or part.get("textured"):
        material = pink
    else:
        color = part["color"]
        key = tuple(color[k] for k in ("r", "g", "b", "a"))
        if key not in details:
            material = bpy.data.materials.new("Detalhe da baggy")
            material.diffuse_color = key
            details[key] = material
        material = details[key]
    mesh.materials.append(material)
(output / "CG_BOTTOM_04.json").write_text(json.dumps(baggy, separators=(",", ":")), encoding="utf-8")
report.append(dict(id="CG_BOTTOM_04", input=str(baggy_path.relative_to(project.parent)),
                   sha256=hashlib.sha256(baggy_path.read_bytes()).hexdigest(),
                   runtime_triangles=sum(len(p["triangles"]) // 3 for p in baggy["parts"]), texture=512))

for bank in (bpy.data.meshes, bpy.data.materials, bpy.data.images):
    for block in list(bank):
        if block.users == 0:
            bank.remove(block)
bpy.context.preferences.filepaths.save_version = 0
bpy.ops.wm.save_as_mainfile(filepath=str(art / "SuppliedWardrobe.blend"), compress=True)
(art / "manifest.json").write_text(json.dumps(report, indent=2) + "\n", encoding="utf-8")
