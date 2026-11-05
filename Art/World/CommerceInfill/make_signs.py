"""Code-authored lettering atlas; uses the game's bundled licensed title font."""
from pathlib import Path
from PIL import Image, ImageDraw, ImageFont
project=Path(__file__).resolve().parents[3]
image=Image.new('RGBA',(512,256),(0,0,0,0));draw=ImageDraw.Draw(image)
font=ImageFont.truetype(str(project/'Assets/Resources/Fonts/ArchivoBlack-Regular.ttf'),48)
for row,label in enumerate(('BARBEARIA','LAVANDERIA','CONSERTOS','DO BAIRRO')):
    box=draw.textbbox((0,0),label,font=font)
    draw.text(((512-(box[2]-box[0]))/2-box[0],row*64+(64-(box[3]-box[1]))/2-box[1]),label,font=font,fill=(255,255,255,255))
image.save(project/'Assets/Resources/CommerceSigns.png')
