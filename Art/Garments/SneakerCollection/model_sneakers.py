"""Blender 5 authoring source. Rebuilds the three catalog shoes, not the avatars."""
import bpy, bmesh, math, json
from mathutils import Vector
from pathlib import Path
ROOT=Path(__file__).resolve().parents[3]
OUT=ROOT/'Assets/Resources/Garments/Streetwear'
ART=Path(__file__).resolve().parent
OUT.mkdir(parents=True,exist_ok=True)
scene=bpy.data.scenes.new('Fit Check — Sneaker Collection');bpy.context.window.scene=scene
current=None;material=None;objects=[]
def xyz(p):return (p[0],-p[2],p[1])
def atlas_uv(tile,u,v):return ((tile%4+.025+.95*u)/4,(tile//4+.025+.95*v)/4)
def mesh(name,vertices,faces,tile=0,smooth=False,uv_coords=None):
    data=bpy.data.meshes.new(name);data.from_pydata([xyz(p) for p in vertices],[],faces);data.update()
    o=bpy.data.objects.new(name,data);current.objects.link(o);data.materials.append(material);objects.append(o)
    layer=data.uv_layers.new(name='Sneaker atlas 512')
    for poly in data.polygons:
        poly.use_smooth=smooth
        for k,li in enumerate(poly.loop_indices):
            p=vertices[data.loops[li].vertex_index]
            if uv_coords:u,v=uv_coords[data.loops[li].vertex_index]
            elif len(poly.loop_indices)==4:u,v=[(0,0),(1,0),(1,1),(0,1)][k]
            else:u,v=(p[2]+.57)/.90,(p[1]-.1)/.36
            layer.data[li].uv=atlas_uv(tile,max(0,min(1,u)),max(0,min(1,v)))
    return o
def tube(name,points,radius,tile=2,sides=5):
    v=[];f=[]
    for i,p in enumerate(points):
        tangent=(Vector(points[min(i+1,len(points)-1)])-Vector(points[max(0,i-1)])).normalized()
        a=tangent.cross(Vector((0,1,0))).normalized()
        if a.length<.1:a=Vector((1,0,0))
        b=tangent.cross(a).normalized()
        for k in range(sides):v.append(tuple(Vector(p)+radius*(a*math.cos(k*math.tau/sides)+b*math.sin(k*math.tau/sides))))
    for i in range(len(points)-1):
        for j in range(sides):a=i*sides+j;b=i*sides+(j+1)%sides;f.append((a,b,b+sides,a+sides))
    f.extend([tuple(reversed(range(sides))),tuple((len(points)-1)*sides+i for i in range(sides))])
    return mesh(name,v,f,tile,True)
def palette(style):
    if style==0:return [( .87,.86,.79),(.22,.36,.52),(.19,.31,.46),(.88,.83,.68),(.13,.24,.38),(.20,.30,.43),(.055,.07,.10),(.035,.04,.05),(.75,.78,.75),(.68,.72,.73),(.61,.51,.30),(.27,.39,.53),(.94,.90,.76),(.12,.20,.31),(.31,.42,.52),(.85,.78,.56)]
    if style==1:return [(.11,.14,.13),(.19,.23,.22),(.77,.32,.06),(.19,.22,.18),(.08,.10,.095),(1,.42,.055),(.055,.07,.065),(.02,.03,.025),(.50,.42,.25),(.09,.12,.115),(.45,.39,.23),(.13,.19,.17),(.56,.39,.19),(.065,.09,.08),(.24,.28,.25),(.78,.42,.13)]
    return [(.56,.65,.12),(.63,.73,.19),(.12,.44,.51),(.48,.55,.10),(.10,.30,.22),(.66,.73,.25),(.14,.20,.065),(.10,.20,.10),(.65,.71,.25),(.54,.62,.20),(.48,.56,.29),(.56,.65,.13),(.61,.74,.24),(.28,.39,.07),(.49,.60,.12),(.62,.73,.30)]
def make_atlas(ident,style):
    image=bpy.data.images.new(ident+' — 512 textile atlas',width=512,height=512,alpha=True);pixels=[];colors=palette(style)
    for y in range(512):
        for x in range(512):
            tile=x//128+(y//128)*4;u=(x%128)/127;v=(y%128)/127;base=colors[tile]
            noise=(((x*73+y*151+x*y*7)%97)/96-.5)*.035
            weave=math.sin((x+y*.72)*2.1)*.014+math.sin((x-y)*.8)*.01
            if tile==1 and style==0:weave+=.055*math.sin((x+y)*1.15);noise*=1.6
            if tile==9:weave=-.10 if x%6<2 or y%6<2 else .03
            if tile==5:weave=.03*math.sin(x*math.pi*.5)+.018*math.sin(y*math.pi*.5)
            if tile==4:weave=-.075 if x%12<3 else .015
            if tile==3:weave-=.025 if v<.1 or v>.9 else 0
            if tile==8 and (u<.04 or u>.96) and y%9<5:base=(.9,.84,.64)
            if tile==11 and style==2:
                d=math.sqrt(((u-.50)/.31)**2+((v-.48)/.44)**2)
                if d<.25:base=(.90,.22,.07)
                elif d<.50:base=(.84,.66,.08)
                elif d<.72:base=(.28,.62,.46)
                elif d<.95:base=(.08,.49,.60)
            pixels.extend([max(0,min(1,c+noise+weave)) for c in base]+[1])
    image.pixels.foreach_set(pixels);image.filepath_raw=str(OUT/(ident+'.png'));image.file_format='PNG';image.save();image.pack()
    m=bpy.data.materials.new('PRINT_'+ident);m.use_nodes=True;m.diffuse_color=(1,1,1,1)
    shader=m.node_tree.nodes.get('Principled BSDF');shader.inputs['Roughness'].default_value=.83
    node=m.node_tree.nodes.new('ShaderNodeTexImage');node.image=image;node.interpolation='Closest';m.node_tree.links.new(node.outputs['Color'],shader.inputs['Base Color'])
    return m
N=24
def make_shoe(ident,style):
    global current,material,objects
    current=bpy.data.collections.new(ident+' — editable sneaker');scene.collection.children.link(current);objects=[];material=make_atlas(ident,style)
    width=[.209,.225,.221][style];front=[.465,.455,.47][style];center=-.095;rear=.39
    def outer(a):
        c=math.cos(a);return (math.sin(a)*width*(.93+.07*abs(math.sin(a))),center+c*(rear if c>0 else front))
    # Broad, flat contact patch with independent outsole/midsole/chamfer rings.
    for name,rings,tile in [('Sola — borracha de contato',[(.985,.095),(1.025,.113),(1.025,.128)],4),('Sola — entressola com quina',[(1.025,.128),(1.035,.153),(1,.184 if style!=2 else .202),(.96,.195 if style!=2 else .215)],3)]:
        v=[];f=[]
        for scale,h in rings:
            for i in range(N):x,z=outer(i*math.tau/N);v.append((x*scale,h,z))
        for row in range(len(rings)-1):
            for i in range(N):a=row*N+i;b=row*N+(i+1)%N;f.append((a,b,b+N,a+N))
        f.extend([tuple(reversed(range(N))),tuple((len(rings)-1)*N+i for i in range(N))]);mesh(name,v,f,tile)
    # Open ankle cavity; shoulder-to-collar strips form a real upper, not an oval capsule.
    def shoulder(a):
        x,z=outer(a);c=math.cos(a)
        return Vector((x*.96,[.255,.300,.285][style]+max(0,c)*(.065 if style!=1 else .095),z*.96))
    def lip(a):
        c=math.cos(a);return Vector((math.sin(a)*(.135 if style==1 else .124),([.405,.445,.425][style])+c*.022,.055+math.cos(a)*.155))
    def surface(a,t,raise_by=.0):
        p=shoulder(a).lerp(lip(a),t);p.x+=math.sin(a)*raise_by;p.z+=math.cos(a)*raise_by*.45;p.y+=raise_by*.65;return tuple(p)
    v=[];f=[]
    for row in range(4):
        for i in range(N):
            a=i*math.tau/N
            if row==0:x,z=outer(a);v.append((x*.96,.186 if style!=2 else .204,z*.97))
            elif row==1:v.append(tuple(shoulder(a)))
            elif row==2:v.append(surface(a,.52))
            else:v.append(tuple(lip(a)))
    for row in range(3):
        for i in range(N):a=row*N+i;b=row*N+(i+1)%N;f.append((a,b,b+N,a+N))
    mesh('Tênis — cabedal com abertura',v,f,0,True)
    # Padded collar is thick outside and dark/deep inside around the leg.
    v=[];f=[]
    for row in range(3):
        for i in range(N):
            a=i*math.tau/N;p=lip(a);p.x*=1 if row==0 else .79;p.z=.055+(p.z-.055)*(1 if row==0 else .78);p.y-=0 if row<2 else .12;v.append(tuple(p))
    for row in range(2):
        for i in range(N):a=row*N+i;b=row*N+(i+1)%N;f.append((a,b,b+N,a+N))
    mesh('Tênis — colarinho acolchoado e forro',v,f,6,True)
    def patch(name,aa,ab,ta,tb,tile,offset=.006):
        v=[];f=[];tex=[]
        for row in range(3):
            for i in range(7):v.append(surface(aa+(ab-aa)*i/6,ta+(tb-ta)*row/2,offset));tex.append((i/6,row/2))
        for row in range(2):
            for i in range(6):a=row*7+i;f.append((a,a+1,a+8,a+7))
        return mesh(name,v,f,tile,True,tex)
    for side in [-1,1]:
        patch('Tênis — painel lateral de material',side*.55,side*2.30,.06,.63,0 if style==0 else 1)
        patch('Tênis — reforço traseiro',side*.06,side*.70,.05,.75,2 if style!=2 else 13)
        patch('Tênis — recorte da biqueira',side*2.18,side*3.10,.02,.23,1 if style==0 else 13)
        if style==0:
            points=[]
            for i in range(10):
                a=math.pi/2+i*math.pi/5;r=1 if i%2==0 else .43
                points.append(surface(side*(1.40+math.cos(a)*.50*r),.37+math.sin(a)*.24*r,.022))
            centerpoint=surface(side*1.40,.37,.022);mesh('Tênis — estrela costurada', [centerpoint]+points,[(0,1+i,1+(i+1)%10) for i in range(10)],1)
            seam=[surface(side*(.55+1.75*i/18),.07,.008) for i in range(19)];tube('Tênis — pesponto lateral',seam,.0024,12,4)
        elif style==1:
            points=[surface(side*a,t,.009) for a,t in [(2.1,.20),(1.7,.43),(1.2,.27),(.8,.54),(.48,.25)]]
            tube('Tênis — recorte skate contrastante',points,.010,2,5)
        else:
            for i in range(3):
                a=side*(.85+i*.48);patch('Tênis — sobreposição heatmap',a-side*.16,a+side*.16,.16,.67,11,.014)
        # Small eyelet rings frame the fat flat laces.
        for k in range(5):
            z=-.085-k*.045;y=.397-k*.016+(style==1)*.035
            mesh('Tênis — ilhós',[(side*.102,y,z-.013),(side*.102,y,z+.013),(side*.125,y-.009,z+.013),(side*.125,y-.009,z-.013)],[(0,1,2,3)],10)
    # Tongue sits under the laces and ahead of the ankle opening.
    tongue=[]
    for k in range(6):
        z=-.065-k*.052;y=.413-k*.018+(style==1)*.035
        tongue.extend([(-.10,y,z),(.10,y,z)])
    mesh('Tênis — língua larga',tongue,[(i*2,i*2+1,i*2+3,i*2+2) for i in range(5)],9,True)
    # Ribbon laces with rounded bridge; 5 strips instead of dense tubular spirals.
    for k in range(5):
        z=-.095-k*.047;y=.404-k*.016+(style==1)*.035;v=[];f=[]
        for i in range(7):
            x=-.11+i*.22/6;h=y+.009*math.sin(i*math.pi/6)
            half=.022 if style==1 else .014
            v.extend([(x,h,z-half+x*.13),(x,h+.004,z+half+x*.13)])
        for i in range(6):a=i*2;f.append((a,a+1,a+3,a+2))
        mesh('Cadarço',v,f,5,True)
    # Molded bumper and visible outsole segmentation differentiate the silhouettes.
    if style==1:
        patch('Tênis — bumper skate na biqueira',2.38,3.90,.0,.14,13)
        tube('Sola — filete skate',[(x*1.035,.158,z) for x,z in [outer(i*math.tau/N) for i in range(N+1)]],.0038,4,4)
    # Keep real triangulated Blender geometry and UVs in all deliverables.
    bpy.ops.object.select_all(action='DESELECT')
    for o in objects:
        bm=bmesh.new();bm.from_mesh(o.data);bmesh.ops.triangulate(bm,faces=list(bm.faces));bmesh.ops.recalc_face_normals(bm,faces=bm.faces);bm.to_mesh(o.data);bm.free();o.data.update();o.select_set(True)
    bpy.context.view_layer.objects.active=objects[0]
    bpy.ops.export_scene.fbx(filepath=str(OUT/(ident+'.fbx')),use_selection=True,object_types={'MESH'},add_leaf_bones=False,bake_anim=False,axis_forward='-Z',axis_up='Y',path_mode='AUTO')
    parts=[];triangles=0
    for o in objects:
        me=o.data;me.calc_loop_triangles();vertices=[];normals=[];uv=[]
        for tri in me.loop_triangles:
            for li,vi in zip(tri.loops,tri.vertices):
                p=me.vertices[vi].co;n=me.vertices[vi].normal if me.polygons[tri.polygon_index].use_smooth else tri.normal;t=me.uv_layers.active.data[li].uv
                vertices.append({'x':p.x,'y':p.z,'z':-p.y});normals.append({'x':n.x,'y':n.z,'z':-n.y});uv.append({'x':t.x,'y':t.y})
        triangles+=len(vertices)//3
        parts.append({'name':o.name.split('.')[0],'side':0,'vertices':vertices,'normals':normals,'uv':uv,'triangles':list(range(len(vertices))),'color':{'r':1,'g':1,'b':1,'a':1},'textured':True})
    assert triangles<3200,(ident,triangles)
    (OUT/(ident+'.json')).write_text(json.dumps({'id':ident,'slot':2,'source':'Blender — SneakerCollection.blend','parts':parts},separators=(',',':')),encoding='utf-8')
    print('SNEAKER_AUTHORED',ident,triangles,'triangles',len(parts),'parts',flush=True)
    root=bpy.data.objects.new(ident+' — EDITÁVEL',None);current.objects.link(root)
    for o in objects:o.parent=root
    root.location.x=(style-1)*.95
    return triangles
counts={ident:make_shoe(ident,i) for i,ident in enumerate(['AH_SHOES_01','SB_SHOES_01','CG_SHOES_01'])}
world=bpy.data.worlds.new('Neutral sneaker studio');scene.world=world;world.use_nodes=True;world.node_tree.nodes['Background'].inputs[0].default_value=(.10,.11,.13,1)
def aim(o,p):o.rotation_euler=(Vector(p)-o.location).to_track_quat('-Z','Y').to_euler()
for location,power,size in [((1,2,4),300,4),((-3,0,2),220,3),((0,-3,3),270,3)]:
    data=bpy.data.lights.new('Softbox','AREA');data.energy=power;data.shape='DISK';data.size=size;o=bpy.data.objects.new('Softbox',data);scene.collection.objects.link(o);o.location=location;aim(o,(0,.1,.2))
data=bpy.data.cameras.new('Sneaker review');camera=bpy.data.objects.new('Sneaker review',data);scene.collection.objects.link(camera);camera.location=(2.7,4,2.4);aim(camera,(0,.1,.22));camera.data.type='ORTHO';camera.data.ortho_scale=3.4;scene.camera=camera
scene.render.engine='CYCLES';scene.cycles.samples=24;scene.render.resolution_x=1536;scene.render.resolution_y=768;scene.render.resolution_percentage=100
scene.view_settings.view_transform='Standard';scene.render.image_settings.file_format='PNG';scene.render.filepath=str(ART/'sneakers-blender-preview.png')
bpy.ops.wm.save_as_mainfile(filepath=str(ART/'SneakerCollection.blend'),compress=True)
bpy.ops.render.render(write_still=True)
(ART/'manifest.json').write_text(json.dumps({'authoring':'Blender 5.0','triangles':counts,'texture_size':512,'references':'User-supplied refs roupas/refs tenis; interpreted silhouettes and textile construction, no imported product photograph.'},indent=2),encoding='utf-8')
