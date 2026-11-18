"""Original procedural compositions and sleeve artwork; no samples from the game.
Run with the Codex Python runtime (numpy/Pillow) and the bundled ffmpeg.
"""
from pathlib import Path
import json, math, subprocess, wave
import numpy as np
from PIL import Image,ImageDraw,ImageFont
ROOT=Path(__file__).resolve().parents[4];ART=Path(__file__).parent
OUT=ROOT/'FitCheck/Assets/Resources/Audio/Game';COVERS=ROOT/'FitCheck/Assets/Resources/UI/ShopRecords'
FF=ROOT/'tools/video-runtime/imageio_ffmpeg/binaries/ffmpeg-win-x86_64-v7.1.exe'
OUT.mkdir(parents=True,exist_ok=True);COVERS.mkdir(parents=True,exist_ok=True)
RATE=22050
specs=[('laje','Laje 92',92,(229,45,56),[48,53,50,55],[0,3,7,10,14]),('passo','Passo Torto',98,(205,230,65),[43,48,46,41],[0,4,7,9,14]),('dez','Depois das Dez',84,(154,119,225),[45,41,48,43],[0,3,7,10,14])]
def tone(midi,duration,kind,rng):
 t=np.arange(int(duration*RATE))/RATE;f=440*2**((midi-69)/12)
 if kind=='bass':return (np.sin(2*np.pi*f*t)+.22*np.sin(4*np.pi*f*t))*np.minimum(t/.012,1)*np.exp(-t*3)
 if kind=='keys':return (np.sin(2*np.pi*f*t+1.7*np.sin(2*np.pi*f*2*t)*np.exp(-t*5))+.16*np.sin(2*np.pi*f*3*t))*np.minimum(t/.006,1)*np.exp(-t*2.3)
 if kind=='pluck':return sum(np.sin(2*np.pi*f*h*t)*np.exp(-t*(3+h))/h for h in range(1,6))*np.minimum(t/.004,1)
 return np.sin(2*np.pi*f*t+.45*np.sin(2*np.pi*5*t))*np.minimum(t/.008,1)*np.exp(-t*1.9)
manifest=[]
for idx,(key,title,bpm,color,roots,chord) in enumerate(specs):
 rng=np.random.default_rng(718+idx);beat=60/bpm;duration=beat*64;n=int(duration*RATE);audio=np.zeros((n,2),dtype=np.float64)
 def add(sound,when,volume=1,pan=0):
  start=int(when*RATE);ids=(np.arange(len(sound))+start)%n
  audio[ids,0]+=sound*volume*math.sqrt((1-pan)/2);audio[ids,1]+=sound*volume*math.sqrt((1+pan)/2)
 t=np.arange(int(.42*RATE))/RATE;kick=np.sin(2*np.pi*(48*t+38*(1-np.exp(-t*30))/30))*np.exp(-t*13)+rng.normal(0,1,len(t))*np.exp(-t*170)*.16
 t=np.arange(int(.24*RATE))/RATE;noise=rng.normal(0,1,len(t));snare=(noise-np.roll(noise,1))*.23*np.exp(-t*20)+np.sin(2*np.pi*180*t)*np.exp(-t*34)*.26
 t=np.arange(int(.09*RATE))/RATE;noise=rng.normal(0,1,len(t));hat=(noise-np.roll(noise,1))*.085*np.exp(-t*74)
 for bar in range(16):
  for b in [0,1.65 if idx==1 else 1.5,2.75,3.5] if bar%2 else [0,1.5,2.5]:add(kick,(bar*4+b)*beat,.48)
  for b in [1,3]:add(snare,(bar*4+b+.025)*beat,.65)
  for h in range(8):add(hat,(bar*4+h*.5+(.075 if h%2 else 0))*beat,.75+(.25 if h%2==0 else -.15),(-.27 if h%2 else .27))
  root=roots[(bar//2)%4];kind='keys' if idx==0 else 'pluck' if idx==1 else 'vibes'
  for b in ([.12,2.55] if idx!=1 else [.25,1.75,3.25]):
   for j,interval in enumerate(chord):add(tone(root+12+interval,2.3,kind,rng),(bar*4+b)*beat,.053 if idx!=1 else .065,(j-2)*.18)
  for b,interval in [(0,0),(1.5,7),(2.25,0),(3.5,10 if bar%2 else 7)]:add(tone(root-12+interval,.65,'bass',rng),(bar*4+b)*beat,.27)
  if bar%2==1:
   for j,interval in enumerate(([19,17,14] if idx==0 else [16,19,21] if idx==1 else [22,19,14])):add(tone(root+24+interval,1.5,'vibes',rng),(bar*4+2+j*.5)*beat,.045,(j-1)*.35)
 audio=np.tanh(audio*1.25)*.78;edge=int(.004*RATE);audio[:edge]*=np.linspace(0,1,edge)[:,None];audio[-edge:]*=np.linspace(1,0,edge)[:,None]
 raw=ART/(key+'-working.wav')
 with wave.open(str(raw),'wb') as w:w.setnchannels(2);w.setsampwidth(2);w.setframerate(RATE);w.writeframes((np.clip(audio,-1,1)*32767).astype('<i2').tobytes())
 source=ART/(key+'.flac');game=OUT/('shop_record_'+key+'.ogg')
 subprocess.run([str(FF),'-v','error','-y','-i',str(raw),'-af','loudnorm=I=-18:TP=-2:LRA=7','-ar',str(RATE),str(source)],check=True)
 subprocess.run([str(FF),'-v','error','-y','-i',str(source),'-c:a','libvorbis','-q:a','3',str(game)],check=True);raw.unlink()
 im=Image.new('RGB',(512,512),(17,16,20));d=ImageDraw.Draw(im);paper=(239,231,211)
 bold=ImageFont.truetype(str(ROOT/'FitCheck/Assets/Resources/Fonts/ArchivoBlack-Regular.ttf'),55 if idx!=2 else 46)
 small=ImageFont.truetype(str(ROOT/'FitCheck/Assets/Resources/Fonts/SpaceGrotesk-Bold.ttf'),17)
 d.text((28,25),'DRIPLAB SESSIONS',font=small,fill=paper);d.text((400,25),'VOL.0'+str(idx+1),font=small,fill=paper)
 if idx==0:
  for j in range(6):d.polygon([(38+j*12,307-j*30),(473,170-j*17),(473,209-j*17),(38+j*12,348-j*30)],fill=color if j%2==0 else paper)
 elif idx==1:
  for j in range(7):d.polygon([(30+j*64,113),(76+j*64,149),(35+j*64,223),(79+j*64,280),(36+j*64,348),(8+j*64,320),(48+j*64,274),(8+j*64,217),(50+j*64,154)],fill=color if j%2 else paper)
 else:
  for r in range(140,25,-20):d.arc((255-r,231-r,255+r,231+r),15,330,fill=color,width=11)
  d.ellipse((241,217,269,245),fill=paper);d.polygon([(63,322),(132,133),(163,312)],fill=paper)
 # Ink grain is deterministic and subtle; leave type untextured for clarity.
 pixels=np.asarray(im).copy();grain=rng.integers(-6,7,(512,512,1));pixels=np.clip(pixels.astype(int)+grain,0,255).astype('uint8');im=Image.fromarray(pixels);d=ImageDraw.Draw(im)
 d.rectangle((0,373,512,512),fill=color);ink=(16,16,19)
 if idx==2:d.text((25,382),'DEPOIS',font=bold,fill=ink);d.text((25,432),'DAS DEZ',font=bold,fill=ink)
 else:d.text((25,391),title.upper(),font=bold,fill=ink)
 d.text((28,478),str(bpm)+' BPM  /  INSTRUMENTAL  /  FIT CHECK',font=small,fill=ink)
 im.save(COVERS/(key+'.png'));manifest.append(dict(id=key,title=title,bpm=bpm,seconds=duration,source=str(source.relative_to(ROOT)),audio=str(game.relative_to(ROOT)),authoring='Original procedural score; no samples; deterministic synthesis',seed=718+idx))
(ART/'records.json').write_text(json.dumps(manifest,ensure_ascii=False,indent=2),encoding='utf-8');print(json.dumps(manifest,ensure_ascii=False))
