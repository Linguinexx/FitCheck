"""48-second edit from the current game's native 30-fps captures. Blender 5.0."""
import bpy,math,json,sys,shutil,wave
from pathlib import Path

PROJECT=Path(__file__).resolve().parents[3]
OUT=PROJECT/'Captures/TeaserAtual';SRC=OUT/'source';OUT.mkdir(parents=True,exist_ok=True)
assert (SRC/'checks.txt').read_text().startswith('CURRENT_TEASER_CAPTURED:')
FPS=30;W=1280;H=720;DURATION=48
scene=bpy.context.scene;scene.sequence_editor_create();seq=scene.sequence_editor.strips
scene.render.resolution_x=W;scene.render.resolution_y=H;scene.render.resolution_percentage=100
scene.render.fps=FPS;scene.frame_start=1;scene.frame_end=FPS*DURATION
scene.render.use_sequencer=True;scene.view_settings.view_transform='Standard'
scene.render.image_settings.media_type='VIDEO';scene.render.image_settings.file_format='FFMPEG'
scene.render.ffmpeg.format='MPEG4';scene.render.ffmpeg.codec='H264';scene.render.ffmpeg.constant_rate_factor='HIGH';scene.render.ffmpeg.ffmpeg_preset='GOOD'
scene.render.ffmpeg.audio_codec='AAC';scene.render.ffmpeg.audio_bitrate=192;scene.render.ffmpeg.audio_mixrate=44100;scene.render.ffmpeg.audio_channels='STEREO'
scene.render.filepath=str(OUT/'FitCheck-Teaser-Atual.mp4')
font=bpy.data.fonts.load(str(PROJECT/'Assets/Resources/Fonts/ArchivoBlack-Regular.ttf'))
small=bpy.data.fonts.load(str(PROJECT/'Assets/Resources/Fonts/SpaceGrotesk-Bold.ttf'))
WHITE=(.94,.91,.83,1);RED=(227/255,6/255,19/255,1)
def fr(t):return round(t*FPS)+1
def anim(obj,prop,a,b,start,end):
    setattr(obj,prop,a);obj.keyframe_insert(data_path=prop,frame=start)
    setattr(obj,prop,b);obj.keyframe_insert(data_path=prop,frame=end)
def fade(s,start,end,n=5):
    anim(s,'blend_alpha',0,1,start,start+n);anim(s,'blend_alpha',1,0,end-n,end-1)
def color(name,start,end,c,channel=3,alpha=1,sx=1,sy=1,ox=0,oy=0):
    s=seq.new_effect(name,type='COLOR',channel=channel,frame_start=fr(start),length=fr(end)-fr(start));s.color=c[:3];s.blend_type='ALPHA_OVER';s.blend_alpha=alpha
    s.transform.scale_x=sx;s.transform.scale_y=sy;s.transform.offset_x=ox;s.transform.offset_y=oy;return s
def txt(label,start,end,size=42,x=.055,y=.16,channel=6,center=False,tint=WHITE):
    s=seq.new_effect(label,type='TEXT',channel=channel,frame_start=fr(start),length=fr(end)-fr(start))
    s.text=label;s.font=font if size>=28 else small;s.font_size=size;s.color=tint;s.location=(x,y)
    s.anchor_x='CENTER' if center else 'LEFT';s.anchor_y='CENTER';s.alignment_x='CENTER' if center else 'LEFT';s.use_shadow=False
    fade(s,fr(start),fr(end),4);return s
def caption(title,sub,start,end,top=False):
    if top:
        color('Interface heading',start,end,(.014,.014,.017),4,.96,sy=.103,oy=H*.4485)
        color('Kaos header accent',start,end,RED,5,.92,sx=.004,sy=.055,ox=-W*.455,oy=H*.453)
        txt(title,start+.08,end,29,x=.5,y=.96,center=True)
        if sub:txt(sub,start+.12,end,17,x=.5,y=.918,center=True,channel=7)
    else:
        color('Caption charcoal',start,end,(.014,.014,.017),4,.88,sy=.195,oy=-H*.4025)
        color('Kaos caption rule',start,end,RED,5,1,sx=.003,sy=.115,ox=-W*.46,oy=-H*.402)
        txt(title,start+.08,end,39,x=.06,y=.143)
        if sub:txt(sub,start+.12,end,20,x=.062,y=.075,channel=7)
def native(name,start,end,offset=0,tail=False):
    files=sorted((SRC/name).glob('*.jpg'));count=fr(end)-fr(start)
    if tail:offset=len(files)-count
    assert offset>=0 and len(files)>=offset+count,(name,len(files),offset,count)
    selected=files[offset:offset+count]
    s=seq.new_image(name,str(selected[0]),channel=2,frame_start=fr(start))
    for p in selected[1:]:s.elements.append(p.name)
    s.frame_final_duration=count
    # Preserve the real interface in full. 16:10 UI uses slim side bars in the 16:9 edit.
    im=bpy.data.images.load(str(selected[0]),check_existing=True);width,height=im.size
    base=min(W/width,H/height);s.transform.scale_x=base;s.transform.scale_y=base
    return s
def logo(start,end,x,y,width,channel=8):
    path=PROJECT/'Assets/Resources/Brand/FitCheck-Selo-Kaos-Red.png'
    s=seq.new_image('Official Selo Kaos',str(path),channel=channel,frame_start=fr(start));s.frame_final_duration=fr(end)-fr(start);s.blend_type='ALPHA_OVER'
    im=bpy.data.images.load(str(path),check_existing=True);base=width/im.size[0]
    s.transform.scale_x=base;s.transform.scale_y=base;s.transform.offset_x=x-W/2;s.transform.offset_y=y-H/2
    for frame in range(fr(start),fr(end),3):
        t=(frame-fr(start))/FPS;scale=base*(1+.018*math.sin(t*1.8));s.transform.scale_x=scale;s.transform.scale_y=scale;s.transform.offset_y=y-H/2+math.sin(t*1.2)*3
        s.transform.keyframe_insert(data_path='scale_x',frame=frame);s.transform.keyframe_insert(data_path='scale_y',frame=frame);s.transform.keyframe_insert(data_path='offset_y',frame=frame)
    fade(s,fr(start),fr(end),7)
    return s

color('Letterbox background',0,48,(.008,.009,.01),1)
native('01-facade',0,3.6)
color('Opening brand panel',0,3.6,(.006,.008,.009),3,.82,sx=.43,ox=-W*.285)
logo(.12,3.55,260,445,300)
txt('A game by DripLab',.24,3.5,20,x=.06,y=.88,tint=RED)
txt('Seu estilo.\nAs histórias da cidade.',.45,3.5,32,x=.055,y=.28)

native('02-creator-ui',3.6,7.2)
caption('CRIE SEU PERSONAGEM.','Pele, cabelo, cor e expressão.',3.6,7.2,True)
native('03-avatar',7.2,9.6)
caption('DO SEU JEITO.','Um avatar para viver as histórias do bairro.',7.2,9.6)
native('04-home',9.6,12)
caption('COMECE EM CASA.','Saia pela sua porta. Conheça o bairro.',9.6,12)
native('05-neighborhood',12,15.6)
caption('ENCONTRE SUA RUA.','Uma cidade noturna com identidade própria.',12,15.6)
native('06-music',15.6,18)
caption('CADA RUA TEM SUA HISTÓRIA.','Comércio, música e encontros pelo caminho.',15.6,18)
native('07-open-shop',18,19.8)
caption('ABRA A DRIPLAB.','O atendimento começa quando você decide.',18,19.8,True)
native('08-arrival',19.8,21,offset=45)
caption('RECEBA OS CLIENTES.','',19.8,21)
native('09-dialogue',21,24.6)
caption('ENTENDA O PEDIDO.','Cada cliente chega com uma ocasião.',21,24.6,True)
native('10-fitting-walk',24.6,27.6)
caption('DO BALCÃO AO PROVADOR.','',24.6,27.6)
native('10-fitting-walk',27.6,29.4,tail=True)
native('11-fit-ui',29.4,35.4)
caption('MONTE O FIT.','Experimente peças e interprete as pistas.',29.4,35.4,True)
native('12-finished-exit',35.4,39)
caption('O VISUAL GANHA A RUA.','O cliente sai com a combinação que você criou.',35.4,39)
native('13-result',39,42)
# Keep the result's right presentation margin clean of a partly entering transient toast.
color('Result right inset',39,42,(15/255,15/255,15/255),3,1,sx=32/W,sy=100/H,ox=1206-W/2,oy=H/2-594)
caption('VEJA O RESULTADO.','Foto, avaliação e Reputação no FitFeed.',39,42,True)
native('14-fuba',42,44.4)
caption('COM FUBÁ NA EQUIPE.','',42,44.4)
native('15-ending',44.4,48)
color('Closing charcoal',44.4,48,(.006,.008,.009),3,.81)
logo(44.5,47.9,W/2,430,330)
txt('A game by DripLab',44.65,47.8,23,x=.5,y=.32,center=True)
txt('SEU OLHAR. O FIT DE ALGUÉM.',44.85,47.8,29,x=.5,y=.205,center=True)
txt('EM DESENVOLVIMENTO  /  BUILD DE TESTE 01',45,47.8,16,x=.5,y=.115,center=True,tint=RED)

# Same original instrumental as the previous edit; game-event foley exported from this build.
music=OUT/'FitCheck-teaser-beat.wav'
if not music.exists():shutil.copy2(PROJECT/'Captures/Teaser/FitCheck-original-teaser-beat.wav',music)
with wave.open(str(music),'rb') as w:assert w.getnframes()==48*44100 and w.getnchannels()==2
song=seq.new_sound('Original 100 BPM edit score',str(music),channel=15,frame_start=1);song.volume=.72
for t,value in [(0,.72),(20.8,.72),(21,.50),(24.6,.72),(39,.5),(42,.6),(44.4,.72)]:song.volume=value;song.keyframe_insert(data_path='volume',frame=fr(t))
def sound(name,t,volume=.5):
    path=SRC/'audio'/(name+'.wav');s=seq.new_sound(name,str(path),channel=16,frame_start=fr(t));s.volume=volume
for t in (3.6,4.8,6,18,21,29.4,31.2,33):sound('click',t,.38)
sound('door',9.6,.34);sound('door',19.8,.40);sound('curtain',28,.58);sound('photo',39,.48);sound('award',40.25,.40)
cat=seq.new_sound('Fuba recorded meow',str(PROJECT/'Assets/Resources/Audio/CatMeow.wav'),channel=17,frame_start=fr(42+20/30));cat.volume=.48

manifest={'duration_seconds':48,'fps':30,'resolution':[1280,720],
          'source':'New native Unity captures of current Teste 01 content; deterministic capture timing; real menus and complete customer flow.',
          'story':['storefront','character creation','home','neighborhood','shop opening','request','fitting room','outfit selection','customer departure','10/10 result','Fuba'],
          'gameplay_frames':(SRC/'capture-manifest.txt').read_text().splitlines(),
          'score':'Original 100 BPM instrumental reused from the previous video; editing soundtrack, not an assertion of integrated game music.',
          'sfx':'Current game-generated foley and recorded CatMeow; Kerzoven CC0, see Art/Audio/CREDITS.md.',
          'ui_aspect':'Original 16:10 UI preserved with side bars; world footage 16:9.',
          'future_features_advertised':False,'release_date_claim':False}
(OUT/'teaser-manifest.json').write_text(json.dumps(manifest,ensure_ascii=False,indent=2),encoding='utf-8')
bpy.ops.wm.save_as_mainfile(filepath=str(OUT/'FitCheck-Teaser-Atual.blend'),compress=True)
if '--preview' in sys.argv:
    scene.render.image_settings.media_type='IMAGE';scene.render.image_settings.file_format='PNG'
    for t in (1.8,5.4,8.4,13.2,16.8,22.8,28.8,32.4,40.2,43.2,46.2):
        scene.frame_set(fr(t));scene.render.filepath=str(OUT/f'preview-{t:04.1f}.png');bpy.ops.render.render(write_still=True)
else:bpy.ops.render.render(animation=True)
print('CURRENT_TEASER_EDIT_COMPLETE',scene.render.filepath,flush=True)
