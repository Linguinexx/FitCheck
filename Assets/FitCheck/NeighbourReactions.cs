using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        int reactedEmoteSerial;Resident reactingResident;float reactionAge;
        public bool NeighbourReacting=>reactingResident!=null&&reactingResident.reaction>0;
        public string NeighbourReactionText {get;private set;}
        public Vector3 NeighbourReactionPoint=>reactingResident==null?Vector3.zero:reactingResident.root.position+Vector3.up*3.1f;
        public float NeighbourReactionAge=>reactionAge;
        void TickNeighbourReactions(float delta){
            reactionAge+=delta;
            foreach(var actor in residents){actor.reaction=Mathf.Max(0,actor.reaction-delta);actor.reactionCooldown=Mathf.Max(0,actor.reactionCooldown-delta);}
            if(IsInterior||reactedEmoteSerial==PlayerEmoteSerial)return;
            reactedEmoteSerial=PlayerEmoteSerial;if(!PlayerEmoteActive)return;
            Resident nearest=null;float best=7;
            var forward=Quaternion.Euler(0,ViewYaw,0)*Vector3.forward;
            foreach(var actor in residents){
                if(!actor.root.gameObject.activeSelf||actor.reactionCooldown>0||actor.oneShot)continue;
                var p=actor.root.localPosition;var offset=new Vector2(p.x,p.z)-Position;float distance=offset.magnitude;
                if(distance<1||distance>=best||Vector2.Dot(offset.normalized,new Vector2(forward.x,forward.z))<.35f||!NeighbourVisible(Position,new Vector2(p.x,p.z)))continue;
                nearest=actor;best=distance;
            }
            if(nearest==null)return;
            reactingResident=nearest;reactionAge=0;nearest.reaction=2.8f;nearest.reactionCooldown=8;nearest.reactionKind=PlayerEmoteId;
            nearest.wait=Mathf.Max(nearest.wait,2.8f);nearest.speed=0;
            NeighbourReactionText=PlayerEmoteId==1?"É isso! Tamo junto.":PlayerEmoteId==2?"Aí sim, merece aplausos!":PlayerEmoteId==3?"Opa! Salve, vizinhança!":PlayerEmoteId==4?"SEIS… SETE!":PlayerEmoteId==6?"Cinema demais esse bairro.":"Eita! Que intimidade é essa?";
        }
        bool NeighbourVisible(Vector2 start,Vector2 end){int count=Mathf.CeilToInt(Vector2.Distance(start,end)/.15f);for(int i=0;i<=count;i++)if(!WorldFree(Vector2.Lerp(start,end,i/(float)Mathf.Max(1,count))))return false;return true;}
        void ApplyNeighbourReactionArm(Resident actor,float side,ref Quaternion upper,ref Quaternion lower){
            if(actor.reaction<=0||actor.prop!=null||side<0)return;
            float age=2.8f-actor.reaction,weight=Mathf.Clamp01(Mathf.Min(age/.25f,actor.reaction/.35f));
            var shoulder=new Vector3(side*.39f,1.84f,.015f);var elbowRest=new Vector3(side*.52f,1.50f,-.025f);var wristRest=new Vector3(side*.485f,1.17f,-.095f);
            var wrist=new Vector3(.48f+Mathf.Sin(age*8)*.07f,2.23f,-.37f);
            if(actor.reactionKind==4)wrist.y=2.02f+Mathf.Sin(age*9)*.12f;
            float a=(elbowRest-shoulder).magnitude,b=(wristRest-elbowRest).magnitude;
            var axis=wrist-shoulder;float d=Mathf.Clamp(axis.magnitude,.05f,a+b-.01f);axis.Normalize();wrist=shoulder+axis*d;
            float along=(a*a-b*b+d*d)/(2*d);var bend=Vector3.ProjectOnPlane(new Vector3(.65f,-1,.15f),axis).normalized;
            var elbow=shoulder+axis*along+bend*Mathf.Sqrt(Mathf.Max(0,a*a-along*along));
            upper=Quaternion.Slerp(upper,Quaternion.FromToRotation(elbowRest-shoulder,elbow-shoulder),weight);
            lower=Quaternion.Slerp(lower,Quaternion.FromToRotation(wristRest-elbowRest,wrist-elbow),weight);
        }
    }
}
