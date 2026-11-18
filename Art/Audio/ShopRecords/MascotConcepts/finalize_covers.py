"""Assemble the selected artwork without changing game assets."""
from pathlib import Path
import re
import sys
import xml.etree.ElementTree as ET
from PIL import Image, ImageDraw, ImageFont

BASE = Path(__file__).parent
OUT = BASE/'Final'
OUT.mkdir(exist_ok=True)
sys.path.insert(0,str(BASE.parents[4]/'tools/logo-runtime'))
import vtracer

def build():
    for key in ('laje','dez'):
        for kind in ('symbol','cover'):
            text = (BASE/f'{key}-{kind}-v4.svg').read_text(encoding='utf-8')
            (OUT/f'{key}-{kind}.svg').write_text(text.replace('v4','final'),encoding='utf-8')
    raw = OUT/'passo-trace.svg'
    vtracer.convert_image_to_svg_py(str(BASE/'passo-source-v6.png'),str(raw),colormode='binary',mode='spline',filter_speckle=24,corner_threshold=60,length_threshold=4,max_iterations=10,splice_threshold=45,path_precision=2)
    tree = ET.fromstring(raw.read_text(encoding='utf-8'))
    body = ''.join(ET.tostring(child,encoding='unicode') for child in tree)
    body = re.sub(r'ns0:','',body)
    body = re.sub(r' xmlns:ns0="[^"]*"','',body)
    body = re.sub(r'fill="#[0-9A-Fa-f]{6}"','fill="currentColor"',body)
    image = Image.open(BASE/'passo-source-v6.png').convert('L')
    x0,y0,x1,y1 = image.point(lambda v:255 if v<128 else 0).getbbox()
    span = max(x1-x0,y1-y0)*1.12
    view = f'{(x0+x1-span)/2:.2f} {(y0+y1-span)/2:.2f} {span:.2f} {span:.2f}'
    (OUT/'passo-symbol.svg').write_text(f'<svg xmlns="http://www.w3.org/2000/svg" viewBox="{view}" width="256" height="256"><title>Passo Torto</title><g color="#000000">{body}</g></svg>',encoding='utf-8')
    (OUT/'passo-cover.svg').write_text(f'<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="1024" height="1024"><title>Passo Torto</title><path fill="#CDE641" d="M0 0H1024V1024H0Z"/><svg x="120" y="120" width="784" height="784" viewBox="{view}"><g color="#000000">{body}</g></svg></svg>',encoding='utf-8')

def board():
    font = ImageFont.truetype(str(BASE.parents[3]/'Assets/Resources/Fonts/SpaceGrotesk-Bold.ttf'),24)
    image = Image.new('RGB',(1500,650),(18,17,21))
    draw = ImageDraw.Draw(image)
    draw.text((36,22),'FITCHECK / COLEÇÃO DE DISCOS',font=font,fill='#F4F0E8')
    for i,(key,label) in enumerate([('laje','LAJE 92'),('passo','PASSO TORTO'),('dez','DEPOIS DAS DEZ')]):
        cover = Image.open(OUT/'renders'/f'{key}-cover.png').convert('RGB')
        image.paste(cover.resize((464,464),Image.Resampling.LANCZOS),(36+i*482,85))
        draw.text((36+i*482,570),label,font=font,fill='#F4F0E8')
    image.save(OUT/'FitCheck-Discos.png')

if __name__ == '__main__':
    board() if '--board' in sys.argv else build()
