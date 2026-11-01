using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class CityView
    {
        [SerializeField] Resident playerBody;
        [SerializeField] List<GameObject> playerHeadPieces=new List<GameObject>();
        [SerializeField] List<Object> playerBodyAssets=new List<Object>();
        public bool PlayerBodyReady=>playerBody!=null&&playerBody.root!=null&&playerBody.parts.Count>0&&playerHeadPieces.Count>0;
        public bool PlayerHeadVisible=>PlayerBodyReady&&playerHeadPieces.Exists(piece=>piece!=null&&piece.activeInHierarchy);
        public Vector3 PlayerBodyPosition=>playerBody.root.position;
        public bool PlayerBodyVisible {get=>PlayerBodyReady&&playerBody.root.gameObject.activeSelf;set{if(PlayerBodyReady)playerBody.root.gameObject.SetActive(value);}}
        public void PreparePlayerBody(StudioView studio)
        {
            if(playerBody!=null&&playerBody.root!=null){playerBody.root.gameObject.SetActive(false);Destroy(playerBody.root.gameObject);}
            foreach(var asset in playerBodyAssets){assets.Remove(asset);if(asset!=null)Destroy(asset);}playerBodyAssets.Clear();
            playerHeadPieces.Clear();
            var rotation=studio.Avatar.rotation;
            try{
                studio.Avatar.rotation=Quaternion.identity;
                int first=assets.Count;
                playerBody=CopyAvatar(studio,"Corpo do jogador",new Vector3(Position.x,-.055f,Position.y),ViewYaw+180,false);
                for(int part=0;part<playerBody.parts.Count;part++)if(playerBody.parts[part].head||playerBody.root.GetChild(part).name=="Pescoço")playerHeadPieces.Add(playerBody.root.GetChild(part).gameObject);
                foreach(var piece in playerHeadPieces)piece.SetActive(false);
                for(int i=first;i<assets.Count;i++)playerBodyAssets.Add(assets[i]);
                StepPlayerBody(0,0);
            }finally{studio.Avatar.rotation=rotation;}
        }
        public void FramePlayerForCamera(Camera camera,bool external)
        {
            if(!PlayerBodyReady)return;
            PlayerBodyVisible=true;
            bool showHead=external&&Vector3.Distance(camera.transform.position,playerBody.root.TransformPoint(new Vector3(0,2.8f,0)))>1.25f;
            foreach(var piece in playerHeadPieces)if(piece!=null)piece.SetActive(showHead);
            if(shadow!=null){shadow.localPosition=new Vector3(Position.x,.005f,Position.y);shadow.gameObject.SetActive(external&&showHead);}
        }
        void StepPlayerBody(float distance,float delta)
        {
            if(!PlayerBodyReady)return;
            var actor=playerBody;actor.cycle+=delta;
            actor.phase=Mathf.Repeat(actor.phase+distance/2.2f,1);
            actor.blend=Mathf.MoveTowards(actor.blend,distance>.00001f?1:0,delta*8);
            float dip=actor.blend*(.035f+.005f*Mathf.Cos(actor.phase*Mathf.PI*4));
            // The body faces with the view, including when strafing or backing up.
            actor.root.localRotation=Quaternion.Euler(0,ViewYaw+180,0);
            var forward=Quaternion.Euler(0,ViewYaw,0)*Vector3.forward;
            actor.root.localPosition=new Vector3(Position.x,-.055f-dip,Position.y)-forward*.38f;
            PoseResident(actor,4,dip);
        }
        public int CheckPlayerBody(Camera camera)
        {
            if(!PlayerBodyReady)throw new System.Exception("Corpo do jogador ausente");
            int visible=0,feet=0,legs=0;
            foreach(var part in playerBody.parts){
                if(part.head){if(PlayerHeadVisible)throw new System.Exception("Cabeça bloqueia câmera em primeira pessoa");continue;}
                foreach(var vertex in part.vertices){
                    var point=camera.WorldToViewportPoint(playerBody.root.TransformPoint(vertex));
                    if(point.z>camera.nearClipPlane&&point.x>0&&point.x<1&&point.y>0&&point.y<1){visible++;if(part.foot)feet++;else if(part.leg)legs++;}
                }
            }
            if(visible<100||feet<10||legs<10)throw new System.Exception("Roupa/pernas/tênis fora da vista ao olhar para baixo: "+visible+" / "+legs+" / "+feet);
            return visible;
        }
    }
}
