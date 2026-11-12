using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        bool controlsOpen;
        int controlsPage;
        GUIStyle controlsKey,controlsSmallKey;
        static readonly string[][] ControlKeyRows={
            new[]{"Esc","1","2","3","4","5","6","7","8","9","0","−","=","Back"},
            new[]{"Tab","Q","W","E","R","T","Y","U","I","O","P","[","]","\\"},
            new[]{"Caps","A","S","D","F","G","H","J","K","L",";","'","Enter"},
            new[]{"Shift","Z","X","C","V","B","N","M",",",".","/","Shift"},
            new[]{"Ctrl","Win","Alt","Espaço","Alt","Fn","Ctrl"}
        };
        static readonly float[][] ControlKeyWidths={
            new[]{1f,1,1,1,1,1,1,1,1,1,1,1,1,2},
            new[]{1.5f,1,1,1,1,1,1,1,1,1,1,1,1,1.5f},
            new[]{1.8f,1,1,1,1,1,1,1,1,1,1,1,2.2f},
            new[]{2.4f,1,1,1,1,1,1,1,1,1,1,2.6f},
            new[]{1.4f,1.2f,1.3f,6.2f,1.3f,1.2f,1.4f}
        };
        void OpenControls(){controlsOpen=true;controlsPage=0;dragging=false;}
        void ControlsKeyInput(Event input)
        {
            if(!settingsOpen||!controlsOpen||input.type!=EventType.KeyDown)return;
            if(input.keyCode==KeyCode.LeftArrow||input.keyCode==KeyCode.RightArrow){controlsPage=1-controlsPage;input.Use();}
        }
        bool ControlKeyActive(string label)
        {
            if(label=="Esc")return Application.platform!=RuntimePlatform.WebGLPlayer;
            if(label=="P"&&Application.platform==RuntimePlatform.WebGLPlayer)return true;
            return controlsPage==0&&(label=="W"||label=="A"||label=="S"||label=="D"||
                label=="E"||label=="F"||label=="Shift"||label=="Tab");
        }
        void DrawControlKey(Rect area,string label,bool active,bool small=false)
        {
            RoundedPanel(new Rect(area.x,area.y+4,area.width,area.height),Color.black,7);
            RoundedPanel(area,active?CreatorRed:KaosControl,7);
            Fill(new Rect(area.x+8,area.y+2,Mathf.Max(0,area.width-16),1),new Color(1,1,1,active?.24f:.10f));
            Txt(area,label,small?controlsSmallKey:controlsKey,active?paper:new Color(.62f,.59f,.62f));
        }
        void DrawControlsKeyboard()
        {
            var board=new Rect(110,253,856,292);
            RoundedPanel(new Rect(board.x,board.y+8,board.width,board.height),Color.black,16);
            RoundedPanel(board,new Color(.12f,.105f,.12f),16);
            RoundedPanel(board,KaosTrack,16,1);
            const float start=126,totalWidth=824,gap=5;
            for(int row=0;row<ControlKeyRows.Length;row++){
                var labels=ControlKeyRows[row];var widths=ControlKeyWidths[row];
                float sum=0;for(int i=0;i<widths.Length;i++)sum+=widths[i];
                float unit=(totalWidth-gap*(labels.Length-1))/sum,x=start;
                for(int key=0;key<labels.Length;key++){
                    float width=unit*widths[key];
                    DrawControlKey(new Rect(x,269+row*53,width,43),labels[key],ControlKeyActive(labels[key]),labels[key].Length>1);
                    x+=width+gap;
                }
            }
        }
        void DrawControlsMouse()
        {
            const float x=1076,y=267,w=132,h=196;
            RoundedPanel(new Rect(x,y+7,w,h),Color.black,58);
            RoundedPanel(new Rect(x,y,w,h),KaosControl,58);
            RoundedPanel(new Rect(x,y,w,h),new Color(.48f,.43f,.47f),58,2);
            // The two buttons follow the outer shell's curved cap, separated by
            // physical seams; they are no longer inset rounded-square tiles.
            GUI.BeginGroup(new Rect(x+4,y+4,w/2-6,87));
            RoundedPanel(new Rect(0,0,w-8,h-8),controlsPage==1?CreatorRed:KaosHover,54);
            GUI.EndGroup();
            GUI.BeginGroup(new Rect(x+w/2+2,y+4,w/2-6,87));
            RoundedPanel(new Rect(-w/2-2+4,0,w-8,h-8),KaosHover,54);
            GUI.EndGroup();
            RoundedPanel(new Rect(x+55,y+22,22,43),KaosPanel,10);
            RoundedPanel(new Rect(x+60,y+27,12,31),controlsPage==1?CreatorRed:paper,6);
            for(int i=0;i<3;i++)Fill(new Rect(x+62,y+33+i*7,8,1),new Color(0,0,0,.5f));
            Txt(new Rect(1010,486,264,28),controlsPage==0?"Mover o mouse":"Clique, arraste e role",controlsKey,paper);
            Txt(new Rect(1010,518,264,26),controlsPage==0?"Olhar ao redor":"Sobre o personagem",controlsSmallKey,muted);
        }
        void ControlsLegend(float x,float y,string key,string title,string detail="")
        {
            float width=key.Length>8?114:key.Length>4?96:76;
            DrawControlKey(new Rect(x,y,width,40),key,true,true);
            Txt(new Rect(x+width+14,y,252-width+40,30),title,caption,paper);
            if(detail.Length>0)Txt(new Rect(x+width+14,y+31,290-width,30),detail,tiny,muted);
        }
        void ControlsUI()
        {
            if(controlsKey==null){
                controlsKey=new GUIStyle(action){alignment=TextAnchor.MiddleCenter,fontSize=19};
                controlsSmallKey=new GUIStyle(controlsKey){fontSize=15};
            }
            DrawKaosPanel(new Rect(64,64,1312,774));
            Fill(new Rect(102,100,36,3),CreatorRed);
            Txt(new Rect(102,114,650,48),"CONTROLES",heading,paper);
            Txt(new Rect(1102,114,230,34),controlsPage==0?"01 / 02":"02 / 02",controlsKey,muted);
            if(Tap(new Rect(102,177,240,48),"Exploração",controlsPage==0))controlsPage=0;
            if(Tap(new Rect(354,177,240,48),"Provador",controlsPage==1))controlsPage=1;
            Txt(new Rect(692,184,270,32),"TECLAS EM DESTAQUE",tiny,muted);
            DrawControlsKeyboard();DrawControlsMouse();
            if(controlsPage==0){
                ControlsLegend(112,587,"W A S D","Andar");
                ControlsLegend(520,587,"Shift","Correr");
                ControlsLegend(928,587,"E","Interagir");
                ControlsLegend(112,664,"F","Personalizar a loja","Com a loja fechada");
                ControlsLegend(520,664,"Tab","Liberar o cursor");
                ControlsLegend(928,664,Application.platform==RuntimePlatform.WebGLPlayer?"P":"Esc","Pausar");
            }else{
                ControlsLegend(112,587,"Clique","Escolher peça","Ou selecionar um botão");
                ControlsLegend(520,587,"Arrastar","Girar personagem","Segure o botão esquerdo");
                ControlsLegend(928,587,"Roda","Aproximar / afastar");
                ControlsLegend(112,664,Application.platform==RuntimePlatform.WebGLPlayer?"P":"Esc",Application.platform==RuntimePlatform.WebGLPlayer?"Pausar":"Pausar / voltar");
                Txt(new Rect(520,668,710,50),"Arraste e role na área do personagem.\nO visual só é enviado quando você finalizar.",caption,muted);
            }
            Txt(new Rect(112,771,740,32),Application.platform==RuntimePlatform.WebGLPlayer?"← →  Trocar página     /     Clique em Voltar para sair":"← →  Trocar página     /     Esc  Voltar",caption,muted);
            if(Tap(new Rect(1064,756,264,52),"Voltar",true))controlsOpen=false;
        }
    }
}
