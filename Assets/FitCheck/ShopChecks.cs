using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        void CaptureShopReview(string folder)
        {
            Home();string before=JsonUtility.ToJson(progress),proofStorage=storageOverride;
            try{
                AmbientSurfaces.Validate();
                EnterShop();paused=false;
                SettleShopArrival();
                if(cityFreeCursor)throw new Exception("Loja não solicita ocultar o cursor ao entrar");
                if(!city.IsInterior||!city.RuntimeReady||!city.PlayerBodyReady||city.ResidentCount!=2)throw new Exception("Interior sem cliente e frequentador");
                city.ValidateShopRoles();
                city.ValidateShopLean();
                if(city.Free(new Vector2(4.4f,6.2f))||city.Free(new Vector2(-1.6f,.3f))||city.Free(new Vector2(7.15f,1))||city.Free(new Vector2(20,0)))throw new Exception("Mobiliário ou limites sem bloqueio");
                if(cam.GetComponent<Skybox>().enabled)throw new Exception("Céu aparece dentro da loja");
                CheckShopLookInput();
                SaveFirstPersonFrame(Path.Combine(folder,"shop-entrance.png"));
                var focus=city.transform.position+city.ResidentPosition(1)+Vector3.up*1.7f;
                cam.transform.position=city.transform.position+new Vector3(2.2f,2.5f,4.9f);cam.transform.LookAt(focus);cam.fieldOfView=52;
                SaveFirstPersonFrame(Path.Combine(folder,"shop-regular-lean.png"),640,800);
                cam.transform.position=city.transform.position+new Vector3(5,3.5f,1.3f);cam.transform.LookAt(city.transform.position+new Vector3(1.8f,2.0f,6.4f));cam.fieldOfView=64;
                SaveFirstPersonFrame(Path.Combine(folder,"shop-two-roles.png"));
                MoveProof(new Vector2(-4.8f,1.8f),270,8);SaveFirstPersonFrame(Path.Combine(folder,"shop-clothing.png"));
                MoveProof(new Vector2(4.6f,-.8f),70,8);SaveFirstPersonFrame(Path.Combine(folder,"shop-shoes.png"));
                MoveProof(new Vector2(2.2f,3.0f),42,4);SaveFirstPersonFrame(Path.Combine(folder,"shop-counter.png"));
                city.PlayerBodyVisible=false;
                cam.transform.position=city.transform.position+new Vector3(5.8f,2.8f,8.1f);cam.transform.LookAt(city.transform.position+new Vector3(5.8f,2.0f,6.2f));SaveFirstPersonFrame(Path.Combine(folder,"shop-register-employee.png"));
                city.PlayerBodyVisible=true;
                MoveProof(CityView.ShopCounter,0,4);
                if(city.ShopInteraction!=1)throw new Exception("Balcão inacessível");
                paused=true;if(InteractShop()||stage!=Stage.City)throw new Exception("Pausa permite atendimento");paused=false;
                var interact=new Event{type=EventType.KeyDown,keyCode=KeyCode.E};ProcessCityEvent(interact);
                if(stage!=Stage.Dialogue||interact.type!=EventType.Used||!city.gameObject.activeSelf||mannequin.gameObject.activeSelf)throw new Exception("Conversa não permanece dentro da loja");
                if(JsonUtility.ToJson(progress)!=before)throw new Exception("Entrar na loja alterou progresso");
                Home();EnterShop();
                MoveProof(CityView.ShopFitting,0,4);CaptureCurtainView(Path.Combine(folder,"shop-curtain-closed.png"));
                if(city.ShopInteraction!=2)throw new Exception("Provador inacessível");
                if(city.FittingOpen||city.Free(new Vector2(-4.9f,5.15f)))throw new Exception("Provador começou aberto");
                if(!InteractShop()||stage!=Stage.City||!shopFittingOpening)throw new Exception("Cortina não começou a abrir antes da entrada");
                paused=true;float progressBefore=city.FittingProgress;city.Frame(cam,false,0);FinishShopFittingEntryIfReady();
                if(city.FittingProgress!=progressBefore||stage!=Stage.City)throw new Exception("Pausa não congelou a abertura");paused=false;
                var motion=Path.Combine(folder,"curtain-motion");Directory.CreateDirectory(motion);
                for(int frame=0;frame<36;frame++){
                    city.Frame(cam,false,1/30f);city.ValidateShopLean();
                    CaptureCurtainView(Path.Combine(motion,frame.ToString("D3")+".png"));
                    if(frame==10)CaptureCurtainView(Path.Combine(folder,"shop-curtain-half.png"));
                }
                if(!city.FittingOpen||!city.Free(new Vector2(-4.9f,5.15f)))throw new Exception("Cortina aberta bloqueia a passagem");
                CaptureCurtainView(Path.Combine(folder,"shop-curtain-open.png"));
                FinishShopFittingEntryIfReady();
                if(stage!=Stage.Dressing||campaignBeforePreview==null)throw new Exception("Entrada não abriu prévia isolada depois da cortina");
                outfit=new[]{"AH_TOP_01","AH_BOTTOM_01","AH_SHOES_01",""};Apply();
                Home();storageOverride=proofStorage;
                if(JsonUtility.ToJson(progress)!=before)throw new Exception("Prévia da loja alterou campanha");
                EnterShop();if(city.FittingProgress!=0)throw new Exception("Cortina não fechou ao retornar");MoveProof(new Vector2(.4f,7.7f),180,7);SaveFirstPersonFrame(Path.Combine(folder,"shop-overview.png"));
                city.SetView(180,88);city.Frame(cam,true,0);city.CheckPlayerBody(cam);SaveFirstPersonFrame(Path.Combine(folder,"shop-player-body.png"));
                var position=city.Position;float yaw=city.ViewYaw;
                var field=typeof(CityView).GetField("residents",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic);
                ((IList)field.GetValue(city)).Clear();UpdateCity(0);
                if(!city.IsInterior||!city.RuntimeReady||!city.PlayerBodyReady||Vector2.Distance(position,city.Position)>.001f||Mathf.Abs(Mathf.DeltaAngle(yaw,city.ViewYaw))>.001f)throw new Exception("Recuperação perdeu interior/posição");
                MoveProof(CityView.ShopSpawn,180,0);
                if(city.ShopInteraction!=3||!InteractShop()||stage!=Stage.Welcome)throw new Exception("Saída da loja falhou");
                EnterCity();if(city.IsInterior||!city.RuntimeReady)throw new Exception("Loja substituiu bairro");city.ValidateFullBodyMotion();
                city.SetView(35,-5);city.Frame(cam,true,0);SaveFirstPersonFrame(Path.Combine(folder,"surface-street.png"));
                city.SetView(225,35);city.Frame(cam,true,0);SaveFirstPersonFrame(Path.Combine(folder,"surface-asphalt.png"));
                for(int pose=0;pose<6;pose++){
                    for(int tick=0;tick<9;tick++){city.Frame(cam,false,1/30f);city.CheckResidents();}
                    var target=city.transform.position+city.ResidentPosition(2)+Vector3.up*1.6f;
                    cam.transform.position=target+new Vector3(2.2f,.2f,-3.3f);cam.transform.LookAt(target);cam.fieldOfView=52;
                    SaveFirstPersonFrame(Path.Combine(folder,"relaxed-walk-"+pose+".png"),640,640);
                }Home();
                CheckShopServiceFlow(folder);CaptureHairReview(folder);CaptureHandReview(folder);
                Home();studio.Apply(new[]{"CG_TOP_01","CG_BOTTOM_01","CG_SHOES_01",""},0);mannequin.rotation=Quaternion.identity;
                cam.transform.position=new Vector3(.9f,.72f,-1.45f);cam.transform.LookAt(new Vector3(0,.29f,-.10f));cam.fieldOfView=36;
                SaveFirstPersonFrame(Path.Combine(folder,"green-sneaker-close.png"),960,640);Home();
                if(JsonUtility.ToJson(progress)!=before)throw new Exception("Passeios alteraram campanha");
                File.WriteAllText(Path.Combine(folder,"checks.txt"),"SHOP_REVIEW_PASSED: nine distinct tops aligned to three drop rails; green sneaker rebuilt with cream cupsole, diagonal panels, crossed laces and sewn surface tongue; no ellipsoid tongue; wavy diagonal hair; employee-facing register; hidden/captured exploration cursor, Tab release; arrival/conversation/camera approach/curtain/dressing/dressed departure/saved FitFeed/next client; pause, recovery and campaign preserved.");
                Debug.Log("FITCHECK_SHOP_REVIEW_PASSED: interior, furnishings, catalog displays, counter/fitting/exit, recovery, original campaign and camera restored.");
            }finally{Home();storageOverride=proofStorage;}
        }
        void CaptureCurtainView(string path)
        {
            bool visible=city.PlayerBodyVisible;
            try{
                // The review camera is outside the player; omit its headless FPS copy.
                city.PlayerBodyVisible=false;
                cam.transform.position=city.transform.position+new Vector3(-4.9f,2.25f,.3f);
                cam.transform.LookAt(city.transform.position+new Vector3(-4.9f,2.0f,5.15f));cam.fieldOfView=54;
                SaveFirstPersonFrame(path,960,540);
            }finally{city.PlayerBodyVisible=visible;}
        }
        void CheckShopLookInput()
        {
            float yaw=city.ViewYaw,pitch=city.ViewPitch;bool oldFree=cityFreeCursor;
            try{
                cityFreeCursor=true;
                // Batch-mode Unity rewrites synthetic mouse Events as Ignore. Exercise
                // the same handler that real OnGUI mouse events call, with raw values.
                HandleCityDragLook(EventType.MouseDown,1,new Vector2(720,450),Vector2.zero,1);
                HandleCityDragLook(EventType.MouseDrag,1,new Vector2(720,510),new Vector2(0,60),1);
                if(city.ViewPitch<pitch+5||Mathf.Abs(Mathf.DeltaAngle(city.ViewYaw,yaw))>.001f)throw new Exception("Arrasto vertical não altera apenas inclinação");
                HandleCityDragLook(EventType.MouseDrag,1,new Vector2(800,510),new Vector2(80,0),1);
                if(Mathf.Abs(Mathf.DeltaAngle(city.ViewYaw,yaw))<10)throw new Exception("Arrasto horizontal sem amplitude");
                HandleCityDragLook(EventType.MouseUp,1,new Vector2(800,510),Vector2.zero,1);
                float stopped=city.ViewPitch;
                HandleCityDragLook(EventType.MouseDrag,1,new Vector2(800,510),new Vector2(0,100),1);
                if(city.ViewPitch!=stopped||cityDragLook)throw new Exception("Câmera continua depois de soltar botão direito");
                Debug.Log("FITCHECK_SHOP_LOOK_PASSED: right-drag vertical/horizontal, mouse release; relative mouse axes retained for captured look.");
            }finally{cityFreeCursor=oldFree;cityDragLook=false;city.SetView(yaw,pitch);city.Frame(cam,true,0);}
        }
    }
}
