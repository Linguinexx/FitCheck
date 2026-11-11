"""Export a game-ready slide from the read-only model inbox."""

import hashlib
import json
from pathlib import Path

import bmesh
import bpy
from mathutils import Matrix


project = Path(__file__).resolve().parents[3]
art = Path(__file__).resolve().parent
output = project / "Assets/Resources/Garments/Streetwear"
inbox = project.parent / "refs roupas/modelos tenis/blue cloud sandal 3d model"
source = inbox / "blue_cloud_sandal_3d_model.fbx"
basecolor = inbox / "blue_cloud_sandal_3d_model.fbm/blue_cloud_sandal_3d_model_basecolor.jpg"
item_id = "AH_SHOES_02"
triangle_budget = 1900

bpy.ops.wm.read_factory_settings(use_empty=True)
bpy.ops.import_scene.fbx(filepath=str(source))
slide, = (obj for obj in bpy.context.selected_objects if obj.type == "MESH")
slide.name = "Slide Nuvem — AH_SHOES_02"
bpy.context.view_layer.objects.active = slide
matrix = slide.matrix_world.copy()
for vertex in slide.data.vertices:
    vertex.co = matrix @ vertex.co
slide.matrix_world = Matrix.Identity(4)

# The strap is towards the negative X end of this specific source.
# Use the same forward and sole clearance as the character's other footwear.
points = [vertex.co.copy() for vertex in slide.data.vertices]
minimum = [min(point[axis] for point in points) for axis in range(3)]
maximum = [max(point[axis] for point in points) for axis in range(3)]
scale = .82 / (maximum[0] - minimum[0])
center_x = (minimum[0] + maximum[0]) * .5
center_y = (minimum[1] + maximum[1]) * .5
for vertex in slide.data.vertices:
    x, y, z = vertex.co
    vertex.co = ((y - center_y) * scale, -(x - center_x) * scale + .07, .095 + (z - minimum[2]) * scale)

original_triangles = sum(len(face.vertices) - 2 for face in slide.data.polygons)
if original_triangles > triangle_budget:
    reduction = slide.modifiers.new("Redução para o jogo", "DECIMATE")
    reduction.ratio = triangle_budget / original_triangles
    bpy.ops.object.modifier_apply(modifier=reduction.name)
mesh = bmesh.new()
mesh.from_mesh(slide.data)
bmesh.ops.triangulate(mesh, faces=list(mesh.faces))
bmesh.ops.recalc_face_normals(mesh, faces=list(mesh.faces))
mesh.to_mesh(slide.data)
mesh.free()
for face in slide.data.polygons:
    face.use_smooth = True

image = bpy.data.images.load(str(basecolor), check_existing=False)
image.scale(512, 512)
image.name = "Slide Nuvem — basecolor 512"
image.file_format = "PNG"
image.filepath_raw = str(output / f"{item_id}.png")
image.save()
image.pack()
material = bpy.data.materials.new("Slide Nuvem")
material.use_nodes = True
bsdf = next(node for node in material.node_tree.nodes if node.type == "BSDF_PRINCIPLED")
bsdf.inputs["Roughness"].default_value = .82
albedo = material.node_tree.nodes.new("ShaderNodeTexImage")
albedo.image = image
material.node_tree.links.new(albedo.outputs["Color"], bsdf.inputs["Base Color"])
slide.data.materials.clear()
slide.data.materials.append(material)
for face in slide.data.polygons:
    face.material_index = 0


def unity(vector):
    return dict(x=round(vector.x, 6), y=round(vector.z, 6), z=round(-vector.y, 6))


part = dict(name="Slide Nuvem", side=0, vertices=[], normals=[], uv=[], triangles=[],
            color=dict(r=1, g=1, b=1, a=1), textured=True)
uv_layer = slide.data.uv_layers.active.data
for face in slide.data.polygons:
    for loop_index in face.loop_indices:
        vertex = slide.data.vertices[slide.data.loops[loop_index].vertex_index]
        uv = uv_layer[loop_index].uv
        part["triangles"].append(len(part["vertices"]))
        part["vertices"].append(unity(vertex.co))
        part["normals"].append(unity(vertex.normal))
        part["uv"].append(dict(x=round(uv.x, 6), y=round(uv.y, 6)))

asset = dict(id=item_id, slot=2, source="Blender — CloudSlide.blend", parts=[part])
(output / f"{item_id}.json").write_text(json.dumps(asset, separators=(",", ":")), encoding="utf-8")
for bank in (bpy.data.meshes, bpy.data.materials, bpy.data.images):
    for block in list(bank):
        if block.users == 0:
            bank.remove(block)
bpy.context.preferences.filepaths.save_version = 0
bpy.ops.wm.save_as_mainfile(filepath=str(art / "CloudSlide.blend"), compress=True)
triangles = len(part["triangles"]) // 3
assert triangles <= triangle_budget, triangles
report = dict(input=str(source.relative_to(project.parent)), source_sha256=hashlib.sha256(source.read_bytes()).hexdigest(),
              source_triangles=original_triangles, runtime_triangles_per_foot=triangles, texture_px=512)
(art / "manifest.json").write_text(json.dumps(report, indent=2) + "\n", encoding="utf-8")
print("CLOUD_SLIDE_READY", json.dumps(report), flush=True)
