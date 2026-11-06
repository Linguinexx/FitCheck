using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class StudioView
    {
        static readonly Vector3 TorsoPivot=new Vector3(0,1.25f,0);
        Transform garmentTorso,garmentHead;
        [SerializeField] Transform[] handPivots=new Transform[2],calves=new Transform[2];
        [SerializeField] Mesh[] calfMeshes=new Mesh[2],armMeshes=new Mesh[2],sleeveMeshes=new Mesh[2];
        readonly Vector3[][] calfRest=new Vector3[2][];
        [SerializeField] int[] skinCuffRows=new int[2],sleeveInnerRows=new int[2],sleeveEndRows=new int[2];
        [SerializeField] Vector3[] leftCalfRest,rightCalfRest;
        const int ArmSides=20;
        int WeightSide {get{return faceProfile==1||faceProfile==4?-1:1;}}
        void CacheCalves()
        {
            int index=0;foreach(Transform child in Avatar)if(child.name=="Canela"){
                calves[index]=child;var mesh=child.GetComponent<MeshFilter>().sharedMesh;calfMeshes[index]=mesh;
                var vertices=mesh.vertices;for(int i=0;i<vertices.Length;i++)vertices[i]=child.localPosition+child.localRotation*Vector3.Scale(vertices[i],child.localScale);
                calfRest[index]=vertices;if(index==0)leftCalfRest=vertices;else rightCalfRest=vertices;child.localPosition=Vector3.zero;child.localRotation=Quaternion.identity;child.localScale=Vector3.one;index++;
            }
        }
        static Vector3 ArmControl(int side,int point,bool loose)
        {
            switch(Mathf.Clamp(point,0,4)){
                case 0:return new Vector3(side*(loose?.22f:.16f),2.00f-side*.015f,.015f);
                case 1:return new Vector3(side*(loose?.47f:.40f),1.94f-side*.012f,.018f);
                case 2:return new Vector3(side*(loose?.55f:.52f),1.63f-side*.01f,side<0?.025f:-.015f);
                case 3:return new Vector3(side*(loose?.525f:.495f),1.37f,side<0?-.010f:-.045f);
                default:return new Vector3(side*(loose?.515f:.485f),1.17f,side<0?-.055f:-.095f);
            }
        }
        static Vector3 ArmPoint(int side,float t,bool loose)
        {
            float x=Mathf.Clamp01(t)*4;int segment=Mathf.Min(3,Mathf.FloorToInt(x));float u=x-segment;
            var a=ArmControl(side,segment-1,loose);var b=ArmControl(side,segment,loose);var c=ArmControl(side,segment+1,loose);var d=ArmControl(side,segment+2,loose);
            return .5f*((2*b)+(-a+c)*u+(2*a-5*b+4*c-d)*u*u+(-a+3*b-3*c+d)*u*u*u);
        }
        static void ArmFrame(int side,float t,bool loose,out Vector3 tangent,out Vector3 across,out Vector3 forward)
        {
            tangent=(ArmPoint(side,Mathf.Min(1,t+.002f),loose)-ArmPoint(side,Mathf.Max(0,t-.002f),loose)).normalized;
            across=Vector3.Cross(tangent,Vector3.forward).normalized;forward=Vector3.Cross(tangent,across).normalized;
        }
        GameObject ArmSurface(int side,Garment top,Material material,bool sleeve,float end,int index)
        {
            var samples=new List<float>();for(int i=0;i<=28;i++)samples.Add(sleeve?Mathf.Lerp(.22f,end,i/28f):Mathf.Lerp(end-.025f,1,i/28f));
            if(!sleeve){samples.Add(end);samples.Sort();skinCuffRows[index]=samples.IndexOf(end);}
            var v=new List<Vector3>();var n=new List<Vector3>();var uv=new List<Vector2>();var triangles=new List<int>();var liningTriangles=new List<int>();
            var shoulder=sleeve?SleeveShoulder(side,top.loose):null;float sleevePhase=0;
            if(sleeve){var center=Vector3.zero;foreach(var p in shoulder)center+=p;center/=shoulder.Count;var d=shoulder[0]-center;sleevePhase=Mathf.Atan2(Vector3.Dot(d,Vector3.back),Vector3.Dot(d,Vector3.down*side));}
            bool longSleeve=top.id.EndsWith("TOP_03"),patternedSleeve=sleeve&&PlayerSleeveTexture(top.id)!=null;float length=0;
            for(int r=0;r<samples.Count;r++){
                float t=samples[r];var center=ArmPoint(side,t,top.loose);Vector3 tangent,across,forward;ArmFrame(side,t,top.loose,out tangent,out across,out forward);
                if(r>0)length+=Vector3.Distance(center,ArmPoint(side,samples[r-1],top.loose));
                float rx,rz;
                if(sleeve){float f=Mathf.SmoothStep(0,1,Mathf.InverseLerp(.22f,end,t));rx=Mathf.Lerp(top.loose?.175f:.145f,longSleeve?.105f:top.loose?.16f:.135f,f);rz=Mathf.Lerp(top.loose?.205f:.18f,longSleeve?.115f:top.loose?.19f:.16f,f);}
                else{rx=Mathf.Lerp(.115f,.072f,Mathf.SmoothStep(.3f,1,t));rz=rx*.9f;}
                for(int i=0;i<=ArmSides;i++){
                    float angle=i*Mathf.PI*2/ArmSides+sleevePhase;var radial=across*Mathf.Cos(angle)*rx+forward*Mathf.Sin(angle)*rz;
                    v.Add(center+radial-TorsoPivot);n.Add((across*Mathf.Cos(angle)/rx+forward*Mathf.Sin(angle)/rz).normalized);
                    uv.Add(patternedSleeve?new Vector2(i/(float)ArmSides,r/(float)(samples.Count-1)):
                        new Vector2(i/(float)ArmSides*Mathf.PI*(rx+rz)/AvatarTileSize,length/AvatarTileSize));
                }
            }
            for(int r=0;r<samples.Count-1;r++)for(int i=0;i<ArmSides;i++){
                int a=r*(ArmSides+1)+i,b=a+ArmSides+1;triangles.AddRange(new[]{a,a+1,b,a+1,b+1,b});
            }
            if(sleeve){
                int last=(samples.Count-1)*(ArmSides+1);sleeveEndRows[index]=last;
                Vector3 tangent,across,forward;ArmFrame(side,end,top.loose,out tangent,out across,out forward);
                float rx=(longSleeve?.105f:top.loose?.16f:.135f)-.012f,rz=(longSleeve?.115f:top.loose?.19f:.16f)-.012f;
                int inner=v.Count;sleeveInnerRows[index]=inner;
                for(int row=0;row<2;row++)for(int i=0;i<=ArmSides;i++){
                    float a=i*Mathf.PI*2/ArmSides+sleevePhase;v.Add(ArmPoint(side,end,top.loose)+across*Mathf.Cos(a)*rx+forward*Mathf.Sin(a)*rz-tangent*(row*.035f)-TorsoPivot);
                    n.Add(row==0?tangent:-(across*Mathf.Cos(a)+forward*Mathf.Sin(a)));uv.Add(new Vector2(i/(float)ArmSides*.7f,length/AvatarTileSize-row*.025f));
                }
                for(int i=0;i<ArmSides;i++){
                    int a=last+i,b=inner+i,c=b+ArmSides+1;triangles.AddRange(new[]{a,a+1,b,a+1,b+1,b});liningTriangles.AddRange(new[]{b,c,b+1,b+1,c,c+1});
                }
            }else{
                for(int cap=0;cap<2;cap++){
                    int row=cap==0?0:samples.Count-1,start=v.Count;Vector3 tangent,across,forward;ArmFrame(side,samples[row],top.loose,out tangent,out across,out forward);
                    v.Add(ArmPoint(side,samples[row],top.loose)-TorsoPivot);n.Add(cap==0?-tangent:tangent);uv.Add(new Vector2(.5f,.5f));
                    for(int i=0;i<ArmSides;i++){int a=row*(ArmSides+1)+i;triangles.AddRange(cap==0?new[]{start,a+1,a}:new[]{start,a,a+1});}
                }
            }
            if(sleeve){
                int first=v.Count;
                for(int i=0;i<=ArmSides;i++){v.Add(shoulder[i%ArmSides]-TorsoPivot);n.Add(n[i]);uv.Add(uv[i]+new Vector2(0,-.08f));}
                for(int i=0;i<ArmSides;i++){int a=first+i;triangles.AddRange(new[]{a,a+1,i,a+1,i+1,i});}
            }
            var go=Surface(sleeve?(longSleeve?"Manga comprida":"Manga"):"Braço contínuo",v,triangles,uv,n,Vector3.zero,Vector3.one,material,garmentTorso);
            var mesh=go.GetComponent<MeshFilter>().sharedMesh;
            if(sleeve){
                sleeveMeshes[index]=mesh;mesh.subMeshCount=2;mesh.SetTriangles(liningTriangles,1);
                go.GetComponent<MeshRenderer>().sharedMaterials=new[]{material,SurfaceMaterial(top.color*.68f,"Cloth")};
            }else armMeshes[index]=mesh;
            return go;
        }
        static bool UpperPiece(string name)
        {
            return name.StartsWith("Detalhe civil")||name.StartsWith("BlenderTopo")||name=="Camiseta com caimento"||name=="Gola"||name=="Barra costurada"||name=="Estampa discreta"||name=="Carretel da fita"||name=="Grafismo da coleção"||name=="Capuz";
        }
        Vector3 LowerPoint(Vector3 p,int side)
        {
            float t=Mathf.Clamp01((p.y-.35f)/.95f);p.x+=WeightSide*.035f*t+side*.014f*(1-t);
            if(side!=0)p.z+=(side<0?-.04f:.02f)*(1-t)-(side<0?.025f:.008f)*Mathf.Sin(t*Mathf.PI);return p;
        }
        Vector3 BlenderLowerPoint(Vector3 p)
        {return Vector3.Lerp(LowerPoint(p,-1),LowerPoint(p,1),Mathf.SmoothStep(0,1,(p.x+.12f)/.24f));}
        void FitRelaxedBody(Garment top,Material fabric)
        {
            if(calfRest[0]==null)calfRest[0]=leftCalfRest;if(calfRest[1]==null)calfRest[1]=rightCalfRest;
            garmentTorso=new GameObject("Postura — tronco e braços").transform;garmentTorso.SetParent(clothes,false);
            garmentHead=new GameObject("Postura — peças da cabeça").transform;garmentHead.SetParent(clothes,false);
            var leftFoot=new GameObject("Apoio — pé esquerdo").transform;leftFoot.SetParent(clothes,false);
            var rightFoot=new GameObject("Apoio — pé direito").transform;rightFoot.SetParent(clothes,false);
            var parts=new List<Transform>();foreach(Transform part in clothes)if(part.GetComponent<MeshFilter>()!=null)parts.Add(part);
            foreach(var part in parts){
                string name=part.name;
                if(part.localPosition.y>=2.5f){part.SetParent(garmentHead,false);continue;}
                if(UpperPiece(name)){part.SetParent(garmentTorso,false);part.localPosition-=TorsoPivot;continue;}
                var filter=part.GetComponent<MeshFilter>();var mesh=filter.sharedMesh;
                bool shoe=name.StartsWith("Tênis")||name.StartsWith("Sola")||name=="Cadarço";
                int side=part.localPosition.x+(part.localRotation*Vector3.Scale(mesh.bounds.center,part.localScale)).x<0?-1:1;
                if(shoe){var pivot=new Vector3(side*.216f,.10f,0);part.SetParent(side<0?leftFoot:rightFoot,false);part.localPosition-=pivot;continue;}
                var posed=Instantiate(mesh);Own(posed);var vertices=posed.vertices;
                for(int i=0;i<vertices.Length;i++){var p=part.localPosition+part.localRotation*Vector3.Scale(vertices[i],part.localScale);vertices[i]=mesh.name.Contains("Blender Baggy")?BlenderLowerPoint(p):LowerPoint(p,name=="Cintura"?0:side);}
                posed.vertices=vertices;posed.RecalculateNormals();posed.RecalculateBounds();filter.sharedMesh=posed;part.localPosition=Vector3.zero;part.localRotation=Quaternion.identity;part.localScale=Vector3.one;
            }
            leftFoot.localPosition=new Vector3(-.230f,.10f,-.04f);leftFoot.localRotation=Quaternion.Euler(0,8,0);
            rightFoot.localPosition=new Vector3(.230f,.10f,.02f);rightFoot.localRotation=Quaternion.Euler(0,-12,0);
            for(int i=0;i<2;i++){
                int side=i==0?-1:1;var mesh=calfMeshes[i];var vertices=new Vector3[calfRest[i].Length];
                for(int j=0;j<vertices.Length;j++)vertices[j]=LowerPoint(calfRest[i][j],side);mesh.vertices=vertices;mesh.RecalculateNormals();mesh.RecalculateBounds();
                float end=top.id.EndsWith("TOP_03")?.97f:top.loose?.40f:.34f;
                ArmSurface(side,top,skin,false,end,i);ArmSurface(side,top,fabric,true,end,i);
                var hand=new GameObject("Punho relaxado").transform;hand.SetParent(garmentTorso,false);handPivots[i]=hand;
                hand.localPosition=ArmPoint(side,1,top.loose)-TorsoPivot;Vector3 tangent,across,forward;ArmFrame(side,1,top.loose,out tangent,out across,out forward);
                hand.localRotation=Quaternion.FromToRotation(Vector3.down,tangent)*Quaternion.Euler(-3,side*80,side*-3);
                BuildRelaxedHand(side,hand);
            }
            PoseRelaxedBody();
        }
        void PoseRelaxedBody()
        {
            if(garmentTorso==null)return;
            float sway=ReducedMotion?0:Mathf.Sin(idle*.8f+faceProfile*.4f)*.65f;
            var position=TorsoPivot+new Vector3(WeightSide*.035f,ReducedMotion?0:Mathf.Sin(idle*1.5f)*.003f,.01f);
            var rotation=Quaternion.Euler(-1.5f,WeightSide*1.5f,WeightSide*2+sway);garmentTorso.localPosition=position;garmentTorso.localRotation=rotation;
            var restHead=new Vector3(0,2.8f+HeadLift(faceProfile),0);var headShift=position+rotation*(restHead-TorsoPivot)-restHead;
            face.localPosition=restHead+headShift;hair.localPosition=headShift;garmentHead.localPosition=headShift;
            var neck=Avatar.Find("Pescoço");neck.localPosition=position+rotation*(new Vector3(0,2.31f+HeadLift(faceProfile)*.5f,.025f)-TorsoPivot);neck.localRotation=rotation;
        }
        public bool IsHeadPart(Transform part){return (face!=null&&part.IsChildOf(face))||(hair!=null&&part.IsChildOf(hair))||(garmentHead!=null&&part.IsChildOf(garmentHead));}
        public void ValidateArmFit()
        {
            ValidateHandMeshes();
            for(int side=0;side<2;side++){
                var skinVertices=armMeshes[side].vertices;var sleeveVertices=sleeveMeshes[side].vertices;
                int skinRow=skinCuffRows[side]*(ArmSides+1),outer=sleeveEndRows[side],inner=sleeveInnerRows[side];
                Vector3 skinCenter=Vector3.zero,sleeveCenter=Vector3.zero,wrist=Vector3.zero;
                // The two final skin vertices are the end-cap centers.
                int wristRow=skinVertices.Length-2-(ArmSides+1);
                for(int i=0;i<ArmSides;i++){
                    skinCenter+=skinVertices[skinRow+i];sleeveCenter+=sleeveVertices[outer+i];wrist+=skinVertices[wristRow+i];
                }
                skinCenter/=ArmSides;sleeveCenter/=ArmSides;wrist/=ArmSides;
                if(Vector3.Distance(skinCenter,sleeveCenter)>.001f)throw new System.Exception("Braço saiu do centro da manga");
                for(int i=0;i<ArmSides;i++){
                    float skinRadius=Vector3.Distance(skinVertices[skinRow+i],skinCenter);
                    float openingRadius=Vector3.Distance(sleeveVertices[inner+i],sleeveCenter);
                    if(openingRadius<skinRadius+.003f)throw new System.Exception("Pele atravessou a abertura da manga");
                }
                if(Vector3.Distance(wrist,handPivots[side].localPosition)>.001f)throw new System.Exception("Mão desencaixou do punho");
                var shirt=garmentTorso.Find("Camiseta com caimento");var body=shirt.GetComponent<MeshFilter>().sharedMesh.vertices;
                for(int i=0;i<ArmSides;i++){
                    var join=garmentTorso.TransformPoint(sleeveVertices[sleeveVertices.Length-ArmSides-1+i]);float nearest=float.MaxValue;
                    foreach(var vertex in body)nearest=Mathf.Min(nearest,Vector3.Distance(join,shirt.TransformPoint(vertex)));
                    if(nearest>.001f)throw new System.Exception("Costura do ombro separou da abertura da blusa");
                }
                var handMesh=handPivots[side].GetComponentInChildren<MeshFilter>();
                if(handMesh==null||sleeveMeshes[side].subMeshCount!=2)throw new System.Exception("Mão ou acabamento interno da manga ausente");
            }
        }
    }
}
