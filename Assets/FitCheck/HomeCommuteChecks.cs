using System;
using System.IO;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        void WalkWorldProof(Vector2 target)
        {
            if(!city.RequestWalk(target))throw new Exception("Sem caminho até interação: "+target);
            for(int frame=0;frame<1200&&Vector2.Distance(city.Position,target)>.035f;frame++){city.Step(Vector2.zero,.05f,false,mannequin);city.Frame(cam,true,0);}
            if(Vector2.Distance(city.Position,target)>.04f)throw new Exception("Caminho bloqueado: "+target);
        }
        void CheckHomeCommute(string folder)
        {
            var original=progress;var look=playerAppearance;string storage=storageOverride;
            try{
                Home();progress=new DayProgress();playerAppearance=new PlayerAppearance{created=true,name="Teste casa",hair=6};storageOverride=Path.Combine(folder,"home-session");Directory.CreateDirectory(storageOverride);
                GoHome();
                if(stage!=Stage.City||!city.IsHome||!city.RuntimeReady||!city.PlayerBodyReady||city.ResidentCount!=0||cam.orthographic)throw new Exception("Casa não abriu em primeira pessoa");
                string baseline=JsonUtility.ToJson(progress);
                city.SetView(310,12);city.Frame(cam,true,0);SaveFirstPersonFrame(Path.Combine(folder,"home-first-person.png"));
                city.Look(new Vector2(25,-18));city.Frame(cam,true,0);if(city.ViewPitch<=0)throw new Exception("Casa sem olhar vertical");
                WalkWorldProof(CityView.HomeWardrobe);InteractHome();if(stage!=Stage.Home||!showWardrobe)throw new Exception("Guarda-roupa inacessível");CloseHomePanel();
                if(stage!=Stage.City||!city.PlayerBodyReady||!city.IsHome)throw new Exception("Voltar ao quarto perdeu corpo/controle");
                WalkWorldProof(CityView.HomeDesk);InteractHome();if(stage!=Stage.Home)throw new Exception("Celular inacessível");CloseHomePanel();
                WalkWorldProof(CityView.HomeBed);InteractHome();if(stage!=Stage.Home)throw new Exception("Cama inacessível");CloseHomePanel();
                WalkWorldProof(CityView.HomeDoor);paused=true;if(InteractHome())throw new Exception("Pausa permitiu sair de casa");paused=false;
                if(!InteractHome()||city.IsInterior||stage!=Stage.City||Vector2.Distance(city.Position,CityView.StreetHomeDoor)>.01f)throw new Exception("Porta da casa não ligou ao bairro");
                city.SetView(0,-6);city.Frame(cam,true,0);SaveFirstPersonFrame(Path.Combine(folder,"home-street-door.png"));
                WalkWorldProof(CityView.StreetShopDoor);if(!InteractStreet()||!city.IsInterior||city.IsHome||ShopOpen||city.CustomerVisible)throw new Exception("Loja não abriu fechada a partir da rua");
                WalkWorldProof(CityView.ShopCounter);if(!InteractShop()||!ShopOpen||!ShopArrivalActive||ShopSequenceActive)throw new Exception("Não abriu expediente após percurso");
                SettleShopArrival();if(!city.CustomerWaitingAtCounter)throw new Exception("Cliente não chegou após abertura manual");
                WalkWorldProof(CityView.ShopSpawn);city.SetView(180,0);if(!InteractShop()||city.IsInterior)throw new Exception("Saída da loja não voltou ao bairro");
                WalkWorldProof(CityView.StreetHomeDoor);if(!InteractStreet()||!city.IsHome)throw new Exception("Não foi possível voltar para casa");
                city.SetView(180,88);city.Frame(cam,true,0);city.CheckPlayerBody(cam);SaveFirstPersonFrame(Path.Combine(folder,"home-player-body.png"));
                if(JsonUtility.ToJson(progress)!=baseline)throw new Exception("Percurso alterou o progresso da campanha");
                var dayBeforeReplay=progress;progress=new DayProgress();progress.Add(new Attempt{outfit=(string[])Catalog.Default.Clone(),question=1});
                WalkWorldProof(CityView.HomeDesk);InteractHome();fromHome=true;stage=Stage.Retrospective;UpdateCamera();
                if(homeRoom.gameObject.activeSelf)throw new Exception("Casa não liberou a câmera para o FitFeed");
                customer=0;Replay();UpdateCamera();if(!mannequin.gameObject.activeInHierarchy||mannequin.position.sqrMagnitude>.001f)throw new Exception("Repetição do FitFeed perdeu o personagem depois da casa");
                Home();progress=dayBeforeReplay;GoHome();
                Home();if(homeRoom.gameObject.activeSelf)throw new Exception("Casa permaneceu ativa no menu");
                Debug.Log("FITCHECK_HOME_COMMUTE_PASSED: first-person home, furniture paths and interactions, pause, home/street/shop doors, manual opening, waiting customer, return trip, visible player body and campaign retained.");
            }finally{Home();progress=original;playerAppearance=look;storageOverride=storage;Home();}
        }
    }
}
