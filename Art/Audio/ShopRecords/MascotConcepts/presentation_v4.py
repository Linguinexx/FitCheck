from pathlib import Path
from PIL import Image, ImageDraw, ImageFont

OUT = Path(__file__).parent
font = ImageFont.truetype(str(OUT.parents[3]/'Assets/Resources/Fonts/SpaceGrotesk-Bold.ttf'),24)
board = Image.new('RGB',(1500,650),(18,17,21))
draw = ImageDraw.Draw(board)
draw.text((36,22),'DISCOS / MASCOTES — REVISÃO 04',font=font,fill='#F4F0E8')
for i,(key,label) in enumerate([('laje','Laje 92 / Guaxinim'),('passo','Passo Torto / Sol'),('dez','Depois das Dez / Coruja')]):
    cover = Image.open(OUT/'renders'/f'{key}-cover-v4.png').convert('RGB')
    board.paste(cover.resize((464,464),Image.Resampling.LANCZOS),(36+i*482,85))
    draw.text((36+i*482,570),label,font=font,fill='#F4F0E8')
board.save(OUT/'concepts-color-v4.png')
