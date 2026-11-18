using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        Texture2D[] recordCovers;
        static readonly string[] RecordNotes={"Rhodes quente, baixo redondo e batida de laje.","Guitarra picada e um groove que anda torto.","Vibrafone, luz baixa e conversa sem pressa."};
        static readonly int[] RecordBpm={92,98,84};
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
            int selected=shopRecordSelected;bool playing=!StreetMemory.musicStopped,chosenPlaying=playing&&StreetMemory.musicChoice==selected;
            DrawKaosPanel(new Rect(110,92,1220,716));
            Txt(new Rect(146,116,900,42),"A VEZ DO DISCO",heading,paper);Txt(new Rect(147,163,1000,30),"DRIPLAB SESSIONS   /   três instrumentais exclusivos da caixa de som",caption,muted);
            RoundedPanel(new Rect(146,216,590,377),new Color32(38,34,39,255),14);
            // The sleeve and vinyl are separate: browsing does not switch playback.
            RecordDisc(new Rect(411,263,278,278),selected,reducedMotion||paused||!chosenPlaying?0:contextClock*44);
            if(recordCovers[selected]!=null)GUI.DrawTexture(new Rect(172,248,292,292),recordCovers[selected],ScaleMode.ScaleToFit);
            RoundedPanel(new Rect(654,241,20,20),new Color(.68f,.68f,.61f),10);
            var m=GUI.matrix;GUI.matrix=m*Matrix4x4.TRS(new Vector2(664,252),Quaternion.Euler(0,0,chosenPlaying?22:8),Vector3.one);RoundedPanel(new Rect(-4,0,8,163),new Color(.64f,.64f,.59f),3);RoundedPanel(new Rect(-8,150,18,36),InkRecord,3);GUI.matrix=m;
            Txt(new Rect(760,222,530,55),ShopMusicNames[selected],heading,paper);
            Txt(new Rect(762,282,500,30),"VOL. 0"+(selected+1)+"   /   "+RecordBpm[selected]+" BPM   /   INSTRUMENTAL",tiny,RecordColors[selected]);
            Txt(new Rect(762,330,482,68),RecordNotes[selected],copy,paper);
            Txt(new Rect(762,417,480,50),chosenPlaying?"NO PRATO AGORA":playing?"TOCANDO: "+ShopMusicNames[StreetMemory.musicChoice]:"AGULHA DESCANSANDO",caption,chosenPlaying?RecordColors[selected]:muted);
            if(Tap(new Rect(762,486,252,58),chosenPlaying?"Tocando":"Colocar para tocar",true,!chosenPlaying))ChooseShopMusic(selected);
            if(Tap(new Rect(1030,486,252,58),"Retirar disco",false,playing))StopShopRecord();
            for(int i=0;i<3;i++){
                var r=new Rect(146+i*382,615,366,97);bool active=selected==i;
                RoundedPanel(r,active?RecordColors[i]*.24f:KaosControl,10);if(active)RoundedPanel(r,RecordColors[i],10,2);
                if(recordCovers[i]!=null)GUI.DrawTexture(new Rect(r.x+9,r.y+9,78,78),recordCovers[i],ScaleMode.ScaleToFit);
                Txt(new Rect(r.x+103,r.y+15,250,30),ShopMusicNames[i],copy,paper);
                Txt(new Rect(r.x+103,r.y+53,250,25),playing&&StreetMemory.musicChoice==i?"NO PRATO  •":"VOL. 0"+(i+1)+"  /  "+RecordBpm[i]+" BPM",tiny,active?RecordColors[i]:muted);
                if(GUI.Button(r,GUIContent.none,GUIStyle.none)){shopRecordSelected=i;Sound(0);}
            }
            Txt(new Rect(148,747,755,29),"Escolha uma capa e coloque o disco para tocar. Volume em Configurações.",tiny,muted);
            if(Tap(new Rect(1030,739,252,48),"Voltar à loja"))CloseShopMoment();
        }
        static readonly Color InkRecord=new Color(.055f,.05f,.06f);
    }
}
