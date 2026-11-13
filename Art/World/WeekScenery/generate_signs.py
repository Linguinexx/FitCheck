"""Small shared print atlas for seven days and the final photograph."""
from pathlib import Path
import re
from PIL import Image, ImageDraw, ImageFont

root = Path(__file__).resolve().parents[3]
source = (root / 'Assets/FitCheck/WeekScenery.cs').read_text(encoding='utf-8')
section = source.split('WeekPrints={', 1)[1].split('};', 1)[0]
labels = re.findall(r'"([^"]+)"', section)
image = Image.new('RGBA', (1024, 512))
draw = ImageDraw.Draw(image)
font_path = root / 'Assets/Resources/Fonts/ArchivoBlack-Regular.ttf'
for i, label in enumerate(labels):
    font = ImageFont.truetype(str(font_path), 128)
    box = draw.textbbox((0, 0), label, font=font)
    glyph = Image.new('RGBA', (box[2] - box[0], box[3] - box[1]))
    ImageDraw.Draw(glyph).text((-box[0], -box[1]), label, font=font, fill='white')
    # Every cell fills the same UV rectangle; world geometry supplies the text aspect.
    glyph = glyph.resize((238, 48), Image.Resampling.LANCZOS)
    image.alpha_composite(glyph, ((i % 4) * 256 + 9, (i // 4) * 64 + 8))
image.save(root / 'Assets/Resources/WeekSigns.png', optimize=True)
