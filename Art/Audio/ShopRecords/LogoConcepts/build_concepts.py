from pathlib import Path
import json
ROOT=Path(__file__).resolve().parents[5];OUT=Path(__file__).parent
TYPE=json.loads((OUT/'type-paths.json').read_text(encoding='utf-8-sig'))
def word(key,cx,y,width):
 t=TYPE[key];scale=width/t['width'];x=cx-width/2
 return f'<g transform="translate({x:.2f} {y:.2f}) scale({scale:.5f}) translate({-t["x"]:.2f} {-t["y"]:.2f})"><path d="{t["path"]}"/></g>'
marks={
 'laje':'''<path fill-rule="evenodd" d="M66 24H88Q128 24 128 64V99Q128 120 117 140L68.50 224H28L78.23 137H66Q20 137 20 91V70Q20 24 66 24ZM66 58Q54 58 54 73V90Q54 103 67 103H81Q94 103 94 89V72Q94 58 81 58Z"/><path d="M143 70Q143 24 189 24H200Q244 24 244 68Q244 93 222 115L176 161Q171 169 171 181V190H242V224H134V184Q134 159 150 141L192 99Q210 80 210 69Q210 58 198 58H187Q177 58 177 70V80H143Z"/>''',
 'passo':'''<g transform="rotate(-15 77 116)"><path fill-rule="evenodd" d="M28 24H84Q134 24 134 76Q134 127 84 127H66V220H28ZM66 59V92H84Q100 92 100 76Q100 59 84 59Z"/></g><g transform="rotate(15 184 150)"><path d="M128 103H242V139H204V225H166V139H128Z"/></g>''',
 'dez':'''<path d="M23 65L63 25H94V222H54V76L23 93.90Z"/><path fill-rule="evenodd" d="M174 23Q242 23 242 123Q242 224 174 224Q106 224 106 123Q106 23 174 23ZM190 58Q158 58 148 86Q140 109 140 132Q140 189 177 189Q208 189 208 132V115Q208 58 190 58Z"/>'''
}
specs=[('laje','Laje 92','#E52D38','#FFFFFF'),('passo','Passo Torto','#CDE641','#000000'),('dez','Depois das Dez','#9A77E1','#000000')]
def svg(body,w=1024,h=1024,title=''):
 return f'<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 {w} {h}" width="{w}" height="{h}" role="img"><title>{title}</title>{body}</svg>'
for key,title,bg,ink in specs:
 mark=marks[key]
 (OUT/(key+'-symbol.svg')).write_text(svg('<g fill="#000000">'+mark+'</g>',256,256,title+' — símbolo'),encoding='utf-8')
 if key=='laje':
  lock=word('laje',512,205,493)+'<g transform="translate(224 393) scale(2.22)">'+mark+'</g>'
 elif key=='passo':
  lock='<g transform="translate(326 164) scale(1.45)">'+mark+'</g>'+word('passo',512,565,586)+word('torto',512,713,614)
 else:
  lock='<g transform="translate(335 152) scale(1.38)">'+mark+'</g>'+word('depois',512,535,470)+word('dez',512,742,544)
 (OUT/(key+'-lockup.svg')).write_text(svg('<g fill="#000000" fill-rule="evenodd">'+lock+'</g>',title=title+' — marca'),encoding='utf-8')
 (OUT/(key+'-cover.svg')).write_text(svg(f'<path fill="{bg}" d="M0 0H1024V1024H0Z"/><g fill="{ink}" fill-rule="evenodd">'+lock+'</g>',title=title+' — conceito de capa'),encoding='utf-8')
print('THREE_VECTOR_CONCEPTS_BUILT')
