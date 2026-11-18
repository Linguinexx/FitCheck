using System;
using System.Collections;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        IEnumerator CheckPolishPreferences(string folder){
            Home();storageOverride=Path.Combine(folder,"sessions",Guid.NewGuid().ToString("N"));Directory.CreateDirectory(storageOverride);progress=new DayProgress();mutedAudio=false;masterVolume=0;paused=false;
            EnterShop();shopMusicOpen=true;shopLeisureCursor=false;cityFreeCursor=true;city.FramePlayerForCamera(cam,false);
            ChooseShopSong(0,0);float deadline=Time.realtimeSinceStartup+25;
            while(ShopRecordClip==null&&Time.realtimeSinceStartup<deadline)yield return null;
            if(ShopRecordClip==null)throw new Exception("Faixa de revisão não carregou");
            TickPresentation(2);yield return null;SeekShopSong(25);
            paused=true;OpenSettings(true);audioSettingsOpen=true;TickPresentation(.1f);
            if(ShopSongSource==null||!ShopSongSource.isPlaying)throw new Exception("Música parou nas configurações da pausa");
            float time=ShopSongTime;yield return new WaitForSecondsRealtime(.2f);
            if(ShopSongTime<=time)throw new Exception("Relógio da faixa congelou na pausa");
            musicVolume=.23f;TickPresentation(.1f);
            if(Mathf.Abs(ShopSongSource.volume-.26f*.23f)>.01f)throw new Exception("Volume não respondeu na pausa");
            mutedAudio=true;TickPresentation(.1f);if(ShopSongSource.isPlaying)throw new Exception("Mute não silenciou a música");
            mutedAudio=false;TickPresentation(.1f);if(!ShopSongSource.isPlaying)throw new Exception("Música não voltou após mute");
            CaptureInterface(Path.Combine(folder,"audio-na-pausa.png"));
            audioSettingsOpen=false;SetOutlinePreference(false);CaptureInterface(Path.Combine(folder,"opcao-contorno.png"));
            if(InkOutlinePreview.Enabled||Shader.GetGlobalFloat("_InkEffectsDisabled")<.5f)throw new Exception("Contorno não desligou");
            SetOutlinePreference(true);if(!InkOutlinePreview.Enabled)throw new Exception("Contorno não religou");
            CloseSettings();paused=false;shopMusicOpen=true;shopRecordSelected=0;CaptureInterface(Path.Combine(folder,"capa-plastico.png"));
            CloseShopMoment();
            MoveProof(new Vector2(1.1f,4.2f),0,0);cam.transform.position=city.transform.TransformPoint(new Vector3(1.1f,2.5f,6.15f));cam.transform.LookAt(city.transform.TransformPoint(CityView.ShopMirrorCenter));
            city.FramePlayerForCamera(cam,false);SaveFirstPersonFrame(Path.Combine(folder,"espelho-menor.png"),1440,900);
            EnterHome();city.PlayerBodyVisible=false;cam.transform.position=city.transform.TransformPoint(new Vector3(.3f,3.1f,-.3f));cam.transform.LookAt(city.transform.TransformPoint(new Vector3(-3.4f,1,2.3f)));cam.fieldOfView=55;SetOutlinePreference(false);SaveFirstPersonFrame(Path.Combine(folder,"contorno-desligado.png"),1440,900);SetOutlinePreference(true);SaveFirstPersonFrame(Path.Combine(folder,"contorno-ligado.png"),1440,900);
            var bark=Resources.Load<AudioClip>("Audio/PetBark");if(bark==null||bark.length<.2f||bark.length>1)throw new Exception("Latido incorreto");
            for(int breed=0;breed<6;breed++){
                var go=new GameObject("Verificação do cão");var dog=go.AddComponent<ShopCat>();dog.Build(new PetAppearance{species=1,breed=breed,name="Revisão"});
                if(!dog.Pet(false)||dog.GetComponent<AudioSource>().clip!=bark||dog.Pet(false))throw new Exception("Latido/carinho/intervalo incorreto");
                dog.Tick(.1f,true,false,true);if(dog.GetComponent<AudioSource>().isPlaying)throw new Exception("Latido tocou na pausa");
                go.SetActive(false);Destroy(go);
            }
            SetOutlinePreference(false);musicVolume=1;paused=false;
            File.WriteAllText(Path.Combine(folder,"checks.txt"),"POLISH_PREFERENCES_PASSED: music continues during pause/settings with live volume and mute; outline off/on and shared furniture pass; sleeve gloss; smaller shop mirror; six dog breeds use the recorded bark with cooldown and pause. Native visual inspection accompanies checks.");
            Debug.Log(File.ReadAllText(Path.Combine(folder,"checks.txt")));
        }
    }
}
