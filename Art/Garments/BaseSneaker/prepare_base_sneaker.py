"""Optimize the supplied everyday shoe. Originals remain read only."""
import bpy, bmesh, json, hashlib
from pathlib import Path
from mathutils import Vector, Matrix

ROOT=Path(__file__).resolve().parents[4]
ART=ROOT/'FitCheck/Art/Garments/BaseSneaker'
OUT=ROOT/'FitCheck/Assets/Resources/Garments/Civilian'
REF=ROOT/'refs roupas/modelos tenis/Base sneaker'
ART.mkdir(parents=True,exist_ok=True); OUT.mkdir(parents=True,exist_ok=True)
source=REF/'Base_sneaker.fbx'
base=next(p for p in REF.rglob('*') if p.is_file() and 'basecolor' in p.name.lower())
scene=bpy.data.scenes.new('Fit Check — everyday sneaker'); bpy.context.window.scene=scene
bpy.ops.import_scene.fbx(filepath=str(source))
meshes=[o for o in bpy.context.selected_objects if o.type=='MESH']
for o in meshes:
    matrix=o.matrix_world.copy()
    for v in o.data.vertices: v.co=matrix@v.co
    o.matrix_world=Matrix.Identity(4)
bpy.ops.object.select_all(action='DESELECT')
for o in meshes: o.select_set(True)
bpy.context.view_layer.objects.active=meshes[0]
if len(meshes)>1: bpy.ops.object.join()
o=bpy.context.object; o.name='Everyday sneaker — CIV_SHOES_BASE'
points=[v.co.copy() for v in o.data.vertices]
lo=Vector(tuple(min(p[i] for p in points) for i in range(3)))
hi=Vector(tuple(max(p[i] for p in points) for i in range(3))); size=hi-lo
positive=sorted(p.z for p in points if p.x>lo.x+size.x*.78)
negative=sorted(p.z for p in points if p.x<lo.x+size.x*.22)
toe=1 if positive[int(len(positive)*.9)]<negative[int(len(negative)*.9)] else -1
for v in o.data.vertices:
    p=v.co.copy()
    v.co=(-toe*(p.y-(lo.y+hi.y)*.5)*.385/size.y,toe*(p.x-(lo.x+hi.x)*.5)*.80/size.x+.10,.095+(p.z-lo.z)*.32/size.z)
before=sum(len(p.vertices)-2 for p in o.data.polygons)
if before>1200:
    modifier=o.modifiers.new('Everyday NPC budget','DECIMATE'); modifier.ratio=1200/before;modifier.use_collapse_triangulate=True
    bpy.ops.object.modifier_apply(modifier=modifier.name)
bm=bmesh.new();bm.from_mesh(o.data);bmesh.ops.triangulate(bm,faces=list(bm.faces));bmesh.ops.recalc_face_normals(bm,faces=bm.faces);bm.to_mesh(o.data);bm.free()
for p in o.data.polygons:p.use_smooth=True
image=bpy.data.images.load(str(base),check_existing=False);image.scale(512,512);image.name='Everyday shoe 512';image.file_format='PNG';image.filepath_raw=str(OUT/'CIV_SHOES_BASE.png');image.save();image.pack()
mat=bpy.data.materials.new('PRINT_CIV_SHOES_BASE');mat.use_nodes=True;mat.diffuse_color=(1,1,1,1)
mat.node_tree.nodes.clear();bs=mat.node_tree.nodes.new('ShaderNodeBsdfPrincipled');bs.inputs['Roughness'].default_value=.85
output=mat.node_tree.nodes.new('ShaderNodeOutputMaterial');mat.node_tree.links.new(bs.outputs['BSDF'],output.inputs['Surface'])
tex=mat.node_tree.nodes.new('ShaderNodeTexImage');tex.image=image;mat.node_tree.links.new(tex.outputs['Color'],bs.inputs['Base Color'])
o.data.materials.clear();o.data.materials.append(mat)
for p in o.data.polygons:p.material_index=0
bpy.ops.object.select_all(action='DESELECT');o.select_set(True)
bpy.ops.export_scene.fbx(filepath=str(OUT/'CIV_SHOES_BASE.fbx'),use_selection=True,object_types={'MESH'},add_leaf_bones=False,bake_anim=False,axis_forward='-Z',axis_up='Y',path_mode='AUTO')
# Emit canonical runtime streams directly: Blender Z-up -> Unity Y-up.
def unity(p):return {'x':p.x,'y':p.z,'z':-p.y}
part={'name':'Tênis cotidiano','side':0,'vertices':[],'normals':[],'uv':[],'triangles':[],'color':{'r':1,'g':1,'b':1,'a':1},'textured':True}
for p in o.data.polygons:
    for loop_index in p.loop_indices:
        loop=o.data.loops[loop_index];v=o.data.vertices[loop.vertex_index];uv=o.data.uv_layers.active.data[loop_index].uv
        part['triangles'].append(len(part['vertices']));part['vertices'].append(unity(v.co));part['normals'].append(unity(v.normal));part['uv'].append({'x':uv.x,'y':uv.y})
asset={'id':'CIV_SHOES_BASE','slot':2,'source':'Blender — BaseSneaker.blend','parts':[part]}
(OUT/'CIV_SHOES_BASE.json').write_text(json.dumps(asset,separators=(',',':')),encoding='utf-8')
report={'input':str(source.relative_to(ROOT)),'sha256':hashlib.sha256(source.read_bytes()).hexdigest(),'input_triangles':before,'runtime_triangles':len(part['triangles'])//3,'texture':512,'toe_sign':toe,'source_bounds':list(size)}
(ART/'manifest.json').write_text(json.dumps(report,indent=2),encoding='utf-8')
for bank in [bpy.data.meshes,bpy.data.materials,bpy.data.images]:
    for item in list(bank):
        if item.users==0:bank.remove(item)
world=bpy.data.worlds.new('Review');scene.world=world;world.use_nodes=True
next(n for n in world.node_tree.nodes if n.type=='BACKGROUND').inputs[0].default_value=(.1,.11,.13,1)
def aim(obj,target):obj.rotation_euler=(Vector(target)-obj.location).to_track_quat('-Z','Y').to_euler()
for loc in [(1,2,3),(-2,-1,2)]:
    data=bpy.data.lights.new('Softbox','AREA');data.energy=200;data.size=3;light=bpy.data.objects.new('Softbox',data);scene.collection.objects.link(light);light.location=loc;aim(light,(0,0,.2))
data=bpy.data.cameras.new('Review');camera=bpy.data.objects.new('Review',data);scene.collection.objects.link(camera);camera.location=(1.4,2,1.1);aim(camera,(0,.05,.22));data.type='ORTHO';data.ortho_scale=1.1;scene.camera=camera
scene.render.engine='CYCLES';scene.cycles.samples=20;scene.render.resolution_x=900;scene.render.resolution_y=600;scene.render.resolution_percentage=100;scene.render.filepath=str(ART/'preview.png')
bpy.ops.wm.save_as_mainfile(filepath=str(ART/'BaseSneaker.blend'),compress=True);bpy.ops.render.render(write_still=True)
print('BASE_SNEAKER_READY',json.dumps(report),flush=True)
