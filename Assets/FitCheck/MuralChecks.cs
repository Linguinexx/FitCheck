using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        void CheckMuralEditor(string folder){
            Home();storageOverride=Path.Combine(folder,"sessions",Guid.NewGuid().ToString("N"));Directory.CreateDirectory(storageOverride);progress=new DayProgress();EnterCity();paused=false;cityFreeCursor=false;
            StreetMemory.stickers=127;StreetMemory.EnsureMuralLayout();
            var front=new Vector2(CityView.CollectiveMural.x,CityView.CollectiveMural.z+2.35f);var path=new List<Vector2>();
            if(!city.Free(front)||!city.PlanRoute(CityView.StreetShopDoor,front,path))throw new Exception("Frente do mural sem acesso físico");
            if(Vector3.Dot(city.transform.Find("Mural coletivo").TransformDirection(Vector3.back),Vector3.forward)<.99f)throw new Exception("Mural voltado para a parede");
            PlaceForStreetActivity(CityView.CollectiveMural);if(!city.LookingAtMural(cam)||!TryStreetActivity()||!muralOpen)throw new Exception("Mural não abre pela frente");
            contextClock+=1;FrameStreetActivities();muralDraft=127;PreviewMural();city.PlayerBodyVisible=false;
            if(cam.transform.position.z<city.MuralWorldPoint(Vector3.zero).z)throw new Exception("Câmera do editor atrás do muro");
            CaptureInterface(Path.Combine(folder,"mural-default.png"));
            int picked=PickMuralPoster(new Vector2(muralX[0],muralY[0]));if(picked!=0)throw new Exception("Seleção não corresponde ao lambe-lambe");
            var desired=new Vector2(.65f,-.46f);var world=city.MuralWorldPoint(new Vector3(desired.x,desired.y,-.06f));
            var ray=new Ray(cam.transform.position,world-cam.transform.position);if(!city.MuralPoint(ray,out var hit)||Vector2.Distance(hit,desired)>.005f)throw new Exception("Cursor não acompanha o plano do mural");
            SelectMuralPoster(picked);MoveMuralPoster(picked,hit);TurnMuralPoster(100);
            if(muralAngle[0]!=30||Mathf.Abs(muralX[0]-.65f)>.01f)throw new Exception("Mover/girar não funciona");
            if(StreetMemory.placed!=0||StreetMemory.muralAngle[0]!=-8||StreetMemory.muralX[0]!=-.96f)throw new Exception("Prévia alterou o save");
            CloseStreetPanel();if(StreetMemory.placed!=0||StreetMemory.muralX[0]!=-.96f)throw new Exception("Cancelar não restaurou o mural");
            PlaceForStreetActivity(CityView.CollectiveMural);TryStreetActivity();contextClock+=1;FrameStreetActivities();muralDraft=127;muralSelected=0;
            for(int i=0;i<7;i++){
                muralSelected=i;TurnMuralPoster(i%2==0?7:-7);MoveMuralPoster(i,new Vector2(muralX[i]+.04f,muralY[i]-.015f));
                var edge=CityView.ClampMuralPoster(i,new Vector2(99,99),muralAngle[i]);if(edge.x>=1.4f||edge.y>=.7f)throw new Exception("Papel pode sair do mural");
            }
            SelectMuralPoster(0);MoveMuralPoster(0,new Vector2(.65f,-.46f));TurnMuralPoster(26);PreviewMural();city.PlayerBodyVisible=false;CaptureInterface(Path.Combine(folder,"mural-edited.png"));
            float savedX=muralX[0],savedAngle=muralAngle[0];SaveMuralDraft();if(muralOpen)throw new Exception("Salvar não fechou o editor");
            var loaded=ProgressStore.Load(DayPath);if(loaded.neighbourhood.placed!=127||loaded.neighbourhood.muralLayer[0]!=6||loaded.neighbourhood.muralX[0]!=savedX||loaded.neighbourhood.muralAngle[0]!=savedAngle||loaded.posts.Count!=0||loaded.reputation!=0)throw new Exception("Composição não persistiu ou alterou campanha");
            ProgressStore.Save(Path.Combine(storageOverride,"other-slot.json"),new DayProgress());var other=ProgressStore.Load(Path.Combine(storageOverride,"other-slot.json"));if(other.neighbourhood.placed!=0||other.neighbourhood.muralX[0]!=-.96f)throw new Exception("Composição vazou entre saves");
            var legacy=JsonUtility.FromJson<DayProgress>("{\"version\":2,\"day\":1,\"neighbourhood\":{\"stickers\":127,\"placed\":127}}");legacy.Validate();if(legacy.neighbourhood.placed!=127||legacy.neighbourhood.muralX.Length!=7)throw new Exception("Migração apagou o mural antigo");
            EnterHome();EnterCity();PlaceForStreetActivity(CityView.CollectiveMural);if(!city.LookingAtMural(cam)||StreetMemory.muralX[0]!=savedX)throw new Exception("Reentrada perdeu composição");
            SaveFirstPersonFrame(Path.Combine(folder,"mural-street.png"));
            cam.transform.position=city.MuralWorldPoint(new Vector3(0,0,2));cam.transform.LookAt(city.MuralWorldPoint(Vector3.zero));if(city.LookingAtMural(cam))throw new Exception("Mural pode ser acionado pelo verso");
            CheckNeighbourhoodActivities(Path.Combine(folder,"regression"));
            File.WriteAllText(Path.Combine(folder,"checks.txt"),"MURAL_EDITOR_PASSED: outward-facing wall/camera and reachable front; ray-to-paper selection/movement; seven torn-paper proportions; rotation/bounds; preview/cancel/save/reentry; legacy migration and isolated saves; no campaign changes; neighbourhood activity regression.");Debug.Log(File.ReadAllText(Path.Combine(folder,"checks.txt")));
        }
    }
}
