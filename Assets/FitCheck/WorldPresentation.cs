using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        float arrivalFade,photoFlash,footDistance;Vector2 lastFootPosition;CityView lastFootSpace;
        AudioClip doorFx,stepStone,stepWood,clothFx,windFx;AudioSource ambience;ParticleSystem motes;Material moteMaterial;Texture2D moteTexture;
        Stage presentationStage;bool curtainWasOpening;
        void BuildPresentation()
        {
            doorFx=Foley("Porta e trinco",.36f,170,.25f);stepStone=Foley("Passo na rua",.12f,110,.10f);stepWood=Foley("Passo no piso",.15f,85,.08f);clothFx=Foley("Cortina de tecido",.5f,50,.28f);windFx=Foley("Ar do bairro",8,28,.015f,true);
            ambience=gameObject.AddComponent<AudioSource>();ambience.clip=windFx;ambience.loop=true;ambience.playOnAwake=false;ambience.volume=.23f;ambience.spatialBlend=0;
            var go=new GameObject("Poeira na luz");go.transform.SetParent(cam.transform,false);go.transform.localPosition=new Vector3(0,0,5);motes=go.AddComponent<ParticleSystem>();
            var main=motes.main;main.playOnAwake=false;main.maxParticles=28;main.startLifetime=7;main.startSize=new ParticleSystem.MinMaxCurve(.012f,.028f);main.startSpeed=.018f;main.simulationSpace=ParticleSystemSimulationSpace.World;main.startColor=new Color(.85f,.77f,.53f,.22f);
            var emission=motes.emission;emission.rateOverTime=3;
            var shape=motes.shape;shape.shapeType=ParticleSystemShapeType.Box;shape.scale=new Vector3(10,4,8);
            moteTexture=new Texture2D(16,16,TextureFormat.RGBA32,false);var pixels=new Color[256];for(int y=0;y<16;y++)for(int x=0;x<16;x++){float a=Mathf.Clamp01(1-Vector2.Distance(new Vector2(x,y),new Vector2(7.5f,7.5f))/7.5f);pixels[y*16+x]=new Color(1,1,1,a*a);}moteTexture.SetPixels(pixels);moteTexture.Apply();
            moteMaterial=new Material(Resources.Load<Shader>("DustParticle"));moteMaterial.mainTexture=moteTexture;go.GetComponent<ParticleSystemRenderer>().sharedMaterial=moteMaterial;
            motes.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
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
            if(motes!=null)motes.Clear();if(!titleStarting&&effects!=null&&!mutedAudio&&doorFx!=null)effects.PlayOneShot(doorFx,.65f);
        }
        void TickPresentation(float delta)
        {
            if(!paused){arrivalFade=Mathf.MoveTowards(arrivalFade,0,delta*2.5f);photoFlash=Mathf.MoveTowards(photoFlash,0,delta*2);}
            if(presentationStage!=stage){
                if(stage==Stage.CharacterCreator||stage==Stage.Welcome||stage==Stage.Title)arrivalFade=reducedMotion?.1f:.65f;
                presentationStage=stage;
            }
            bool world=stage==Stage.City&&city!=null;
            if(world){
                if(lastFootSpace!=city){lastFootSpace=city;lastFootPosition=city.Position;footDistance=0;}
                float distance=Vector2.Distance(lastFootPosition,city.Position);lastFootPosition=city.Position;
                if(!paused&&!showShopDecor&&distance<1){footDistance+=distance;if(footDistance>(city.Running?1.55f:1.25f)){
                    footDistance=0;if(!mutedAudio)effects.PlayOneShot(city.IsInterior?stepWood:stepStone,.40f);
                }}
                if(!paused&&!reducedMotion&&city.Running&&!ShopSequenceActive)cam.fieldOfView=CityView.ViewFieldOfView+3;
                bool opening=shopFittingOpening||shopSequence==ShopSequence.EnteringFitting;
                if(opening&&!curtainWasOpening&&!mutedAudio)effects.PlayOneShot(clothFx,.65f);curtainWasOpening=opening;
            }
            bool air=world&&!city.IsInterior&&!paused&&!mutedAudio;
            if(air){if(!ambience.isPlaying)ambience.UnPause();if(!ambience.isPlaying)ambience.Play();}else if(ambience.isPlaying)ambience.Pause();
            bool dust=world&&!reducedMotion;
            if(!dust){if(motes.isPlaying)motes.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);}
            else if(paused){if(motes.isPlaying)motes.Pause();}else if(!motes.isPlaying)motes.Play();
        }
        void DrawPresentation()
        {
            if(!paused&&arrivalFade>.001f)Fill(new Rect(0,0,1440,900),new Color(.025f,.03f,.025f,Mathf.SmoothStep(0,1,arrivalFade)));
            if(!paused&&photoFlash>.001f&&!reducedMotion)Fill(new Rect(0,0,1440,900),new Color(.94f,.91f,.82f,photoFlash));
        }
        void DisposePresentation(){foreach(var clip in new[]{doorFx,stepStone,stepWood,clothFx,windFx})if(clip!=null)Destroy(clip);if(moteMaterial!=null)Destroy(moteMaterial);if(moteTexture!=null)Destroy(moteTexture);}
    }
}
