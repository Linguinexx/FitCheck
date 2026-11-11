using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        Texture2D titleIntroAtlas;
        const float TitleIntroDuration=1.6f;
        const float TitleIntroHold=.35f,TitleIntroFade=.3f;
        const float TitleIntroTotal=TitleIntroDuration+TitleIntroHold+TitleIntroFade;

        void DrawTitleIdentity(Rect area,float progress=1,bool idleMotion=true)
        {
            if(titleIntroAtlas==null)titleIntroAtlas=Resources.Load<Texture2D>("Brand/FitCheck-Intro-Layers");
            if(titleIntroAtlas==null){DrawAnimatedTitleLogo(area);return;}
            var frame=new Rect(area.x,area.center.y-area.width*405/1024f,area.width,area.width*405/512f);
            float time=Mathf.Clamp01(progress);
            if(idleMotion&&!reducedMotion){
                float idle=titleSceneTime;
                float pulse=1+.018f*Mathf.Sin(idle*1.8f);
                frame=new Rect(frame.center.x-frame.width*pulse/2,frame.center.y-frame.height*pulse/2+
                    Mathf.Sin(idle*1.2f)*3,frame.width*pulse,frame.height*pulse);
            }
            DrawIntroPart(frame,0,new Vector2(.5f,.5f),time,0,.4f,.67f,.72f,1.012f,-7,1,0,0);
            DrawIntroPart(frame,1,new Vector2(.497f,.346f),time,.18f,.49f,.72f,.84f,1.02f,-5,0,28,-5);
            DrawIntroPart(frame,2,new Vector2(.760f,.406f),time,.46f,.72f,1,.38f,1.16f,-38,9,0,0);
            DrawIntroPart(frame,3,new Vector2(.5f,.701f),time,.27f,.60f,.80f,.82f,1.021f,0,0,42,-7);
        }

        void DrawTitleIntro()
        {
            if(Event.current.type!=EventType.Repaint)return;
            titleIntroPresented=true;
            float opacity=1-Mathf.InverseLerp(TitleIntroDuration+TitleIntroHold,TitleIntroTotal,titleIntroClock);
            var matrix=GUI.matrix;
            GUI.matrix=Matrix4x4.identity;
            Fill(new Rect(0,0,Screen.width,Screen.height),new Color(0,0,0,opacity));
            GUI.matrix=matrix;
            var color=GUI.color;
            GUI.color=new Color(color.r,color.g,color.b,color.a*opacity);
            DrawTitleIdentity(new Rect(480,245,480,410),titleIntroClock/TitleIntroDuration,false);
            GUI.color=color;
        }

        void DrawIntroPart(Rect frame,int tile,Vector2 anchor,float time,float start,float peak,float end,
            float startScale,float peakScale,float startAngle,float peakAngle,float startY,float peakY)
        {
            if(time<=start)return;
            bool entering=time<peak;
            float t=Mathf.InverseLerp(entering?start:peak,entering?peak:end,time);
            float eased=1-Mathf.Pow(1-t,entering?3:2);
            float scale=entering?Mathf.Lerp(startScale,peakScale,eased):Mathf.Lerp(peakScale,1,eased);
            float angle=entering?Mathf.Lerp(startAngle,peakAngle,eased):Mathf.Lerp(peakAngle,0,eased);
            float offset=frame.height/1085f*(entering?Mathf.Lerp(startY,peakY,eased):Mathf.Lerp(peakY,0,eased));
            var pivot=frame.position+Vector2.Scale(frame.size,anchor)+Vector2.up*offset;
            var rect=new Rect(pivot-Vector2.Scale(frame.size*scale,anchor),frame.size*scale);
            var uv=new Rect((tile%2)*.5f,1-(tile/2)*.5f-405/1024f,.5f,405/1024f);
            var matrix=GUI.matrix;
            var color=GUI.color;
            GUI.color=new Color(color.r,color.g,color.b,color.a*(entering?eased:1));
            if(angle!=0)GUIUtility.RotateAroundPivot(angle,pivot);
            GUI.DrawTextureWithTexCoords(rect,titleIntroAtlas,uv,true);
            GUI.matrix=matrix;
            GUI.color=color;
        }

        void DrawAnimatedTitleLogo(Rect rect,float? clock=null)
        {
            float t=reducedMotion?0:(clock??titleSceneTime);float pulse=reducedMotion?1:1+.018f*Mathf.Sin(t*1.8f);
            var r=new Rect(rect.center.x-rect.width*pulse/2,rect.center.y-rect.height*pulse/2+(reducedMotion?0:Mathf.Sin(t*1.2f)*3),rect.width*pulse,rect.height*pulse);
            var old=GUI.color;GUI.color=new Color(0,0,0,.4f*old.a);GUI.DrawTexture(new Rect(r.x+3,r.y+5,r.width,r.height),titleLogo,ScaleMode.ScaleToFit);GUI.color=old;
            GUI.DrawTexture(r,titleLogo,ScaleMode.ScaleToFit);
        }
    }
    public partial class StudioView
    {
        public void PrepareResidentLids(){for(int i=0;i<2;i++){UpdateLid(i,false,Mathf.Max(.006f,upperCover[i]));pupils[i].gameObject.SetActive(true);}}
        public void RestoreResidentLids(){for(int i=0;i<2;i++)UpdateLid(i,false,upperCover[i]);}
        public float ResidentLidCover(Transform part){return upperCover[part==upperLidObjects[0].transform?0:1];}
        public void ResidentClosedLid(Transform part,out Vector3[] vertices,out Vector3[] normals)
        {
            int eye=part==upperLidObjects[0].transform?0:1;UpdateLid(eye,false,.999f);vertices=upperLids[eye].vertices;normals=upperLids[eye].normals;UpdateLid(eye,false,Mathf.Max(.006f,upperCover[eye]));
        }
        public void ValidateBlinkClock()
        {
            float previous=blinkClock,previousIdle=idle;bool reduced=ReducedMotion;
            try{ReducedMotion=true;Animate(.1f,false);if(blinkClock!=previous)throw new System.Exception("Pausa moveu piscada");Animate(.1f,true);if(blinkClock<=previous||idle!=previousIdle)throw new System.Exception("Piscada depende do balanço do corpo");}
            finally{blinkClock=previous;idle=previousIdle;ReducedMotion=reduced;AnimateFace();}
        }
    }
    public partial class CityView
    {
        [SerializeField] Resident watcher;
        public static float ResidentBlink(float time,int index){float phase=Mathf.Repeat(time+index*.617f,4.3f+(index%3)*.17f);return phase<.16f?Mathf.Sin(phase/.16f*Mathf.PI):0;}
        // Match the original player's spherical cap at every intermediate frame.
        void EvaluateResidentLid(ResidentPart part,int vertex,float blink,out Vector3 position,out Vector3 normal)
        {
            const int columns=36,rows=10,layerSize=1+columns*rows;
            int layer=vertex/layerSize,local=vertex%layerSize;
            var radius=layer==0?new Vector3(.174f,.184f,.109f):new Vector3(.168f,.178f,.103f);
            if(blink<=Mathf.Max(.006f,part.lidBaseCover)){position=part.rest[vertex];normal=part.normals[vertex];return;}
            float cover=Mathf.Max(.006f,Mathf.Max(part.lidBaseCover,blink)),limit=Mathf.Acos(1-Mathf.Min(cover,.9995f)*2);
            Vector3 direction=Vector3.up;
            if(local>0){int row=(local-1)/columns+1,column=(local-1)%columns;float phi=row/(float)rows*limit,theta=column/(float)columns*Mathf.PI*2;direction=new Vector3(Mathf.Sin(theta)*Mathf.Sin(phi),Mathf.Cos(phi),-Mathf.Cos(theta)*Mathf.Sin(phi));}
            position=part.lidToAvatar.MultiplyPoint3x4(Vector3.Scale(direction,radius));
            normal=part.lidRotation*(layer==0?direction:-direction);if(local>0)normal=part.lidRotation*((layer==0?1:-1)*new Vector3(direction.x/radius.x,direction.y/radius.y,direction.z/radius.z).normalized);
        }
        public void AdvanceDressingBlinks(float delta)
        {
            for(int i=0;i<residents.Count;i++)AdvanceStillBlink(residents[i],i,delta);
            if(playerBody!=null)AdvanceStillBlink(playerBody,4,delta);
        }
        void AdvanceStillBlink(Resident actor,int index,float delta)
        {
            actor.blinkClock+=delta;var body=ResidentBody(actor,index);var leg=ResidentLeg(actor,1,0,body);
            foreach(var part in actor.parts)if(part.closedLid!=null)PoseResidentPart(part,actor,index,body,leg);
            UpdateBlinkVisibility(actor,index);
        }
        void BuildWatcher(StudioView studio)
        {
            var look=studio.PlayerLook;int hair=studio.HairOverride,expression=studio.ExpressionOverride;
            try{
                studio.PlayerLook=CitizenAppearance(7);studio.HairOverride=0;studio.ExpressionOverride=(int)AvatarExpression.Skeptical;
                studio.ApplyCivilian(7);
                watcher=CopyAvatar(studio,"Fiscal do drip — observador da janela",new Vector3(-100.3f,5.55f,35.25f),0,false);
                watcher.root.localScale=Vector3.one*.72f;
            }finally{studio.PlayerLook=look;studio.HairOverride=hair;studio.ExpressionOverride=expression;}
        }
        void TickWatcher(float delta)
        {
            if(playerBody!=null)playerBody.blinkClock+=delta;
            if(watcher==null||watcher.root==null)return;watcher.cycle+=delta;watcher.blinkClock+=delta;
            var offset=new Vector3(Position.x,0,Position.y)-new Vector3(watcher.home.x,0,watcher.home.z);
            if(offset.magnitude<18)watcher.facing=Quaternion.RotateTowards(watcher.facing,Quaternion.LookRotation(-offset),delta*35);
            watcher.root.localRotation=watcher.facing;if(ShouldPoseResident(watcher,delta))PoseResident(watcher,8,0);
        }
    }
}
