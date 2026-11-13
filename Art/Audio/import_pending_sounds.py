from pathlib import Path
import audioop,hashlib,io,json,uuid,wave,zipfile
root=next(p for p in Path(__file__).resolve().parents if (p/'FitCheck/Assets').is_dir())
assets=root/'FitCheck/Assets/Resources/Audio/Game'
names=['cat_purr','cat_meow_alt_01','cat_meow_alt_02','car_horn_01','skate_pass_01','skate_pass_02','laundry_machine','shop_fridge','small_applause_01','ui_back']
template=(assets/'ui_confirm.wav.meta').read_text(encoding='utf-8-sig');report=[]
with zipfile.ZipFile(root/'audio recebidos/AUDIOS-20261107T024841Z-1-001.zip') as z:
    for name in names:
        entry=next(p for p in z.namelist() if Path(p).name==name+'.wav');raw=z.read(entry)
        with wave.open(io.BytesIO(raw),'rb') as f:
            ch,w,rate,n=f.getnchannels(),f.getsampwidth(),f.getframerate(),f.getnframes();data=f.readframes(n)
        data=audioop.lin2lin(data,w,2)
        if ch==2:data=audioop.tomono(data,2,.5,.5)
        if rate!=22050:data,_=audioop.ratecv(data,2,1,rate,22050,None)
        with wave.open(str(assets/(name+'.wav')),'wb') as f:
            f.setparams((1,2,22050,len(data)//2,'NONE','not compressed'));f.writeframes(data)
        meta=assets/(name+'.wav.meta')
        if not meta.exists():meta.write_text(template.replace(next(l for l in template.splitlines() if l.startswith('guid:')),'guid: '+uuid.uuid4().hex),encoding='utf-8')
        report.append(dict(id=name,source=entry,source_sha256=hashlib.sha256(raw).hexdigest(),seconds=n/rate,bytes=len(data)+44))
(root/'FitCheck/Art/Audio/COMPANION_AND_CITY_SOUNDS.json').write_text(json.dumps(report,indent=2)+'\n',encoding='utf-8')
print('Imported',len(report),'sounds,',sum(p['bytes'] for p in report),'bytes; originals untouched.')
