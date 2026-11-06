"""Normalize user FBX copies in Blender; original reference files are read only."""
import bpy,bmesh,json,math,hashlib,sys
from pathlib import Path
from mathutils import Vector,Matrix
ROOT=Path(__file__).resolve().parents[3];REF=ROOT.parent/'refs roupas/modelos tenis';OUT=ROOT/'Assets/Resources/Garments/Streetwear';ART=Path(__file__).resolve().parent

blue_only='--blue-only' in sys.argv
models=[('blue sneaker 2.0','AH_SHOES_01',.410,.345),('Fire sneaker','SB_SHOES_01',.440,.375),('Green sneaker','CG_SHOES_01',.440,.370)]
if blue_only:
    bpy.ops.wm.open_mainfile(filepath=str(ART/'SuppliedSneakerCollection.blend'))
    scene=bpy.context.scene
    old=next(obj for obj in scene.objects if obj.type=='MESH' and 'AH_SHOES_01' in obj.name)
    bpy.data.objects.remove(old,do_unlink=True)
    report=json.loads((ART/'manifest.json').read_text(encoding='utf-8'))[1:]
    models=models[:1]
else:
    scene=bpy.data.scenes.new('Fit Check — supplied sneaker lowpoly');bpy.context.window.scene=scene
    report=[]
for index,(folder,ident,width,height) in enumerate(models):
    source=next((REF/folder).glob('*.fbx'));base=next(p for p in (REF/folder).rglob('*') if p.is_file() and 'basecolor' in p.name.lower())
    bpy.ops.object.select_all(action='DESELECT');bpy.ops.import_scene.fbx(filepath=str(source));meshes=[o for o in bpy.context.selected_objects if o.type=='MESH']
    for o in meshes:
        matrix=o.matrix_world.copy()
        for v in o.data.vertices:v.co=matrix@v.co
        o.matrix_world=Matrix.Identity(4)
    bpy.ops.object.select_all(action='DESELECT')
    for o in meshes:o.select_set(True)
    bpy.context.view_layer.objects.active=meshes[0];bpy.ops.object.join();o=bpy.context.object;o.name='Tênis — '+ident+' — supplied optimized'
    points=[v.co.copy() for v in o.data.vertices];lo=Vector(tuple(min(p[i] for p in points) for i in range(3)));hi=Vector(tuple(max(p[i] for p in points) for i in range(3)));size=hi-lo
    # Inputs use X for length, Y for width and Z for vertical. Locate the lower toe end.
    positive=sorted(p.z for p in points if p.x>lo.x+size.x*.78);negative=sorted(p.z for p in points if p.x<lo.x+size.x*.22)
    top_pos=positive[int(len(positive)*.9)];top_neg=negative[int(len(negative)*.9)];toe=1 if top_pos<top_neg else -1
    for v in o.data.vertices:
        p=v.co.copy();v.co=(-toe*(p.y-(lo.y+hi.y)*.5)*width/size.y,toe*(p.x-(lo.x+hi.x)*.5)*.85/size.x+.10,.095+(p.z-lo.z)*height/size.z)
    before=sum(len(p.vertices)-2 for p in o.data.polygons)
    modifier=o.modifiers.new('Game-ready reduction, UV retained','DECIMATE');modifier.ratio=2500/before;modifier.use_collapse_triangulate=True
    bpy.context.view_layer.objects.active=o;bpy.ops.object.modifier_apply(modifier=modifier.name)
    bm=bmesh.new();bm.from_mesh(o.data);bmesh.ops.triangulate(bm,faces=list(bm.faces));bmesh.ops.recalc_face_normals(bm,faces=bm.faces);bm.to_mesh(o.data);bm.free()
    for poly in o.data.polygons:poly.use_smooth=True
    after=sum(len(p.vertices)-2 for p in o.data.polygons);assert after<=3000,(ident,after)
    image=bpy.data.images.load(str(base),check_existing=False);image.scale(512,512)
    image.name=ident+' — supplied basecolor 512';image.file_format='PNG';image.filepath_raw=str(OUT/(ident+'.png'));image.save();image.pack()
    material=bpy.data.materials.new('PRINT_'+ident);material.use_nodes=True;material.diffuse_color=(1,1,1,1);bs=next(node for node in material.node_tree.nodes if node.type=='BSDF_PRINCIPLED');bs.inputs['Roughness'].default_value=.85
    tex=material.node_tree.nodes.new('ShaderNodeTexImage');tex.image=image;material.node_tree.links.new(tex.outputs['Color'],bs.inputs['Base Color'])
    o.data.materials.clear();o.data.materials.append(material)
    for poly in o.data.polygons:poly.material_index=0
    bpy.ops.object.select_all(action='DESELECT');o.select_set(True)
    bpy.ops.export_scene.fbx(filepath=str(OUT/(ident+'.fbx')),use_selection=True,object_types={'MESH'},add_leaf_bones=False,bake_anim=False,axis_forward='-Z',axis_up='Y',path_mode='AUTO')
    entry={'id':ident,'input':str(source.relative_to(REF)),'sha256':hashlib.sha256(source.read_bytes()).hexdigest(),'input_triangles':before,'runtime_triangles':after,'texture':512,'toe_sign':toe,'toe_height_positive':top_pos,'toe_height_negative':top_neg,'target_width':width,'target_height':height}
    if blue_only:report.insert(0,entry)
    else:report.append(entry)
    print('SUPPLIED_SNEAKER_READY',ident,before,'->',after,'toe',toe,flush=True)
    o.location.x=(index-1)*.95
# Remove unused high-poly datablocks/materials and original-resolution images from the editable game copy.
for bank in [bpy.data.meshes,bpy.data.materials,bpy.data.images]:
    for item in list(bank):
        if item.users==0:bank.remove(item)
if not blue_only:
    world=bpy.data.worlds.new('Sneaker review');scene.world=world;world.use_nodes=True;world.node_tree.nodes['Background'].inputs[0].default_value=(.08,.09,.11,1)
    def aim(o,p):o.rotation_euler=(Vector(p)-o.location).to_track_quat('-Z','Y').to_euler()
    for loc in [(1,2,4),(-3,-1,3)]:
        data=bpy.data.lights.new('Softbox','AREA');data.energy=400;data.size=4;o=bpy.data.objects.new('Softbox',data);scene.collection.objects.link(o);o.location=loc;aim(o,(0,0,.2))
    data=bpy.data.cameras.new('Review');camera=bpy.data.objects.new('Review',data);scene.collection.objects.link(camera);camera.location=(2.7,4,2.4);aim(camera,(0,.1,.22));data.type='ORTHO';data.ortho_scale=3.5;scene.camera=camera
scene.render.engine='CYCLES';scene.cycles.samples=24;scene.render.resolution_x=1536;scene.render.resolution_y=768;scene.render.resolution_percentage=100;scene.render.filepath=str(ART/'optimized-preview.png')
bpy.context.preferences.filepaths.save_version=0
bpy.ops.wm.save_as_mainfile(filepath=str(ART/'SuppliedSneakerCollection.blend'),compress=True);bpy.ops.render.render(write_still=True)
(ART/'manifest.json').write_text(json.dumps(report,indent=2),encoding='utf-8')
