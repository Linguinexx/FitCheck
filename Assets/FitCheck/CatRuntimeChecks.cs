using System;
using System.IO;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        void CheckCatCompanion(string folder)
        {
            var appearance=playerAppearance;bool oldMuted=mutedAudio;
            try{
                Home();mutedAudio=true;EnterHome();var homeCat=city.Cat;
                if(homeCat==null||homeCat.MeowClip==null||homeCat.MeowClip.length<.5f||homeCat.MeowClip.length>5)throw new Exception("Gato ou miado ausente em casa");
                var clip=homeCat.MeowClip;var samples=new float[clip.samples*clip.channels];
                if(!clip.GetData(samples,0)||Array.TrueForAll(samples,value=>Mathf.Abs(value)<.001f))throw new Exception("Miado silencioso ou ilegível");
                MoveProof(CityView.HomeBed,240,25);cam.transform.LookAt(homeCat.PetTarget);
                paused=true;if(TryPetCat())throw new Exception("Pausa permitiu interagir com gato");paused=false;
                if(!TryPetCat()||homeCat.MeowCount!=1||!TryPetCat()||homeCat.MeowCount!=1||stage!=Stage.City)throw new Exception("Carinho/intervalo do miado não isolou a interação");
                float age=homeCat.Age;homeCat.Tick(1,true,false,true);if(homeCat.Age!=age)throw new Exception("Gato animou durante pausa");
                homeCat.Tick(3,false,false,true);if(!TryPetCat()||homeCat.MeowCount!=2)throw new Exception("Gato não voltou a miar após intervalo");
                CaptureCatView(Path.Combine(folder,"cat-home.png"),homeCat);
                EnterShop();var shopCat=city.Cat;
                if(shopCat==null||shopCat.MeowClip!=clip||shopCat.name!=homeCat.name)throw new Exception("Gato mudou entre casa e loja");
                MoveProof(new Vector2(-3.2f,-2.6f),270,55);cam.transform.LookAt(shopCat.PetTarget);
                if(!TryPetCat()||shopCat.MeowCount!=1||ShopOpen||shopService)throw new Exception("Gato da loja não mia ou iniciou atendimento");
                cam.transform.rotation=Quaternion.LookRotation(-cam.transform.forward);if(TryPetCat())throw new Exception("Gato interage sem olhar para ele");
                CaptureCatView(Path.Combine(folder,"cat-shop.png"),shopCat);
                Debug.Log("FITCHECK_CAT_PASSED: matching resting cats in home/shop, non-silent recorded meow, gaze/range interaction, cooldown, pause and no campaign/shop side effects.");
            }finally{Home();playerAppearance=appearance;mutedAudio=oldMuted;Home();}
        }
        void CaptureCatView(string path,ShopCat cat)
        {
            city.PlayerBodyVisible=false;cam.transform.position=cat.transform.TransformPoint(new Vector3(-.9f,.95f,-1.9f));
            cam.transform.LookAt(cat.transform.TransformPoint(new Vector3(0,.31f,0)));cam.fieldOfView=43;
            SaveFirstPersonFrame(path,960,640);
        }
    }
}
