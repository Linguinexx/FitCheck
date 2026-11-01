using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        static string FirstPersonProofOutput()
        {
            var args=Environment.GetCommandLineArgs();for(int i=0;i<args.Length-1;i++)if(args[i]=="-proof-output")return args[i+1];
            return Path.Combine(Application.temporaryCachePath,"FitCheck-FirstPersonProof");
        }
        IEnumerator CaptureFirstPersonProof()
        {
            yield return null;yield return null;
            string folder=FirstPersonProofOutput();Directory.CreateDirectory(folder);
            try{
                EnterCity();paused=false;city.Frame(cam,true,0);SaveFirstPersonFrame(Path.Combine(folder,"first-person-preview.png"));Home();UpdateCamera();
                CitySmokeCheck();Debug.Log("FITCHECK_FACE_RECOVERY_PASSED: "+studio.ValidateFaceReloadRecovery()+" expressions after lost bindings; original meshes recovered and retained outside garment assets.");ArtSmokeCheck();RoundedAvatarSmokeCheck();HairFitSmokeCheck();PostureSmokeCheck();RefinementSmokeCheck();EyelidSmokeCheck();PreviewSubmissionSmokeCheck();CreatorSmokeCheck(folder);
                if(Array.IndexOf(Environment.GetCommandLineArgs(),"-city-scale-review")>=0){CaptureCityScaleReview(folder);File.WriteAllText(Path.Combine(folder,"checks.txt"),"CITY_SCALE_REVIEW_PASSED: baseline checks and five native views; shop geometry preserved.");Application.Quit(0);yield break;}
                if(Array.IndexOf(Environment.GetCommandLineArgs(),"-shop-review")>=0){CaptureShopReview(folder);CheckHomeCommute(folder);CheckSaveSlots(folder);Application.Quit(0);yield break;}
                CaptureHairReview(folder);
                if(Array.IndexOf(Environment.GetCommandLineArgs(),"-body-review")>=0){CaptureBodyMotionReview(folder);File.WriteAllText(Path.Combine(folder,"checks.txt"),"BODY_REVIEW_PASSED: fringe may cover brows; NPC speed +50%; player speed 4.4; player outfit visible looking down at 88 degrees; full-body gait, ankle contact, runtime recovery and reduced-motion simulation.");Application.Quit(0);yield break;}
                if(Array.IndexOf(Environment.GetCommandLineArgs(),"-expression-review")>=0){ExpressionLifeReview(folder);File.WriteAllText(Path.Combine(folder,"checks.txt"),"EXPRESSION_LIFE_PASSED: visible sculpted brows; hair profiles; reduced-motion NPC simulation; reload recovery; default unfiltered render.");Application.Quit(0);yield break;}
                if(Array.IndexOf(Environment.GetCommandLineArgs(),"-hair-review")>=0){File.WriteAllText(Path.Combine(folder,"checks.txt"),"HAIR_REVIEW_PASSED: complete baseline checks, creator persistence, hats and 18 native hair renders.");Application.Quit(0);yield break;}
                EnterCity();paused=false;city.SetView(40,-14);city.Frame(cam,true,0);SaveFirstPersonFrame(Path.Combine(folder,"first-person-driplab.png"));
                MoveProof(new Vector2(-19,-.5f),20,-7);SaveFirstPersonFrame(Path.Combine(folder,"first-person-cafe.png"));
                MoveProof(new Vector2(-15.4f,-.8f),0,0);SaveFirstPersonFrame(Path.Combine(folder,"first-person-character.png"));
                city.SetView(5,55);city.Frame(cam,true,0);SaveFirstPersonFrame(Path.Combine(folder,"first-person-pavement.png"));
                MoveProof(CityView.Spawn,335,-18);SaveFirstPersonFrame(Path.Combine(folder,"city-skyline.png"));
                MoveProof(new Vector2(-3,-15),0,-8);
                var residentBefore=city.ResidentPosition(2);float timeBefore=city.AmbientTime;
                for(int frame=0;frame<400;frame++){city.Frame(cam,false,.05f);city.CheckResidents();}
                if(Vector3.Distance(residentBefore,city.ResidentPosition(2))<3)throw new Exception("Moradores não percorrem o bairro");
                var residentPaused=city.ResidentPosition(2);float frozenTime=city.AmbientTime;
                city.Frame(cam,false,0);
                if(city.ResidentPosition(2)!=residentPaused)throw new Exception("Pausa moveu NPC");
                for(int step=0;step<180;step++)city.Frame(cam,true,.05f);
                if(Vector3.Distance(city.ResidentPosition(2),residentPaused)<.1f)throw new Exception("Movimento reduzido paralisou NPC");
                if(city.AmbientTime!=frozenTime)throw new Exception("Pausa/movimento reduzido alterou atividade");
                SaveFirstPersonFrame(Path.Combine(folder,"night-activity.png"));
                string motion=Path.Combine(folder,"motion");Directory.CreateDirectory(motion);
                for(int frame=0;frame<180;frame++){
                    city.Frame(cam,false,1/30f);
                    SaveFirstPersonFrame(Path.Combine(motion,frame.ToString("D3")+".png"),640,360);
                }
                for(int pose=0;pose<6;pose++){
                    for(int tick=0;tick<9;tick++)city.Frame(cam,false,1/30f);
                    var resident=city.ResidentPosition(2);var focus=CityView.Origin+resident+Vector3.up*.65f;
                    cam.transform.position=focus+new Vector3(2.8f,.8f,-3);cam.transform.LookAt(focus);
                    SaveFirstPersonFrame(Path.Combine(folder,"stride-"+pose+".png"));
                }
                MoveProof(new Vector2(-3,-10.2f),232,5);SaveFirstPersonFrame(Path.Combine(folder,"night-traffic.png"));
                city.CheckResidents();Debug.Log("FITCHECK_NIGHT_ACTIVITY_PASSED: residents walking with articulated limbs, architecture clearance, traffic, wind, radio, pause and reduced motion; native motion frames.");
                File.WriteAllText(Path.Combine(folder,"checks.txt"),"FIRST_PERSON_PROOF_PASSED: skybox support/restoration, animated sky pause, city, camera/controls, cursor, pause, player body visible looking down, optional 720-line filter, mipmapped pavement, art, expressions and preview submission. Native game renders 1280x720.");
                Debug.Log("FIRST_PERSON_PROOF_PASSED");
                Home();OpenCreator();creatorDraft.name="Duda";creatorDraft.skin=3;creatorDraft.hair=5;creatorDraft.hairColor=2;creatorDraft.expression=1;RefreshCreator();
                StartCoroutine(CaptureCreatorInterface(folder));
            }catch(Exception exception){File.WriteAllText(Path.Combine(folder,"checks.txt"),exception.ToString());Debug.LogException(exception);Application.Quit(2);}
        }
        IEnumerator CaptureCreatorInterface(string folder)
        {
            yield return null;yield return null;
            string path=Path.Combine(folder,"character-creator-ui.png");ScreenCapture.CaptureScreenshot(path);
            for(int i=0;i<12&&!File.Exists(path);i++)yield return null;
            if(!File.Exists(path)){Debug.LogError("Creator UI capture missing");Application.Quit(2);}
            else{Debug.Log("FITCHECK_CREATOR_UI_CAPTURED");Application.Quit(0);}
        }
        void MoveProof(Vector2 target,float yaw,float pitch)
        {
            if(!city.RequestWalk(target))throw new Exception("Proof destination blocked: "+target);
            int frames=0;while(Vector2.Distance(city.Position,target)>.03f&&frames++<4000)city.Step(Vector2.zero,1/60f,false,mannequin);
            if(frames>=4000)throw new Exception("Proof destination unreachable");city.SetView(yaw,pitch);city.Frame(cam,true,0);
        }
        void SaveFirstPersonFrame(string path,int width=1280,int height=720)
        {
            var oldTarget=cam.targetTexture;var oldActive=RenderTexture.active;float oldAspect=cam.aspect;
            var target=RenderTexture.GetTemporary(width,height,24,RenderTextureFormat.ARGB32);var image=new Texture2D(width,height,TextureFormat.RGB24,false);
            try{
                cam.targetTexture=target;cam.aspect=width/(float)height;cam.Render();RenderTexture.active=target;image.ReadPixels(new Rect(0,0,width,height),0,0);image.Apply();File.WriteAllBytes(path,image.EncodeToPNG());
            }finally{cam.targetTexture=oldTarget;cam.aspect=oldAspect;RenderTexture.active=oldActive;RenderTexture.ReleaseTemporary(target);Destroy(image);}
        }
    }
}
