"""Original retro application chrome; four opaque strips leave real glass untouched."""
from pathlib import Path
from PIL import Image, ImageDraw, ImageFont
import shutil
R=next(p for p in Path(__file__).resolve().parents if (p/'FitCheck/Assets').is_dir())
out=R/'FitCheck/Assets/Resources/Decor';out.mkdir(parents=True,exist_ok=True)
font=ImageFont.truetype('C:/Windows/Fonts/consola.ttf',14)
bold=ImageFont.truetype('C:/Windows/Fonts/consolab.ttf',17)
for style in ('Paint95','FitPlayer'):
    bg='#c4c7c7' if style=='Paint95' else '#272c39'
    image=Image.new('RGB',(512,1024),bg);d=ImageDraw.Draw(image)
    def bevel(r,pressed=False):
        x,y,x2,y2=r;a,b=('#52565b','#f7f6ec') if pressed else ('#f7f6ec','#52565b')
        d.rectangle(r,fill=bg,outline='#14171d',width=2)
        d.line([(x+2,y2-2),(x+2,y+2),(x2-2,y+2)],fill=a,width=3)
        d.line([(x2-2,y+3),(x2-2,y2-2),(x+3,y2-2)],fill=b,width=3)
    bevel((0,0,511,1023))
    d.rectangle((7,7,504,34),fill='#283f70' if style=='Paint95' else '#850b24')
    d.text((14,11),'sem titulo - Paint' if style=='Paint95' else 'FIT PLAYER // ON AIR',font=bold,fill='white')
    for i,mark in enumerate(('_','[]','X')):
        x=426+i*24;bevel((x,11,x+21,31));d.text((x+3,12),mark,font=font,fill='#111111')
    d.text((14,43),'Arquivo Editar Ver Imagem Cores' if style=='Paint95' else 'Arquivo  Mix  Visualizar  Ajuda',font=font,fill='#181c25' if style=='Paint95' else '#f3eee1')
    # Glass aperture x=100..492 / y=70..948; it is not part of the frame mesh.
    bevel((96,66,496,952),True)
    for row in range(9):
        for col in range(2):
            x=10+col*42;y=76+row*48;bevel((x,y,x+36,y+42),row==3 and col==0)
            ink='#202630' if style=='Paint95' else '#dfcfad'
            cx=x+18;cy=y+21;kind=(row*2+col)%9
            if kind==0:d.line([(x+8,y+31),(x+27,y+10)],fill=ink,width=4)
            elif kind==1:d.rectangle((x+8,y+12,x+28,y+31),outline=ink,width=2)
            elif kind==2:d.ellipse((x+8,y+12,x+28,y+31),outline=ink,width=2)
            elif kind==3:d.polygon([(cx,y+9),(x+29,y+31),(x+7,y+31)],outline=ink)
            elif kind==4:d.text((x+10,y+12),'A',font=bold,fill=ink)
            elif kind==5:d.arc((x+6,y+9,x+29,y+31),0,300,fill=ink,width=3)
            elif kind==6:d.line([(x+8,y+28),(cx,y+13),(x+27,y+28)],fill=ink,width=3)
            elif kind==7:d.line([(cx,y+10),(cx,y+31),(x+7,cy),(x+28,cy)],fill=ink,width=2)
            else:d.rounded_rectangle((x+9,y+12,x+27,y+30),radius=5,outline=ink,width=2)
    if style=='Paint95':
        colors=['#000000','#777777','#9b1825','#d59329','#155a38','#2c8c9d','#1d3979','#8a3983','#ffffff','#c5c5b5','#ed2c34','#f1dc60','#4adf87','#67d7e4','#7182df','#d784ce']
        for i,c in enumerate(colors):
            x=49+i%8*55;y=959+i//8*23;d.rectangle((x,y,x+49,y+19),fill=c,outline='#25252b',width=2)
        d.text((12,1005),'Ajuda: procure o Seu Jota.',font=font,fill='#202630')
    else:
        d.text((13,536),'LIVE',font=bold,fill='#f0783d')
        for i in range(10):
            d.rectangle((17,581+i*15,75,590+i*15),fill='#78d397' if i<6 else '#444c53')
        for i,mark in enumerate(('|<','||','>','>|')):
            x=17+i*71;bevel((x,964,x+61,994));d.text((x+18,972),mark,font=font,fill='#e8ddc8')
        d.text((312,975),'REPEAT: YOU',font=font,fill='#f4d0bc')
        d.text((12,1005),'FIT CHECK - bairro em movimento',font=font,fill='#d5d5c8')
    image.save(out/(style+'.png'),optimize=True)
art=R/'FitCheck/Art/Decor/MirrorFrames';art.mkdir(parents=True,exist_ok=True)
shutil.copy2(__file__,art/'make_mirror_frames.py')
print('Created original 512x1024 retro frames:',[(p.name,p.stat().st_size) for p in out.glob('*.png')])
