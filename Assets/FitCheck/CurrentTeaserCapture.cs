using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        const float TeaserDelta=1f/30;
        RenderTexture teaserTarget;Texture2D teaserPixels;
        readonly List<string> teaserClips=new List<string>();
        void TeaserClock()
        {
            studio.Animate(TeaserDelta,true);cam.GetComponent<RetroCamera>().AdvancePaint(TeaserDelta);TickContext(TeaserDelta);
            if(city!=null&&city.gameObject.activeSelf)city.TickCat(TeaserDelta,false,false,true);
        }
        void TeaserFrame(string path,bool ui=false)
        {
            if(ui){CaptureInterface(path);return;}
            var previous=cam.targetTexture;var active=RenderTexture.active;float aspect=cam.aspect;
            try{cam.targetTexture=teaserTarget;cam.aspect=16f/9;cam.Render();RenderTexture.active=teaserTarget;teaserPixels.ReadPixels(new Rect(0,0,1280,720),0,0);teaserPixels.Apply();File.WriteAllBytes(path,teaserPixels.EncodeToJPG(87));}
            finally{cam.targetTexture=previous;cam.aspect=aspect;RenderTexture.active=active;}
        }
        void TeaserClip(string folder,string name,int count,Action<int> tick,bool ui=false)
        {
            string output=Path.Combine(folder,name);Directory.CreateDirectory(output);
            for(int frame=0;frame<count;frame++){tick(frame);TeaserClock();TeaserFrame(Path.Combine(output,frame.ToString("D4")+".jpg"),ui);}
            teaserClips.Add(name+":"+count);Debug.Log("CURRENT_TEASER_CLIP: "+name+"; "+count+" native frames");
        }
        void ExportTeaserSound(string folder,string name,AudioClip clip)
        {
            var data=new float[clip.samples*clip.channels];if(!clip.GetData(data,0))throw new Exception("Missing sound samples: "+name);
            using(var writer=new BinaryWriter(File.Create(Path.Combine(folder,name+".wav")))){
                int size=data.Length*2;writer.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));writer.Write(36+size);writer.Write(System.Text.Encoding.ASCII.GetBytes("WAVEfmt "));writer.Write(16);writer.Write((short)1);writer.Write((short)clip.channels);writer.Write(clip.frequency);writer.Write(clip.frequency*clip.channels*2);writer.Write((short)(clip.channels*2));writer.Write((short)16);writer.Write(System.Text.Encoding.ASCII.GetBytes("data"));writer.Write(size);foreach(float sample in data)writer.Write((short)Mathf.RoundToInt(Mathf.Clamp(sample,-1,1)*32767));
            }
        }
        void CaptureCurrentTeaser(string folder)
        {
            var original=progress;string oldStorage=storageOverride;var oldLook=playerAppearance;
            try{
                Home();progress=new DayProgress();record=new Attempt();storageOverride=Path.Combine(folder,"capture-session");Directory.CreateDirectory(storageOverride);
                playerAppearance=new PlayerAppearance{created=true,name="Lia",skin=3,hair=5,hairColor=2,expression=4};studio.PlayerLook=playerAppearance;
                reducedMotion=false;retroFilter=false;paused=false;cam.GetComponent<RetroCamera>().Filter=false;
                teaserTarget=new RenderTexture(1280,720,24);teaserTarget.Create();teaserPixels=new Texture2D(1280,720,TextureFormat.RGB24,false);
                string audio=Path.Combine(folder,"audio");Directory.CreateDirectory(audio);
                ExportTeaserSound(audio,"click",clickSound);ExportTeaserSound(audio,"door",doorFx);ExportTeaserSound(audio,"curtain",clothFx);ExportTeaserSound(audio,"photo",photoSound);ExportTeaserSound(audio,"award",awardSound);
                StartLiveTitle();TeaserClip(folder,"01-facade",108,i=>FrameLiveTitle(TeaserDelta));Home();
                OpenCreator(Stage.Home);creatorDraft.name="Lia";
                TeaserClip(folder,"02-creator-ui",108,i=>{
                    if(i==0||i==36||i==72){int variant=i/36;creatorDraft.skin=new[]{1,3,2}[variant];creatorDraft.hair=new[]{0,5,6}[variant];creatorDraft.hairColor=new[]{2,0,3}[variant];creatorDraft.expression=new[]{0,4,1}[variant];RefreshCreator();}
                    rotation=-15+Mathf.Sin(i*.035f)*12;UpdateCamera();
                },true);
                TeaserClip(folder,"03-avatar",72,i=>{mannequin.rotation=Quaternion.Euler(0,-25+i*.7f,0);cam.transform.position=new Vector3(1.2f,2.1f,-5.4f);cam.transform.LookAt(new Vector3(0,1.7f,0));cam.fieldOfView=39;});
                if(!SaveCreator())throw new Exception("Could not stage creator-to-home flow");
                MoveProof(new Vector2(0,-2.3f),-40,8);
                TeaserClip(folder,"04-home",72,i=>{city.SetView(-40+i*.6f,8);city.Frame(cam,false,TeaserDelta);});
                Home();EnterCity();MoveProof(new Vector2(-103,-3.5f),65,-7);
                TeaserClip(folder,"05-neighborhood",108,i=>{city.SetView(65,-7);city.Step(new Vector2(0,.5f),TeaserDelta,false,mannequin);city.Frame(cam,false,TeaserDelta);});
                MoveProof(new Vector2(-38,-10.5f),145,-9);
                TeaserClip(folder,"06-music",72,i=>{city.SetView(145+i*.14f,-9);city.Frame(cam,false,TeaserDelta);});
                EnterShop();MoveProof(CityView.ShopCounter,180,0);TickContext(3);
                TeaserClip(folder,"07-open-shop",54,i=>{city.Frame(cam,false,TeaserDelta);},true);
                if(!InteractShop()||!ShopOpen)throw new Exception("Could not open shop for capture");
                string arrival=Path.Combine(folder,"08-arrival");Directory.CreateDirectory(arrival);int frames=0;
                while(ShopArrivalActive&&frames<1400){city.Frame(cam,false,TeaserDelta);UpdateShopSequence(TeaserDelta);TeaserClock();TeaserFrame(Path.Combine(arrival,(frames++).ToString("D4")+".jpg"));}
                if(!city.CustomerWaitingAtCounter)throw new Exception("Customer arrival not complete");teaserClips.Add("08-arrival:"+frames);city.CheckCustomerClothing(true);
                MoveProof(CityView.ShopCounter,180,0);if(!InteractShop()||!shopService)throw new Exception("Could not start conversation");question=1;
                TeaserClip(folder,"09-dialogue",108,i=>{shopCameraTime+=TeaserDelta;FrameShopDialogue();},true);
                ProceedToFitting();string fitting=Path.Combine(folder,"10-fitting-walk");Directory.CreateDirectory(fitting);frames=0;
                while(stage==Stage.City&&frames<1400){city.Frame(cam,false,TeaserDelta);UpdateShopSequence(TeaserDelta);TeaserClock();TeaserFrame(Path.Combine(fitting,(frames++).ToString("D4")+".jpg"));}
                if(stage!=Stage.Dressing||!shopDressingInStore)throw new Exception("Fitting sequence incomplete");teaserClips.Add("10-fitting-walk:"+frames);
                TeaserClip(folder,"11-fit-ui",180,i=>{
                    if(i==0){outfit=new[]{"SB_TOP_01","SB_BOTTOM_01","SB_SHOES_01",""};category=0;Apply();}
                    if(i==54){outfit=new[]{"CG_TOP_01","CG_BOTTOM_01","CG_SHOES_01",""};category=1;Apply();}
                    if(i==108){outfit=new[]{"AH_TOP_01","AH_BOTTOM_01","AH_SHOES_01",""};category=2;Apply();}
                    rotation=-8+Mathf.Sin(i*.04f)*12;shopDressingTime+=TeaserDelta;FrameShopDressing();
                },true);
                if(DayOne.Score(customer,outfit)!=10)throw new Exception("Final captured fit does not answer the request");
                Submit();city.CheckCustomerClothing(false);string exit=Path.Combine(folder,"12-finished-exit");Directory.CreateDirectory(exit);frames=0;
                while(stage==Stage.City&&frames<1800){city.Frame(cam,false,TeaserDelta);UpdateShopSequence(TeaserDelta);TeaserClock();TeaserFrame(Path.Combine(exit,(frames++).ToString("D4")+".jpg"));}
                if(stage!=Stage.Post||progress.posts.Count!=1||record.score!=10)throw new Exception("Captured fit result was not saved");teaserClips.Add("12-finished-exit:"+frames);
                TeaserClip(folder,"13-result",90,i=>{},true);
                Home();EnterShop();MoveProof(new Vector2(-3.2f,-2.6f),270,55);var cat=city.Cat;city.PlayerBodyVisible=false;
                TeaserClip(folder,"14-fuba",72,i=>{city.Frame(cam,false,TeaserDelta);city.PlayerBodyVisible=false;cam.transform.position=cat.transform.TransformPoint(new Vector3(-.9f,.95f,-1.9f));cam.transform.LookAt(cat.PetTarget);cam.fieldOfView=43;if(i==20&&!TryPetCat())throw new Exception("Cat interaction was not captured");});
                Home();StartLiveTitle();TeaserClip(folder,"15-ending",108,i=>FrameLiveTitle(TeaserDelta));
                File.WriteAllLines(Path.Combine(folder,"capture-manifest.txt"),teaserClips);
                File.WriteAllText(Path.Combine(folder,"checks.txt"),"CURRENT_TEASER_CAPTURED: native current-game motion and real UI; creator/home/neighborhood/shop/conversation/fitting/selected clothes/10-point result/cat; only isolated capture saves.");
                Debug.Log("CURRENT_TEASER_CAPTURED: "+teaserClips.Count+" clips; complete real customer flow; result 10/10");
            }finally{
                Home();progress=original;storageOverride=oldStorage;playerAppearance=oldLook;
                if(teaserTarget!=null){teaserTarget.Release();Destroy(teaserTarget);}if(teaserPixels!=null)Destroy(teaserPixels);if(interfaceVideoTexture!=null){Destroy(interfaceVideoTexture);interfaceVideoTexture=null;}
            }
        }
    }
}
