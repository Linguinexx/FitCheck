using System.Collections.Generic;
using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        sealed class ShopSong
        {
            public readonly string title,artist,path;
            public ShopSong(string title,string artist,string path){this.title=title;this.artist=artist;this.path=path;}
        }
        static readonly ShopSong[][] ShopSongs={
            new[]{new ShopSong("It Was a Good Day","Ice Cube","laje_01"),new ShopSong("Big Poppa","The Notorious B.I.G.","laje_02"),new ShopSong("Regulate","Warren G feat. Nate Dogg","laje_03"),new ShopSong("Gin and Juice","Snoop Dogg","laje_04"),new ShopSong("Superman","Eminem feat. Dina Rae","laje_05")},
            new[]{new ShopSong("O Descobridor dos Sete Mares","Tim Maia","passo_01"),new ShopSong("Samurai","Djavan","passo_02"),new ShopSong("Tudo Vai Dar Certo","Natiruts e Amani Kush","passo_03"),new ShopSong("Construção","Chico Buarque","passo_04"),new ShopSong("Cotidiano","Chico Buarque","passo_05")},
            new[]{new ShopSong("Flow","Sade","dez_01"),new ShopSong("Mr Wrong","Sade","dez_02"),new ShopSong("On & On","Erykah Badu","dez_03"),new ShopSong("Sometimes…","Erykah Badu","dez_04"),new ShopSong("4:00 A.M.","Taeko Ohnuki","dez_05")}
        };
        readonly Dictionary<string,AudioClip> shopSongCache=new Dictionary<string,AudioClip>();
        readonly List<string> shopSongRelease=new List<string>();
        ResourceRequest shopSongRequest;string shopSongRequestKey;
        bool shopPlaybackPaused,shopSongWasPlaying;string shopSongFailure,lastShopSongKey;float lastShopSongTime;
        ShopSong CurrentShopSong=>ShopSongs[StreetMemory.musicChoice][Mathf.Clamp(StreetMemory.musicTrack,0,4)];
        AudioClip LoadShopSong(){
            if(shopSongRequest!=null&&shopSongRequest.isDone){
                var loaded=shopSongRequest.asset as AudioClip;
                if(loaded!=null)shopSongCache[shopSongRequestKey]=loaded;
                else shopSongFailure=shopSongRequestKey;
                shopSongRequest=null;
            }
            string key=CurrentShopSong.path;
            if(shopSongCache.TryGetValue(key,out var clip))return clip;
            if(shopSongRequest==null&&shopSongFailure!=key){shopSongRequestKey=key;shopSongRequest=Resources.LoadAsync<AudioClip>("Audio/ShopRecords/"+key);}
            return null;
        }
        bool ShopSongLoading=>!StreetMemory.musicStopped&&!shopSongCache.ContainsKey(CurrentShopSong.path)&&shopSongFailure!=CurrentShopSong.path;
        AudioSource ShopSongSource{
            get{
                if(!shopSongCache.TryGetValue(CurrentShopSong.path,out var clip))return null;
                return incomingMusic!=null&&incomingMusic.clip==clip?incomingMusic:music!=null&&music.clip==clip?music:null;
            }
        }
        float ShopSongTime=>ShopSongSource!=null?ShopSongSource.time:0;
        float ShopSongLength=>shopSongCache.TryGetValue(CurrentShopSong.path,out var clip)?clip.length:0;
        void ChooseShopSong(int disc,int song){
            if(ShopOpen||disc<0||disc>2||song<0||song>4)return;
            int before=StreetMemory.musicChoice,track=StreetMemory.musicTrack;bool stopped=StreetMemory.musicStopped;
            StreetMemory.musicChoice=disc;StreetMemory.musicTrack=song;StreetMemory.musicStopped=false;
            if(!TrySave()){StreetMemory.musicChoice=before;StreetMemory.musicTrack=track;StreetMemory.musicStopped=stopped;return;}
            shopPlaybackPaused=false;shopSongFailure=null;
        }
        void StepShopSong(int direction){ChooseShopSong(StreetMemory.musicChoice,(StreetMemory.musicTrack+direction+5)%5);}
        void TickShopPlaylist(bool inShop){
            if(!inShop){shopPlaybackPaused=false;shopSongWasPlaying=false;lastShopSongKey=null;}
            if(inShop&&!StreetMemory.musicStopped&&!shopPlaybackPaused&&!mutedAudio){
                var source=ShopSongSource;
                bool ended=source!=null&&lastShopSongKey==CurrentShopSong.path&&shopSongWasPlaying&&!source.isPlaying&&lastShopSongTime>source.clip.length-1;
                if(source!=null&&source.clip.length>1&&(source.isPlaying&&source.time>=source.clip.length-.15f||ended)){
                    // End-of-track advance also works during the workday; only manual editing is closed.
                    int old=StreetMemory.musicTrack;StreetMemory.musicTrack=(old+1)%5;
                    if(!TrySave())StreetMemory.musicTrack=old;
                }
                lastShopSongKey=CurrentShopSong.path;lastShopSongTime=source!=null?source.time:0;shopSongWasPlaying=source!=null&&source.isPlaying;
            }
            shopSongRelease.Clear();
            foreach(var pair in shopSongCache){
                if(pair.Key==CurrentShopSong.path&&inShop||music!=null&&music.clip==pair.Value||incomingMusic!=null&&incomingMusic.clip==pair.Value)continue;
                Resources.UnloadAsset(pair.Value);shopSongRelease.Add(pair.Key);
            }
            foreach(var key in shopSongRelease)shopSongCache.Remove(key);
        }
        void SeekShopSong(float position){var source=ShopSongSource;if(source!=null)source.time=Mathf.Clamp(position,0,Mathf.Max(0,source.clip.length-.25f));}
        static string SongClock(float seconds){int value=Mathf.Max(0,Mathf.FloorToInt(seconds));return value/60+":"+(value%60).ToString("00");}
    }
}
