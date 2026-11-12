using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        struct WorldHint {public string key,label;public Bounds bounds;public bool valid;}
        float contextClock,regionUntil,speechAge,answerAge;string contextRegion="",speechRequestKey="",speechAnswerKey="";Texture2D bubbleTail;
        float greetedAt,garageRecordUntil;
        int lastGarageDeck;
        static readonly string[] GarageRecordTitles={"Batida da Laje","Frequência Noturna","Fita de Garagem"};
        bool TryUseGarageDeck()
        {
            if(!StreetBubblesAllowed||!city.LookingAtGarageDeck(cam,progress,out int deck,out _))return false;
            if(!progress.RegisterGarageRecord(deck))return false;
            lastGarageDeck=deck;
            garageRecordUntil=contextClock+1.8f;
            if(progress.UniformUnlocked(DayProgress.GarageDjUniform))notices.Enqueue(DayProgress.GarageDjAward);
            TrySave();
            return true;
        }
        int greetedIndex=-1;
        bool TryGreetPostedCustomer()
        {
            if(!StreetBubblesAllowed||!city.TryLookAtPostedCustomer(cam,progress,out int index,out _))return false;
            bool alreadyUnlocked=progress.UniformUnlocked(DayProgress.NeighbourhoodUniform);
            if(!progress.RegisterCustomerGreeting(index))return false;
            greetedIndex=index;greetedAt=contextClock;
            if(!alreadyUnlocked&&progress.UniformUnlocked(DayProgress.NeighbourhoodUniform))notices.Enqueue(DayProgress.NeighbourhoodAward);
            TrySave();
            return true;
        }
        void TickContext(float delta)
        {
            if(!paused){contextClock+=delta;speechAge+=delta;answerAge+=delta;affordanceTapAge=Mathf.Min(1,affordanceTapAge+delta);}
            if(!paused&&shopDressingInStore&&(stage==Stage.Dressing||stage==Stage.Confirm)&&city!=null)city.AdvanceDressingBlinks(delta);
            if(stage==Stage.City&&city!=null){string name=city.IsHome?"EM CASA":city.IsInterior?"DRIPLAB":city.Place;if(name!=contextRegion){contextRegion=name;regionUntil=contextClock+2.8f;}}
            string request=stage==Stage.Dialogue?customer.ToString():"";if(request!=speechRequestKey){speechRequestKey=request;speechAge=0;}
            string answer=request+"/"+question;if(answer!=speechAnswerKey){speechAnswerKey=answer;answerAge=0;}
        }
        WorldHint Hint(string key,string label,Vector3 p,Vector3 size)
        {return new WorldHint{key=key,label=label,bounds=new Bounds(p,size),valid=true};}
        WorldHint CurrentWorldHint()
        {
            if(city==null||stage!=Stage.City||paused||ShopSequenceActive||showShopDecor||shopGuideOpen||homeDoorExitRemaining>0)return default;
            Vector3 origin=city.transform.position;
            if(city.CatInteraction(cam))return Hint("E","Fazer carinho em "+city.Cat.DisplayName,city.Cat.transform.position+Vector3.up*.35f,new Vector3(1.5f,.8f,1));
            if(city.IsHome){if(city.LookingAtHomeDecor(cam))return Hint("F","Personalizar o quarto",origin+CityView.HomeDecorBook,new Vector3(.7f,.13f,.5f));switch(city.HomeInteraction){
                case 1:return Hint("E","Sair de casa",origin+new Vector3(0,1.85f,-4.43f),new Vector3(1.9f,3.7f,.14f));
                case 2:return Hint("E","Trocar de roupa",origin+CityView.HomeWardrobeCenter,new Vector3(1.65f,3.6f,1.45f));
                case 3:return Hint("E","Descansar / organizar o dia",origin+new Vector3(-3.4f,.84f,2.375f),new Vector3(2.3f,1.63f,3.51f));
                case 4:return Hint("E","Abrir FitFeed",origin+CityView.HomePhonePosition-Vector3.up*.044f,new Vector3(.63f,.094f,.35f));
            }}else if(!city.IsInterior){
                if(city.StreetInteraction==1)return Hint("E","Entrar na DripLab",origin+new Vector3(10.4f,2,3.3f),new Vector3(2.2f,4,.3f));
                if(city.StreetInteraction==2)return Hint("E","Entrar em casa",origin+new Vector3(CityView.StreetHomeDoor.x,2,8.65f),new Vector3(2.1f,4,.25f));
                if(city.TryLookAtPostedCustomer(cam,progress,out int index,out Vector3 point))
                    return Hint("E","Cumprimentar "+DayOne.Customers[index].name,point-Vector3.up*.65f,new Vector3(1.3f,3.25f,1.2f));
                if(city.LookingAtGarageDeck(cam,progress,out _,out Vector3 deck))
                    return Hint("E","Conferir o disco",origin+deck,new Vector3(1.1f,.65f,.68f));
            }else{
                if(!shopFittingOpening&&city.LookingAtShopGuide(cam))return Hint("E","Conversar com "+CityView.ShopGuideName,city.ShopGuideTarget-Vector3.up*.8f,new Vector3(1.2f,3.4f,1));
                if(city.ShopInteraction==1){if(ShopOpen&&!city.CustomerWaitingAtCounter&&!progress.DayComplete)return default;return Hint("E",ShopActionLabel(),origin+new Vector3(4.4f,.82f,6.15f),new Vector3(5.95f,1.64f,1.62f));}
                if(!ShopOpen&&Vector2.Distance(city.Position,new Vector2(2.1f,7.55f))<2)return Hint("F","Personalizar a loja",origin+new Vector3(2.1f,1.58f,6.35f),new Vector3(1.7f,.15f,1.25f));
                if(city.ShopInteraction==2)return Hint("E",ShopActionLabel(),origin+new Vector3(-4.9f,1.85f,5.15f),new Vector3(4.86f,3.5f,.2f));
                if(city.ShopInteraction==3)return Hint("E","Sair da loja",origin+new Vector3(0,2.1f,-4.8f),new Vector3(2.8f,4.2f,.2f));
            }
            return default;
        }
        bool LogicalPoint(Vector3 world,out Vector2 point)
        {
            // The retro render target is stretched over the display. Project in normalized
            // viewport coordinates so the UI stays aligned at either render resolution.
            var viewport=cam.WorldToViewportPoint(world);
            var screenPoint=new Vector3(viewport.x*Screen.width,(1-viewport.y)*Screen.height,0);
            point=GUI.matrix.inverse.MultiplyPoint3x4(screenPoint);
            return viewport.z>cam.nearClipPlane;
        }
        void DrawAffordance()
        {
            if(!cityFreeCursor){Fill(new Rect(718,448,4,4),new Color(.95f,.92f,.85f,.6f));}
            var hint=affordanceTapAge<AffordanceTapDuration?affordanceTapped:CurrentWorldHint();if(!hint.valid)return;
            Vector2 centre;if(!LogicalPoint(hint.bounds.center,out centre))return;
            var lo=new Vector2(float.PositiveInfinity,float.PositiveInfinity);
            var hi=new Vector2(float.NegativeInfinity,float.NegativeInfinity);
            var bounds=hint.bounds;
            bool allInFront=true;
            for(int i=0;i<8;i++){
                var corner=new Vector3((i&1)==0?bounds.min.x:bounds.max.x,
                    (i&2)==0?bounds.min.y:bounds.max.y,(i&4)==0?bounds.min.z:bounds.max.z);
                if(!LogicalPoint(corner,out var point)){allInFront=false;continue;}
                lo=Vector2.Min(lo,point);
                hi=Vector2.Max(hi,point);
            }
            if(hi.x<0||lo.x>1440||hi.y<0||lo.y>900)return;
            var r=Rect.MinMaxRect(lo.x,lo.y,hi.x,hi.y);
            var accent=CreatorRed;accent.a=reducedMotion?.9f:.72f+.2f*Mathf.Sin(contextClock*3);
            if(allInFront){
                float arm=Mathf.Min(15,r.width*.4f,r.height*.4f);
                for(int i=0;i<4;i++){
                    bool right=(i&1)!=0,bottom=(i&2)!=0;
                    float x=right?r.xMax:r.xMin,y=bottom?r.yMax:r.yMin;
                    if(x<0||x>1440||y<0||y>900)continue;
                    Fill(new Rect(right?x-arm:x,bottom?y-2:y,arm,2),accent);
                    Fill(new Rect(right?x-2:x,bottom?y-arm:y,2,arm),accent);
                }
            }
            float w=Mathf.Clamp(90+hint.label.Length*9,240,530);
            float left=Mathf.Clamp((allInFront?r.center.x:centre.x)-w/2,24,1416-w);
            float top=Mathf.Clamp((allInFront?r.yMin:centre.y)-60,24,800);
            DrawAffordancePanel(new Rect(left,top,w,48),hint);
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
            if(!StreetBubblesAllowed)return;
            if(contextClock<garageRecordUntil){
                int record=progress.GarageRecordsFound;
                ComicBubble(new Rect(487,697,466,122),GarageRecordTitles[lastGarageDeck],"VITROLAS  /  "+record+" DE "+DayProgress.GarageDjRecordGoal,contextClock-(garageRecordUntil-1.8f),false);
                return;
            }
            if(greetedIndex>=0&&contextClock-greetedAt<2.8f){
                ComicBubble(new Rect(487,697,466,122),"Valeu pelo look! Bom te ver por aqui.",DayOne.Customers[greetedIndex].name,contextClock-greetedAt,false);
                return;
            }
            if(CurrentWorldHint().valid)return;
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
