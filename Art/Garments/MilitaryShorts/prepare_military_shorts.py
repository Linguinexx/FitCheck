"""Long camouflage shorts fitted to the existing avatar and leg animation."""
import json
import math
from pathlib import Path

import bpy
from mathutils import Vector, noise

project = Path(__file__).resolve().parents[3]
art = Path(__file__).resolve().parent
output = project / "Assets/Resources/Garments/Streetwear"
identifier = "CG_BOTTOM_05"
bpy.ops.wm.read_factory_settings(use_empty=True)
source = json.loads((project / "Assets/Resources/Garments/BaggyDenim01.json").read_text(encoding="utf-8-sig"))
lowest = min(v["y"] for p in source["parts"] for v in p["vertices"])
palette = [(0.19, .21, .12), (.29, .32, .18), (.38, .39, .24), (.13, .15, .09)]
image = bpy.data.images.new("Camuflagem — trama 512", 512, 512)
pixels = []
for y in range(512):
    for x in range(512):
        warp = noise.noise_vector(Vector((x / 110, y / 110, 3)))
        value = noise.noise(Vector((x / 38 + warp.x * .8, y / 48 + warp.y * .8, 7)))
        overlap = noise.noise(Vector((x / 31 + warp.y + 23, y / 39 + warp.x + 11, 19)))
        index = 3 if value > .14 else 2 if overlap > .10 else 0 if overlap < -.23 else 1
        weave = .96 + ((x + y * 2) % 5) * .012
        pixels.extend([c * weave for c in palette[index]] + [1])
image.pixels[:] = pixels
image.file_format = "PNG"
image.filepath_raw = str(output / (identifier + ".png"))
image.save()
image.pack()
fabric = bpy.data.materials.new("Sarja camuflada")
fabric.use_nodes = True
shader = next(n for n in fabric.node_tree.nodes if n.type == "BSDF_PRINCIPLED")
shader.inputs["Roughness"].default_value = .86
texture = fabric.node_tree.nodes.new("ShaderNodeTexImage")
texture.image = image
fabric.node_tree.links.new(texture.outputs["Color"], shader.inputs["Base Color"])
olive = bpy.data.materials.new("Costura oliva")
olive.diffuse_color = (.32, .33, .16, 1)
yellow = bpy.data.materials.new("Estampa amarela")
yellow.diffuse_color = (.73, .74, .28, 1)
parts = []
surfaces = []

def export_mesh(obj, textured, color, side=0):
    mesh = obj.data
    mesh.calc_loop_triangles()
    uv = mesh.uv_layers.active
    part = dict(name=obj.name, side=side, vertices=[], normals=[], uv=[], triangles=[],
                textured=textured, color=dict(zip(("r", "g", "b", "a"), color)))
    matrix = obj.matrix_world
    normal_matrix = matrix.to_3x3().inverted().transposed()
    for triangle in mesh.loop_triangles:
        for loop_index in triangle.loops:
            index = mesh.loops[loop_index].vertex_index
            vertex = mesh.vertices[index]
            p = matrix @ vertex.co
            n = (normal_matrix @ vertex.normal).normalized()
            tex = uv.data[loop_index].uv if uv else (0, 0)
            part["triangles"].append(len(part["vertices"]))
            part["vertices"].append(dict(x=round(p.x, 6), y=round(p.z, 6), z=round(-p.y, 6)))
            part["normals"].append(dict(x=round(n.x, 6), y=round(n.z, 6), z=round(-n.y, 6)))
            part["uv"].append(dict(x=round(tex[0], 6), y=round(tex[1], 6)))
    parts.append(part)

for index, part in enumerate(source["parts"]):
    vertices = []
    for p in part["vertices"]:
        height = p["y"]
        if height < 1.18:
            height = .60 + (height - lowest) * .58 / (1.18 - lowest)
        vertices.append((p["x"], -p["z"], height))
    triangles = part["triangles"]
    mesh = bpy.data.meshes.new("Corte largo — " + str(index))
    mesh.from_pydata(vertices, [], [triangles[i:i+3] for i in range(0, len(triangles), 3)])
    uv = mesh.uv_layers.new()
    for loop in mesh.loops:
        tex = part["uv"][loop.vertex_index]
        uv.data[loop.index].uv = (tex["x"], tex["y"])
    textured = part.get("denim", False)
    for face in mesh.polygons:
        face.use_smooth = textured
    obj = bpy.data.objects.new("Bermuda — " + str(index), mesh)
    bpy.context.collection.objects.link(obj)
    mesh.materials.append(fabric if textured else olive)
    if textured:
        surfaces.append(obj)
    export_mesh(obj, textured, (1, 1, 1, 1) if textured else (.32, .33, .16, 1), part.get("side", 0))

# Elastic ridges follow the waistband without adding a rigid belt.
for height in (1.365, 1.395, 1.425):
    curve = bpy.data.curves.new("Friso do cós", "CURVE")
    curve.dimensions = "3D"
    curve.bevel_depth = .006
    curve.bevel_resolution = 0
    spline = curve.splines.new("POLY")
    spline.points.add(47)
    for i, p in enumerate(spline.points):
        angle = i * math.tau / 48
        p.co = (.42 * math.cos(angle), .255 * math.sin(angle), height, 1)
    spline.use_cyclic_u = True
    obj = bpy.data.objects.new("Cós elástico", curve)
    bpy.context.collection.objects.link(obj)
    bpy.context.view_layer.objects.active = obj
    obj.select_set(True)
    bpy.ops.object.convert(target="MESH")
    obj.data.materials.append(olive)
    export_mesh(obj, False, (.27, .29, .14, 1))
    obj.select_set(False)

vertices, faces = [], []
bpy.context.view_layer.update()
def printed_vertex(x, height):
    hits = []
    for surface in surfaces:
        hit, point, _, _ = surface.ray_cast(Vector((x, 1, height)), Vector((0, -1, 0)))
        if hit:
            hits.append(point.y)
    assert hits, (x, height)
    return x, max(hits) + .002, height

digits = [("111", "101", "101", "101", "111"), ("111", "001", "001", "010", "010")]
for digit, rows in enumerate(digits):
    for row, line in enumerate(rows):
        for col, value in enumerate(line):
            if value != "1":
                continue
            x = -.34 + (digit * 4 + col) * .027
            z = 1.20 - row * .027
            first = len(vertices)
            vertices += [printed_vertex(x, z), printed_vertex(x, z+.026), printed_vertex(x+.026, z+.026), printed_vertex(x+.026, z)]
            faces.append((first, first+1, first+2, first+3))
mesh = bpy.data.meshes.new("07")
mesh.from_pydata(vertices, [], faces)
obj = bpy.data.objects.new("Estampa 07", mesh)
bpy.context.collection.objects.link(obj)
mesh.materials.append(yellow)
export_mesh(obj, False, (.73, .74, .28, 1))
asset = dict(id=identifier, slot=1, source="Blender — MilitaryShorts.blend", parts=parts)
(output / (identifier + ".json")).write_text(json.dumps(asset, separators=(",", ":")), encoding="utf-8")
triangles = sum(len(p["triangles"]) // 3 for p in parts)
assert triangles < 7500
bpy.context.preferences.filepaths.save_version = 0
bpy.ops.wm.save_as_mainfile(filepath=str(art / "MilitaryShorts.blend"), compress=True)
print("MILITARY_SHORTS_READY", triangles, flush=True)
