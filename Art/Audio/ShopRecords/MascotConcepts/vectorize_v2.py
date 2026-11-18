from pathlib import Path
import sys,re,xml.etree.ElementTree as ET
from PIL import Image,ImageOps
ROOT=Path(__file__).resolve().parents[5];sys.path.insert(0,str(ROOT/'tools/logo-runtime'))
import vtracer
OUT=Path(__file__).parent
specs=[('laje','Laje 92','#E52D38','#FFFFFF'),('passo','Passo Torto','#CDE641','#000000'),('dez','Depois das Dez','#9A77E1','#000000')]
for key,title,bg,ink in specs:
 source=OUT/(key+'-source-v2.png');raw=OUT/(key+'-trace-v2.svg')
 vtracer.convert_image_to_svg_py(str(source),str(raw),colormode='binary',mode='spline',filter_speckle=24,corner_threshold=60,length_threshold=4,max_iterations=10,splice_threshold=45,path_precision=2)
 tree=ET.fromstring(raw.read_text(encoding='utf-8'));body=''.join(ET.tostring(child,encoding='unicode') for child in tree)
 body=re.sub(r'ns0:','',body);body=re.sub(r' xmlns:ns0="[^"]*"','',body);body=re.sub(r'fill="#[0-9A-Fa-f]{6}"','fill="currentColor"',body)
 grayscale=Image.open(source).convert('L');bounds=grayscale.point(lambda v:255 if v<128 else 0).getbbox();x0,y0,x1,y1=bounds;span=max(x1-x0,y1-y0)*1.12;cx=(x0+x1)/2;cy=(y0+y1)/2
 view=f'{cx-span/2:.2f} {cy-span/2:.2f} {span:.2f} {span:.2f}'
 symbol=f'<svg xmlns="http://www.w3.org/2000/svg" viewBox="{view}" width="256" height="256" role="img"><title>{title} — mascote v2</title><g color="#000000">{body}</g></svg>'
 (OUT/(key+'-symbol-v2.svg')).write_text(symbol,encoding='utf-8')
 cover=f'<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1024 1024" width="1024" height="1024"><title>{title} — capa sem texto v2</title><path fill="{bg}" d="M0 0H1024V1024H0Z"/><svg x="120" y="120" width="784" height="784" viewBox="{view}"><g color="{ink}">{body}</g></svg></svg>'
 (OUT/(key+'-cover-v2.svg')).write_text(cover,encoding='utf-8')
 print(key,'source=',grayscale.size,'ink=',bounds,'svg bytes=',len(symbol))
