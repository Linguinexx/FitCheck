using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        float arrivalFade,photoFlash,footDistance;Vector2 lastFootPosition;CityView lastFootSpace;
        AudioClip doorFx,stepStone,stepWood,clothFx,windFx;AudioSource ambience,music,incomingMusic;
        float musicBlend;
        const float MusicFadeSeconds=1.6f;
        AudioClip[] tracks, backgrounds;
        AudioClip[][] steps;
        int stepIndex;
        string exitDoor="door_casa_close";ParticleSystem motes;Material moteMaterial;Texture2D moteTexture;
        Stage presentationStage;float lastCurtainProgress;
        int curtainDirection;
        void BuildPresentation()
        {
            doorFx=Foley("Porta e trinco",.36f,170,.25f);stepStone=Foley("Passo na rua",.12f,110,.10f);stepWood=Foley("Passo no piso",.15f,85,.08f);clothFx=Foley("Cortina de tecido",.5f,50,.28f);windFx=Foley("Ar do bairro",8,28,.015f,true);
            ambience=gameObject.AddComponent<AudioSource>();ambience.clip=windFx;ambience.loop=true;ambience.playOnAwake=false;ambience.volume=.23f;ambience.spatialBlend=0;
            music=CreateMusicSource();
            incomingMusic=CreateMusicSource();
            tracks=new[]{Clip("mus_menu"),Clip("mus_bairro_noite"),Clip("mus_quarto"),Clip("mus_loja")};
            backgrounds=new[]{null,Clip("amb_bairro_noite")??windFx,Clip("amb_quarto_noite"),Clip("amb_loja_interior")};
            steps=new[]{Steps("calcada"),Steps("madeira"),Steps("loja")};
            BuildAdditionalSounds();
            var go=new GameObject("Poeira na luz");go.transform.SetParent(cam.transform,false);go.transform.localPosition=new Vector3(0,0,5);motes=go.AddComponent<ParticleSystem>();
            var main=motes.main;main.playOnAwake=false;main.maxParticles=28;main.startLifetime=7;main.startSize=new ParticleSystem.MinMaxCurve(.012f,.028f);main.startSpeed=.018f;main.simulationSpace=ParticleSystemSimulationSpace.World;main.startColor=new Color(.85f,.77f,.53f,.22f);
            var emission=motes.emission;emission.rateOverTime=3;
            var shape=motes.shape;shape.shapeType=ParticleSystemShapeType.Box;shape.scale=new Vector3(10,4,8);
            moteTexture=new Texture2D(16,16,TextureFormat.RGBA32,false);var pixels=new Color[256];for(int y=0;y<16;y++)for(int x=0;x<16;x++){float a=Mathf.Clamp01(1-Vector2.Distance(new Vector2(x,y),new Vector2(7.5f,7.5f))/7.5f);pixels[y*16+x]=new Color(1,1,1,a*a);}moteTexture.SetPixels(pixels);moteTexture.Apply();
            moteMaterial=new Material(Resources.Load<Shader>("DustParticle"));moteMaterial.mainTexture=moteTexture;go.GetComponent<ParticleSystemRenderer>().sharedMaterial=moteMaterial;
            motes.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        static AudioClip Clip(string id)=>Resources.Load<AudioClip>("Audio/Game/"+id);
        static AudioClip[] Steps(string surface)
        {
            var clips=new AudioClip[4];
            for(int i=0;i<clips.Length;i++)clips[i]=Clip($"step_{surface}_{i+1:00}");
            return clips;
        }
        void PlayClip(AudioClip clip,float volume=1)
        {
            if(clip!=null&&effects!=null&&!mutedAudio&&!paused)effects.PlayOneShot(clip,volume);
        }
        AudioSource CreateMusicSource()
        {
            var source=gameObject.AddComponent<AudioSource>();
            source.playOnAwake=false;
            source.loop=true;
            source.volume=0;
            return source;
        }
        void TickMusic(AudioClip target,float delta,bool hold)
        {
            if(incomingMusic.clip!=null&&target==music.clip){
                (music,incomingMusic)=(incomingMusic,music);
                musicBlend=1-musicBlend;
            }
            if(incomingMusic.clip==null&&music.clip!=target){
                incomingMusic.clip=target;
                musicBlend=0;
            }
            if(!hold&&incomingMusic.clip!=null)
                musicBlend=Mathf.MoveTowards(musicBlend,1,delta/MusicFadeSeconds);
            float angle=musicBlend*Mathf.PI*.5f;
            float volume=.26f*musicVolume;
            music.volume=volume*Mathf.Cos(angle);
            incomingMusic.volume=volume*Mathf.Sin(angle);
            TickLoop(music,music.clip,hold);
            TickLoop(incomingMusic,incomingMusic.clip,hold);
            if(musicBlend>=1){
                music.Stop();
                music.clip=null;
                (music,incomingMusic)=(incomingMusic,music);
                musicBlend=0;
            }
        }
        static void TickLoop(AudioSource source,AudioClip clip,bool hold)
        {
            if(source.clip!=clip){source.Stop();source.clip=clip;}
            if(clip==null)return;
            if(hold){if(source.isPlaying)source.Pause();return;}
            if(!source.isPlaying){source.UnPause();if(!source.isPlaying)source.Play();}
        }
        AudioClip Foley(string name,float duration,float low,float amount,bool loop=false)
        {
            const int hz=22050;int count=Mathf.CeilToInt(duration*hz);var data=new float[count];var random=new System.Random(name.Length*139);float filtered=0;
            for(int i=0;i<count;i++){
                float t=i/(float)hz;filtered=Mathf.Lerp(filtered,(float)random.NextDouble()*2-1,.12f);
                float envelope=loop?.5f+.25f*Mathf.Sin(t*Mathf.PI*2/duration):Mathf.Min(1,t/.008f)*Mathf.Pow(1-t/duration,2);
                data[i]=(filtered*amount+Mathf.Sin(t*low*Mathf.PI*2)*Mathf.Exp(-t*35)*.17f)*envelope;
                if(loop)data[i]*=Mathf.Sin(t/duration*Mathf.PI)*Mathf.Sin(t/duration*Mathf.PI);
            }
            var clip=AudioClip.Create(name,count,1,hz,false);clip.SetData(data,0);return clip;
        }
        void ArrivalPresentation()
        {
            arrivalFade=reducedMotion?.15f:1;footDistance=0;lastFootSpace=city;if(city!=null)lastFootPosition=city.Position;
            lastCurtainProgress=city!=null?city.FittingProgress:0;curtainDirection=0;
            if(motes!=null)motes.Clear();if(!titleStarting&&city!=null)PlayClip(Clip(city.IsHome?"door_casa_open":city.IsInterior?"door_loja_open":exitDoor)??doorFx,.65f);
            if(city!=null&&city.IsInterior)exitDoor=city.IsHome?"door_casa_close":"door_loja_close";
        }
        void TickPresentation(float delta)
        {
            TickAdditionalSounds(delta);
            if(!paused){arrivalFade=Mathf.MoveTowards(arrivalFade,0,delta*2.5f);photoFlash=Mathf.MoveTowards(photoFlash,0,delta*2);}
            if(presentationStage!=stage){
                if(stage==Stage.CharacterCreator||stage==Stage.Welcome||stage==Stage.Title)arrivalFade=reducedMotion?.1f:.65f;
                if(stage==Stage.Post&&record!=null){PlayClip(Clip("feed_publish"),.30f);PlayClip(Clip(record.score==10?"result_10":record.score>=6?"result_7":record.score>0?"result_3":"result_0"));}
                presentationStage=stage;
            }
            bool world=stage==Stage.City&&city!=null;
            if(world){
                if(lastFootSpace!=city){lastFootSpace=city;lastFootPosition=city.Position;footDistance=0;}
                float distance=Vector2.Distance(lastFootPosition,city.Position);lastFootPosition=city.Position;
                if(!paused&&!showShopDecor&&distance<1){footDistance+=distance;if(footDistance>(city.Running?1.55f:1.25f)){
                    footDistance=0;var bank=steps[city.IsHome?1:city.IsInterior?2:0];
                    PlayClip(bank[stepIndex]??(city.IsInterior?stepWood:stepStone),.65f);
                    stepIndex=(stepIndex+1)%bank.Length;
                }}
                if(!paused&&!reducedMotion&&city.Running&&!ShopSequenceActive)cam.fieldOfView=CityView.ViewFieldOfView+3;
                if(city.IsInterior&&!city.IsHome){
                    float progress=city.FittingProgress;
                    int direction=progress>lastCurtainProgress?1:progress<lastCurtainProgress?-1:0;
                    if(direction!=0&&direction!=curtainDirection)
                        PlayClip(Clip(direction>0?"curtain_open":"curtain_close")??clothFx,.65f);
                    lastCurtainProgress=progress;
                    curtainDirection=direction;
                }
            }
            bool menu=stage==Stage.Title||stage==Stage.Welcome||stage==Stage.CharacterCreator;
            bool activeWorld=city!=null&&city.gameObject.activeInHierarchy;
            int zone=menu?0:activeWorld?(city.IsHome?2:city.IsInterior?3:1):3;
            TickMusic(tracks[zone],delta,paused||mutedAudio);
            TickLoop(ambience,activeWorld&&!menu?backgrounds[zone]:null,paused||mutedAudio);
            AudioListener.volume=masterVolume;
            effects.mute=mutedAudio||paused;
            bool dust=world&&!reducedMotion;
            if(!dust){if(motes.isPlaying)motes.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);}
            else if(paused){if(motes.isPlaying)motes.Pause();}else if(!motes.isPlaying)motes.Play();
        }
        void DrawPresentation()
        {
            if(!paused&&arrivalFade>.001f)Fill(new Rect(0,0,1440,900),new Color(.025f,.03f,.025f,Mathf.SmoothStep(0,1,arrivalFade)));
            if(!paused&&photoFlash>.001f&&!reducedMotion)Fill(new Rect(0,0,1440,900),new Color(.94f,.91f,.82f,photoFlash));
        }
        void DisposePresentation(){AudioListener.volume=1;foreach(var clip in new[]{doorFx,stepStone,stepWood,clothFx,windFx})if(clip!=null)Destroy(clip);if(moteMaterial!=null)Destroy(moteMaterial);if(moteTexture!=null)Destroy(moteTexture);}
    }
}
