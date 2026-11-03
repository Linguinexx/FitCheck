using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        float MeasureCityWalk(Vector2 start,Vector2 end)
        {
            city.RestoreWalk(start,90,0,mannequin);
            if(Vector2.Distance(city.Position,start)>.01f||!city.RequestWalk(end))throw new Exception("Percurso residencial indisponível");
            float elapsed=0;
            while(Vector2.Distance(city.Position,end)>.03f&&elapsed<50){
                var before=city.Position;city.Step(Vector2.zero,1/60f,false,mannequin);elapsed+=1/60f;
                if(!city.Free(city.Position)||Vector2.Distance(city.Position,before)>CityView.PlayerSpeed/60+.001f)throw new Exception("Percurso atravessou obstáculo ou acelerou o jogador");
            }
            if(Vector2.Distance(city.Position,end)>.03f||elapsed<30||elapsed>45)throw new Exception("Percurso fora de 30–45 segundos: "+elapsed);
            return elapsed;
        }
        void CheckCompletedCity(string folder)
        {
            var original=JsonUtility.ToJson(progress);
            try{
                Home();EnterCity();paused=false;
                if(studio.LegacyHomeVisible||(plaza!=null&&plaza.activeSelf))throw new Exception("Cenário de quarto/foto vazou para o bairro");
                if(city.ResidentCount!=CityView.StreetResidentCount||!city.RuntimeReady)throw new Exception("Bairro expandido incompleto");
                float outward=MeasureCityWalk(CityView.StreetHomeDoor,CityView.StreetShopDoor);
                float back=MeasureCityWalk(CityView.StreetShopDoor,CityView.StreetHomeDoor);
                // Both old addresses must cease to be home interactions.
                foreach(var old in new[]{new Vector2(-4.8f,7.65f),new Vector2(21.8f,3.8f)}){
                    MoveProof(old,0,0);if(city.StreetInteraction==2)throw new Exception("Endereço antigo ainda abre a casa");
                }
                var targets=new[]{new Vector2(-124.8f,-3),new Vector2(-112,-2.5f),new Vector2(-96.5f,-2.5f),new Vector2(-76,7),new Vector2(-49,-2.5f),new Vector2(-31.5f,-2.5f)};
                var names=new[]{"district-residential","district-market","district-gallery","district-court","district-music","district-passage"};
                for(int i=0;i<targets.Length;i++){
                    MoveProof(targets[i],0,i==3?-7:-10);city.Frame(cam,false,1/30f);
                    SaveFirstPersonFrame(Path.Combine(folder,names[i]+".png"));
                }
                foreach(var blocked in new[]{new Vector2(-137,9),new Vector2(-96.5f,8),new Vector2(-49,9),new Vector2(-87.2f,12),new Vector2(-76,20.25f)})
                    if(city.Free(blocked))throw new Exception("Marco ou fechamento urbano sem colisão: "+blocked);
                MoveProof(new Vector2(-61,-2),315,-8);SaveFirstPersonFrame(Path.Combine(folder,"district-street.png"));
                // Simulate the new walking loops; exercise the same pause clock as the square.
                float residentialTravel=0,galleryTravel=0;
                for(int i=0;i<500;i++){
                    var a=city.ResidentPosition(4);var b=city.ResidentPosition(5);city.Frame(cam,false,1/30f);city.CheckResidents();
                    residentialTravel+=Vector3.Distance(a,city.ResidentPosition(4));galleryTravel+=Vector3.Distance(b,city.ResidentPosition(5));
                }
                if(residentialTravel<2||galleryTravel<2)throw new Exception("Novos pedestres parados");
                var pausedResident=city.ResidentPosition(4);city.Frame(cam,false,0);
                if(city.ResidentPosition(4)!=pausedResident)throw new Exception("Pausa moveu novo pedestre");
                var points=new List<Vector2>();if(!city.PlanRoute(CityView.StreetHomeDoor,CityView.StreetShopDoor,points))throw new Exception("Rota perdeu acesso após animação");
                File.WriteAllText(Path.Combine(folder,"commute-measurement.json"),"{\"outward_seconds\":"+outward.ToString("F3",System.Globalization.CultureInfo.InvariantCulture)+",\"return_seconds\":"+back.ToString("F3",System.Globalization.CultureInfo.InvariantCulture)+",\"player_speed\":4.4,\"simulation_hz\":60,\"required_min\":30,\"required_max\":45}");
                if(JsonUtility.ToJson(progress)!=original)throw new Exception("Revisão urbana alterou campanha");
                Home();int oldCustomer=customer;
                try{
                    foreach(int index in new[]{1,2,3}){
                        customer=index;RenderCurrentPhoto();
                        if(plaza.GetComponent<CityView>()==null)throw new Exception("Foto não reutiliza o marco do bairro");
                        File.WriteAllBytes(Path.Combine(folder,"district-fitfeed-"+index+".png"),photo.EncodeToPNG());
                    }
                }finally{customer=oldCustomer;}
                Debug.Log("FITCHECK_CITY_COMPLETION_PASSED: measured native 60 Hz home/store walk: outward="+outward.ToString("F2")+"s return="+back.ToString("F2")+"s; gallery, court, music, market, residential address, collisions, seven residents and pause.");
            }finally{Home();}
        }
    }
}
