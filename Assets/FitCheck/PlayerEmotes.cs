using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        int playerEmote,queuedEmote;
        float emoteAge,emoteBlend;bool emoteEnding;
        ResidentPart gestureHand,normalGestureHand;
        sealed class GesturePalm{public ResidentPart part,source;public Vector3[] points,normals;public Vector3 wrist;}
        GesturePalm[,] gesturePalms=new GesturePalm[6,2];
        readonly ResidentPart[] originalPalms=new ResidentPart[2];
        public bool PlayerEmoteActive=>playerEmote!=0;
        public int PlayerEmoteId=>playerEmote;
        public float PlayerEmoteTime=>emoteAge;
        public bool PlayPlayerEmote(int id)
        {
            if(!PlayerBodyReady||id<1||id>5)return false;
            if(PlayerEmoteActive){queuedEmote=playerEmote==id?0:id;emoteEnding=true;return true;}
            playerEmote=id;emoteAge=0;emoteBlend=0;emoteEnding=false;playerPoseCycle=float.NaN;return true;
        }
        public void CancelPlayerEmote(bool immediate=false)
        {
            queuedEmote=0;emoteEnding=true;
            if(immediate){playerEmote=0;emoteBlend=0;RestoreEmoteHand();playerPoseCycle=float.NaN;if(PlayerBodyReady)EnsurePlayerPose();}
        }
        void ResetPlayerEmotes(){playerEmote=queuedEmote=0;emoteAge=emoteBlend=0;emoteEnding=false;gestureHand=normalGestureHand=null;gesturePalms=new GesturePalm[6,2];originalPalms[0]=originalPalms[1]=null;playerPoseArmsOnly=false;}
        void TickPlayerEmote(float delta,float distance)
        {
            if(!PlayerEmoteActive)return;emoteAge+=delta;
            if(distance>.0001f){queuedEmote=0;emoteEnding=true;}
            if(emoteAge>(playerEmote==2?2.5f:2.1f)-.25f)emoteEnding=true;
            emoteBlend=Mathf.MoveTowards(emoteBlend,emoteEnding?0:1,delta*6);
            if(emoteEnding&&emoteBlend<=0){int next=queuedEmote;playerEmote=queuedEmote=0;RestoreEmoteHand();playerPoseCycle=float.NaN;if(next>0)PlayPlayerEmote(next);}
        }
        void ApplyPlayerEmoteArm(Resident actor,float side,ref Quaternion upper,ref Quaternion lower)
        {
            if(actor!=playerBody||!PlayerEmoteActive||emoteBlend<=0)return;
            if(!GestureUsesHand(playerEmote,side))return;
            EmoteArmPose(side,out var a,out var b,out _);
            float weight=Mathf.SmoothStep(0,1,emoteBlend);upper=Quaternion.Slerp(upper,a,weight);lower=Quaternion.Slerp(lower,b,weight);
        }
        static bool GestureUsesHand(int emote,float side)=>side>0||emote==2||emote==3;
        void EmoteArmPose(float side,out Quaternion upper,out Quaternion lower,out Vector3 wrist)
        {
            int hand=side>0?1:0;var source=originalPalms[hand];
            var shoulder=new Vector3(side*.39f,1.84f,.015f);var elbowRest=new Vector3(side*.52f,1.50f,side<0?.005f:-.025f);
            var wristRest=source==null?new Vector3(side*.485f,1.17f,-.095f):source.handRestMatrix.MultiplyPoint3x4(Vector3.zero);
            float pulse=Mathf.Sin(emoteAge*9);
            wrist=new Vector3(.27f,2.22f+pulse*.025f,-.50f);
            if(playerEmote==2){float gap=.055f+.22f*(.5f+.5f*Mathf.Cos((emoteAge-.35f)*Mathf.PI*5));wrist=new Vector3(side*gap,2.04f,-.52f);}
            if(playerEmote==3)wrist=side>0?new Vector3(.94f,2.16f,-.36f):new Vector3(.08f,2.19f,-.52f);
            if(playerEmote==4)wrist=new Vector3(-.06f+pulse*.008f,2.12f,-.57f);
            if(playerEmote==5)wrist=new Vector3(.28f,2.20f+pulse*.012f,-.51f);
            float a=(elbowRest-shoulder).magnitude,b=(wristRest-elbowRest).magnitude;
            var axis=wrist-shoulder;float distance=Mathf.Clamp(axis.magnitude,.05f,a+b-.012f);axis.Normalize();wrist=shoulder+axis*distance;
            float along=(a*a-b*b+distance*distance)/(2*distance),height=Mathf.Sqrt(Mathf.Max(0,a*a-along*along));
            var bend=Vector3.ProjectOnPlane(new Vector3(side*.65f,-1,.15f),axis).normalized;
            var elbow=shoulder+axis*along+bend*height;
            upper=Quaternion.FromToRotation(elbowRest-shoulder,elbow-shoulder);
            lower=Quaternion.FromToRotation(wristRest-elbowRest,wrist-elbow);
        }
        Quaternion EmotePalmRotation(float side)
        {
            if(playerEmote==2)return Quaternion.LookRotation(Vector3.right*side,Vector3.down);
            if(playerEmote==3)return side>0?Quaternion.LookRotation(Vector3.back,Vector3.left):Quaternion.Euler(160,0,70);
            if(playerEmote==4)return Quaternion.Euler(155,0,Mathf.Sin(emoteAge*9)*3);
            // Palm is local -Z and fingers local -Y. Both face the camera after
            // this half turn; forearm bending must not decide wrist orientation.
            return Quaternion.Euler(180,0,playerEmote==1?Mathf.Sin(emoteAge*9)*5:0);
        }
        // Presentation transform only: the mirror always uses the anatomical body
        // pose. First-person hands sit in front of the lens and follow its pitch.
        void FrameEmoteArms(bool external)
        {
            float weight=external||!PlayerEmoteActive?0:Mathf.SmoothStep(0,1,emoteBlend);
            var rotation=Quaternion.Slerp(Quaternion.identity,Quaternion.Euler(-ViewPitch,0,0),weight);
            var pivot=new Vector3(0,EyeHeight+.055f,-.38f);
            float lift=playerEmote==3?.35f:.42f;
            var offset=new Vector3(0,lift,playerEmote==3?-1.35f:-.65f)*weight;
            var position=pivot+rotation*(offset-pivot);
            foreach(var part in playerBody.parts)if(part.arm&&part.node!=null){part.node.transform.localPosition=position;part.node.transform.localRotation=rotation;}
            foreach(var palm in gesturePalms)if(palm!=null){palm.part.node.transform.localPosition=position;palm.part.node.transform.localRotation=rotation;}
        }
        void PrepareEmoteHand()
        {
            foreach(var part in playerBody.parts)if(part.node.name=="Mão")originalPalms[part.side>0?1:0]=part;
            normalGestureHand=originalPalms[1];
            for(int emote=1;emote<=5;emote++)for(int hand=0;hand<2;hand++){
                float side=hand==0?-1:1;if(!GestureUsesHand(emote,side)||originalPalms[hand]==null)continue;
                var source=originalPalms[hand];var mesh=StudioView.CreateGestureHand((int)side,emote);var points=mesh.vertices;var normals=mesh.normals;
                assets.Add(mesh);playerBodyAssets.Add(mesh);var node=new GameObject("Gesto alternativo — mão");node.transform.SetParent(playerBody.root,false);
                node.AddComponent<MeshFilter>().sharedMesh=mesh;node.AddComponent<MeshRenderer>().sharedMaterial=source.node.GetComponent<MeshRenderer>().sharedMaterial;node.SetActive(false);
                var part=new ResidentPart{mesh=mesh,rest=points,normals=normals,vertices=new Vector3[points.Length],posedNormals=new Vector3[points.Length],arm=true,side=side,node=node};
                gesturePalms[emote,hand]=new GesturePalm{part=part,source=source,points=points,normals=normals,wrist=source.handRestMatrix.MultiplyPoint3x4(Vector3.zero)};
                if(emote==5)gestureHand=part;
            }
        }
        void RestoreEmoteHand()
        {
            foreach(var palm in gesturePalms)if(palm!=null&&palm.part.node!=null)palm.part.node.SetActive(false);
            foreach(var palm in originalPalms)if(palm!=null&&palm.node!=null)palm.node.GetComponent<MeshRenderer>().enabled=true;
        }
        void PoseEmoteHand(float dip)
        {
            RestoreEmoteHand();if(!PlayerEmoteActive||emoteBlend<=.15f)return;
            var body=ResidentBody(playerBody,4);float weight=Mathf.SmoothStep(0,1,emoteBlend);
            for(int hand=0;hand<2;hand++){
                var palm=gesturePalms[playerEmote,hand];if(palm==null)continue;var part=palm.part;float side=part.side;
                EmoteArmPose(side,out var upper,out var lower,out _);
                upper=Quaternion.Slerp(Quaternion.identity,upper,weight);lower=Quaternion.Slerp(Quaternion.identity,lower,weight);
                var shoulder=new Vector3(side*.39f,1.84f,.015f);var elbowRest=new Vector3(side*.52f,1.50f,side<0?.005f:-.025f);
                var wrist=shoulder+upper*(elbowRest-shoulder)+lower*(palm.wrist-elbowRest);
                var rotation=Quaternion.Slerp(palm.source.handRestMatrix.rotation,EmotePalmRotation(side),weight);
                for(int i=0;i<part.rest.Length;i++){part.vertices[i]=body.TorsoPoint(wrist+rotation*palm.points[i]);part.posedNormals[i]=body.torso*rotation*palm.normals[i];}
                part.mesh.vertices=part.vertices;part.mesh.normals=part.posedNormals;part.mesh.RecalculateBounds();
                palm.source.node.GetComponent<MeshRenderer>().enabled=false;part.node.SetActive(true);
            }
        }
    }
    public partial class FirstCustomer
    {
        void ReadPlayerEmotes()
        {
            if(paused||loadingActive||showShopDecor||shopGuideOpen||shopFittingOpening||ShopSequenceActive||homeDoorExitRemaining>0||!CityInputFocused)return;
            if(Input.GetKeyDown(KeyCode.Alpha0)||Input.GetKeyDown(KeyCode.Keypad0))city.CancelPlayerEmote();
            else if(Input.GetKeyDown(KeyCode.Comma))city.PlayPlayerEmote(5);
            else for(int i=0;i<4;i++)if(Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1+i))||Input.GetKeyDown((KeyCode)((int)KeyCode.Keypad1+i))){city.PlayPlayerEmote(i+1);break;}
        }
    }
}
