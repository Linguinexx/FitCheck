using System;
using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        public static readonly Vector3 CollectiveMural=new Vector3(-103,1.23f,17.99f),CourtBallSpot=new Vector3(-76,.30f,40),CourtHoop=new Vector3(-76,5.3f,47.35f);
        public static readonly string[] StickerNames={"Estrela Kaos","Disco de rua","Raio","Lua da laje","Patinha","Olhar da rua","Só cesta"};
        static readonly Vector2[] StickerAnchors={new Vector2(-126,-1.5f),new Vector2(-112,-2),new Vector2(-96.5f,28.5f),new Vector2(-76,38),new Vector2(-49,-18),new Vector2(-31.5f,-2.5f),new Vector2(-16,1)};
        Transform[] streetStickers,muralStickers;Transform courtBall;Vector3 shotStart,shotEnd;
        public int ChallengeResident {get;private set;}=-1;
        int challengeGesture,streetSpeaker=-1;float challengeAge;float ballFlight=-1;
        public bool BallHeld {get;private set;}
        public bool BallFlying=>ballFlight>=0;
        public bool InCourt=>!IsInterior&&Position.x> -86&&Position.x< -66&&Position.y>34&&Position.y<49;
        public Vector3 StickerPoint(int id)=>streetStickers[id].localPosition+Vector3.up*1.55f;
        public Vector3 BallPoint=>courtBall==null?transform.TransformPoint(CourtBallSpot):courtBall.position;
        Transform ActivityGeometry(string name,Action build){
            if(batches.Count>0)FinishGeometry();
            var root=new GameObject(name).transform;root.SetParent(transform,false);int first=transform.childCount;
            build();FinishGeometry();while(transform.childCount>first)transform.GetChild(first).SetParent(root,false);return root;
        }
        void ActivityDisc(Vector3 p,Vector2 radius,Color color){for(int i=0;i<24;i++){float a=i*Mathf.PI/12,b=(i+1)*Mathf.PI/12;Triangle(p,p+new Vector3(Mathf.Cos(b)*radius.x,Mathf.Sin(b)*radius.y,0),p+new Vector3(Mathf.Cos(a)*radius.x,Mathf.Sin(a)*radius.y,0),color,"Paper");}}
        void StickerIcon(Vector3 p,int id,float size){
            var color=new[]{Kaos,new Color(.57f,.22f,.88f),new Color(1,.62f,.06f),new Color(.12f,.68f,.79f),new Color(.19f,.70f,.36f),Ink,new Color(.90f,.37f,.11f)}[id];
            LambePaper(p,id,size);p+=Vector3.back*.016f+Vector3.up*size*.12f;size*=.80f;
            if(id==0){var points=new Vector3[10];for(int i=0;i<10;i++){float a=Mathf.PI*.5f+i*Mathf.PI/5,r=i%2==0?size*.48f:size*.21f;points[i]=p+new Vector3(Mathf.Cos(a)*r,Mathf.Sin(a)*r,0);}for(int i=0;i<10;i++)Triangle(p,points[(i+1)%10],points[i],color,"Paper");}
            if(id==1){ActivityDisc(p,Vector2.one*size*.46f,Ink);ActivityDisc(p+Vector3.back*.003f,Vector2.one*size*.37f,color);ActivityDisc(p+Vector3.back*.006f,Vector2.one*size*.30f,Ink);ActivityDisc(p+Vector3.back*.009f,Vector2.one*size*.13f,Kaos);ActivityDisc(p+Vector3.back*.012f,Vector2.one*size*.035f,Cream);}
            if(id==2){var v=new[]{new Vector2(.02f,.49f),new Vector2(-.30f,-.07f),new Vector2(-.03f,-.07f),new Vector2(-.11f,-.49f),new Vector2(.30f,.11f),new Vector2(.04f,.11f)};var t=new[]{0,1,2,0,2,5,2,3,4,2,4,5};for(int i=0;i<t.Length;i+=3){var a=p+(Vector3)(v[t[i]]*size);var b=p+(Vector3)(v[t[i+1]]*size);var c=p+(Vector3)(v[t[i+2]]*size);if(Vector3.Cross(b-a,c-a).z>0)Triangle(a,c,b,color,"Paper");else Triangle(a,b,c,color,"Paper");}}
            if(id==3){ActivityDisc(p,Vector2.one*size*.45f,color);ActivityDisc(p+new Vector3(size*.18f,size*.12f,-.003f),Vector2.one*size*.36f,Cream);}
            if(id==4){ActivityDisc(p+Vector3.down*size*.12f,new Vector2(.23f,.19f)*size,color);for(int i=0;i<4;i++)ActivityDisc(p+new Vector3((i-1.5f)*size*.19f,size*(i==0||i==3?.15f:.29f),0),new Vector2(.09f,.115f)*size,color);}
            if(id==5){Block(p,new Vector3(size*.81f,size*.51f,.014f),Ink,"Paper",.02f);Block(p+Vector3.up*size*.30f,new Vector3(size*.25f,size*.13f,.012f),Ink,"Paper",.01f);ActivityDisc(p+Vector3.back*.012f,Vector2.one*size*.22f,Cream);ActivityDisc(p+Vector3.back*.015f,Vector2.one*size*.16f,new Color(.10f,.65f,.72f));ActivityDisc(p+new Vector3(-size*.04f,size*.05f,-.018f),Vector2.one*size*.05f,Cream);}
            if(id==6){ActivityDisc(p,Vector2.one*size*.47f,Ink);ActivityDisc(p+Vector3.back*.003f,Vector2.one*size*.43f,color);for(int axis=0;axis<2;axis++){var d=axis==0?Vector3.up:Vector3.right;Rod(p-d*size*.42f+Vector3.back*.008f,p+d*size*.42f+Vector3.back*.008f,size*.018f,Ink,"Paper",4);}for(int side=-1;side<=1;side+=2)for(int i=0;i<12;i++){float y=Mathf.Lerp(-.40f,.40f,i/12f),z=Mathf.Lerp(-.40f,.40f,(i+1)/12f);Rod(p+new Vector3(side*(.19f+.25f*y*y),y,-.009f)*size,p+new Vector3(side*(.19f+.25f*z*z),z,-.009f)*size,size*.014f,Ink,"Paper",4);}}
        }
        void BuildStreetActivities(){
            var board=ActivityGeometry("Mural coletivo",()=>{Block(Vector3.zero,new Vector3(3,2.25f,.055f),Ink,"Wood");Block(Vector3.back*.04f,new Vector3(2.89f,2.13f,.025f),new Color(.22f,.23f,.20f),"CityStucco");Words("ENCONTRO",new Vector3(0,.89f,-.061f),.031f,Cream);});
            board.localPosition=CollectiveMural;board.localRotation=MuralFacing;
            streetStickers=new Transform[7];muralStickers=new Transform[7];
            for(int id=0;id<7;id++){
                int sticker=id;var anchor=StickerAnchors[id];
                if(!WorldFree(anchor,.3f))for(int step=1;step<=12;step++){var candidate=anchor+new Vector2((step%2==0?1:-1)*Mathf.Ceil(step/2f)*.5f,0);if(WorldFree(candidate,.3f)){anchor=candidate;break;}}
                streetStickers[id]=ActivityGeometry("Adesivo encontrado — "+StickerNames[id],()=>{foreach(float x in new[]{-.22f,.22f}){Rod(new Vector3(x,.05f,.12f),new Vector3(x,1.94f,.12f),.025f,Ink,"CityMetal",6);Block(new Vector3(x,.035f,0),new Vector3(.16f,.07f,.44f),Ink,"CityMetal");}Block(new Vector3(0,1.53f,.035f),new Vector3(.76f,.93f,.075f),new Color(.38f,.25f,.15f),"Wood",.025f);StickerIcon(new Vector3(0,1.55f,-.015f),sticker,.43f);Words("DO BAIRRO",new Vector3(0,1.17f,-.01f),.009f,Cream);});streetStickers[id].localPosition=new Vector3(anchor.x,0,anchor.y);
                muralStickers[id]=ActivityGeometry("No mural — "+StickerNames[id],()=>StickerIcon(Vector3.zero,sticker,.43f));muralStickers[id].localScale=new Vector3(1,1,.015f);muralStickers[id].gameObject.SetActive(false);
            }
            BuildCourtBall();
        }
        void BuildCourtBall(){
            const int sides=16,rows=10;var v=new Vector3[(sides+1)*(rows+1)];var n=new Vector3[v.Length];var c=new Color[v.Length];var uv=new Vector2[v.Length];var tri=new int[sides*rows*6];
            for(int r=0;r<=rows;r++)for(int i=0;i<=sides;i++){int k=r*(sides+1)+i;float phi=r*Mathf.PI/rows,a=i*Mathf.PI*2/sides;n[k]=new Vector3(Mathf.Sin(phi)*Mathf.Cos(a),Mathf.Cos(phi),Mathf.Sin(phi)*Mathf.Sin(a));v[k]=n[k]*.28f;uv[k]=new Vector2(i/(float)sides,r/(float)rows);c[k]=i%4==0||r==5?Ink:new Color(.90f,.34f,.075f);}
            for(int r=0;r<rows;r++)for(int i=0;i<sides;i++){int a=r*(sides+1)+i,b=a+sides+1,k=(r*sides+i)*6;tri[k]=a;tri[k+1]=a+1;tri[k+2]=b;tri[k+3]=a+1;tri[k+4]=b+1;tri[k+5]=b;}
            var mesh=new Mesh{name="Bola compartilhada da quadra"};mesh.vertices=v;mesh.normals=n;mesh.colors=c;mesh.uv=uv;mesh.triangles=tri;mesh.RecalculateBounds();assets.Add(mesh);
            courtBall=new GameObject("Bola — pegue e arremesse").transform;courtBall.SetParent(transform,false);courtBall.localPosition=CourtBallSpot;courtBall.gameObject.AddComponent<MeshFilter>().sharedMesh=mesh;var material=CityMaterial("Leather");assets.Add(material);courtBall.gameObject.AddComponent<MeshRenderer>().sharedMaterial=material;
        }
        public void ApplyStreetMemories(NeighbourhoodProgress data,int preview=-1,float[] x=null,float[] y=null,float[] angle=null,int[] layer=null){
            if(streetStickers==null)return;data.EnsureMuralLayout();
            for(int i=0;i<7;i++){
                streetStickers[i].gameObject.SetActive(!data.HasSticker(i));muralStickers[i].gameObject.SetActive(((preview<0?data.placed:preview)&(1<<i))!=0);
                float a=(angle??data.muralAngle)[i];var p=ClampMuralPoster(i,new Vector2((x??data.muralX)[i],(y??data.muralY)[i]),a);
                muralStickers[i].localPosition=CollectiveMural+MuralFacing*new Vector3(p.x,p.y,-.058f-(layer??data.muralLayer)[i]*.001f);
                muralStickers[i].localRotation=MuralFacing*Quaternion.Euler(0,0,a);
            }
        }
        public bool ActivityLook(Camera camera,Vector3 local,float range=3.5f){var offset=transform.TransformPoint(local)-camera.transform.position;return offset.sqrMagnitude<range*range&&Vector3.Dot(camera.transform.forward,offset.normalized)>.93f;}
        public int LookedSticker(Camera camera,NeighbourhoodProgress data){if(streetStickers==null)return -1;for(int i=0;i<7;i++)if(!data.HasSticker(i)&&ActivityLook(camera,StickerPoint(i)))return i;return -1;}
        public int LookedStreetResident(Camera camera){for(int i=0;i<Mathf.Min(StreetResidentCount,residents.Count);i++)if(ActivityLook(camera,residents[i].root.localPosition+Vector3.up*2.2f,4)&&NeighbourVisible(Position,new Vector2(residents[i].root.localPosition.x,residents[i].root.localPosition.z)))return i;return -1;}
        public void StartStreetChallenge(int index,int gesture){ChallengeResident=index;challengeGesture=gesture;challengeAge=0;}
        public void HoldStreetSpeaker(int index){streetSpeaker=index;}
        bool IsStreetSpeaker(Resident actor)=>streetSpeaker>=0&&streetSpeaker<residents.Count&&residents[streetSpeaker]==actor;
        public void EndStreetSpeaker(){if(streetSpeaker>=0&&streetSpeaker<residents.Count)residents[streetSpeaker].wait=0;streetSpeaker=-1;}
        public void ReactToCourt(bool made){if(residents.Count<=6)return;residents[6].reaction=2.8f;residents[6].reactionKind=made?2:3;}
        public void EndStreetChallenge(){if(ChallengeResident>=0&&ChallengeResident<residents.Count){residents[ChallengeResident].reaction=0;residents[ChallengeResident].wait=0;}ChallengeResident=-1;}
        bool ApplyStreetChallengeArm(Resident actor,float side,ref Quaternion upper,ref Quaternion lower){
            if(ChallengeResident<0||residents[ChallengeResident]!=actor)return false;
            if(!GestureUsesHand(challengeGesture,side))return true;
            int old=playerEmote;float age=emoteAge;
            try{playerEmote=challengeGesture;emoteAge=challengeAge;EmoteArmPose(side,out var a,out var b,out _);float w=Mathf.SmoothStep(0,1,challengeAge/.3f);upper=Quaternion.Slerp(upper,a,w);lower=Quaternion.Slerp(lower,b,w);}
            finally{playerEmote=old;emoteAge=age;}
            return true;
        }
        public void HoldCourtBall(){BallHeld=true;ballFlight=-1;}
        public void ThrowCourtBall(Vector3 start,bool made){BallHeld=false;shotStart=transform.InverseTransformPoint(start);shotEnd=CourtHoop+new Vector3(made?0:1.7f,0,0);ballFlight=0;}
        public void ResetCourtBall(){BallHeld=false;ballFlight=-1;if(courtBall!=null)courtBall.localPosition=CourtBallSpot;}
        public void FrameHeldCourtBall(Camera camera){if(BallHeld&&courtBall!=null){courtBall.position=camera.transform.position+camera.transform.forward*1.25f-camera.transform.up*.72f+camera.transform.right*.45f;}}
        void TickStreetActivitiesWorld(float delta){
            if(IsInterior||courtBall==null)return;
            if(streetSpeaker>=0&&streetSpeaker<residents.Count){var actor=residents[streetSpeaker];actor.wait=1;actor.speed=0;var to=Position-new Vector2(actor.root.localPosition.x,actor.root.localPosition.z);if(to.sqrMagnitude>.01f)actor.facing=Quaternion.RotateTowards(actor.facing,Quaternion.LookRotation(new Vector3(-to.x,0,-to.y)),delta*180);}
            if(ChallengeResident>=0){challengeAge+=delta;var actor=residents[ChallengeResident];actor.wait=1;actor.speed=0;actor.reaction=.3f;actor.reactionKind=challengeGesture;}
            if(ballFlight>=0){ballFlight+=delta;float t=ballFlight/.95f;if(t<=1)courtBall.localPosition=Vector3.Lerp(shotStart,shotEnd,t)+Vector3.up*(Mathf.Sin(t*Mathf.PI)*3.8f);else courtBall.localPosition=shotEnd+new Vector3((t-1)*.5f,-Mathf.Min(5,9*(t-1)*(t-1)),0);courtBall.Rotate(delta*220,delta*100,0);if(ballFlight>2)ResetCourtBall();}
        }
    }
}
