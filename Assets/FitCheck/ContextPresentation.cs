using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        struct WorldHint {public string key,label;public Bounds bounds;public bool valid;}
        float contextClock,regionUntil,speechAge,answerAge;string contextRegion="",speechRequestKey="",speechAnswerKey="";Texture2D bubbleTail;
        void TickContext(float delta)
        {
            if(!paused){contextClock+=delta;speechAge+=delta;answerAge+=delta;}
            if(!paused&&shopDressingInStore&&(stage==Stage.Dressing||stage==Stage.Confirm)&&city!=null)city.AdvanceDressingBlinks(delta);
            if(stage==Stage.City&&city!=null){string name=city.IsHome?"EM CASA":city.IsInterior?"DRIPLAB":city.Place;if(name!=contextRegion){contextRegion=name;regionUntil=contextClock+2.8f;}}
            string request=stage==Stage.Dialogue?customer.ToString():"";if(request!=speechRequestKey){speechRequestKey=request;speechAge=0;}
            string answer=request+"/"+question;if(answer!=speechAnswerKey){speechAnswerKey=answer;answerAge=0;}
        }
        WorldHint Hint(string key,string label,Vector3 p,Vector3 size)
        {return new WorldHint{key=key,label=label,bounds=new Bounds(p,size),valid=true};}
        WorldHint CurrentWorldHint()
        {
            if(city==null||stage!=Stage.City||paused||ShopSequenceActive||showShopDecor||homeDoorExitRemaining>0)return default;
            Vector3 origin=city.transform.position;
            if(city.CatInteraction(cam))return Hint("E","Fazer carinho em Fubá",city.Cat.transform.position+Vector3.up*.35f,new Vector3(1.5f,.8f,1));
            if(city.IsHome){switch(city.HomeInteraction){
                case 1:return Hint("E","Sair de casa",origin+new Vector3(0,2,-4.4f),new Vector3(2.2f,4,.2f));
                case 2:return Hint("E","Trocar de roupa",origin+new Vector3(4.4f,1.8f,2.9f),new Vector3(1.8f,3.6f,1.4f));
                case 3:return Hint("E","Descansar / organizar o dia",origin+new Vector3(-3.4f,.7f,2.3f),new Vector3(2.3f,1.4f,3.5f));
                case 4:return Hint("E","Abrir FitFeed",origin+new Vector3(-3.3f,1.5f,-2.6f),new Vector3(.45f,.2f,.75f));
            }}else if(!city.IsInterior){
                if(city.StreetInteraction==1)return Hint("E","Entrar na DripLab",origin+new Vector3(10.4f,2,3.3f),new Vector3(2.2f,4,.3f));
                if(city.StreetInteraction==2)return Hint("E","Entrar em casa",origin+new Vector3(CityView.StreetHomeDoor.x,2,8.65f),new Vector3(2.1f,4,.25f));
            }else{
                if(city.ShopInteraction==1){if(ShopOpen&&!city.CustomerWaitingAtCounter&&!progress.DayComplete)return default;return Hint("E",ShopActionLabel(),origin+new Vector3(4.4f,.85f,6.2f),new Vector3(3.0f,1.7f,1.5f));}
                if(!ShopOpen&&Vector2.Distance(city.Position,new Vector2(2.1f,7.55f))<2)return Hint("F","Personalizar a loja",origin+new Vector3(2.1f,1.58f,6.35f),new Vector3(1.7f,.15f,1.25f));
                if(city.ShopInteraction==2)return Hint("E",ShopActionLabel(),origin+new Vector3(-4.9f,1.85f,5.15f),new Vector3(2.2f,3.6f,.3f));
                if(city.ShopInteraction==3)return Hint("E","Sair da loja",origin+new Vector3(0,2.1f,-4.8f),new Vector3(2.8f,4.2f,.2f));
            }
            return default;
        }
        bool LogicalPoint(Vector3 world,out Vector2 point)
        {
            var p=cam.WorldToScreenPoint(world);var q=GUI.matrix.inverse.MultiplyPoint3x4(new Vector3(p.x,Screen.height-p.y,0));point=new Vector2(q.x,q.y);return p.z>.08f;
        }
        void DrawAffordance()
        {
            if(!cityFreeCursor){Fill(new Rect(718,448,4,4),new Color(.95f,.92f,.85f,.6f));}
            var hint=CurrentWorldHint();if(!hint.valid)return;
            Vector2 centre;if(!LogicalPoint(hint.bounds.center,out centre))return;
            Vector2 lo=new Vector2(99999,99999),hi=-lo;var b=hint.bounds;
            foreach(float x in new[]{b.min.x,b.max.x})foreach(float y in new[]{b.min.y,b.max.y})foreach(float z in new[]{b.min.z,b.max.z}){Vector2 p;if(LogicalPoint(new Vector3(x,y,z),out p)){lo=Vector2.Min(lo,p);hi=Vector2.Max(hi,p);}}
            float width=Mathf.Clamp(hi.x-lo.x,36,480),height=Mathf.Clamp(hi.y-lo.y,30,440);var r=new Rect(centre.x-width/2,centre.y-height/2,width,height);
            var accent=CreatorRed;accent.a=reducedMotion?.9f:.72f+.2f*Mathf.Sin(contextClock*3);
            foreach(float x in new[]{r.xMin,r.xMax-2})foreach(float y in new[]{r.yMin,r.yMax-2}){
                Fill(new Rect(x==r.xMin?x:x-13,y,15,2),accent);Fill(new Rect(x,y==r.yMin?y:y-13,2,15),accent);
            }
            float w=Mathf.Clamp(90+hint.label.Length*9,240,530);float left=Mathf.Clamp(centre.x-w/2,24,1416-w),top=Mathf.Clamp(r.yMin-60,112,800);
            RoundedPanel(new Rect(left+2,top+4,w,48),new Color(0,0,0,.35f));RoundedPanel(new Rect(left,top,w,48),new Color(.035f,.03f,.035f,.96f));RoundedPanel(new Rect(left,top,w,48),CreatorRed,7,1.2f);
            KaosButtonFace(new Rect(left+8,top+7,36,35),hint.key,true,true);Txt(new Rect(left+56,top+8,w-66,34),hint.label,action,paper);
        }
        void ComicBubble(Rect rect,string text,string speaker,float age,bool tailLeft=true)
        {
            float a=reducedMotion?1:Mathf.SmoothStep(0,1,age/.20f);
            if(a<.001f)return;
            float pop=reducedMotion?0:Mathf.Sin(Mathf.Clamp01(age/.42f)*Mathf.PI)*.055f;
            float scale=.82f+.18f*a+pop;
            var r=new Rect(rect.center.x-rect.width*scale/2,rect.center.y-rect.height*scale/2,rect.width*scale,rect.height*scale);
            var originalMatrix=GUI.matrix;if(!reducedMotion)GUIUtility.RotateAroundPivot(Mathf.Sin(Mathf.Clamp01(age/.38f)*Mathf.PI)*-1.4f,r.center);
            float radius=Mathf.Min(58,r.height*.3f);
            RoundedPanel(new Rect(r.x+5,r.y+7,r.width,r.height),new Color(0,0,0,.48f*a),radius);
            RoundedPanel(r,new Color(.99f,.975f,.93f,a),radius);RoundedPanel(r,new Color(.06f,.045f,.055f,a),radius,3);
            if(tailLeft){
            if(bubbleTail==null){bubbleTail=new Texture2D(32,32,TextureFormat.RGBA32,false){filterMode=FilterMode.Bilinear};var pixels=new Color[1024];for(int y=0;y<32;y++)for(int x=0;x<32;x++)pixels[y*32+x]=new Color(1,1,1,x>=Mathf.Abs(y-15.5f)*2?1:0);bubbleTail.SetPixels(pixels);bubbleTail.Apply();}
            var tail=new Rect(tailLeft?r.x-17:r.x+r.width*.28f,tailLeft?r.y+r.height*.58f:r.yMax-11,23,28);var matrix=GUI.matrix;var color=GUI.color;
            if(!tailLeft)GUIUtility.RotateAroundPivot(-90,tail.center);
            GUI.color=new Color(.06f,.045f,.055f,a);GUI.DrawTexture(new Rect(tail.x-3,tail.y-3,tail.width+6,tail.height+6),bubbleTail);
            GUI.color=new Color(.99f,.975f,.93f,a);GUI.DrawTexture(tail,bubbleTail);GUI.color=color;GUI.matrix=matrix;
            }
            for(int y=0;y<3;y++)for(int x=0;x<5-y;x++)RoundedPanel(new Rect(r.xMax-38-x*7,r.y+20+y*7,2.5f,2.5f),new Color(.16f,.10f,.13f,.14f*a),2);
            var old=GUI.color;GUI.color=new Color(1,1,1,a);
            Txt(new Rect(r.x+25,r.y+22,r.width-60,24),speaker,tiny,CreatorRed);
            Txt(new Rect(r.x+25,r.y+53,r.width-50,r.height-64),text,caption,new Color(.08f,.065f,.07f));GUI.color=old;GUI.matrix=originalMatrix;
        }
        bool StreetBubblesAllowed=>stage==Stage.City&&city!=null&&city.gameObject.activeInHierarchy&&!city.IsInterior;
        void DrawAmbientBubble()
        {
            if(!StreetBubblesAllowed||CurrentWorldHint().valid)return;
            int nearest=-1;float distance=8;
            for(int i=0;i<CityView.StreetResidentCount;i++){
                var p=city.ResidentPosition(i);float d=Vector2.Distance(city.Position,new Vector2(p.x,p.z));var view=cam.WorldToViewportPoint(city.transform.position+p+Vector3.up*3.2f);
                if(d<distance&&view.z>0&&view.x>.1f&&view.x<.85f&&view.y>.1f&&view.y<.85f){nearest=i;distance=d;}
            }
            if(nearest<0)return;float phase=Mathf.Repeat(contextClock+nearest*1.7f,13);if(phase>3.7f)return;
            string[] lines={"Só mais uma música e eu vou…","Esse drop vai render assunto.","O corre não tira folga.","Passinho liberado!","Fiado? Só amanhã, chefia.","Arte boa é a que puxa conversa.","A quadra já tá no meu nome."};
            string text=lines[nearest%lines.Length];
            Vector2 point;if(!LogicalPoint(city.transform.position+city.ResidentPosition(nearest)+Vector3.up*3.6f,out point))return;
            ComicBubble(new Rect(Mathf.Clamp(point.x-150,32,1100),Mathf.Clamp(point.y-132,125,700),300,124),text,"PELO BAIRRO",phase,false);
        }
    }
}
