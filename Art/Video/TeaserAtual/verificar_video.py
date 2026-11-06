"""Decode the final MP4 and verify timing, sound and representative rendered frames."""
import bpy,aud,json
import numpy as np
from pathlib import Path
project=Path(__file__).resolve().parents[3];root=project/'Captures/TeaserAtual';path=root/'FitCheck-Teaser-Atual.mp4'
assert path.stat().st_size>1_000_000
scene=bpy.context.scene;scene.sequence_editor_create()
movie=scene.sequence_editor.strips.new_movie('Final encoded current-game teaser',str(path),channel=1,frame_start=1)
meta={'frames':movie.frame_duration,'fps':movie.fps,'width':movie.elements[0].orig_width,'height':movie.elements[0].orig_height,'bytes':path.stat().st_size}
assert meta['frames']==1440 and abs(meta['fps']-30)<.01,meta
assert (meta['width'],meta['height'])==(1280,720),meta
container=path.read_bytes();assert b'avc1' in container and b'mp4a' in container
pcm=aud.Sound.file(str(path)).data();meta['audio_peak']=float(np.max(np.abs(pcm)));meta['audio_rms']=float(np.sqrt(np.mean(pcm**2)))
assert .05<meta['audio_peak']<1.05 and .01<meta['audio_rms']<.5,meta
scene.render.resolution_x=1280;scene.render.resolution_y=720;scene.render.resolution_percentage=100
scene.render.fps=30;scene.render.use_sequencer=True;scene.view_settings.view_transform='Standard'
scene.render.image_settings.media_type='IMAGE';scene.render.image_settings.file_format='PNG'
checks=(1.8,5.4,13.2,22.8,28.8,32.4,40.2,43.2,46.2)
for t in checks:
    scene.frame_set(round(t*30)+1);target=root/f'encoded-check-{t:04.1f}.png';scene.render.filepath=str(target);bpy.ops.render.render(write_still=True)
    im=bpy.data.images.load(str(target),check_existing=False);rgb=np.asarray(im.pixels[:],dtype=np.float32).reshape(-1,4)[:,:3]
    assert rgb.mean()>.015 and rgb.std()>.04,(t,rgb.mean(),rgb.std())
meta['decoded_frames_checked']=list(checks);meta['codecs']=['H.264','AAC'];meta['duration_seconds']=48
(root/'teaser-validation.json').write_text(json.dumps(meta,indent=2),encoding='utf-8')
print('CURRENT_TEASER_VIDEO_VERIFIED',json.dumps(meta),flush=True)
