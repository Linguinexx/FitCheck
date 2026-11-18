import bpy,math,json
from pathlib import Path
from mathutils import Vector
ROOT=Path(__file__).resolve().parents[4]
OUT=ROOT/'FitCheck/Assets/Resources/Environment/RoomCourt';OUT.mkdir(parents=True,exist_ok=True)
bpy.ops.wm.read_factory_settings(use_empty=True)
COLORS={'wood':('Wood',(.36,.205,.105)),'dark':('CityMetal',(.065,.073,.08)),'mattress':('Cloth',(.92,.88,.78)),'duvet':('Cloth',(.54,.12,.25)),'fold':('Cloth',(.74,.24,.32)),'seam':('Cloth',(.40,.08,.18)),'rim':('CityMetal',(.96,.26,.045)),'board':('Wood',(.89,.89,.81)),'net':('Leather',(.93,.90,.80)),'red':('Leather',(.89,.025,.06))}
materials={}
for name,(texture,color) in COLORS.items():
 m=bpy.data.materials.new(name);m.use_nodes=True;bs=next(n for n in m.node_tree.nodes if n.type=='BSDF_PRINCIPLED');bs.inputs['Base Color'].default_value=(*color,1);bs.inputs['Roughness'].default_value=.8;m['texture']=texture;m['pigment']=list(color);materials[name]=m

def coord(v):return (v[0],-v[2],v[1])
def cube(name,p,s,mat,bevel=.035,angle=0):
 bpy.ops.mesh.primitive_cube_add(size=1,location=coord(p));o=bpy.context.object;o.name=name;o.dimensions=(s[0],s[2],s[1]);bpy.ops.object.transform_apply(location=False,rotation=False,scale=True);o.rotation_euler.z=angle*math.pi/180
 if bevel:
  m=o.modifiers.new('Soft silhouette','BEVEL');m.width=min(bevel,min(s)*.45);m.segments=3;bpy.ops.object.modifier_apply(modifier=m.name)
 o.data.materials.append(materials[mat]);return o

def tube(name,points,r,mat):
 c=bpy.data.curves.new(name,'CURVE');c.dimensions='3D';c.resolution_u=1;c.bevel_depth=r;c.bevel_resolution=0;c.resolution_u=1
 s=c.splines.new('POLY');s.points.add(len(points)-1)
 for p,v in zip(s.points,points):p.co=(*coord(v),1)
 o=bpy.data.objects.new(name,c);bpy.context.collection.objects.link(o);c.materials.append(materials[mat]);bpy.context.view_layer.objects.active=o;o.select_set(True);bpy.ops.object.convert(target='MESH');o.select_set(False);return o

def export(name,objects):
 groups={}
 for o in objects:
  if o.type!='MESH':continue
  me=o.data;me.calc_loop_triangles();mat=o.data.materials[0];key=mat['texture'];g=groups.setdefault(key,{'texture':key,'p':[],'n':[],'c':[]})
  for tri in me.loop_triangles:
   for index in tri.loops:
    loop=me.loops[index];v=o.matrix_world@me.vertices[loop.vertex_index].co
    try:n=o.matrix_world.to_3x3()@me.corner_normals[index].vector
    except:n=o.matrix_world.to_3x3()@me.vertices[loop.vertex_index].normal
    n.normalize();g['p']+=list((round(v.x,6),round(v.z,6),round(-v.y,6)));g['n']+=list((round(n.x,5),round(n.z,5),round(-n.y,5)));g['c']+=list(mat['pigment'])
 (OUT/(name+'.json')).write_text(json.dumps({'groups':list(groups.values())},separators=(',',':')),encoding='utf-8')
 print(name,'triangles',sum(len(g['p'])//9 for g in groups.values()),'groups',len(groups))

start=set(bpy.data.objects)
cube('Floating walnut platform',(0,.42,0),(2.23,.29,3.42),'wood',.10)
cube('Mattress support',(0,.575,0),(2.13,.09,3.30),'dark',.03)
for x in [-.88,.88]:
 for z in [-1.38,1.38]:cube('Recessed foot',(x,.17,z),(.13,.34,.13),'dark',.04)
cube('Soft mattress',(0,.75,0),(2.13,.34,3.30),'mattress',.15)
cube('Duvet',(0,.955,-.40),(2.16,.17,2.30),'duvet',.075)
cube('Duvet hanging foot',(0,.84,-1.55),(2.16,.32,.10),'duvet',.04)
for x in [-1.055,1.055]:cube('Duvet soft side',(x,.84,-.40),(.10,.32,2.18),'duvet',.045)
cube('Folded duvet',(0,1.047,.64),(2.15,.15,.34),'fold',.06)
for z in [-1.38,-.99,-.60,-.21,.18]:tube('Quilt channel',[(-.99,1.039,z),(-.48,1.047,z+.013),(0,1.049,z),(.48,1.047,z-.012),(.99,1.039,z)],.004,'seam')
for x,angle in [(-.51,-6),(.51,6)]:
 cube('Plump pillow',(x,1.075,1.18),(.91,.28,.66),'mattress',.12,angle)
 tube('Pillow stitch',[(x-.33,1.16,.93),(x+.33,1.16,.93)],.004,'mattress')
cube('Headboard',(0,.99,1.75),(2.30,1.72,.16),'wood',.07)
for i in range(7):cube('Inset headboard rib',(-.93+i*.31,1.18,1.64),(.25,1.12,.12),'wood',.04)
bed=[o for o in bpy.data.objects if o not in start];export('Bed',bed)
for o in bed:o.hide_set(True)
start=set(bpy.data.objects)
cube('Post cushion',(0,.98,1.35),(.34,1.96,.34),'dark',.075)
tube('Steel upright',[(0,0,1.35),(0,6.14,1.35),(0,6.14,.85)],.10,'dark')
tube('Angled support',[(0,5.22,1.35),(0,5.92,.86)],.055,'dark')
cube('Board frame',(0,6.10,.83),(3.24,2.04,.18),'dark',.08)
cube('Painted backboard',(0,6.10,.715),(3.06,1.86,.075),'board',.04)
for x in [-.625,.625]:cube('Target side',(x,5.82,.662),(.035,.76,.015),'red',.005)
for y in [5.44,6.20]:cube('Target horizontal',(0,y,.662),(1.285,.035,.015),'red',.005)
cube('Rim mounting plate',(0,5.32,.61),(.38,.26,.09),'red',.025)
for x in [-.14,.14]:tube('Rim bracket',[(x,5.32,.61),(x,5.30,.45)],.034,'rim')
tube('Orange rim',[(math.cos(i*math.pi/16)*.65,5.30,math.sin(i*math.pi/16)*.65) for i in range(33)],.035,'rim')
for strand in range(12):
 pts=[]
 for row in range(5):
  a=(strand+(row%2)*.5)*math.pi/6;r=.615-row*.069;pts.append((math.cos(a)*r,5.27-row*.16,math.sin(a)*r))
 tube('Diamond net',pts,.008,'net')
for row in range(1,5):
 r=.615-row*.069;tube('Net horizontal stitch',[(math.cos(i*math.pi/6)*r,5.27-row*.16,math.sin(i*math.pi/6)*r) for i in range(13)],.0055,'net')
hoop=[o for o in bpy.data.objects if o not in start];export('Hoop',hoop)
for o in bed:o.hide_set(False)
bpy.ops.wm.save_as_mainfile(filepath=str(Path(__file__).resolve().parent/'RoomCourt.blend'))
