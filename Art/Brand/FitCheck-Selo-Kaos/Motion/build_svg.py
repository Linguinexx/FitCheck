from pathlib import Path
import re
out=Path('FitCheck/Art/Brand/FitCheck-Selo-Kaos/Motion')
src=(out.parent/'fitcheck-selo-kaos-red.svg').read_text(encoding='utf-8')
paths=re.findall(r'M[^Z]+Z',src)
assert len(paths)==10
groups={'contorno':paths[:2],'fit':paths[2:5],'estrela':paths[5:6],'check':paths[6:]}
svg='<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1372 1085" width="1372" height="1085" role="img"><title>Fit Check — Selo Kaos</title>'
for name,parts in groups.items():
 svg+=f'<path id="{name}" fill="#E30613" fill-rule="evenodd" d="{" ".join(parts)}"/>'
(out/'logo.svg').write_text(svg+'</svg>',encoding='utf-8')
