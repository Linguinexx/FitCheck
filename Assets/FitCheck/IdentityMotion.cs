using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        void DrawAnimatedTitleLogo(Rect rect)
        {
            float t=reducedMotion?0:titleSceneTime;float pulse=reducedMotion?1:1+.018f*Mathf.Sin(t*1.8f);
            var r=new Rect(rect.center.x-rect.width*pulse/2,rect.center.y-rect.height*pulse/2+(reducedMotion?0:Mathf.Sin(t*1.2f)*3),rect.width*pulse,rect.height*pulse);
            var old=GUI.color;GUI.color=new Color(0,0,0,.4f);GUI.DrawTexture(new Rect(r.x+3,r.y+5,r.width,r.height),titleLogo,ScaleMode.ScaleToFit);GUI.color=old;
            GUI.DrawTexture(r,titleLogo,ScaleMode.ScaleToFit);
        }
    }
    public partial class StudioView
    {
        public void PrepareResidentLids(){for(int i=0;i<2;i++){UpdateLid(i,false,Mathf.Max(.006f,upperCover[i]));pupils[i].gameObject.SetActive(true);}}
        public void RestoreResidentLids(){for(int i=0;i<2;i++)UpdateLid(i,false,upperCover[i]);}
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
        public static float ResidentBlink(float time,int index){float phase=Mathf.Repeat(time+index*.617f,3.8f+(index%3)*.37f);return phase<.18f?Mathf.Sin(phase/.18f*Mathf.PI):0;}
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
                studio.Apply(new[]{"CG_TOP_01","SB_BOTTOM_01","SB_SHOES_01",""},-1);CivilianClothes(studio,7);
                watcher=CopyAvatar(studio,"Fiscal do drip — observador da janela",new Vector3(-100.3f,4.05f,35.75f),0,false);
                foreach(var part in watcher.parts)if(!part.head)part.node.SetActive(false);
            }finally{studio.PlayerLook=look;studio.HairOverride=hair;studio.ExpressionOverride=expression;}
        }
        void TickWatcher(float delta)
        {
            if(playerBody!=null)playerBody.blinkClock+=delta;
            if(watcher==null||watcher.root==null)return;watcher.cycle+=delta;watcher.blinkClock+=delta;
            var offset=new Vector3(Position.x,0,Position.y)-new Vector3(watcher.home.x,0,watcher.home.z);
            if(offset.magnitude<18)watcher.facing=Quaternion.RotateTowards(watcher.facing,Quaternion.LookRotation(-offset),delta*35);
            watcher.root.localRotation=watcher.facing;PoseResident(watcher,8,0);
        }
    }
}
