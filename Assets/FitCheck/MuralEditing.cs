using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        float[] muralX,muralY,muralAngle;
        int[] muralLayer;
        readonly Vector2[] muralOutline=new Vector2[4];
        int muralSelected=-1,muralDrag=-1,muralControl;
        Vector2 muralDragOffset;bool muralSaveError;
        void BeginMuralDraft(){
            StreetMemory.EnsureMuralLayout();muralDraft=StreetMemory.placed;muralX=(float[])StreetMemory.muralX.Clone();muralY=(float[])StreetMemory.muralY.Clone();muralAngle=(float[])StreetMemory.muralAngle.Clone();
            muralLayer=(int[])StreetMemory.muralLayer.Clone();
            muralSelected=muralDrag=-1;muralSaveError=false;
        }
        void PreviewMural()=>city.ApplyStreetMemories(StreetMemory,muralDraft,muralX,muralY,muralAngle,muralLayer);
        void SelectMuralPoster(int id){int rank=muralLayer[id];for(int i=0;i<7;i++)if(muralLayer[i]>rank)muralLayer[i]--;muralLayer[id]=6;muralSelected=id;muralDraft|=1<<id;PreviewMural();}
        void MoveMuralPoster(int id,Vector2 point){var p=CityView.ClampMuralPoster(id,point,muralAngle[id]);muralX[id]=p.x;muralY[id]=p.y;PreviewMural();}
        void TurnMuralPoster(float amount){if(muralSelected<0)return;muralAngle[muralSelected]=Mathf.Clamp(muralAngle[muralSelected]+amount,-30,30);MoveMuralPoster(muralSelected,new Vector2(muralX[muralSelected],muralY[muralSelected]));}
        void ReleaseMuralDrag(){muralDrag=-1;if(muralControl!=0&&GUIUtility.hotControl==muralControl)GUIUtility.hotControl=0;muralControl=0;}
        void SaveMuralDraft(){
            int before=StreetMemory.placed;var x=StreetMemory.muralX;var y=StreetMemory.muralY;var angle=StreetMemory.muralAngle;var layer=StreetMemory.muralLayer;
            StreetMemory.placed=muralDraft;StreetMemory.muralX=(float[])muralX.Clone();StreetMemory.muralY=(float[])muralY.Clone();StreetMemory.muralAngle=(float[])muralAngle.Clone();
            StreetMemory.muralLayer=(int[])muralLayer.Clone();
            if(TrySave()){CloseStreetPanel();return;}
            StreetMemory.placed=before;StreetMemory.muralX=x;StreetMemory.muralY=y;StreetMemory.muralAngle=angle;StreetMemory.muralLayer=layer;muralSaveError=true;
        }
        bool MuralMousePoint(Vector2 logical,out Vector2 point){
            var pixel=GUI.matrix.MultiplyPoint3x4(logical);var ray=cam.ViewportPointToRay(new Vector3(pixel.x/Screen.width,1-pixel.y/Screen.height,0));return city.MuralPoint(ray,out point);
        }
        int PickMuralPoster(Vector2 point){
            int picked=-1,rank=-1;
            for(int i=6;i>=0;i--){if((muralDraft&(1<<i))==0)continue;var p=Quaternion.Euler(0,0,-muralAngle[i])*(Vector3)(point-new Vector2(muralX[i],muralY[i]));var half=CityView.MuralPosterSize(i)*.5f;
                if(Mathf.Abs(p.x)<=half.x&&Mathf.Abs(p.y)<=half.y&&muralLayer[i]>rank){picked=i;rank=muralLayer[i];}
            }
            return picked;
        }
        void MuralPointer(){
            int control=GUIUtility.GetControlID(917361,FocusType.Passive);var e=Event.current;
            if(!GUI.enabled||paused||(!reducedMotion&&contextClock-streetPanelAt<.55f))return;
            if(e.type==EventType.MouseDown&&e.button==0&&e.mousePosition.x<940&&MuralMousePoint(e.mousePosition,out var point)){
                int i=PickMuralPoster(point);if(i<0)return;
                SelectMuralPoster(i);muralDrag=i;muralDragOffset=point-new Vector2(muralX[i],muralY[i]);muralControl=control;GUIUtility.hotControl=control;e.Use();
            }else if(e.type==EventType.MouseDrag&&muralDrag>=0&&GUIUtility.hotControl==control){if(MuralMousePoint(e.mousePosition,out var p))MoveMuralPoster(muralDrag,p-muralDragOffset);e.Use();}
            else if(e.type==EventType.MouseUp&&e.button==0&&muralDrag>=0){ReleaseMuralDrag();e.Use();}
        }
        void MuralSelectionOutline(){
            if(muralSelected<0||(muralDraft&(1<<muralSelected))==0||Event.current.type!=EventType.Repaint)return;
            var half=CityView.MuralPosterSize(muralSelected)*.5f;var q=Quaternion.Euler(0,0,muralAngle[muralSelected]);var points=muralOutline;
            for(int i=0;i<4;i++){var p=new Vector3(i==0||i==3?-half.x:half.x,i<2?half.y:-half.y,0);p=q*p+new Vector3(muralX[muralSelected],muralY[muralSelected],-.059f-muralLayer[muralSelected]*.001f);if(!LogicalPoint(city.MuralWorldPoint(p),out points[i]))return;}
            for(int i=0;i<4;i++){var a=points[i];var b=points[(i+1)%4];var matrix=GUI.matrix;GUI.matrix=matrix*Matrix4x4.TRS(a,Quaternion.Euler(0,0,Mathf.Atan2(b.y-a.y,b.x-a.x)*Mathf.Rad2Deg),Vector3.one);Fill(new Rect(0,-1,Vector2.Distance(a,b),2),CityViewSelectionColor);GUI.matrix=matrix;}
        }
        static readonly Color CityViewSelectionColor=new Color(227/255f,6/255f,19/255f);
        void MuralEditorUI(){
            MuralPointer();MuralSelectionOutline();
            DrawKaosPanel(new Rect(962,123,440,676));Txt(new Rect(988,145,392,52),"Mural do bairro",heading,paper);Txt(new Rect(988,208,382,55),"Cole seus lambe-lambes. Arraste no mural para mudar de lugar.",caption,muted);
            for(int i=0;i<7;i++){bool owned=StreetMemory.HasSticker(i),placed=(muralDraft&(1<<i))!=0;
                if(Tap(new Rect(988,275+i*48,386,40),owned?(placed?"Colado / ":"+ ")+CityView.StickerNames[i]:"??? / encontre no bairro",muralSelected==i,owned))SelectMuralPoster(i);
            }
            Txt(new Rect(988,608,386,26),muralSelected<0?"Selecione um lambe-lambe para editar.":CityView.StickerNames[muralSelected],tiny,muted);
            if(Tap(new Rect(988,641,120,44),"Girar -",false,muralSelected>=0))TurnMuralPoster(-5);
            if(Tap(new Rect(1118,641,120,44),"Girar +",false,muralSelected>=0))TurnMuralPoster(5);
            if(Tap(new Rect(1248,641,126,44),"Retirar",false,muralSelected>=0)){muralDraft&=~(1<<muralSelected);muralSelected=-1;PreviewMural();}
            Txt(new Rect(988,693,386,28),muralSaveError?"Não salvou. Tente novamente.":"As mudanças só ficam ao salvar.",tiny,muralSaveError?CityViewSelectionColor:muted);
            if(Tap(new Rect(988,733,183,44),"Salvar",true))SaveMuralDraft();
            if(Tap(new Rect(1181,733,193,44),"Cancelar"))CloseStreetPanel();
        }
    }
}
