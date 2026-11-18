using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        Texture2D[] recordCovers;
        static readonly string[] RecordNotes={"HIP-HOP / RAP","BRASIL / GROOVE","SOUL / MADRUGADA"};
        GUIStyle songTitle;
        static readonly Color[] RecordColors={new Color32(229,45,56,255),new Color32(205,230,65,255),new Color32(154,119,225,255)};
        void RecordDisc(Rect rect,int choice,float angle){
            var center=rect.center;RoundedPanel(rect,new Color(.025f,.023f,.028f),rect.width*.5f);
            for(int i=0;i<7;i++){float inset=10+i*7;var ring=new Rect(rect.x+inset,rect.y+inset,rect.width-2*inset,rect.height-2*inset);RoundedPanel(ring,new Color(.16f,.15f,.17f),ring.width*.5f,1);}
            float d=rect.width*.28f;RoundedPanel(new Rect(center.x-d/2,center.y-d/2,d,d),RecordColors[choice],d/2);
            var matrix=GUI.matrix;GUI.matrix=matrix*Matrix4x4.TRS(center,Quaternion.Euler(0,0,angle),Vector3.one);Fill(new Rect(0,-d*.39f,3,d*.22f),paper);GUI.matrix=matrix;
            RoundedPanel(new Rect(center.x-4,center.y-4,8,8),KaosPanel,4);
        }
        void ShopRecordsUI(){
            if(recordCovers==null)recordCovers=new[]{Resources.Load<Texture2D>("UI/ShopRecords/laje"),Resources.Load<Texture2D>("UI/ShopRecords/passo"),Resources.Load<Texture2D>("UI/ShopRecords/dez")};
            if(songTitle==null)songTitle=new GUIStyle(copy){fontSize=18,wordWrap=false,clipping=TextClipping.Clip};
            int selected=shopRecordSelected;bool playing=!StreetMemory.musicStopped,chosenPlaying=playing&&StreetMemory.musicChoice==selected;
            DrawKaosPanel(new Rect(90,70,1260,760));
            Txt(new Rect(124,94,920,42),"A VEZ DO DISCO",heading,paper);
            Txt(new Rect(125,143,1050,30),"DRIPLAB SESSIONS   /   escolha uma capa, encontre sua faixa",caption,muted);
            RoundedPanel(new Rect(124,191,514,383),new Color32(38,34,39,255),14);
            // The sleeve and vinyl are separate: browsing does not switch playback.
            bool spinning=chosenPlaying&&!shopPlaybackPaused&&!ShopSongLoading&&!mutedAudio;
            RecordDisc(new Rect(354,244,250,250),selected,reducedMotion||paused||!spinning?0:contextClock*44);
            DrawRecordSleeve(new Rect(144,218,296,296),recordCovers[selected]);
            RoundedPanel(new Rect(587,207,20,20),new Color(.68f,.68f,.61f),10);
            var m=GUI.matrix;GUI.matrix=m*Matrix4x4.TRS(new Vector2(597,218),Quaternion.Euler(0,0,spinning?22:8),Vector3.one);RoundedPanel(new Rect(-4,0,8,163),new Color(.64f,.64f,.59f),3);RoundedPanel(new Rect(-8,150,18,36),InkRecord,3);GUI.matrix=m;
            Txt(new Rect(144,534,470,25),RecordNotes[selected]+"   /   5 FAIXAS",caption,RecordColors[selected]);
            Txt(new Rect(668,191,635,45),ShopMusicNames[selected],heading,paper);
            Txt(new Rect(670,233,590,24),"FAIXA / ARTISTA                                      CLIQUE PARA TOCAR",tiny,muted);
            for(int i=0;i<5;i++){
                var r=new Rect(668,266+i*59,646,53);var song=ShopSongs[selected][i];
                bool current=chosenPlaying&&StreetMemory.musicTrack==i;
                bool hover=r.Contains(Event.current.mousePosition);
                RoundedPanel(r,current?RecordColors[selected]*.20f:hover?KaosControl*1.18f:KaosControl,8);
                if(current)RoundedPanel(r,RecordColors[selected],8,2);
                Txt(new Rect(r.x+13,r.y+14,28,25),(i+1).ToString("00"),caption,current?RecordColors[selected]:muted);
                Txt(new Rect(r.x+54,r.y+3,535,27),song.title,songTitle,paper);
                Txt(new Rect(r.x+54,r.y+29,540,22),song.artist,tiny,current?RecordColors[selected]:muted);
                if(current)Txt(new Rect(r.xMax-42,r.y+12,30,28),shopPlaybackPaused?"II":"•",copy,RecordColors[selected]);
                if(GUI.Button(r,new GUIContent("",song.title+" — "+song.artist),GUIStyle.none)){ChooseShopSong(selected,i);Sound(0);}
            }
            for(int i=0;i<3;i++){
                var r=new Rect(124+i*173,600,164,59);bool active=selected==i;
                RoundedPanel(r,active?RecordColors[i]*.24f:KaosControl,10);if(active)RoundedPanel(r,RecordColors[i],10,2);
                Txt(new Rect(r.x+12,r.y+7,142,26),ShopMusicNames[i],caption,paper);
                Txt(new Rect(r.x+12,r.y+34,142,21),playing&&StreetMemory.musicChoice==i?"NO PRATO  •":"VOL. 0"+(i+1),tiny,active?RecordColors[i]:muted);
                if(GUI.Button(r,GUIContent.none,GUIStyle.none)){shopRecordSelected=i;Sound(0);}
            }
            if(Tap(new Rect(668,600,235,53),chosenPlaying?"Disco no prato":"Colocar disco",true,!chosenPlaying))ChooseShopMusic(selected);
            if(Tap(new Rect(919,600,185,53),"Retirar disco",false,playing))StopShopRecord();
            if(Tap(new Rect(1120,600,194,53),"Voltar à loja"))CloseShopMoment();
            RoundedPanel(new Rect(124,686,1190,112),InkRecord,12);
            string status=!playing?"AGULHA DESCANSANDO":ShopSongLoading?"CARREGANDO FAIXA":shopSongFailure==CurrentShopSong.path?"FAIXA INDISPONÍVEL":shopPlaybackPaused?"EM PAUSA":"TOCANDO AGORA";
            Txt(new Rect(144,697,500,20),status,tiny,RecordColors[StreetMemory.musicChoice]);
            Txt(new Rect(144,722,510,27),playing?CurrentShopSong.title:"Escolha uma música",songTitle,paper);
            Txt(new Rect(144,752,490,24),playing?CurrentShopSong.artist+" / "+ShopMusicNames[StreetMemory.musicChoice]:"Três discos. Quinze faixas.",caption,muted);
            if(Tap(new Rect(682,703,108,44),"Anterior",false,playing))StepShopSong(-1);
            if(Tap(new Rect(802,703,116,44),shopPlaybackPaused?"Continuar":"Pausar",true,playing&&!ShopSongLoading))shopPlaybackPaused=!shopPlaybackPaused;
            if(Tap(new Rect(930,703,108,44),"Próxima",false,playing))StepShopSong(1);
            Txt(new Rect(1070,710,235,26),SongClock(ShopSongTime)+" / "+SongClock(ShopSongLength),caption,paper);
            float length=ShopSongLength,currentTime=ShopSongTime;
            GUI.enabled=playing&&length>0;
            float seek=SettingsSlider(new Rect(682,752,610,33),length>0?currentTime/length:0);
            GUI.enabled=true;
            if(length>0&&Mathf.Abs(seek-currentTime/length)>.001f)SeekShopSong(seek*length);
        }
        static readonly Color InkRecord=new Color(.055f,.05f,.06f);
    }
}
