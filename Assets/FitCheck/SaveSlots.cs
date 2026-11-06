using System;
using System.IO;
using UnityEngine;

namespace FitCheck
{
    public sealed class SaveSlotInfo
    {
        public bool exists;public string error;
        public PlayerAppearance appearance=new PlayerAppearance();public DayProgress progress=new DayProgress();
    }
    public static partial class SaveSlots
    {
        public const int Count=3;
        public static string Folder(string library,int slot)
        {if(slot<0||slot>=Count)throw new ArgumentOutOfRangeException(nameof(slot));return Path.Combine(library,"saves","slot-"+(slot+1));}
        public static SaveSlotInfo Read(string library,int slot)
        {
            string folder=Folder(library,slot);var result=new SaveSlotInfo{exists=Directory.Exists(folder)&&Directory.GetFiles(folder).Length>0};
            if(!result.exists)return result;
            try{
                string day=Path.Combine(folder,"day-one.json");
                if(File.Exists(day)||File.Exists(day+".bak")){
                    try{result.progress=ProgressStore.Load(day);}catch{result.progress=ProgressStore.Load(day+".bak");}
                }else if(File.Exists(Path.Combine(folder,"first-customer.json"))){
                    var attempt=JsonUtility.FromJson<Attempt>(File.ReadAllText(Path.Combine(folder,"first-customer.json")));
                    if(attempt!=null&&attempt.completed)result.progress.Add(attempt);
                }
                result.appearance=AppearanceStore.Load(Path.Combine(folder,"player-appearance.json"));
            }catch(Exception e){result.error="Não foi possível abrir este save. Os arquivos foram preservados. "+e.Message;}
            return result;
        }
        public static void ImportLegacy(string library)
        {
            if(File.Exists(Path.Combine(library,"saves","legacy-imported.flag")))return;
            string folder=Folder(library,0);
            if(Directory.Exists(folder)&&Directory.GetFiles(folder).Length>0){File.WriteAllText(Path.Combine(library,"saves","legacy-imported.flag"),"1");return;}
            string[] names={"day-one.json","day-one.json.bak","first-customer.json","player-appearance.json","player-appearance.json.bak"};
            bool found=false;foreach(string name in names)if(File.Exists(Path.Combine(library,name)))found=true;
            if(!found)return;
            Directory.CreateDirectory(folder);
            // Keep the originals and the absolute photo paths used by the existing FitFeed.
            foreach(string name in names){string source=Path.Combine(library,name);if(File.Exists(source))File.Copy(source,Path.Combine(folder,name),false);}
            File.WriteAllText(Path.Combine(library,"saves","legacy-imported.flag"),"1");
        }
    }
    public partial class FirstCustomer
    {
        int activeSaveSlot=-1,selectedSave;string saveLibraryOverride;
        SaveSlotInfo[] saveSlots=new SaveSlotInfo[SaveSlots.Count];Texture2D titlePhoto,titleLogo;
        string SaveLibrary=>saveLibraryOverride??Application.persistentDataPath;
        string SessionFolder=>storageOverride??(activeSaveSlot<0?Application.persistentDataPath:SaveSlots.Folder(SaveLibrary,activeSaveSlot));
        void OpenSaveSelection()
        {
            Home();paused=false;
            try{SaveSlots.ImportLegacy(SaveLibrary);for(int i=0;i<SaveSlots.Count;i++)saveSlots[i]=SaveSlots.Read(SaveLibrary,i);PreviewSave(selectedSave);}
            catch(Exception e){message="Não foi possível ler os saves: "+e.Message;}
        }
        void PreviewSave(int slot)
        {
            selectedSave=slot;var info=saveSlots[slot];if(info==null)return;
            studio.PlayerLook=info.appearance;studio.ApplyUniform(info.progress.EquippedUniform);rotation=-12;distance=7.2f;UpdateCamera();
        }
        bool OpenSelectedSave()
        {
            var info=SaveSlots.Read(SaveLibrary,selectedSave);
            if(info.error!=null){message=info.error;return false;}
            if(pendingPhotoBytes!=null&&!TrySave())return false;
            Home();activeSaveSlot=selectedSave;storageOverride=null;progress=info.progress;playerAppearance=info.appearance;
            record=new Attempt();photo=null;inventory=null;question=-1;customer=0;outfit=Catalog.Default;
            notices.Clear();notice="";message="";ShowPlayer();
            // A cached shop can contain a customer from another save with the same index.
            if(shop!=null){shop.gameObject.SetActive(false);Destroy(shop.gameObject);shop=null;}
            if(info.exists&&playerAppearance.created)GoHome();else OpenCreator(Stage.Home);
            return true;
        }
        void ReturnToTitle(){Home();paused=false;StartLiveTitle();}
        void BuildTitlePhoto()
        {
            EnterCity();city.Frame(cam,true,0);city.PlayerBodyVisible=false;
            cam.transform.position=city.transform.position+new Vector3(-5.5f,3.1f,-12.5f);
            cam.transform.LookAt(city.transform.position+new Vector3(7.5f,4.2f,4.0f));cam.fieldOfView=51;
            var oldTarget=cam.targetTexture;var oldActive=RenderTexture.active;float aspect=cam.aspect;
            var target=RenderTexture.GetTemporary(1440,900,24,RenderTextureFormat.ARGB32);
            try{
                cam.targetTexture=target;cam.aspect=1.6f;cam.Render();RenderTexture.active=target;
                titlePhoto=new Texture2D(1440,900,TextureFormat.RGB24,false){name="Fachada DripLab — foto do menu",filterMode=FilterMode.Bilinear};
                titlePhoto.ReadPixels(new Rect(0,0,1440,900),0,0);titlePhoto.Apply();
            }finally{cam.targetTexture=oldTarget;cam.aspect=aspect;RenderTexture.active=oldActive;RenderTexture.ReleaseTemporary(target);Home();}
        }
        void TitleUI()
        {
            if(!titleSceneActive&&titlePhoto!=null)GUI.DrawTexture(new Rect(0,0,1440,900),titlePhoto,ScaleMode.ScaleAndCrop);
            Fill(new Rect(0,0,500,900),new Color(.025f,.028f,.025f,.87f));
            Txt(new Rect(58,88,400,30),"DRIPLAB  /  DEPOIS DO HORÁRIO",caption,CreatorRed);
            if(titleLogo==null)titleLogo=Resources.Load<Texture2D>("Brand/FitCheck-Selo-Kaos-Red");
            if(titleLogo!=null)DrawAnimatedTitleLogo(new Rect(52,160,390,260));else Txt(new Rect(50,178,450,200),"FIT\nCHECK",display);
            Txt(new Rect(60,430,410,140),"Seu estilo.\nAs histórias da cidade.",heading);
            if(Tap(new Rect(60,651,360,70),"Jogar  →",true))BeginLoading(OpenSaveSelection);
            if(Tap(new Rect(60,746,360,56),"Configurações"))OpenSettings(false);
            Txt(new Rect(60,832,420,55),"A game by DripLab",caption,paper);
        }
        void SaveSelectionUI()
        {
            Fill(new Rect(0,88,545,812),new Color(.055f,.06f,.055f,.97f));
            Txt(new Rect(40,122,470,30),"ESCOLHA SEU SAVE",caption,CreatorRed);
            Txt(new Rect(40,168,470,65),"Sua história.",heading);
            for(int i=0;i<SaveSlots.Count;i++){
                var info=saveSlots[i]??new SaveSlotInfo();float y=261+i*134;
                if(Tap(new Rect(40,y,462,118),"",selectedSave==i))PreviewSave(i);
                var color=paper;
                Txt(new Rect(58,y+12,420,30),"0"+(i+1)+"  /  "+(info.exists?info.appearance.name:"Novo save"),action,color);
                string subtitle=info.error!=null?"Save indisponível — arquivos preservados":!info.exists?"Crie um personagem e comece sua semana":info.progress.Complete?"Semana concluída  /  9 publicações":"Dia 0"+info.progress.day+"  /  "+info.progress.posts.Count+" publicações";
                Txt(new Rect(58,y+59,420,48),subtitle,caption,color);
            }
            var chosen=saveSlots[selectedSave]??new SaveSlotInfo();
            if(Tap(new Rect(40,693,462,62),chosen.exists?"Continuar com este personagem  →":"Criar personagem  →",true,chosen.error==null))BeginLoading(()=>{OpenSelectedSave();});
            if(Tap(new Rect(40,776,180,48),"Voltar"))ReturnToTitle();
            if(Tap(new Rect(237,776,265,48),"Excluir save",false,chosen.exists))RequestSaveDeletion();
            Txt(new Rect(40,839,462,48),message.Length>0?message:"Até três saves. Cada personagem tem sua própria semana.",tiny,muted);
        }
    }
}
