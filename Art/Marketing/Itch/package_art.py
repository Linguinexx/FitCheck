"""Verify and package the marketing artwork without modifying any image."""
from pathlib import Path
import hashlib
import json
import struct
import zlib
from zipfile import ZipFile, ZIP_DEFLATED

folder=Path(__file__).resolve().parent
images=sorted(folder.glob('*.png'))
assert len(images)==10, 'Expected approved cover and nine new illustrations'
manifest=[]
for path in images:
    data=path.read_bytes()
    assert data[:8]==b'\x89PNG\r\n\x1a\n'
    width,height=struct.unpack('>II',data[16:24])
    offset=8
    compressed=bytearray()
    ended=False
    while offset<len(data):
        length=struct.unpack('>I',data[offset:offset+4])[0]
        kind=data[offset+4:offset+8]
        payload=data[offset+8:offset+8+length]
        crc=struct.unpack('>I',data[offset+8+length:offset+12+length])[0]
        assert zlib.crc32(kind+payload)&0xffffffff==crc
        if kind==b'IDAT':compressed.extend(payload)
        if kind==b'IEND':ended=True
        offset+=length+12
    assert ended and zlib.decompress(compressed)
    assert width>=1000 and height>=600
    manifest.append(dict(file=path.name,width=width,height=height,
        bytes=path.stat().st_size,sha256=hashlib.sha256(path.read_bytes()).hexdigest()))
(folder/'manifest.json').write_text(json.dumps(manifest,indent=2),encoding='utf-8')
archive=folder.parents[2]/'Builds'/'FitCheck-Itch-Artes-v1.zip'
with ZipFile(archive,'w',ZIP_DEFLATED,compresslevel=6) as z:
    for path in sorted(folder.iterdir()):
        if path.is_file() and path.suffix in {'.png','.md','.txt','.json'}:
            z.write(path,path.name)
with ZipFile(archive) as z:
    assert z.testzip() is None
print(json.dumps(dict(images=len(images),zip_bytes=archive.stat().st_size,
    archive=str(archive),manifest=manifest),indent=2))
