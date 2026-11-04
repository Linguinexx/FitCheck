from pathlib import Path
import math, random, struct, zlib
# New repeatable, brand-free civilian fabric swatches, generated once on disk.
out=Path(__file__).resolve().parents[3]/'Assets/Resources/Garments/Finishes';out.mkdir(parents=True,exist_ok=True)
def save_png(path,size,pixels):
 def chunk(k,d):return struct.pack('>I',len(d))+k+d+struct.pack('>I',zlib.crc32(k+d)&0xffffffff)
 raw=b''.join(b'\0'+bytes(sum(pixels[y*size:(y+1)*size],())) for y in range(size))
 path.write_bytes(b'\x89PNG\r\n\x1a\n'+chunk(b'IHDR',struct.pack('>IIBBBBB',size,size,8,2,0,0,0))+chunk(b'IDAT',zlib.compress(raw))+chunk(b'IEND',b''))
for name,size in [('Cotton',256),('Denim',256),('Canvas',128),('Stripes',256)]:
 rnd=random.Random(name);pixels=[]
 for y in range(size):
  for x in range(size):
   base=245 if name!='Stripes' or (y//13)%4 else 94
   if name=='Denim':base=231+(7 if (x+2*y)%9<3 else -3)
   if name=='Canvas':base=239+(5 if (x//2+y//2)%2==0 else -3)
   v=max(0,min(255,int(base+rnd.uniform(-1.5,1.5)+(1.3 if (x+2*y)%5<2 else -.5))))
   pixels.append((v,v,max(0,v-1)))
 save_png(out/(name+'.png'),size,pixels)
print('Four brand-free fabric tiles generated; no larger than 256px.')
