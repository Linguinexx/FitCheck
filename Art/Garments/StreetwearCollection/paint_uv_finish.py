from pathlib import Path
import math, random, struct, zlib, json
import base64
project=Path(__file__).resolve().parents[3]
lettering=json.loads(Path(__file__).with_name('finish-lettering.json').read_text(encoding='utf-8-sig'))
out=project/'Assets/Resources/Garments/Streetwear';out.mkdir(parents=True,exist_ok=True)
FONT={
'A':['01110','10001','10001','11111','10001','10001','10001'],'B':['11110','10001','10001','11110','10001','10001','11110'],'C':['01111','10000','10000','10000','10000','10000','01111'],
'D':['11110','10001','10001','10001','10001','10001','11110'],'E':['11111','10000','10000','11110','10000','10000','11111'],'F':['11111','10000','10000','11110','10000','10000','10000'],
'G':['01111','10000','10000','10111','10001','10001','01111'],'H':['10001','10001','10001','11111','10001','10001','10001'],'I':['111','010','010','010','010','010','111'],
'J':['00111','00010','00010','00010','10010','10010','01100'],'K':['10001','10010','10100','11000','10100','10010','10001'],'L':['10000','10000','10000','10000','10000','10000','11111'],
'M':['10001','11011','10101','10101','10001','10001','10001'],'N':['10001','11001','11001','10101','10011','10011','10001'],'O':['01110','10001','10001','10001','10001','10001','01110'],
'P':['11110','10001','10001','11110','10000','10000','10000'],'R':['11110','10001','10001','11110','10100','10010','10001'],'S':['01111','10000','10000','01110','00001','00001','11110'],
'T':['11111','00100','00100','00100','00100','00100','00100'],'U':['10001','10001','10001','10001','10001','10001','01110'],'V':['10001','10001','10001','10001','10001','01010','00100'],
'W':['10001','10001','10001','10101','10101','10101','01010'],'X':['10001','10001','01010','00100','01010','10001','10001'],'Y':['10001','10001','01010','00100','00100','00100','00100'],
'Z':['11111','00001','00010','00100','01000','10000','11111'],'0':['01110','10001','10011','10101','11001','10001','01110'],'1':['00100','01100','00100','00100','00100','00100','01110'],
'2':['01110','10001','00001','00010','00100','01000','11111'],'3':['11110','00001','00001','01110','00001','00001','11110'],'4':['10010','10010','10010','11111','00010','00010','00010'],
'7':['11111','00001','00010','00100','01000','01000','01000'],'9':['01110','10001','10001','01111','00001','00001','01110'],' ':['000']*7,'-':['00000','00000','00000','11111','00000','00000','00000']}
S=512;cream=(232,221,191);pink=(246,49,122);lime=(171,237,65)
spec={
'AH_TOP_01':dict(base=(223,216,196),kind='tee',sleeve=(.87,.84,.76)),
'CG_TOP_01':dict(base=(164,22,36),kind='basket',sleeve=(.88,.85,.78)),
'SB_TOP_01':dict(base=(58,58,62),kind='vintage',sleeve=(.23,.23,.24)),
'AH_TOP_02':dict(base=(25,39,72),kind='graphic',sleeve=(.10,.15,.28)),
'CG_TOP_02':dict(base=(24,112,59),kind='racing',sleeve=(.09,.44,.23)),
'SB_TOP_02':dict(base=(228,222,206),kind='patch',sleeve=(.89,.87,.81)),
'AH_TOP_03':dict(base=(31,28,32),kind='leather',sleeve=(.12,.11,.125)),
'CG_TOP_03':dict(base=(23,74,156),kind='fleece',sleeve=(.09,.29,.61)),
'SB_TOP_03':dict(base=(28,26,33),kind='hoodie',sleeve=(.11,.10,.13)),
'AH_BOTTOM_01':dict(base=(40,41,44),kind='twill'),
'SB_BOTTOM_01':dict(base=(52,57,59),kind='cargo'),
'CG_BOTTOM_02':dict(base=(222,75,25),kind='basketshort'),
'SB_BOTTOM_02':dict(base=(153,152,146),kind='patchshort'),
'AH_BOTTOM_02':dict(base=(121,34,193),kind='neonshort')}
def png(path,pixels):
 def chunk(k,d):return struct.pack('>I',len(d))+k+d+struct.pack('>I',zlib.crc32(k+d)&0xffffffff)
 raw=b''.join(bytes([0])+bytes(sum(pixels[y*S:(y+1)*S],())) for y in range(S))
 path.write_bytes(b'\x89PNG\r\n\x1a\n'+chunk(b'IHDR',struct.pack('>IIBBBBB',S,S,8,2,0,0,0))+chunk(b'IDAT',zlib.compress(raw))+chunk(b'IEND',b''))
for ident,cfg in spec.items():
 rnd=random.Random(ident);base=cfg['base'];kind=cfg['kind'];pixels=[]
 for y in range(S):
  for x in range(S):
   noise=rnd.uniform(-1,1)*(3 if kind=='fleece' else 1.5 if kind=='leather' else 2)
   weave=(1.1 if (x+2*y)%5<2 else -.55);fade=3.5*math.sin(x*.018)*math.sin(y*.013)
   # Broad painted folds, with no time-dependent movement or extra mesh details.
   edge=math.exp(-((x-128)/20)**2)+math.exp(-((x-384)/20)**2)
   fade-=edge*(3+2*math.sin(y*.021))
   if kind in ('cargo','twill','patchshort'):weave+=(1.6 if (x+y*2)%7<2 else -.6)
   if 'short' in kind or kind in ['cargo','twill']:fade+=9*math.exp(-((x-S*.75)/(S*.14))**2)*math.sin(y/S*math.pi)
   pixels.append(tuple(max(0,min(255,int(c+noise+weave+fade))) for c in base))
 def put(x,y,c):
  if 0<=y<S:pixels[y*S+x%S]=c
 def rect(x,y,w,h,c):
  for yy in range(max(0,int(y)),min(S,int(y+h))):
   for xx in range(int(x),int(x+w)):put(xx,yy,c)
 def text(label,cx,y,scale,c):
  mask=lettering[label+'|'+str(scale)];width=mask['w'];height=mask['h'];alpha=base64.b64decode(mask['alpha']);left=int(cx-width/2)
  for yy in range(height):
   ty=y+yy
   if not 0<=ty<S:continue
   for xx in range(width):
    a=alpha[yy*width+xx]/255
    if a:
     x=(left+xx)%S;old=pixels[ty*S+x];put(x,ty,tuple(round(old[k]*(1-a)+c[k]*a) for k in range(3)))
 def line(x0,y0,x1,y1,c,w=2):
  steps=max(abs(int(x1-x0)),abs(int(y1-y0)),1)
  for k in range(steps+1):rect(x0+(x1-x0)*k/steps,y0+(y1-y0)*k/steps,w,w,c)
 def star(cx,cy,r,c):
  points=[]
  for i in range(10):a=i*math.pi/5-math.pi/2;rr=r if i%2==0 else r*.42;points.append((cx+math.cos(a)*rr,cy+math.sin(a)*rr))
  for i in range(10):line(*points[i],*points[(i+1)%10],c,3)
 if kind=='tee':
  text('AFTER',256,163,2,(47,43,42));text('HOURS',256,182,2,(47,43,42));text('AFTER HOURS',0,128,3,(58,50,42));star(0,238,67,(58,50,42));text('DRIP LAB',0,333,3,(58,50,42))
 elif kind=='basket':
  for x in [128,384]:rect(x-7,95,14,365,cream);rect(x-2,95,4,365,(234,174,65))
  text('LOCAL',256,151,4,cream);text('23',256,208,9,cream);text('COMMON GROUND',0,166,2,cream);text('23',0,216,10,cream)
 elif kind=='vintage':
  rect(220,165,72,35,(190,177,149));text('SIDE B',256,171,2,(43,43,43));text('GARAGE SESSION',0,165,3,(190,177,149));star(0,263,48,(190,177,149))
 elif kind=='graphic':
  text('NIGHT ROUTE',256,144,3,cream);star(256,245,67,(233,147,47));text('DRIP LAB',256,326,3,cream);text('AFTER HOURS',0,160,4,cream);line(-77,280,77,230,(233,147,47),9)
 elif kind=='racing':
  rect(182,138,35,296,cream);rect(216,138,6,296,lime);text('COURT',270,153,3,cream);text('07',270,215,7,cream);text('GROUND CREW',0,176,3,cream);text('07',0,236,9,lime)
 elif kind=='patch':
  rect(118,270,142,168,(56,58,66));rect(282,340,95,100,(107,113,110));text('SIDE B',271,168,3,(37,37,43));star(0,225,73,(61,63,72));text('CUT AND PASTE',0,333,3,(61,63,72))
 elif kind=='leather':
  for x in [128,384]:rect(x,70,3,410,(70,65,68))
  text('AFTER',296,166,2,(144,138,128));text('HOURS',296,184,2,(144,138,128))
 elif kind=='fleece':
  for x in range(S):
   for y in range(136,440):
    if abs(x-180-21*math.sin(y*.022))<24:put(x,y,(15,38,82))
  text('LOCAL',286,178,2,cream)
 elif kind=='hoodie':
  text('UNRELEASED',256,147,3,cream);star(256,228,47,pink);text('SIDE B',0,175,5,pink);text('NO SIGNAL',0,256,4,cream)
 elif kind=='basketshort':
  for x in [128,384]:rect(x,0,12,512,cream);rect(x+13,0,5,512,(32,40,35))
  text('23',340,290,6,cream)
 elif kind=='patchshort':
  rect(40,180,110,230,(86,92,100));rect(310,230,95,180,(199,192,172));text('SIDE B',270,300,3,(43,43,48))
 elif kind=='neonshort':
  for x in [130,384]:
   line(x,130,x+24,370,lime,8);line(x+24,370,x-14,419,lime,8)
  text('LATE',256,280,4,cream)
 # Two rows of subtle hem stitching; texture-space only.
 if 'TOP' in ident:
  dark=tuple(int(c*.73) for c in base);light=tuple(min(255,int(c*1.045+3)) for c in base)
  for yy in [469,475]:
   for xx in range(S):
    if xx%7<4:put(xx,yy,dark);put(xx,yy-1,light)
 png(out/(ident+'.png'),pixels)
Path(__file__).with_name('finish-spec.json').write_text(json.dumps(spec,indent=2),encoding='utf-8')
print('Painted',len(spec),'512px textures')
