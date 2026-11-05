using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class CityView
    {
        [System.Serializable] sealed class ResidentPart
        {
            [SerializeField] Vector3[] vertexPose;
            public bool rigidHead,rigidVerticesDirty;public Matrix4x4 rigidHeadMatrix;public Vector3[] headLocalVertices,headLocalNormals;public float lastLidCover=-1;
            public Vector3[] vertices {
                get{
                    if(vertexPose==null||vertexPose.Length!=rest.Length)vertexPose=new Vector3[rest.Length];
                    if(rigidHead&&rigidVerticesDirty){var source=headLocalVertices??rest;for(int i=0;i<source.Length;i++)vertexPose[i]=rigidHeadMatrix.MultiplyPoint3x4(source[i]);rigidVerticesDirty=false;}
                    return vertexPose;
                }
                set{vertexPose=value;}
            }
            [System.NonSerialized] public ResidentSkinCache skinCache;public Mesh mesh;public Vector3[] rest,normals,posedNormals,closedLid,closedNormals;public Matrix4x4 lidToAvatar;public Quaternion lidRotation;public float lidBaseCover;public bool arm,foot,leg,head,pelvis,mouth,blenderDenim;public float side;public Vector3 center;public GameObject node;public int mouthKind;public bool faceVisible=true;
        }
        [System.Serializable] sealed class Resident
        {
            public Transform root,shadow;public Vector3 home;public Quaternion facing;
            public List<ResidentPart> parts=new List<ResidentPart>();
            public Vector2[] path;public int target;public float cycle,blinkClock,wait,phase,speed,blend,poseDelay;public bool walking;
            public bool wallLean,oneShot;public Vector3 leanAnkle;public float leanBlend,leanTarget;public int idleStyle;public Transform prop;public AvatarExpression expression;
        }
        [SerializeField] List<Resident> residents=new List<Resident>();
        public const int StreetResidentCount=7;
        public bool CustomerSpeaking;
        const int ResidentArtRevision=33;
        [SerializeField] int motionRigVersion;
        public bool RuntimeReady {get{
            if(IsInterior&&(cat==null||!cat.Ready))return false;
            if(motionRigVersion!=ResidentArtRevision||residents.Count!=(IsHome?0:IsInterior?2:StreetResidentCount))return false;
            if(IsInterior&&!IsHome&&(fittingCurtains==null||fittingCurtains.Length!=2||fittingCurtains[0]==null||fittingCurtains[1]==null))return false;
            foreach(var actor in residents)if(actor.root==null||actor.parts.Count==0)return false;
            return true;
        }}
        Resident RegisterResident(Transform root)
        {
            var actor=new Resident{root=root,home=root.localPosition,facing=root.localRotation};residents.Add(actor);return actor;
        }
        void RegisterResidentMesh(Resident actor,Mesh mesh,string name,bool head,bool blenderDenim=false)
        {
            bool arm=name.Contains("Manga")||name.Contains("Braço")||name=="Mão"||name=="Polegar"||name=="Dedos relaxados";
            bool leg=mesh.bounds.max.y<1.45f&&name!="Cintura";
            actor.parts.Add(new ResidentPart{mesh=mesh,rest=mesh.vertices,normals=mesh.normals,vertices=new Vector3[mesh.vertexCount],posedNormals=new Vector3[mesh.vertexCount],arm=arm,leg=leg,head=head,mouth=name.StartsWith("Boca"),center=mesh.bounds.center,pelvis=name=="Cintura",foot=name.StartsWith("Tênis")||name.StartsWith("Sola")||name=="Cadarço",side=mesh.bounds.center.x<0?-1:1});
            actor.parts[actor.parts.Count-1].blenderDenim=blenderDenim;mesh.MarkDynamic();
        }
        bool ResidentBlocks(Vector2 point)
        {
            foreach(var actor in residents){if(!actor.root.gameObject.activeSelf)continue;var p=actor.root.localPosition;if(Mathf.Abs(point.x-p.x)<.78f&&Mathf.Abs(point.y-p.z)<.73f)return true;}return false;
        }
        void TickResidents(float delta)
        {
            for(int index=0;index<residents.Count;index++){
                var actor=residents[index];if(!actor.root.gameObject.activeSelf)continue;actor.cycle+=delta;actor.blinkClock+=delta;actor.walking=false;
                var pos=actor.root.localPosition;float travelled=0;
                // Unity can restore a null route as an empty array after a script reload.
                if(actor.path!=null&&actor.path.Length==0)actor.path=null;
                if(actor.path!=null){
                    actor.target=Mathf.Clamp(actor.target,0,actor.path.Length-1);
                    if(actor.wait>0){actor.wait-=delta;actor.speed=Mathf.MoveTowards(actor.speed,0,delta*3);}
                    else{
                        var current=new Vector2(pos.x,pos.z);var destination=actor.path[actor.target];
                        var direction=destination-current;float distance=direction.magnitude;
                        if(actor.oneShot&&actor.target<actor.path.Length-1&&distance<.20f){
                            destination=actor.path[++actor.target];direction=destination-current;distance=direction.magnitude;
                        }
                        var desired=distance>.001f?Quaternion.LookRotation(new Vector3(-direction.x,0,-direction.y)):actor.facing;
                        actor.facing=Quaternion.RotateTowards(actor.facing,desired,delta*170);
                        // Turn before travelling, then accelerate and ease into each stop.
                        float alignment=Quaternion.Angle(actor.facing,desired);
                        bool passingCorner=actor.oneShot&&actor.target<actor.path.Length-1;
                        float maxSpeed=actor.oneShot?1.8f:index==1?1.38f:1.59f;
                        float desiredSpeed=alignment>(actor.oneShot?55:32)?0:Mathf.Min(maxSpeed,passingCorner?maxSpeed:distance*3.3f);
                        actor.speed=Mathf.MoveTowards(actor.speed,desiredSpeed,delta*2.7f);
                        var next=Vector2.MoveTowards(current,destination,delta*actor.speed);
                        // The avoidance circle must cover the rectangular collision envelope (.78 x .73).
                        bool occupied=!actor.oneShot&&Vector2.Distance(next,Position)<1.16f;
                        foreach(var other in residents){if(other==actor||!other.root.gameObject.activeSelf)continue;var p=other.root.localPosition;if(Vector2.Distance(next,new Vector2(p.x,p.z))<1.0f)occupied=true;}
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
                actor.leanBlend=actor.wallLean?Mathf.MoveTowards(actor.leanBlend,actor.leanTarget,delta*3.2f):0;
                float dip=actor.blend*(.035f+.005f*Mathf.Cos(actor.phase*Mathf.PI*4));
                pos.y=actor.home.y-dip;
                actor.root.localPosition=pos;actor.root.localRotation=actor.facing;
                if(actor.shadow!=null)actor.shadow.localPosition=new Vector3(pos.x,.005f,pos.z);
                if(ShouldPoseResident(actor,delta))PoseResident(actor,index,dip);
            }
            TickWatcher(delta);
        }
        bool ShouldPoseResident(Resident actor,float delta)
        {
            actor.poseDelay+=delta;
            if(IsInterior){actor.poseDelay=0;return true;}
            var p=actor.root.localPosition;var offset=new Vector2(p.x-Position.x,p.z-Position.y);float distance=offset.magnitude;
            var forward=Quaternion.Euler(0,ViewYaw,0)*Vector3.forward;
            float facing=distance>.001f?Vector2.Dot(offset/distance,new Vector2(forward.x,forward.z)):1;
            // Keep close characters and a generous visible cone at full update rate.
            // Navigation, clocks and root motion always advance, including off screen.
            float interval=distance<16||distance<50&&facing>.15f?0:facing>.15f?.1f:.25f;
            if(actor.poseDelay+0.00001f<interval)return false;actor.poseDelay=0;return true;
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
            float b=actor.blend,wave=Mathf.Sin(actor.phase*Mathf.PI*2)*b,idle=1-b;
            var torso=Quaternion.Euler(-2.6f*b,-wave*4.5f+Mathf.Sin(actor.cycle*.7f+index)*.5f*idle,-wave*1.5f);
            var body=new BodyPose{
                pelvis=Quaternion.Euler(0,wave*3,wave*1.8f),torso=torso,
                head=torso*Quaternion.Euler(1.2f*b,wave*1.5f+Mathf.Sin(actor.cycle*.6f+index)*1.2f*idle,wave*.6f),
                pelvisShift=new Vector3(wave*.027f,0,-.011f*b),
                torsoShift=new Vector3(wave*.027f,Mathf.Sin(actor.cycle*1.3f)*.004f*idle,-.022f*b)
            };
            if(actor.idleStyle==2&&b<.1f){body.torso=Quaternion.Euler(-4,0,-2);body.head=Quaternion.Euler(-23,-12+Mathf.Sin(actor.cycle*.5f)*2,-3);}
            if(actor.wallLean){
                var lean=ShopLeanBody(actor);float f=Mathf.SmoothStep(0,1,actor.leanBlend);
                body.pelvis=Quaternion.Slerp(body.pelvis,lean.pelvis,f);body.torso=Quaternion.Slerp(body.torso,lean.torso,f);body.head=Quaternion.Slerp(body.head,lean.head,f);
                body.pelvisShift=Vector3.Lerp(body.pelvisShift,lean.pelvisShift,f);body.torsoShift=Vector3.Lerp(body.torsoShift,lean.torsoShift,f);
            }
            return body;
        }
        LegPose ResidentLeg(Resident actor,float side,float dip,BodyPose body)
        {
            float phase=Mathf.Repeat(actor.phase+(side<0?0:.5f),1),swing=Mathf.Clamp01((phase-.5f)*2);
            float z=phase<.5f?Mathf.Lerp(-.225f,.225f,phase*2):Mathf.Lerp(.225f,-.225f,Mathf.SmoothStep(0,1,swing));
            var leg=new LegPose{hipRest=new Vector3(side*.225f,1.175f,0),kneeRest=new Vector3(side*.225f,.675f,side<0?-.025f:.012f),ankleRest=new Vector3(side*.225f,.265f,side<0?-.040f:.020f)};
            leg.hip=body.PelvisPoint(leg.hipRest);
            leg.ankle=leg.ankleRest+new Vector3(0,(phase<.5f?0:Mathf.Sin(swing*Mathf.PI)*.13f)*actor.blend+dip,z*actor.blend);
            if(actor.wallLean)leg.ankle=Vector3.Lerp(leg.ankle,side>0?actor.leanAnkle:leg.ankleRest,Mathf.SmoothStep(0,1,actor.leanBlend));
            float upper=Vector3.Distance(leg.hipRest,leg.kneeRest),lower=Vector3.Distance(leg.kneeRest,leg.ankleRest);
            var line=leg.ankle-leg.hip;float distance=Mathf.Clamp(line.magnitude,.05f,upper+lower-.0001f);var direction=line.normalized;
            float along=(upper*upper-lower*lower+distance*distance)/(2*distance);
            var bend=Vector3.ProjectOnPlane(Vector3.back,direction).normalized;
            leg.knee=leg.hip+direction*along+bend*Mathf.Sqrt(Mathf.Max(0,upper*upper-along*along));
            leg.upper=Quaternion.FromToRotation(leg.kneeRest-leg.hipRest,leg.knee-leg.hip);
            leg.lower=Quaternion.FromToRotation(leg.ankleRest-leg.kneeRest,leg.ankle-leg.knee);
            leg.foot=Quaternion.Euler(phase<.5f?0:-Mathf.Sin(swing*Mathf.PI)*7*actor.blend,0,0);
            if(actor.wallLean)leg.foot=Quaternion.Slerp(leg.foot,side>0?ShopLeanFoot:Quaternion.identity,Mathf.SmoothStep(0,1,actor.leanBlend));return leg;
        }
        void PoseResident(Resident actor,int index,float dip)
        {
            var body=ResidentBody(actor,index);var left=ResidentLeg(actor,-1,dip,body);var right=ResidentLeg(actor,1,dip,body);
            denimLeft=left;denimRight=right;
            foreach(var part in actor.parts)PoseResidentPart(part,actor,index,body,part.side<0?left:right);
            AnimateResidentExpression(actor,index);
            if(actor.prop!=null)foreach(var part in actor.parts)if(part.mesh.name.StartsWith("Mão")&&part.side>0){actor.prop.localPosition=part.mesh.bounds.center+new Vector3(0,.075f,-.04f);actor.prop.localRotation=Quaternion.Euler(12,0,-8);break;}
        }
        void PoseResidentPart(ResidentPart part,Resident actor,int index,BodyPose body,LegPose leg)
        {
            if(part.head&&!part.mouth){PoseRigidHead(part,actor,index,body);return;}
            float side=part.side,wave=Mathf.Cos(actor.phase*Mathf.PI*2)*side;
            float idle=Mathf.Sin(actor.cycle*1.1f+index+side)*1.6f*(1-actor.blend);
            var shoulder=new Vector3(side*.39f,1.84f,.015f);var elbowRest=new Vector3(side*.52f,1.50f,side<0?.005f:-.025f);
            var upperArm=Quaternion.Euler(wave*14*actor.blend+idle,0,0);
            var elbow=shoulder+upperArm*(elbowRest-shoulder);
            var forearm=upperArm*Quaternion.Euler(2+(3+Mathf.Max(0,wave)*8)*actor.blend,0,-side*2*actor.blend);
            if(actor.idleStyle==2&&actor.blend<.1f&&side>0){upperArm=Quaternion.Euler(25,0,-side*12);elbow=shoulder+upperArm*(elbowRest-shoulder);forearm=upperArm*Quaternion.Euler(48,0,side*8);}
            if(!part.head&&!referenceSkin){PoseCachedSkin(part,actor,body,leg,shoulder,elbowRest,elbow,upperArm,forearm);return;}
            for(int v=0;v<part.rest.Length;v++){
                var p=part.rest[v];var n=part.normals[v];Vector3 posed,normal;
                if(part.head){
                    if(part.closedLid!=null)EvaluateResidentLid(part,v,ResidentBlink(actor.blinkClock,index),out p,out n);
                    if(part.mouth){float pulse=IsInterior&&index==0&&CustomerSpeaking?Mathf.Max(0,Mathf.Sin(actor.cycle*11)*Mathf.Sin(actor.cycle*3.8f)):Mathf.Pow(Mathf.Max(0,Mathf.Sin(actor.cycle*.9f+index)),8)*.12f;p.y=part.center.y+(p.y-part.center.y)*(1+pulse*.8f);}
                    posed=body.HeadPoint(p);normal=body.head*n;
                }
                else if(part.arm){
                    float upperWeight=MotionWeight(1.42f,1.60f,p.y);
                    posed=Vector3.Lerp(elbow+forearm*(p-elbowRest),shoulder+upperArm*(p-shoulder),upperWeight);
                    normal=Vector3.Lerp(forearm*n,upperArm*n,upperWeight);
                    float anchor=part.node.name.Contains("Manga")&&v>=part.rest.Length-21?0:1-MotionWeight(1.78f,1.94f,p.y);posed=Vector3.Lerp(p,posed,anchor);if(anchor==0)normal=n;
                    posed=body.TorsoPoint(posed);normal=body.torso*normal;
                }else if(part.blenderDenim){
                    Vector3 lp,ln,rp,rn;DenimLegPoint(p,n,denimLeft,body,out lp,out ln);DenimLegPoint(p,n,denimRight,body,out rp,out rn);
                    float mix=Mathf.SmoothStep(0,1,(p.x+.12f)/.24f),weight=actor.wallLean?1:actor.blend;
                    posed=Vector3.Lerp(p,Vector3.Lerp(lp,rp,mix),weight);normal=Vector3.Lerp(n,Vector3.Lerp(ln,rn,mix),weight);
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
                PoseResident(actor,residents.IndexOf(actor),actor.home.y-actor.root.localPosition.y);
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
