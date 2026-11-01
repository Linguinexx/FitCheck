using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        CityView city,street,shop;Color beforeCityBackground;CameraClearFlags beforeCityClearFlags;Material beforeCitySky;bool beforeCitySkyEnabled;
        float beforeCityFov,beforeCityNearClip;bool cityFreeCursor,cityLookCaptured,cityDragLook;int cityLookReadyFrame;
        bool shopFittingOpening;
        static readonly bool CityProofMode=System.Array.IndexOf(System.Environment.GetCommandLineArgs(),"-city-proof")>=0;
        bool ProofMode=>CityProofMode;
        void EnterCity()
        {EnterSpace(false);}
        void EnterShop()
        {
            bool resumeWork=stage==Stage.Post&&ShopOpen;
            shopService=false;EnterSpace(true);
            if(resumeWork){city.RestoreWalk(shopWorkPosition,shopWorkYaw,shopWorkPitch,mannequin);city.Frame(cam,true,0);}
            StartShopArrival();
        }
        void EnterSpace(bool interior,bool home=false)
        {
            if(stage==Stage.City){if(city.IsInterior==interior&&city.IsHome==home)return;ExitCity();}
            else if(city!=null&&city.IsHome&&city.gameObject.activeSelf)ExitCity();
            if(campaignBeforePreview!=null)Home();
            beforeCityClearFlags=cam.clearFlags;var sky=cam.GetComponent<Skybox>();beforeCitySky=sky!=null?sky.material:null;beforeCitySkyEnabled=sky!=null&&sky.enabled;
            studio.SetHome(false);beforeCityBackground=cam.backgroundColor;beforeCityFov=cam.fieldOfView;beforeCityNearClip=cam.nearClipPlane;
            int nextCustomer=Mathf.Clamp(progress.posts.Count,0,8);
            if(interior&&!home&&shop!=null&&shop.ShopCustomerIndex!=nextCustomer){shop.gameObject.SetActive(false);Destroy(shop.gameObject);shop=null;}
            city=home?homeRoom:interior?shop:street;
            if(city==null){city=new GameObject(home?"Casa — quarto do jogador":interior?"DripLab — interior da loja":"Bairro — Praça do Encontro").AddComponent<CityView>();if(home)city.BuildHome();else if(interior)city.BuildShop(studio,nextCustomer);else city.Build(studio);if(home)homeRoom=city;else if(interior)shop=city;else street=city;}
            city.gameObject.SetActive(true);studio.PreviewProportions=false;studio.TallPreview=false;studio.HairOverride=-1;studio.ExpressionOverride=-1;
            studio.PlayerLook=playerAppearance;studio.ApplyUniform(progress.uniform);city.ResetWalk(mannequin);city.PreparePlayerBody(studio);if(interior&&!home)city.ResetFittingCurtain();shopFittingOpening=false;stage=Stage.City;paused=false;showRequest=false;showDrops=false;
            city.Frame(cam,true,0);mannequin.gameObject.SetActive(false);cityFreeCursor=false;cityLookCaptured=false;cityDragLook=false;SyncCityCursor();
        }
        void ExitCity()
        {
            if(city==null)return;bool discardShop=city.IsInterior&&(shopDressingInStore||ShopSequenceActive||ShopArrivalActive);
            studio.EndCityWalk();city.gameObject.SetActive(false);mannequin.position=Vector3.zero;mannequin.gameObject.SetActive(true);
            cam.clearFlags=beforeCityClearFlags;var sky=cam.GetComponent<Skybox>();if(sky!=null){sky.material=beforeCitySky;sky.enabled=beforeCitySkyEnabled;}
            cam.orthographic=false;cam.backgroundColor=beforeCityBackground;cam.fieldOfView=beforeCityFov;cam.nearClipPlane=beforeCityNearClip;
            cityLookCaptured=false;cityDragLook=false;shopFittingOpening=false;shopSequence=ShopSequence.None;shopDressingInStore=false;
            homeDoorExitRemaining=0;
            if(discardShop&&shop==city){var previousShop=shop;shop=null;Destroy(previousShop.gameObject);}
            if(!ProofMode){Cursor.lockState=CursorLockMode.None;Cursor.visible=true;}
        }
        void SyncCityCursor(bool apply=false)
        {
            if(!apply||ProofMode||Application.isBatchMode)return;
            // Cursor capture requested by a GUI click is applied on the next Update.
            // Locking inside OnGUI can be cancelled by the editor's button control.
            bool focused=CityInputFocused;
            bool capture=stage==Stage.City&&!paused&&!ShopSequenceActive&&!cityFreeCursor&&focused;
            if(capture){
                if(!cityLookCaptured||Cursor.lockState!=CursorLockMode.Locked){
                    if(Cursor.lockState!=CursorLockMode.Locked)Cursor.lockState=CursorLockMode.Locked;
                    cityLookCaptured=true;cityLookReadyFrame=Time.frameCount+2;
                }
            }else{cityLookCaptured=false;if(paused||!focused)cityDragLook=false;if(Cursor.lockState!=CursorLockMode.None)Cursor.lockState=CursorLockMode.None;}
            Cursor.visible=stage!=Stage.City||paused||cityFreeCursor||!focused;
        }
        bool CityInputFocused {
            get {
                if(!Application.isFocused)return false;
#if UNITY_EDITOR
                var window=UnityEditor.EditorWindow.focusedWindow;
                return window!=null&&window.GetType().FullName=="UnityEditor.GameView";
#else
                return true;
#endif
            }
        }
        void LateUpdate()
        {
            // Reconcile after frame/UI work. A temporary editor/OS release must
            // never become the player's explicit Tab choice to free the pointer.
            if(stage==Stage.City)SyncCityCursor(true);
        }
        void UpdateCity(float delta)
        {
            if(!city.RuntimeReady||!city.PlayerBodyReady){
                var old=city;var position=old.Position;float yaw=old.ViewYaw,pitch=old.ViewPitch;
                old.PrepareRuntimeDisposal();old.gameObject.SetActive(false);mannequin.gameObject.SetActive(true);
                city=new GameObject(old.IsHome?"Casa — quarto do jogador":old.IsInterior?"DripLab — interior da loja":"Bairro — Praça do Encontro").AddComponent<CityView>();if(old.IsHome){city.BuildHome();homeRoom=city;}else if(old.IsInterior){city.BuildShop(studio,Mathf.Clamp(progress.posts.Count,0,8));shop=city;}else{city.Build(studio);street=city;}
                studio.ApplyUniform(progress.uniform);city.RestoreWalk(position,yaw,pitch,mannequin);city.PreparePlayerBody(studio);mannequin.gameObject.SetActive(false);Destroy(old.gameObject);
                shopFittingOpening=false;
            }
            SyncCityCursor(true);
            Vector2 input=Vector2.zero;
            if(!paused&&!shopFittingOpening&&!ShopSequenceActive&&homeDoorExitRemaining<=0&&(!cityFreeCursor||cityDragLook)&&!ProofMode&&CityInputFocused){
                if(cityLookCaptured&&Time.frameCount>=cityLookReadyFrame&&Cursor.lockState==CursorLockMode.Locked)city.Look(new Vector2(Input.GetAxisRaw("Mouse X"),Input.GetAxisRaw("Mouse Y")));
                float x=(Input.GetKey(KeyCode.D)||Input.GetKey(KeyCode.RightArrow)?1:0)-(Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.LeftArrow)?1:0);
                float y=(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.UpArrow)?1:0)-(Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.DownArrow)?1:0);
                var basis=Quaternion.Euler(0,city.ViewYaw,0);var direction=(basis*Vector3.right)*x+(basis*Vector3.forward)*y;input=new Vector2(direction.x,direction.z);
            }
            if(!paused&&homeDoorExitRemaining>0&&!city.IsInterior){float exitStep=Mathf.Min(Mathf.Min(delta,.05f),homeDoorExitRemaining);homeDoorExitRemaining-=exitStep;input=new Vector2(0,-.55f);delta=exitStep;}
            city.Step(input,delta,paused||shopFittingOpening||ShopSequenceActive,mannequin);
        }
        static bool CityHitArea(Vector2 p)
        {
            return new Rect(0,88,1440,812).Contains(p)&&!new Rect(28,100,415,65).Contains(p)&&!new Rect(1185,100,235,65).Contains(p)&&!new Rect(28,783,690,90).Contains(p);
        }
        void CityUI()
        {
            if(ShopSequenceActive){Fill(new Rect(440,814,560,58),new Color(.04f,.035f,.045f,.94f));Txt(new Rect(458,822,524,43),ShopSequenceLabel,action,paper);return;}
            if(cityFreeCursor&&Tap(new Rect(38,111,350,48),"Clique para olhar  /  Tab")){cityFreeCursor=false;SyncCityCursor();}
            if(Tap(new Rect(1200,111,203,48),"Voltar ao início"))ReturnToTitle();
            Fill(new Rect(38,796,660,67),new Color(.055f,.043f,.077f,.88f));
            Txt(new Rect(58,803,620,24),city.IsHome?"EM CASA  /  DIA 0"+progress.day:city.IsInterior?"DRIPLAB  /  LOJA":city.Place,tiny,violet);
            Txt(new Rect(58,831,620,24),"WASD andar  /  Mouse olhar  /  E interagir  /  Esc pausa",tiny,paper);
            if(city.CatInteraction(cam)){
                Fill(new Rect(440,680,560,76),new Color(.04f,.035f,.045f,.94f));Txt(new Rect(458,696,524,45),"E  /  Fazer carinho em Fubá",action,paper);
            }else if(city.IsHome||!city.IsInterior){
                string worldAction=city.IsHome?HomeActionLabel():homeDoorExitRemaining>0?"Saindo de casa…":city.StreetInteraction==1?"Entrar na DripLab":city.StreetInteraction==2?"Entrar em casa":"";
                if(worldAction.Length>0){Fill(new Rect(440,680,560,76),new Color(.04f,.035f,.045f,.94f));Txt(new Rect(458,696,524,45),"E  /  "+worldAction,action,paper);}
                else {Fill(new Rect(38,111,540,74),new Color(.04f,.035f,.045f,.88f));Txt(new Rect(56,123,502,52),city.IsHome?"Saia pela porta quando quiser ir à loja.":"Entre pela porta sob o letreiro DRIPLAB.",caption,paper);}
            }else if(city.IsInterior){
                string label=ShopActionLabel();
                if(label.Length>0){Fill(new Rect(440,680,560,76),new Color(.04f,.035f,.045f,.94f));Txt(new Rect(458,696,524,45),shopFittingOpening?label:"E  /  "+label,action,paper);}
                else if(!cityFreeCursor){Fill(new Rect(38,111,515,66),new Color(.04f,.035f,.045f,.88f));Txt(new Rect(56,121,480,48),progress.DayComplete?"Expediente concluído. Confira o fechamento no balcão.":!ShopOpen?"Loja fechada. Vá ao balcão para abrir quando quiser.":city.CustomerWaitingAtCounter?"Cliente no balcão. Passe por trás para atender.":"Loja aberta. O cliente está chegando.",caption,paper);}
            }
            if(cityFreeCursor)Txt(new Rect(440,750,560,32),"Também pode segurar o botão direito para olhar.",caption,paper);
            ProcessCityEvent(Event.current);
        }
        void ProcessCityEvent(Event e)
        {
            if(paused||stage!=Stage.City||ShopSequenceActive)return;
            if(ProcessCityDragLook(e))return;
            if(e.type==EventType.KeyDown&&e.keyCode==KeyCode.E){if(TryPetCat()||(city.IsHome?InteractHome():city.IsInterior?InteractShop():InteractStreet()))e.Use();return;}
            if(e.type==EventType.KeyDown&&e.keyCode==KeyCode.Tab){cityFreeCursor=!cityFreeCursor;SyncCityCursor();e.Use();return;}
            if(!CityHitArea(e.mousePosition))return;
            if(e.type==EventType.MouseDown&&CaptureCityPointer(e.mousePosition,e.button))e.Use();
        }
        bool CaptureCityPointer(Vector2 point,int button)
        {
            if(paused||stage!=Stage.City||button!=0||!cityFreeCursor||!CityHitArea(point))return false;
            cityFreeCursor=false;SyncCityCursor();return true;
        }
        bool ProcessCityDragLook(Event e)
        {
            float scale=Mathf.Min(Screen.width/1440f,Screen.height/900f);
            if(!HandleCityDragLook(e.type,e.button,e.mousePosition,e.delta,scale))return false;
            e.Use();return true;
        }
        bool HandleCityDragLook(EventType type,int button,Vector2 point,Vector2 delta,float scale)
        {
            if(paused||stage!=Stage.City||!cityFreeCursor)return false;
            if(type==EventType.MouseDown&&button==1&&CityHitArea(point)){cityDragLook=true;return true;}
            if(cityDragLook&&type==EventType.MouseDrag&&button==1){city.Look(new Vector2(delta.x,-delta.y)*scale*.1f);return true;}
            if(cityDragLook&&type==EventType.MouseUp&&button==1){cityDragLook=false;return true;}
            return false;
        }
        string ShopActionLabel()
        {
            if(shopFittingOpening)return "Abrindo o provador…";
            switch(city.ShopInteraction){case 1:return progress.DayComplete?"Ver fechamento do expediente":!ShopOpen?"Abrir a loja":city.CustomerWaitingAtCounter?"Atender "+DayOne.Customers[progress.posts.Count].name:"Aguardando o cliente chegar…";case 2:return "Entrar no provador";case 3:return "Sair da loja";default:return "";}
        }
        bool InteractShop()
        {
            if(paused||stage!=Stage.City||!city.IsInterior||city.IsHome||ShopSequenceActive)return false;
            if(shopFittingOpening)return true;
            int action=city.ShopInteraction;if(action==0)return false;
            if(action==3){EnterStreetAt(CityView.StreetShopDoor,180);return true;}
            if(pendingPhotoBytes!=null&&!TrySave())return false;
            if(action==2){shopFittingOpening=true;city.OpenFittingCurtain();return true;}
            if(progress.DayComplete){ExitCity();stage=Stage.Summary;return true;}
            if(!ShopOpen){OpenShopForDay();return true;}
            if(!city.CustomerWaitingAtCounter)return false;
            StartShopConversation();
            SyncCityCursor();return true;
        }
        void FinishShopFittingEntryIfReady()
        {
            if(!shopFittingOpening||paused||stage!=Stage.City||!city.FittingOpen)return;
            ExitCity();stage=Stage.Welcome;PreviewSession();EnterDressing();UpdateCamera();SyncCityCursor();
        }
    }
}
