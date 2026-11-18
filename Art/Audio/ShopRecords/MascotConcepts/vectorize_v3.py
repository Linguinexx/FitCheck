"""Targeted mascot revisions; preserve previous concepts and raccoon geometry."""
from pathlib import Path
import runpy

OUT = Path(__file__).parent
source = (OUT / 'vectorize_v2.py').read_text(encoding='utf-8')
source = source.replace("'#FFFFFF'", "'#000000'").replace('v2', 'v3')
# Reuse the exact approved raccoon trace; only its cover ink changes.
source = source.replace("source=OUT/(key+'-source-v3.png')", "source=OUT/(key+('-source-v2.png' if key=='laje' else '-source-v3.png'))")
source = source.replace(" vtracer.convert_image_to_svg_py", " if key=='laje':\n  raw.write_text((OUT/'laje-trace-v2.svg').read_text(encoding='utf-8'),encoding='utf-8')\n else:\n  vtracer.convert_image_to_svg_py")
exec(compile(source, str(OUT/'vectorize_v3.py'), 'exec'))
