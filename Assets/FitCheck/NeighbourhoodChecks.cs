using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        void PlaceForStreetActivity(Vector3 target){
            var route=new List<Vector2>();bool found=false;
            for(int i=0;i<12;i++){
                float a=i*Mathf.PI/6;var p=new Vector2(target.x+Mathf.Sin(a)*2.35f,target.z-Mathf.Cos(a)*2.35f);
                if(!city.Free(p)||!city.PlanRoute(CityView.StreetShopDoor,p,route))continue;
                city.RestoreWalk(p,0,0,mannequin);city.Frame(cam,true,0);found=true;break;
            }
            if(!found)throw new Exception("Atividade inacessível: "+target);
            cam.transform.LookAt(city.transform.TransformPoint(target));var angles=cam.transform.eulerAngles;city.SetView(angles.y,Mathf.DeltaAngle(0,angles.x));city.Frame(cam,true,0);
        }
        void CheckNeighbourhoodActivities(string folder){
            storageOverride=Path.Combine(folder,"sessions",Guid.NewGuid().ToString("N"));Directory.CreateDirectory(storageOverride);
            Home();progress=new DayProgress();EnterCity();paused=false;cityFreeCursor=false;
            int residents=city.ResidentCount,lights=city.GetComponentsInChildren<Light>(true).Length;
            var route=new List<Vector2>();
            foreach(var point in new[]{CityView.StreetHomeDoor,new Vector2(-96.5f,28.5f),new Vector2(-76,39),new Vector2(-49,-18)})if(!city.PlanRoute(CityView.StreetShopDoor,point,route))throw new Exception("Passagem bloqueada");
            for(int sticker=0;sticker<7;sticker++){
                PlaceForStreetActivity(city.StickerPoint(sticker));if(!TryStreetActivity()||!StreetMemory.HasSticker(sticker))throw new Exception("Adesivo não coletado: "+sticker);
            }
            PlaceForStreetActivity(CityView.CollectiveMural);if(!TryStreetActivity()||!muralOpen)throw new Exception("Mural não abriu");
            contextClock+=1;muralDraft=127;city.ApplyStreetMemories(StreetMemory,muralDraft);FrameStreetActivities();CaptureInterface(Path.Combine(folder,"mural.png"));CloseStreetPanel();
            if(StreetMemory.placed!=0)throw new Exception("Cancelar gravou o mural");
            StreetMemory.placed=127;if(!TrySave())throw new Exception("Mural não salvou");city.ApplyStreetMemories(StreetMemory);
            PlaceForStreetActivity(city.ResidentPosition(6)+Vector3.up*2.2f);if(!TryStreetActivity()||city.ChallengeResident!=6)throw new Exception("Desafio não começou");
            for(int i=0;i<35;i++){city.Frame(cam,false,.05f);TickContext(.05f);}CaptureInterface(Path.Combine(folder,"emote-challenge.png"));
            city.PlayPlayerEmote(2);TickStreetActivities();if(StreetMemory.ChallengeDone(1))throw new Exception("Gesto errado venceu");city.CancelPlayerEmote(true);
            city.PlayPlayerEmote(3);TickStreetActivities();if(!StreetMemory.ChallengeDone(1)||city.ChallengeResident>=0)throw new Exception("Gesto correto não venceu");city.CancelPlayerEmote(true);
            for(int attempt=0;attempt<3;attempt++){
                PlaceForStreetActivity(CityView.CourtBallSpot);if(!TryStreetActivity()||!city.BallHeld)throw new Exception("Bola não foi pega");
                var cameraTarget=city.transform.TransformPoint(CityView.CourtHoop);cam.transform.LookAt(cameraTarget);var angles=cam.transform.eulerAngles;city.SetView(angles.y,Mathf.DeltaAngle(0,angles.x));
                contextClock=courtReadyAt+Mathf.PI/(2*2.8f);FrameStreetActivities();if(attempt==0)CaptureInterface(Path.Combine(folder,"basketball-meter.png"));
                if(!TryStreetActivity()||!city.BallFlying)throw new Exception("Arremesso não partiu");
                paused=true;var frozen=city.BallPoint;city.Frame(cam,false,0);TickContext(.5f);if(city.BallPoint!=frozen)throw new Exception("Bola moveu na pausa");paused=false;
                for(int i=0;i<43;i++){city.Frame(cam,false,.05f);TickContext(.05f);if(attempt==0&&i==14)CaptureInterface(Path.Combine(folder,"basketball-shot.png"));}
            }
            if(StreetMemory.courtBest[0]!=3)throw new Exception("Recorde da rodada incorreto");
            PlaceForStreetActivity(city.ResidentPosition(0)+Vector3.up*2.2f);if(!TryStreetActivity()||!streetStoryOpen)throw new Exception("Conversa não abriu");for(int i=0;i<25;i++){city.Frame(cam,false,.05f);TickContext(.05f);}CaptureInterface(Path.Combine(folder,"story-day-1.png"));StreetMemory.Talk(1,0,true);TrySave();CloseStreetPanel();
            for(int i=0;i<3;i++)progress.Add(new Attempt{outfit=Catalog.Default});progress.Close();progress.Advance();city.ApplyWeekScenery(progress.day);
            PlaceForStreetActivity(city.ResidentPosition(0)+Vector3.up*2.2f);TryStreetActivity();for(int i=0;i<25;i++){city.Frame(cam,false,.05f);TickContext(.05f);}CaptureInterface(Path.Combine(folder,"story-day-2.png"));if(streetReply.Length>0||!StreetMemory.Talked(1,0))throw new Exception("História não continuou no dia seguinte");CloseStreetPanel();
            if(!TrySave())throw new Exception("Save das atividades falhou");var loaded=ProgressStore.Load(DayPath);
            if(loaded.neighbourhood.placed!=127||loaded.neighbourhood.courtBest[0]!=3||!loaded.neighbourhood.ChallengeDone(1)||!loaded.neighbourhood.Talked(1,0))throw new Exception("Atividades perdidas ao recarregar");
            string other=Path.Combine(storageOverride,"other-slot.json");ProgressStore.Save(other,new DayProgress());if(ProgressStore.Load(other).neighbourhood.stickers!=0)throw new Exception("Atividades vazaram entre espaços");
            var legacy=JsonUtility.FromJson<DayProgress>(JsonUtility.ToJson(new DayProgress()));legacy.neighbourhood=null;legacy.Validate();if(legacy.neighbourhood==null)throw new Exception("Save antigo não recebeu defaults");
            if(city.ResidentCount!=residents||city.GetComponentsInChildren<Light>(true).Length!=lights)throw new Exception("Atividades criaram atores ou luzes permanentes");
            EnterHome();if(StreetBubblesAllowed||StreetPanelOpen||courtPending)throw new Exception("Atividade vazou para casa");EnterCity();if(StreetMemory.placed!=127||city.BallHeld||city.ChallengeResident>=0)throw new Exception("Reentrada incorreta");
            File.WriteAllText(Path.Combine(folder,"checks.txt"),"NEIGHBOURHOOD_ACTIVITIES_PASSED: seven reachable collectibles, mural preview/cancel/save, wrong/right daily emote, three real basket trajectories and pause, daily story continuation, legacy defaults, isolated saves/reentry, existing routes and unchanged actor/light counts.");Debug.Log(File.ReadAllText(Path.Combine(folder,"checks.txt")));
        }
    }
}
