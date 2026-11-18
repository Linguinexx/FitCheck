using System;
using System.Collections;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        IEnumerator CheckSuppliedPlaylist(string folder){
            Home();storageOverride=Path.Combine(folder,"sessions",Guid.NewGuid().ToString("N"));Directory.CreateDirectory(storageOverride);progress=new DayProgress();paused=false;mutedAudio=false;masterVolume=0;
            EnterShop();shopMusicOpen=true;shopLeisureCursor=false;cityFreeCursor=true;city.FramePlayerForCamera(cam,false);
            for(int disc=0;disc<3;disc++)for(int song=0;song<5;song++){
                ChooseShopSong(disc,song);float deadline=Time.realtimeSinceStartup+30;
                while(ShopRecordClip==null&&Time.realtimeSinceStartup<deadline)yield return null;
                var clip=ShopRecordClip;if(clip==null||clip.length<30)throw new Exception("Faixa ausente: "+disc+"/"+song);
                if(clip.channels!=2)throw new Exception("Faixa perdeu estéreo");
                TickPresentation(2);yield return null;
                if(ShopSongSource==null||ShopSongLength<30)throw new Exception("Fonte não recebeu a faixa");
                SeekShopSong(30);if(Mathf.Abs(ShopSongTime-30)>1)throw new Exception("Busca na faixa falhou");
                shopPlaybackPaused=true;TickPresentation(.1f);if(ShopSongSource.isPlaying)throw new Exception("Pausa do player falhou");
                shopPlaybackPaused=false;paused=true;TickPresentation(.1f);if(!ShopSongSource.isPlaying)throw new Exception("Música parou no menu de pausa");
                paused=false;TickPresentation(.1f);
                if(shopSongCache.Count>3)throw new Exception("Cache de músicas cresceu sem limite");
                var saved=ProgressStore.Load(DayPath).neighbourhood;
                if(saved.musicChoice!=disc||saved.musicTrack!=song||saved.musicStopped)throw new Exception("Faixa não persistiu");
                if(song==0){shopRecordSelected=disc;CaptureInterface(Path.Combine(folder,"disco-"+disc+".png"));}
            }
            int previousDisc=StreetMemory.musicChoice,previousSong=StreetMemory.musicTrack;shopRecordSelected=0;
            if(StreetMemory.musicChoice!=previousDisc||StreetMemory.musicTrack!=previousSong)throw new Exception("Navegar trocou reprodução");
            shopPlaybackPaused=true;CaptureInterface(Path.Combine(folder,"pausado.png"));shopPlaybackPaused=false;
            StepShopSong(1);if(StreetMemory.musicTrack!=0)throw new Exception("Última faixa não voltou à primeira");
            StepShopSong(-1);if(StreetMemory.musicTrack!=4)throw new Exception("Anterior não voltou à última");
            StopShopRecord();TickPresentation(.1f);if(music.clip!=null||incomingMusic.clip!=null)throw new Exception("Retirar não parou");
            var legacy=JsonUtility.FromJson<NeighbourhoodProgress>("{\"musicChoice\":1}");legacy.Validate();if(legacy.musicTrack!=0)throw new Exception("Migração legada incorreta");
            ChooseShopSong(1,2);EnterHome();TickPresentation(2);yield return null;
            if(ShopLeisurePanelOpen||music.clip!=tracks[2])throw new Exception("Música da loja vazou para casa");
            var other=new DayProgress();if(other.neighbourhood.musicTrack!=0||other.neighbourhood.musicChoice!=0)throw new Exception("Preferência vazou entre saves");
            File.WriteAllText(Path.Combine(folder,"checks.txt"),"SUPPLIED_PLAYLIST_PASSED: 15 stereo tracks loaded asynchronously, per-track seek/play/pause and game pause, persistence, bounded cache, browse isolation, track wrap, stop, legacy and home isolation. Audio measured, not evaluated by direct listening.");
            Debug.Log(File.ReadAllText(Path.Combine(folder,"checks.txt")));
        }
    }
}
