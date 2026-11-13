using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer : MonoBehaviour
    {
        CustomerRequest Customer=>DayOne.Customers[customer];
        string Request=>Customer.request+(SpecialOrderActive?"\n\n"+SpecialOrderText:"");
        string[] questions=>Customer.questions;
        string[] answers=>Customer.answers;
        enum Stage { Welcome, Dialogue, Dressing, Confirm, Leaving, Post, Summary, Home, Retrospective, City, CharacterCreator, Title, WeekMemory }
        DayProgress progress=new DayProgress(); int customer;
        int replayIndex=-1; bool fromHome; float remaining=30; bool timerWarned;
        string[] inventory;
        string[] ActiveInventory=>inventory??progress.Available;
        float FittingSeconds=>SpecialCustomers.Seconds(customer);
        bool Timed=>FittingSeconds>0;
        bool IsAvailable(Garment g)=>ActiveInventory.Contains(g.id);

        Stage stage; Attempt record=new Attempt(); string[] outfit=Catalog.Default; int question=-1, category;
        Camera cam; Transform mannequin; string storageOverride;
        Texture2D photo; float rotation=-12, distance=7.2f, transition;
        Vector2 scroll, detailsScroll; bool paused, showRequest, showDrops; string message=""; GameObject plaza;
        string SavePath=>Path.Combine(SessionFolder,"first-customer.json");
        string NewPhotoPath()=>Path.Combine(SessionFolder,"look-"+customer+"-"+Guid.NewGuid().ToString("N")+".png");
        string pendingPhotoPath;byte[] pendingPhotoBytes;
        string DayPath=>Path.Combine(SessionFolder,"day-one.json");
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)] static void Boot()
        { if(!FindAnyObjectByType<FirstCustomer>()) new GameObject("Fit Check — primeiro atendimento").AddComponent<FirstCustomer>(); }
        void Start()
        {
            Cursor.lockState=CursorLockMode.None;Cursor.visible=true;
            Application.targetFrameRate=60;
            Application.runInBackground=true;
            CatalogTesting.Enabled=false;
            if(!ProofMode&&PlayerPrefs.GetInt(CatalogTesting.PreferenceKey,0)!=0){
                PlayerPrefs.SetInt(CatalogTesting.PreferenceKey,0);PlayerPrefs.Save();
            }
            if(!ProofMode&&!Application.isEditor&&Application.platform!=RuntimePlatform.WebGLPlayer)
                SetFullscreen(PlayerPrefs.GetInt(FullscreenPreference,1)!=0);
            if(ProofMode){storageOverride=Path.Combine(FirstPersonProofOutput(),"session");Directory.CreateDirectory(storageOverride);}
            if(ProofMode)try {
                if(File.Exists(DayPath)) {
                    try{progress=ProgressStore.Load(DayPath);}catch{progress=ProgressStore.Load(DayPath+".bak");message="Progresso recuperado da cópia de segurança.";}
                }
                else if(File.Exists(SavePath)) {
                    record=JsonUtility.FromJson<Attempt>(File.ReadAllText(SavePath));
                    if(record.completed){Rules.Evaluate(record.outfit); progress.Add(record); SaveDay();}
                }
            }
            catch(Exception e) { message="Não foi possível ler o save: "+e.Message; progress=new DayProgress(); record=new Attempt(); }
            foreach(var c in FindObjectsByType<Camera>()) c.gameObject.SetActive(false);
            LoadAppearance();BuildWorld();BuildAudio();BuildPresentation();BuildThumbnails();RefreshPhotoArt();stage=Stage.Welcome;ShowPlayer();
            if(!ProofMode){BuildTitlePhoto();StartLiveTitle();}
#if !UNITY_WEBGL || UNITY_EDITOR
            if(ProofMode){mutedAudio=true;StartCoroutine(CaptureFirstPersonProof());}
#endif
        }
        Material Mat(Color color) { var m=new Material(Resources.Load<Material>("PrototypeBase")); m.color=color; return m; }
        GameObject Shape(string name, PrimitiveType kind, Vector3 pos, Vector3 scale, Material mat, Transform parent=null)
        { var o=GameObject.CreatePrimitive(kind); o.name=name; o.transform.SetParent(parent); o.transform.localPosition=pos; o.transform.localScale=scale; o.GetComponent<Renderer>().sharedMaterial=mat; return o; }
        StudioView studio;
        void BuildWorld()
        {
            studio=new GameObject("Estúdio Fit Check").AddComponent<StudioView>();
            studio.PlayerLook=playerAppearance;studio.Build();cam=studio.Camera;mannequin=studio.Avatar;Apply();UpdateCamera();
        }
        void Apply(){if(shopService&&stage==Stage.Dialogue)studio.ApplyCivilian(customer,true);else studio.Apply(outfit,customer);}
        void UpdateCamera()
        {
            if(stage==Stage.WeekMemory)return;
            if(stage==Stage.Title&&titleSceneActive){FrameLiveTitle(Time.unscaledDeltaTime);return;}
            if(stage!=Stage.City&&stage!=Stage.Home&&city!=null&&city.IsHome&&city.gameObject.activeSelf)ExitCity();
            if(stage==Stage.Home&&city!=null&&city.IsHome&&city.gameObject.activeSelf){if(showWardrobe)FrameHomeWardrobe();else{mannequin.gameObject.SetActive(false);city.Frame(cam,reducedMotion,0);city.FramePlayerForCamera(cam,false);}return;}
            if(shopDressingInStore&&(stage==Stage.Dressing||stage==Stage.Confirm)){FrameShopDressing();return;}
            if(campaignBeforePreview!=null&&(stage==Stage.Dressing||stage==Stage.Confirm)){studio.SetHome(false);studio.Pose(rotation,Mathf.Clamp(distance*.75f,4.8f,6.0f),.65f);cam.fieldOfView=52;return;}
            if(stage==Stage.Dialogue&&shopService){FrameShopDialogue();return;}
            if(stage==Stage.CharacterCreator){studio.SetHome(false);FrameCreatorPet();if(!creatorPetTab)studio.Pose(rotation,distance,0);return;}
            if(stage==Stage.City){
                if(showShopDecor){city.Frame(cam,reducedMotion,paused?0:Time.unscaledDeltaTime);FrameShopDecor();return;}
                var p=cam.transform.position;var r=cam.transform.rotation;float fov=cam.fieldOfView;
                city.Frame(cam,reducedMotion,paused?0:Time.unscaledDeltaTime);
                if(ShopSequenceActive&&paused){cam.transform.SetPositionAndRotation(p,r);cam.fieldOfView=fov;}
                else UpdateShopSequence(Time.unscaledDeltaTime);
                if(stage==Stage.City){
                    city.FramePlayerForCamera(cam,ShopSequenceActive);
                    if(!paused){if(city.IsHome)city.RenderHomeMirror(cam);else if(city.IsInterior)city.RenderShopMirror(cam,ShopSequenceActive);}
                }
                return;
            }
            studio.SetHome(stage==Stage.Home);studio.Pose(rotation,distance,stage==Stage.Welcome||stage==Stage.Home?-.95f:.9f);
        }
        void Update()
        {
            if(loadingActive){loadingClock+=Time.unscaledDeltaTime;return;}
            if(stage==Stage.WeekMemory)return;
            ShopProbeMark(0);
            studio.Talking=stage==Stage.Dialogue&&!paused;
            if(city!=null)city.CustomerSpeaking=stage==Stage.Dialogue&&shopService&&!paused;
            var retro=cam.GetComponent<RetroCamera>();retro.ReducedMotion=reducedMotion;retro.Paused=paused;retro.Filter=retroFilter;
            studio.ReducedMotion=reducedMotion;studio.Animate(Time.deltaTime,!paused);
            ShopProbeMark(1);
            if(stage==Stage.City)UpdateCity(Time.deltaTime);
            ShopProbeMark(2);
            if(!paused){if(stage==Stage.Dialogue&&shopService)shopCameraTime+=Time.unscaledDeltaTime;if(shopDressingInStore)shopDressingTime+=Time.unscaledDeltaTime;}
            UpdateCamera();
            ShopProbeMark(3);
            if(city!=null&&city.gameObject.activeInHierarchy)city.TickCat(Time.deltaTime,paused,reducedMotion,mutedAudio);
            if(stage==Stage.CharacterCreator&&creatorPetPreview!=null&&creatorPetTab)creatorPetPreview.Tick(Time.deltaTime,paused,reducedMotion,true);
            FinishShopFittingEntryIfReady();
            AdvanceClock(Time.deltaTime);
            if(stage==Stage.Leaving && !paused) {transition+=Time.deltaTime; if(transition>1.2f) stage=Stage.Post;}
            TickPresentation(Time.unscaledDeltaTime);
            TickContext(Time.unscaledDeltaTime);
            ShopProbeMark(4);
        }
        void AdvanceClock(float delta)
        {
            if(!paused&&Timed&&(stage==Stage.Dressing||stage==Stage.Confirm)){
                remaining=Mathf.Max(0,remaining-delta);
                if(remaining<=10&&!timerWarned){timerWarned=true;message="Últimos 10 segundos!";Sound(2);}
                if(remaining<=0)Submit();
            }
        }
        void Begin() {
            shopService=false;
            if(pendingPhotoBytes!=null&&!TrySave())return;
            if(progress.DayComplete){stage=Stage.Summary;return;}
            message="";
            replayIndex=-1;fromHome=false;inventory=progress.Available;remaining=FittingSeconds>0?FittingSeconds:120;timerWarned=false;
            customer=progress.posts.Count; outfit=Catalog.Default; question=-1; showRequest=false; showDrops=false;
            detailsScroll=Vector2.zero; rotation=-12;distance=7.2f;stage=Stage.Dialogue;Apply();UpdateCamera();
            mannequin.name=Customer.name+" — manequim provisório";
        }
        void SaveDay(){if(activeSaveSlot<0&&storageOverride==null)throw new InvalidOperationException("Selecione um save antes de salvar.");ProgressStore.Save(DayPath,progress);}
        void EnterDressing(){stage=Stage.Dressing;remaining=FittingSeconds>0?FittingSeconds:120;timerWarned=false;showRequest=false;}
        void GoHome(){if(DeferLoading(GoHome))return;shopService=false;if(!playerAppearance.created){OpenCreator(Stage.Home);return;}fromHome=true;replayIndex=-1;showWardrobe=false;EnterHome();}
        void Replay()
        {
            shopService=false;
            replayIndex=customer;record=progress.posts[customer];inventory=(string[])record.available.Clone();
            outfit=(string[])record.outfit.Clone();question=record.question;remaining=FittingSeconds>0?FittingSeconds:120;timerWarned=false;
            rotation=-12;distance=7.2f;showRequest=false;showDrops=false;stage=Stage.Dressing;Apply();
            message="Repetição: peças, pergunta e tempo do atendimento original.";
        }
        void RenderCurrentPhoto()
        {
            var previous=cam.targetTexture; var active=RenderTexture.active; var rt=RenderTexture.GetTemporary(480,480,24);
            var viewport=cam.rect;bool orthographic=cam.orthographic;float originalFov=cam.fieldOfView;cam.orthographic=false;cam.fieldOfView=36;cam.rect=new Rect(0,0,1,1);
            var position=mannequin.position; var cameraPosition=cam.transform.position; var cameraRotation=cam.transform.rotation; var pose=mannequin.rotation;
            try {
                if(plaza!=null){plaza.SetActive(false);Destroy(plaza);}
                plaza=PhotoScenery.Build(customer);
                mannequin.position=new Vector3(30,0,0); mannequin.rotation=Quaternion.identity;
                cam.transform.position=new Vector3(30,2.4f,-6.6f-studio.FramingPadding);cam.transform.LookAt(new Vector3(30,1.9f,0));
                cam.targetTexture=rt;cam.aspect=1;cam.Render(); RenderTexture.active=rt; if(photo!=null)Destroy(photo); photo=new Texture2D(480,480,TextureFormat.RGB24,false); photo.ReadPixels(new Rect(0,0,480,480),0,0); photo.Apply();
            }
            finally {mannequin.position=position; mannequin.rotation=pose; cam.transform.SetPositionAndRotation(cameraPosition,cameraRotation); cam.targetTexture=previous;cam.ResetAspect(); RenderTexture.active=active; RenderTexture.ReleaseTemporary(rt);cam.rect=viewport;cam.orthographic=orthographic;cam.fieldOfView=originalFov;}
        }
        void Submit()
        {
            if(campaignBeforePreview!=null){SubmitVisualPreview();return;}
            if(!SpecialOrderSatisfied()){message=SpecialOrderText;stage=Stage.Dressing;return;}
            if(replayIndex>=0&&DayOne.Score(customer,outfit)<=progress.posts[customer].score){
                record=progress.posts[customer];Continue();message="Seu melhor visual foi mantido. Sem alteração na Reputação.";replayIndex=-1;return;
            }

            RenderCurrentPhoto();
            var previousAchievements=progress.achievements.ToArray();bool wasReplay=replayIndex>=0;int reputationBefore=progress.reputation;
            string photoPath=NewPhotoPath();
            if(replayIndex>=0){progress.Improve(replayIndex,outfit);record=progress.posts[replayIndex];InvalidateFeedPhoto(record.photo);record.photo=photoPath;record.photoStyle=ArtRevision;replayIndex=-1;}
            else{record=new Attempt{completed=true,question=question,outfit=(string[])outfit.Clone(),photo=photoPath,photoStyle=ArtRevision};progress.Add(record);}
            NotifyAchievements(previousAchievements);

            pendingPhotoPath=record.photo;pendingPhotoBytes=photo.EncodeToPNG();
            if(TrySave())message=wasReplay?"Melhor resultado atualizado. Reputação +"+(progress.reputation-reputationBefore)+".":"Progresso salvo neste dispositivo.";
            Sound(1);transition=0;stage=Stage.Leaving;
            if(shopService)StartShopExit();
        }
        void SubmitVisualPreview()
        {
            DayProgress.CheckOutfit(outfit,ActiveInventory);
            RenderCurrentPhoto();
            record=new Attempt{completed=true,question=question,outfit=(string[])outfit.Clone(),available=(string[])ActiveInventory.Clone(),score=DayOne.Score(customer,outfit),reputation=0,photo="",photoStyle=ArtRevision};
            message="Prévia do visual. Sua campanha e Reputação foram preservadas.";
            Sound(1);transition=0;stage=Stage.Leaving;
        }
        void Continue()
        {
            outfit=(string[])record.outfit.Clone(); question=record.question; Apply();
            if(photo!=null)Destroy(photo); photo=null;
            try {if(File.Exists(record.photo)){photo=new Texture2D(2,2);if(!photo.LoadImage(File.ReadAllBytes(record.photo),true)){Destroy(photo);photo=null;}}}
            catch(Exception e){if(photo!=null)Destroy(photo);photo=null;message="Foto indisponível: "+e.Message;}
            stage=Stage.Post;
        }
        public void SmokeCheck()
        {
            var old=record; var oldProgress=progress; var oldCustomer=customer;var oldPreviewCampaign=campaignBeforePreview;var oldStorage=storageOverride;campaignBeforePreview=null;
            storageOverride=Path.Combine(Application.temporaryCachePath,"FitCheck-Smoke"); Directory.CreateDirectory(storageOverride);
            try {
                progress=new DayProgress(); Begin(); question=0; outfit=new[]{"AH_TOP_01","AH_BOTTOM_01","AH_SHOES_01",""}; Apply(); Submit();
                var saved=JsonUtility.FromJson<DayProgress>(File.ReadAllText(DayPath)); saved.Validate(); var loaded=saved.posts[0];
                if(loaded.score!=10 || loaded.reputation!=100 || loaded.question!=0 || !File.Exists(loaded.photo))throw new Exception("Falha na avaliação/foto/save");
                record=loaded; Continue(); if(photo==null || photo.width!=480)throw new Exception("Falha ao recuperar foto");
                Begin(); outfit=new[]{"CG_TOP_01","SB_BOTTOM_01","SB_SHOES_01","CG_HEAD_01"}; Apply(); Submit();
                Begin(); outfit=new[]{"AH_TOP_01","CG_BOTTOM_02","AH_SHOES_01",""}; Apply(); Submit();
                progress.Close(); SaveDay(); saved=JsonUtility.FromJson<DayProgress>(File.ReadAllText(DayPath)); saved.Validate();
                if(saved.posts.Count!=3 || saved.reputation!=300 || saved.rewards.Count!=6 || !saved.closed)throw new Exception("Falha no expediente/recompensas");
                for(int i=0;i<3;i++){customer=i;record=saved.posts[i];Continue();if(photo==null || photo.width!=480)throw new Exception("Foto de cliente ausente");}
                Debug.Log("FITCHECK_RUNTIME_PASSED: 3 clientes, 3 fotos, 300 de Reputação, 6 recompensas e recuperação de save. Teste isolado do jogador.");
            } finally {
                foreach(var file in Directory.GetFiles(storageOverride))File.Delete(file);
                storageOverride=oldStorage;campaignBeforePreview=oldPreviewCampaign; record=old; progress=oldProgress;customer=oldCustomer;
                stage=Stage.Welcome; outfit=Catalog.Default; question=-1; message=""; Apply();
            }
        }
    }
}
