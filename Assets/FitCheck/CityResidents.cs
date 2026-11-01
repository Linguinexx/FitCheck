using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class CityView
    {
        [System.Serializable] sealed class ResidentPart
        {
            public Mesh mesh;public Vector3[] rest,normals,vertices,posedNormals;public bool arm,foot,leg,head,pelvis;public float side;
        }
        [System.Serializable] sealed class Resident
        {
            public Transform root,shadow;public Vector3 home;public Quaternion facing;
            public List<ResidentPart> parts=new List<ResidentPart>();
            public Vector2[] path;public int target;public float cycle,wait,phase,speed,blend;public bool walking;
            public bool wallLean,oneShot;public Vector3 leanAnkle;
        }
        [SerializeField] List<Resident> residents=new List<Resident>();
        const int ResidentArtRevision=5;
        [SerializeField] int motionRigVersion;
        public bool RuntimeReady {get{
            if(motionRigVersion!=ResidentArtRevision||residents.Count!=(IsInterior?1:4))return false;
            if(IsInterior&&(fittingCurtains==null||fittingCurtains.Length!=2||fittingCurtains[0]==null||fittingCurtains[1]==null))return false;
            foreach(var actor in residents)if(actor.root==null||actor.parts.Count==0)return false;
            return true;
        }}
        Resident RegisterResident(Transform root)
        {
            var actor=new Resident{root=root,home=root.localPosition,facing=root.localRotation};residents.Add(actor);return actor;
        }
        void RegisterResidentMesh(Resident actor,Mesh mesh,string name,bool head)
        {
            bool arm=name.Contains("Manga")||name.Contains("Braço")||name=="Mão"||name=="Polegar"||name=="Dedos relaxados";
            bool leg=mesh.bounds.max.y<1.45f&&name!="Cintura";
            actor.parts.Add(new ResidentPart{mesh=mesh,rest=mesh.vertices,normals=mesh.normals,vertices=new Vector3[mesh.vertexCount],posedNormals=new Vector3[mesh.vertexCount],arm=arm,leg=leg,head=head,pelvis=name=="Cintura",foot=name.StartsWith("Tênis")||name.StartsWith("Sola")||name=="Cadarço",side=mesh.bounds.center.x<0?-1:1});
            mesh.MarkDynamic();
        }
        bool ResidentBlocks(Vector2 point)
        {
            foreach(var actor in residents){var p=actor.root.localPosition;if(Mathf.Abs(point.x-p.x)<.78f&&Mathf.Abs(point.y-p.z)<.73f)return true;}return false;
        }
        void TickResidents(float delta)
        {
            for(int index=0;index<residents.Count;index++){
                var actor=residents[index];actor.cycle+=delta;actor.walking=false;
                var pos=actor.root.localPosition;float travelled=0;
                // Unity can restore a null route as an empty array after a script reload.
                if(actor.path!=null&&actor.path.Length==0)actor.path=null;
                if(actor.path!=null){
                    actor.target=Mathf.Clamp(actor.target,0,actor.path.Length-1);
                    if(actor.wait>0){actor.wait-=delta;actor.speed=Mathf.MoveTowards(actor.speed,0,delta*3);}
                    else{
                        var current=new Vector2(pos.x,pos.z);var destination=actor.path[actor.target];
                        var direction=destination-current;float distance=direction.magnitude;
                        var desired=distance>.001f?Quaternion.LookRotation(new Vector3(-direction.x,0,-direction.y)):actor.facing;
                        actor.facing=Quaternion.RotateTowards(actor.facing,desired,delta*170);
                        // Turn before travelling, then accelerate and ease into each stop.
                        float alignment=Quaternion.Angle(actor.facing,desired);
                        float desiredSpeed=alignment>32?0:Mathf.Min(index==1?1.38f:1.59f,distance*3.3f);
                        actor.speed=Mathf.MoveTowards(actor.speed,desiredSpeed,delta*2.7f);
                        var next=Vector2.MoveTowards(current,destination,delta*actor.speed);
                        bool occupied=!actor.oneShot&&Vector2.Distance(next,Position)<1.0f;
                        foreach(var other in residents){if(other==actor)continue;var p=other.root.localPosition;if(Vector2.Distance(next,new Vector2(p.x,p.z))<1.0f)occupied=true;}
                        if(!occupied){
                            travelled=Vector2.Distance(next,current);actor.walking=travelled>.00001f;pos.x=next.x;pos.z=next.y;
                            if(Vector2.Distance(next,destination)<.04f){
                                if(actor.oneShot&&actor.target==actor.path.Length-1){actor.path=null;actor.speed=0;}
                                else{actor.target=(actor.target+1)%actor.path.Length;actor.wait=actor.oneShot?0:actor.target%2==0?2.5f:1.1f;}
                            }
                        }else actor.speed=0;
                    }
                }
                actor.phase=Mathf.Repeat(actor.phase+travelled/.90f,1);
                actor.blend=Mathf.MoveTowards(actor.blend,actor.walking?1:0,delta*5);
                float dip=actor.blend*(.035f+.005f*Mathf.Cos(actor.phase*Mathf.PI*4));
                pos.y=actor.home.y-dip;
                actor.root.localPosition=pos;actor.root.localRotation=actor.facing;
                if(actor.shadow!=null)actor.shadow.localPosition=new Vector3(pos.x,.005f,pos.z);
                PoseResident(actor,index,dip);
            }
        }
        static float MotionWeight(float low,float high,float value)
        {return Mathf.SmoothStep(0,1,Mathf.InverseLerp(low,high,value));}
        struct BodyPose
        {
            public Quaternion pelvis,torso,head;public Vector3 pelvisShift,torsoShift;
            static readonly Vector3 Pivot=new Vector3(0,1.175f,0),Neck=new Vector3(0,2.16f,.015f);
            public Vector3 PelvisPoint(Vector3 p){return Pivot+pelvis*(p-Pivot)+pelvisShift;}
            public Vector3 TorsoPoint(Vector3 p){return Pivot+torso*(p-Pivot)+torsoShift;}
            public Vector3 Point(Vector3 p){return Vector3.Lerp(PelvisPoint(p),TorsoPoint(p),MotionWeight(1.10f,1.60f,p.y));}
            public Vector3 Normal(Vector3 n,float y){return Vector3.Lerp(pelvis*n,torso*n,MotionWeight(1.10f,1.60f,y));}
            public Vector3 HeadPoint(Vector3 p){return TorsoPoint(Neck)+head*(p-Neck);}
        }
        struct LegPose
        {
            public Vector3 hipRest,hip,kneeRest,ankleRest,knee,ankle;
            public Quaternion upper,lower,foot;
        }
        BodyPose ResidentBody(Resident actor,int index)
        {
            if(actor.wallLean)return ShopLeanBody(actor);
            float b=actor.blend,wave=Mathf.Sin(actor.phase*Mathf.PI*2)*b,idle=1-b;
            var torso=Quaternion.Euler(-2.6f*b,-wave*4.5f+Mathf.Sin(actor.cycle*.7f+index)*.5f*idle,-wave*1.5f);
            return new BodyPose{
                pelvis=Quaternion.Euler(0,wave*3,wave*1.8f),torso=torso,
                head=torso*Quaternion.Euler(1.2f*b,wave*1.5f+Mathf.Sin(actor.cycle*.6f+index)*1.2f*idle,wave*.6f),
                pelvisShift=new Vector3(wave*.027f,0,-.011f*b),
                torsoShift=new Vector3(wave*.027f,Mathf.Sin(actor.cycle*1.3f)*.004f*idle,-.022f*b)
            };
        }
        LegPose ResidentLeg(Resident actor,float side,float dip,BodyPose body)
        {
            float phase=Mathf.Repeat(actor.phase+(side<0?0:.5f),1),swing=Mathf.Clamp01((phase-.5f)*2);
            float z=phase<.5f?Mathf.Lerp(-.225f,.225f,phase*2):Mathf.Lerp(.225f,-.225f,Mathf.SmoothStep(0,1,swing));
            var leg=new LegPose{hipRest=new Vector3(side*.225f,1.175f,0),kneeRest=new Vector3(side*.225f,.675f,side<0?-.025f:.012f),ankleRest=new Vector3(side*.225f,.265f,side<0?-.040f:.020f)};
            leg.hip=body.PelvisPoint(leg.hipRest);
            leg.ankle=leg.ankleRest+new Vector3(0,(phase<.5f?0:Mathf.Sin(swing*Mathf.PI)*.13f)+dip,z);
            if(actor.wallLean)leg.ankle=side>0?actor.leanAnkle:leg.ankleRest;
            float upper=Vector3.Distance(leg.hipRest,leg.kneeRest),lower=Vector3.Distance(leg.kneeRest,leg.ankleRest);
            var line=leg.ankle-leg.hip;float distance=Mathf.Clamp(line.magnitude,.05f,upper+lower-.0001f);var direction=line.normalized;
            float along=(upper*upper-lower*lower+distance*distance)/(2*distance);
            var bend=Vector3.ProjectOnPlane(Vector3.back,direction).normalized;
            leg.knee=leg.hip+direction*along+bend*Mathf.Sqrt(Mathf.Max(0,upper*upper-along*along));
            leg.upper=Quaternion.FromToRotation(leg.kneeRest-leg.hipRest,leg.knee-leg.hip);
            leg.lower=Quaternion.FromToRotation(leg.ankleRest-leg.kneeRest,leg.ankle-leg.knee);
            leg.foot=actor.wallLean?(side>0?ShopLeanFoot:Quaternion.identity):Quaternion.Euler(phase<.5f?0:-Mathf.Sin(swing*Mathf.PI)*7,0,0);return leg;
        }
        void PoseResident(Resident actor,int index,float dip)
        {
            var body=ResidentBody(actor,index);var left=ResidentLeg(actor,-1,dip,body);var right=ResidentLeg(actor,1,dip,body);
            foreach(var part in actor.parts)PoseResidentPart(part,actor,index,body,part.side<0?left:right);
        }
        void PoseResidentPart(ResidentPart part,Resident actor,int index,BodyPose body,LegPose leg)
        {
            float side=part.side,wave=Mathf.Cos(actor.phase*Mathf.PI*2)*side;
            float idle=Mathf.Sin(actor.cycle*1.1f+index+side)*1.6f*(1-actor.blend);
            var shoulder=new Vector3(side*.39f,1.84f,.015f);var elbowRest=new Vector3(side*.52f,1.50f,side<0?.005f:-.025f);
            var upperArm=Quaternion.Euler(wave*14*actor.blend+idle,0,0);
            var elbow=shoulder+upperArm*(elbowRest-shoulder);
            var forearm=upperArm*Quaternion.Euler(2+(3+Mathf.Max(0,wave)*8)*actor.blend,0,-side*2*actor.blend);
            for(int v=0;v<part.rest.Length;v++){
                var p=part.rest[v];var n=part.normals[v];Vector3 posed,normal;
                if(part.head){posed=body.HeadPoint(p);normal=body.head*n;}
                else if(part.arm){
                    float upperWeight=MotionWeight(1.42f,1.60f,p.y);
                    posed=Vector3.Lerp(elbow+forearm*(p-elbowRest),shoulder+upperArm*(p-shoulder),upperWeight);
                    normal=Vector3.Lerp(forearm*n,upperArm*n,upperWeight);
                    float anchor=1-MotionWeight(1.78f,1.94f,p.y);posed=Vector3.Lerp(p,posed,anchor);
                    posed=body.TorsoPoint(posed);normal=body.torso*normal;
                }else if(part.leg){
                    if(part.foot){posed=leg.ankle+leg.foot*(p-leg.ankleRest);normal=leg.foot*n;}
                    else{
                        float upperWeight=MotionWeight(leg.kneeRest.y-.10f,leg.kneeRest.y+.10f,p.y);
                        posed=Vector3.Lerp(leg.knee+leg.lower*(p-leg.kneeRest),leg.hip+leg.upper*(p-leg.hipRest),upperWeight);
                        normal=Vector3.Lerp(leg.lower*n,leg.upper*n,upperWeight);
                        float cuff=1-MotionWeight(.39f,.62f,p.y);
                        posed=Vector3.Lerp(posed,leg.ankle+leg.foot*(p-leg.ankleRest),cuff);
                        normal=Vector3.Lerp(normal,leg.foot*n,cuff);
                        float waist=MotionWeight(1.08f,1.28f,p.y);posed=Vector3.Lerp(posed,body.Point(p),waist);normal=Vector3.Lerp(normal,body.Normal(n,p.y),waist);
                    }
                    float weight=actor.wallLean?1:actor.blend;
                    posed=Vector3.Lerp(p,posed,weight);normal=Vector3.Lerp(n,normal,weight);
                }else{posed=body.Point(p);normal=body.Normal(n,p.y);}
                part.vertices[v]=posed;part.posedNormals[v]=normal;
            }
            part.mesh.vertices=part.vertices;part.mesh.normals=part.posedNormals;part.mesh.RecalculateBounds();
        }
        public Vector3 ResidentPosition(int index){return residents[index].root.localPosition;}
        public void CheckResidents()
        {
            foreach(var actor in residents){
                var p=actor.root.localPosition;if(!WorldFree(new Vector2(p.x,p.z)))throw new System.Exception("Morador atravessou arquitetura");
                foreach(var part in actor.parts){
                    foreach(var v in part.vertices)if(float.IsNaN(v.x)||float.IsInfinity(v.y))throw new System.Exception("Animação inválida");
                    float phase=Mathf.Repeat(actor.phase+(part.side<0?0:.5f),1);
                    if(part.foot&&actor.blend>.99f&&phase<.48f){
                        float original=float.MaxValue,current=float.MaxValue;
                        for(int i=0;i<part.rest.Length;i++){original=Mathf.Min(original,part.rest[i].y);current=Mathf.Min(current,part.vertices[i].y);}
                        if(Mathf.Abs(current+actor.root.localPosition.y-original-actor.home.y)>.025f)throw new System.Exception("Pé de apoio deslizou na vertical");
                    }
                }
            }
        }
    }
}
