using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        CityView city,street,shop;Color beforeCityBackground;CameraClearFlags beforeCityClearFlags;Material beforeCitySky;bool beforeCitySkyEnabled;
        DayProgress shopBuiltCampaign;int shopBuiltDay;
        float beforeCityFov,beforeCityNearClip;bool cityFreeCursor,cityLookCaptured,cityDragLook;int cityLookReadyFrame;
        bool shopFittingOpening;
        bool shopGuideOpen,shopGuidePreviousCursor;
        int shopGuideTopic;
        float shopGuideAnswerAt;
        static readonly string[] ShopGuideTopics={
            "Como começo o expediente?", "O que faço quando o cliente chega?",
            "Como monto um bom visual?", "Onde vejo as fotos e recompensas?", "Como avanço para o próximo dia?"
        };
        static readonly string[] ShopGuideAnswers={
            "Vai até o balcão e aperta E pra abrir a loja. O primeiro cliente vem sozinho. Enquanto a loja estiver fechada, dá pra usar F na área de personalização.",
            "Espera a pessoa chegar ao balcão e aperta E pra atender. Escuta o pedido e usa uma das perguntas pra esclarecer o que ela quer. Quando continuar, ela vai pro provador.",
            "O cliente entra com o próprio look. Troca só o que quiser em Cima, Baixo, Tênis e Cabeça. Usa as pistas do pedido pra combinar as peças. Se mudar de ideia, volta à peça original pela opção no topo.",
            "O FitFeed mostra a foto e a nota do atendimento. Em casa, dá pra abrir o feed pela mesa. A reputação libera novas peças, entregues no fechamento do expediente.",
            "Atende os três clientes do dia e volta ao balcão pra ver o fechamento. Depois, volta pra casa e interage com a cama pra dormir e começar o próximo dia."
        };
#if UNITY_WEBGL && !UNITY_EDITOR
        const bool CityProofMode=false;
#else
        static readonly bool CityProofMode=System.Array.IndexOf(System.Environment.GetCommandLineArgs(),"-city-proof")>=0;
#endif
        bool ProofMode=>CityProofMode;
        void EnterCity()
        {EnterSpace(false);}
        void EnterShop()
        {
            if(DeferLoading(EnterShop))return;
            bool resumeWork=stage==Stage.Post&&ShopOpen||stage==Stage.Summary;
            shopService=false;EnterSpace(true);
            if(resumeWork){city.RestoreWalk(shopWorkPosition,shopWorkYaw,shopWorkPitch,mannequin);city.Frame(cam,true,0);}
            StartShopArrival();
        }
        void EnterSpace(bool interior,bool home=false)
        {
            if(plaza!=null)plaza.SetActive(false);
            if(stage==Stage.City){if(city.IsInterior==interior&&city.IsHome==home)return;ExitCity();}
            else if(city!=null&&city.IsHome&&city.gameObject.activeSelf)ExitCity();
            if(campaignBeforePreview!=null)Home();
            beforeCityClearFlags=cam.clearFlags;var sky=cam.GetComponent<Skybox>();beforeCitySky=sky!=null?sky.material:null;beforeCitySkyEnabled=sky!=null&&sky.enabled;
            studio.SetHome(false);beforeCityBackground=cam.backgroundColor;beforeCityFov=cam.fieldOfView;beforeCityNearClip=cam.nearClipPlane;
            studio.SetExploration(true);
            int nextCustomer=Mathf.Clamp(progress.posts.Count,0,DayOne.Customers.Length-1);
            if(interior&&!home&&shop!=null&&(shop.ShopCustomerIndex!=nextCustomer||!ReferenceEquals(shopBuiltCampaign,progress)||shopBuiltDay!=progress.day)){shop.gameObject.SetActive(false);Destroy(shop.gameObject);shop=null;}
            city=home?homeRoom:interior?shop:street;
            if(city==null){city=new GameObject(home?"Casa — quarto do jogador":interior?"DripLab — interior da loja":"Bairro — Praça do Encontro").AddComponent<CityView>();if(home)city.BuildHome();else if(interior)city.BuildShop(studio,nextCustomer);else city.Build(studio);if(home)homeRoom=city;else if(interior)shop=city;else street=city;}
            city.gameObject.SetActive(true);studio.PreviewProportions=false;studio.TallPreview=false;studio.HairOverride=-1;studio.ExpressionOverride=-1;
            if(interior&&!home){shopBuiltCampaign=progress;shopBuiltDay=progress.day;}
            city.MouseLookScale=mouseSensitivity;
            if(interior&&!home)city.ApplyShopDecor(CurrentShopDecor);
            if(!interior&&!home){
                if(titleStarting)city.HideFitFeedResidents();
                else city.SyncFitFeedResidents(studio,progress);
            }
            studio.PlayerLook=playerAppearance;studio.ApplyUniform(progress.EquippedUniform);city.ResetWalk(mannequin);city.PreparePlayerBody(studio);if(interior&&!home)city.ResetFittingCurtain();shopFittingOpening=false;stage=Stage.City;paused=false;showRequest=false;showDrops=false;
            city.Frame(cam,true,0);mannequin.gameObject.SetActive(false);cityFreeCursor=false;cityLookCaptured=false;cityDragLook=false;SyncCityCursor();
            ArrivalPresentation();
        }
        void ExitCity()
        {
            greetedIndex=-1;
            garageRecordUntil=0;
            if(shopGuideOpen)CloseShopGuide();
            if(showShopDecor)CloseShopDecor(false);
            studio.SetExploration(false);
            if(city==null)return;bool discardShop=city.IsInterior&&(shopDressingInStore||ShopSequenceActive||ShopArrivalActive);
            studio.EndCityWalk();city.gameObject.SetActive(false);mannequin.position=Vector3.zero;mannequin.gameObject.SetActive(true);
            cam.clearFlags=beforeCityClearFlags;var sky=cam.GetComponent<Skybox>();if(sky!=null){sky.material=beforeCitySky;sky.enabled=beforeCitySkyEnabled;}
            cam.orthographic=false;cam.backgroundColor=beforeCityBackground;cam.fieldOfView=beforeCityFov;cam.nearClipPlane=beforeCityNearClip;
            cityLookCaptured=false;cityDragLook=false;shopFittingOpening=false;shopSequence=ShopSequence.None;shopDressingInStore=false;
            homeDoorExitRemaining=0;
            if(discardShop&&shop==city){var previousShop=shop;shop=null;Destroy(previousShop.gameObject);}
            if(!ProofMode){
#if UNITY_WEBGL && !UNITY_EDITOR
                if(!loadingActive)BrowserPointer.Capture(false);
#else
                Cursor.lockState=CursorLockMode.None;Cursor.visible=true;
#endif
            }
        }
        void SyncCityCursor(bool apply=false)
        {
            if(!apply||ProofMode||Application.isBatchMode||loadingActive)return;
            // Cursor capture requested by a GUI click is applied on the next Update.
            // Locking inside OnGUI can be cancelled by the editor's button control.
            bool focused=CityInputFocused;
            bool capture=stage==Stage.City&&!paused&&!ShopSequenceActive&&!cityFreeCursor&&focused;
#if UNITY_WEBGL && !UNITY_EDITOR
            BrowserPointer.Capture(capture);
            bool locked=capture&&BrowserPointer.Locked;
            if(locked&&!cityLookCaptured)cityLookReadyFrame=Time.frameCount+2;
            cityLookCaptured=locked;
            if(!capture&&(!focused||paused))cityDragLook=false;
            return;
#else
            if(capture){
                if(!cityLookCaptured||Cursor.lockState!=CursorLockMode.Locked){
                    if(Cursor.lockState!=CursorLockMode.Locked)Cursor.lockState=CursorLockMode.Locked;
                    cityLookCaptured=true;cityLookReadyFrame=Time.frameCount+2;
                }
            }else{cityLookCaptured=false;if(paused||!focused)cityDragLook=false;if(Cursor.lockState!=CursorLockMode.None)Cursor.lockState=CursorLockMode.None;}
            Cursor.visible=stage!=Stage.City||paused||cityFreeCursor||!focused;
#endif
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
#if UNITY_WEBGL && !UNITY_EDITOR
            if(BrowserPointer.TakeEscape())HandleEscapeAction();
#endif
            // Reconcile after frame/UI work. A temporary editor/OS release must
            // never become the player's explicit Tab choice to free the pointer.
            if(stage==Stage.City&&!loadingActive)SyncCityCursor(true);
        }
        void UpdateCity(float delta)
        {
            city.MouseLookScale=mouseSensitivity;
            if(!city.RuntimeReady||!city.PlayerBodyReady){
                var old=city;var position=old.Position;float yaw=old.ViewYaw,pitch=old.ViewPitch;
                old.PrepareRuntimeDisposal();old.gameObject.SetActive(false);mannequin.gameObject.SetActive(true);
                city=new GameObject(old.IsHome?"Casa — quarto do jogador":old.IsInterior?"DripLab — interior da loja":"Bairro — Praça do Encontro").AddComponent<CityView>();if(old.IsHome){city.BuildHome();homeRoom=city;}else if(old.IsInterior){city.BuildShop(studio,Mathf.Clamp(progress.posts.Count,0,DayOne.Customers.Length-1));shop=city;}else{city.Build(studio);street=city;}
                if(!old.IsInterior&&!old.IsHome)city.SyncFitFeedResidents(studio,progress);
                studio.ApplyUniform(progress.EquippedUniform);city.RestoreWalk(position,yaw,pitch,mannequin);city.PreparePlayerBody(studio);mannequin.gameObject.SetActive(false);Destroy(old.gameObject);
                shopFittingOpening=false;
            }
            SyncCityCursor(true);
            Vector2 input=Vector2.zero;
            if(!paused&&!showShopDecor&&!shopGuideOpen&&!shopFittingOpening&&!ShopSequenceActive&&homeDoorExitRemaining<=0&&(!cityFreeCursor||cityDragLook)&&!ProofMode&&CityInputFocused){
                if(cityLookCaptured&&Time.frameCount>=cityLookReadyFrame&&BrowserPointer.Locked)city.Look(new Vector2(Input.GetAxisRaw("Mouse X"),Input.GetAxisRaw("Mouse Y")));
                float x=(Input.GetKey(KeyCode.D)||Input.GetKey(KeyCode.RightArrow)?1:0)-(Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.LeftArrow)?1:0);
                float y=(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.UpArrow)?1:0)-(Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.DownArrow)?1:0);
                var basis=Quaternion.Euler(0,city.ViewYaw,0);var direction=(basis*Vector3.right)*x+(basis*Vector3.forward)*y;input=new Vector2(direction.x,direction.z);
            }
            if(!paused&&homeDoorExitRemaining>0&&!city.IsInterior){float exitStep=Mathf.Min(Mathf.Min(delta,.05f),homeDoorExitRemaining);homeDoorExitRemaining-=exitStep;input=new Vector2(0,-.55f);delta=exitStep;}
            city.SprintRequested=!ProofMode&&!paused&&!cityFreeCursor&&homeDoorExitRemaining<=0&&(Input.GetKey(KeyCode.LeftShift)||Input.GetKey(KeyCode.RightShift));
            if(shopProbeActive)input=shopProbeMoving?new Vector2(0,Mathf.Sin(Time.frameCount*.06f)>0?.3f:-.3f):Vector2.zero;
            city.Step(input,delta,paused||shopFittingOpening||ShopSequenceActive,mannequin);
        }
        static bool CityHitArea(Vector2 p)
        {
            return new Rect(0,0,1440,900).Contains(p);
        }
        void CityUI()
        {
            if(shopGuideOpen){ShopGuideUI();return;}
            if(showShopDecor){ShopDecorUI();return;}
            if(ShopSequenceActive){ComicBubble(new Rect(455,740,530,125),ShopSequenceLabel,"DRIPLAB",1,false);return;}
            if(cityFreeCursor&&Tap(new Rect(38,111,350,48),"Clique para olhar  /  Tab")){cityFreeCursor=false;SyncCityCursor();}
            if(contextClock<regionUntil){RoundedPanel(new Rect(38,176,380,42),new Color(.035f,.03f,.035f,.84f));Txt(new Rect(52,183,352,28),contextRegion,caption,paper);}
            DrawAffordance();DrawAmbientBubble();ProcessCityEvent(Event.current);
        }
        void ProcessCityEvent(Event e)
        {
            if(loadingActive)return;
            if(e.type==EventType.KeyDown&&e.keyCode==KeyCode.F&&!showShopDecor){OpenShopDecor();if(showShopDecor)e.Use();return;}
            if(paused||stage!=Stage.City||ShopSequenceActive)return;
            if(ProcessCityDragLook(e))return;
            if(e.type==EventType.KeyDown&&e.keyCode==KeyCode.E){if(TryPetCat()||(city.IsHome?InteractHome():city.IsInterior?InteractShop():InteractStreet()||TryGreetPostedCustomer()||TryUseGarageDeck()))e.Use();return;}
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
            if(shopFittingOpening)return city.FittingOpening?"Abrindo a cortina…":"Fechando a cortina…";
            switch(city.ShopInteraction){case 1:return progress.DayComplete?"Ver fechamento do expediente":!ShopOpen?"Abrir a loja":city.CustomerWaitingAtCounter?"Atender "+DayOne.Customers[progress.posts.Count].name:"Aguardando o cliente chegar…";case 2:return city.FittingOpening?"Fechar a cortina":"Abrir a cortina";case 3:return "Sair da loja";default:return "";}
        }
        bool InteractShop()
        {
            if(loadingActive||paused||stage!=Stage.City||!city.IsInterior||city.IsHome||ShopSequenceActive||shopGuideOpen)return false;
            if(shopFittingOpening)return true;
            if(city.LookingAtShopGuide(cam)){
                shopGuidePreviousCursor=cityFreeCursor;shopGuideOpen=true;shopGuideTopic=-1;shopGuideAnswerAt=contextClock;
                city.StopWalk();cityDragLook=false;cityFreeCursor=true;SyncCityCursor(true);
                return true;
            }
            int action=city.ShopInteraction;if(action==0)return false;
            if(action==3){EnterStreetAt(CityView.StreetShopDoor,180);return true;}
            if(pendingPhotoBytes!=null&&!TrySave())return false;
            if(action==2){shopFittingOpening=true;city.ToggleFittingCurtain();return true;}
            if(progress.DayComplete){ExitCity();stage=Stage.Summary;return true;}
            if(!ShopOpen){OpenShopForDay();return true;}
            if(!city.CustomerWaitingAtCounter)return false;
            StartShopConversation();
            SyncCityCursor();return true;
        }
        void CloseShopGuide()
        {
            shopGuideOpen=false;cityFreeCursor=shopGuidePreviousCursor;SyncCityCursor(true);
        }
        void ShopGuideUI()
        {
            var panel=new Rect(916,162,482,510);
            DrawKaosPanel(panel);
            Txt(new Rect(944,184,425,52),"Bora trocar ideia?",heading,paper);
            for(int i=0;i<ShopGuideTopics.Length;i++){
                if(Tap(new Rect(944,250+i*57,424,48),ShopGuideTopics[i],shopGuideTopic==i)){
                    shopGuideTopic=i;shopGuideAnswerAt=contextClock;
                }
            }
            if(Tap(new Rect(944,570,424,48),"Valeu, até mais  /  Esc")){CloseShopGuide();return;}
            ComicBubble(new Rect(96,613,750,216),shopGuideTopic<0?"Eu sou o Nando, mas pode chamar de Chave. Manda a dúvida que eu te ajudo.":ShopGuideAnswers[shopGuideTopic],
                CityView.ShopGuideName,contextClock-shopGuideAnswerAt,false);
        }
        void FinishShopFittingEntryIfReady()
        {
            if(!shopFittingOpening||paused||stage!=Stage.City||city.FittingMoving)return;
            shopFittingOpening=false;
            if(city.FittingOpen&&!city.CustomerVisible&&progress.DiscoverFittingGhost()){
                notices.Enqueue(DayProgress.FittingGhostAward);
                TrySave();
            }
        }
    }
}
