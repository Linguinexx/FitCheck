using System;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        public void CheckNaturalActors(Camera camera)
        {
            var actor=residents[0];ResidentPart palm=null;foreach(var part in actor.parts)if(part.mesh.name.StartsWith("Mão")&&part.side>0)palm=part;
            if(actor.prop==null||palm==null)throw new Exception("Celular sem referência à mão");
            int surprised=-1;for(int i=0;i<residents.Count;i++)if(residents[i].expression==AvatarExpression.Surprised){surprised=i;break;}
            if(surprised<0)throw new Exception("Revisão sem expressão surpresa");
            int open=0,closed=0;
            for(int frame=0;frame<260;frame++){
                Frame(camera,false,1/30f);
                if(Vector3.Distance(actor.prop.localPosition,palm.mesh.bounds.center+new Vector3(0,.075f,-.04f))>.015f)throw new Exception("Celular se soltou da mão animada");
                bool gasp=false;foreach(var part in residents[surprised].parts)if(part.mouthKind==3&&part.node.activeSelf)gasp=true;
                if(gasp)open++;else closed++;
            }
            if(open<3||closed<180)throw new Exception("Surpresa não alterna reação e repouso: "+open+" / "+closed);
        }
    }
    public partial class FirstCustomer
    {
        void CheckKaosCity(string folder)
        {
            var original=progress;string storage=storageOverride;float sensitivity=mouseSensitivity;string state=JsonUtility.ToJson(progress);
            try{
                Home();EnterCity();SetMouseSensitivity(.5f);city.SetView(0,0);city.Look(new Vector2(10,0));float slow=city.ViewYaw;
                SetMouseSensitivity(1.5f);city.SetView(0,0);city.Look(new Vector2(10,0));if(Mathf.Abs(city.ViewYaw-slow*3)>.01f)throw new Exception("Sensibilidade não altera o olhar");
                SetMouseSensitivity(9);if(mouseSensitivity!=SensitivityMax)throw new Exception("Sensibilidade fora do limite");
                SetMouseSensitivity(-1);if(mouseSensitivity!=0)throw new Exception("Sensibilidade não chega a zero");
                city.SetView(0,0);city.Look(new Vector2(10,5));if(city.ViewYaw!=0||city.ViewPitch!=0)throw new Exception("Sensibilidade zero ainda move o olhar");
                SetMouseSensitivity(1);
                city.CheckNaturalActors(cam);
                var cafe=city.transform.position+city.ResidentPosition(0);cam.transform.position=cafe+new Vector3(1.5f,2.1f,-2);cam.transform.LookAt(cafe+Vector3.up*1.8f);cam.fieldOfView=50;SaveFirstPersonFrame(Path.Combine(folder,"citizen-phone-attached.png"));
                MoveProof(new Vector2(-76,39),55,-10);SaveFirstPersonFrame(Path.Combine(folder,"monument-green-hill.png"));
                MoveProof(new Vector2(-97,15.5f),0,-8);SaveFirstPersonFrame(Path.Combine(folder,"rhyme-pocket.png"));
                Home();StartLiveTitle();var initial=city.ResidentPosition(2);float sky=city.AmbientTime;
                for(int i=0;i<100;i++)FrameLiveTitle(.05f);
                if(stage!=Stage.Title||!titleSceneActive||city.PlayerBodyVisible||city.AmbientTime<=sky||Vector3.Distance(initial,city.ResidentPosition(2))<.5f)throw new Exception("Menu não mantém uma cena viva");
                SaveFirstPersonFrame(Path.Combine(folder,"title-live-scene.png"));Home();if(titleSceneActive||street.gameObject.activeSelf||studio.ExplorationMode)throw new Exception("Menu animado vazou para seleção");
                EnterHome();city.ResetWalk(mannequin);city.SetView(180,0);city.Frame(cam,true,0);if(CurrentWorldHint().valid)throw new Exception("Dica de interação permanente no centro do quarto");
                MoveProof(CityView.HomeDoor,180,0);if(!CurrentWorldHint().valid||CurrentWorldHint().key!="E")throw new Exception("Porta sem affordance próxima");
                Home();progress=new DayProgress();storageOverride=Path.Combine(folder,"kaos-decor-"+Guid.NewGuid().ToString("N"));savedDecor=null;EnterShop();OpenShopDecor();
                for(int color=0;color<8;color++){decorDraft.wall=color;decorDraft.accent=color;city.ApplyShopDecor(decorDraft);}
                decorDraft.wall=4;decorDraft.accent=3;city.ApplyShopDecor(decorDraft);CloseShopDecor(true);
                var loaded=ShopDecorStore.Load(ShopDecorPath);if(loaded.wall!=4||loaded.accent!=3)throw new Exception("Cores vivas não persistiram");
                if(!Resources.Load<Texture2D>("Brand/FitCheck-Selo-Kaos-Red"))throw new Exception("Marca oficial ausente");
                Home();progress=original;if(JsonUtility.ToJson(progress)!=state)throw new Exception("UX alterou a campanha");
                Debug.Log("FITCHECK_KAOS_CITY_PASSED: sensitivity gain/clamp, phone attached over animation, transient surprise, live title/restoration, contextual proximity hints, eight vivid decor colors and independent campaign.");
            }finally{settingsOpen=false;Home();progress=original;storageOverride=storage;savedDecor=null;SetMouseSensitivity(sensitivity);}
        }
    }
}
