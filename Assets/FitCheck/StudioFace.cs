using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public enum AvatarExpression { Neutral,Curious,Skeptical,Sleepy,Happy,Surprised }
    public partial class StudioView
    {
        const float HeadRadius=.56f;
        [SerializeField] Transform[] eyes=new Transform[2],brows=new Transform[2],pupils=new Transform[2];
        [SerializeField] Mesh[] upperLids=new Mesh[2],lowerLids=new Mesh[2];
        [SerializeField] Mesh[] pupilMeshes=new Mesh[2];
        [SerializeField] GameObject[] upperLidObjects=new GameObject[2],lowerLidObjects=new GameObject[2];
        [SerializeField] float[] upperCover=new float[2],lowerCover=new float[2],lastUpper={-1,-1},lastLower={-1,-1};
        Transform smile,openMouth,surprisedMouth,face;Mesh mouthLineMesh;int faceProfile;
        Vector3 bodyScale=Vector3.one;AvatarExpression expression;float expressionStart,blinkClock;
        bool feminineAppearance;
        public bool TallPreview,PreviewProportions;
        public int ExpressionOverride=-1;
        public string ExpressionName {get{return new[]{"Neutra","Curiosa","Desconfiada","Sonolenta","Animada","Surpresa"}[(int)expression];}}
        public AvatarExpression CurrentExpression {get{return expression;}}
        public float FramingPadding {get{return Mathf.Max(0,bodyScale.y-1)*12+HeadLift(faceProfile)*2.5f;}}
        // The compact, short-neck body is the single approved base.
        float HeadLift(int profile){return 0;}
        Vector3 HeadProportion(int profile)
        {var body=BodyProportion(profile);return new Vector3(1/body.x,1/body.y,1/body.z);}
        Vector3 BodyProportion(int profile){return feminineAppearance?new Vector3(.91f,.94f,.98f):new Vector3(.98f,.94f,1f);}
        static float FaceZ(float x,float y){return -Mathf.Sqrt(Mathf.Max(.001f,HeadRadius*HeadRadius-x*x-(y-2.8f)*(y-2.8f)))-.008f;}
        Material FaceInk(Color c)
        {
            var m=Material(c);m.mainTexture=Texture2D.whiteTexture;m.SetFloat("_Motion",0);m.SetFloat("_Outline",0);m.SetFloat("_SmoothLight",1);return m;
        }
        GameObject FaceMesh(string name,Vector3[] vertices,int[] triangles,Vector3 p,Material mat)
        {
            var mesh=new Mesh{name=name,vertices=vertices,triangles=triangles};mesh.uv=new Vector2[vertices.Length];mesh.RecalculateNormals();mesh.RecalculateBounds();Own(mesh);
            var go=new GameObject(name);go.transform.SetParent(face,false);go.transform.localPosition=p;
            go.AddComponent<MeshFilter>().sharedMesh=mesh;go.AddComponent<MeshRenderer>().sharedMaterial=mat;return go;
        }
        Transform Disc(string name,Vector3 p,float width,float height,Material mat,bool project=false)
        {
            const int segments=24;var vertices=new Vector3[segments+1];var triangles=new int[segments*3];
            if(project)vertices[0].z=FaceZ(p.x,p.y);
            for(int i=0;i<segments;i++){
                float a=i*Mathf.PI*2/segments,x=Mathf.Cos(a)*width*.5f,y=Mathf.Sin(a)*height*.5f;
                vertices[i+1]=new Vector3(x,y,project?FaceZ(p.x+x,p.y+y):0);
                triangles[i*3]=0;triangles[i*3+1]=(i+1)%segments+1;triangles[i*3+2]=i+1;
            }
            return FaceMesh(name,vertices,triangles,p,mat).transform;
        }
        Transform Arc(string name,Vector3 p,float width,float curve,float thickness,Material mat)
        {
            var vertices=ArcVertices(p,width,curve,thickness,0);var triangles=new int[12*6];
            for(int i=0;i<12;i++){int a=i*2;triangles[i*6]=a;triangles[i*6+1]=a+1;triangles[i*6+2]=a+2;triangles[i*6+3]=a+2;triangles[i*6+4]=a+1;triangles[i*6+5]=a+3;}
            return FaceMesh(name,vertices,triangles,p,mat).transform;
        }
        Vector3[] ArcVertices(Vector3 p,float width,float curve,float thickness,float slope)
        {
            if(feminineAppearance){width*=.90f;thickness*=1.3f;curve-=.003f;}
            var vertices=new Vector3[26];for(int i=0;i<=12;i++)for(int edge=0;edge<2;edge++){
                float q=i/6f-1,x=q*width*.5f,y=q*q*curve+q*slope+(edge==0?-thickness:thickness)*.5f;
                vertices[i*2+edge]=new Vector3(x,y,FaceZ(p.x+x,p.y+y)-p.z);
            }return vertices;
        }
        void BuildLid(int eye,bool bottom)
        {
            var go=new GameObject(bottom?"Pálpebra inferior":"Pálpebra superior");go.transform.SetParent(eyes[eye],false);
            var mesh=new Mesh{name=go.name};Own(mesh);go.AddComponent<MeshFilter>().sharedMesh=mesh;go.AddComponent<MeshRenderer>().sharedMaterial=skin;
            if(bottom){lowerLids[eye]=mesh;lowerLidObjects[eye]=go;}else{upperLids[eye]=mesh;upperLidObjects[eye]=go;}
        }
        void BuildPupil(int eye,Material material)
        {
            var go=new GameObject("Pupila");go.transform.SetParent(eyes[eye],false);pupils[eye]=go.transform;
            var mesh=new Mesh{name="Pupila projetada no olho"};Own(mesh);pupilMeshes[eye]=mesh;
            go.AddComponent<MeshFilter>().sharedMesh=mesh;go.AddComponent<MeshRenderer>().sharedMaterial=material;
        }
        void UpdatePupil(int eye,float width,float height,float dx,float dy)
        {
            if(!EnsureFaceBindings())return;
            const int sides=24;var v=new Vector3[sides+1];var triangles=new int[sides*3];
            for(int i=0;i<=sides;i++){
                float a=(i-1)*Mathf.PI*2/sides,x=dx+(i==0?0:Mathf.Cos(a)*width*.5f),y=dy+(i==0?0:Mathf.Sin(a)*height*.5f);
                v[i]=new Vector3(x,y,-.105f*Mathf.Sqrt(Mathf.Max(.01f,1-x*x/(.17f*.17f)-y*y/(.18f*.18f)))-.003f);
                if(i<sides){triangles[i*3]=0;triangles[i*3+1]=(i+1)%sides+1;triangles[i*3+2]=i+1;}
            }
            var mesh=pupilMeshes[eye];mesh.vertices=v;mesh.triangles=triangles;mesh.uv=new Vector2[v.Length];mesh.RecalculateNormals();mesh.RecalculateBounds();
        }
        void UpdateLid(int eye,bool bottom,float cover)
        {
            var last=bottom?lastLower:lastUpper;if(Mathf.Abs(last[eye]-cover)<.001f)return;last[eye]=cover;
            var go=bottom?lowerLidObjects[eye]:upperLidObjects[eye];go.SetActive(cover>.004f);if(!go.activeSelf)return;
            const int columns=36,rows=10;var v=new List<Vector3>();var n=new List<Vector3>();var uv=new List<Vector2>();var t=new List<int>();
            // A closed shell continues behind the eyeball and joins its inner
            // lining at the opening. No exposed back edge remains in profile.
            float limit=Mathf.Acos(1-Mathf.Min(cover,.9995f)*2);int layerSize=1+rows*columns;
            for(int layer=0;layer<2;layer++){
                var radius=layer==0?new Vector3(.174f,.184f,.109f):new Vector3(.168f,.178f,.103f);
                var pole=bottom?Vector3.down:Vector3.up;v.Add(Vector3.Scale(pole,radius));n.Add(layer==0?pole:-pole);uv.Add(new Vector2(.5f,0));
                for(int r=1;r<=rows;r++)for(int i=0;i<columns;i++){
                    float phi=r/(float)rows*limit,theta=i/(float)columns*Mathf.PI*2;
                    var d=new Vector3(Mathf.Sin(theta)*Mathf.Sin(phi),(bottom?-1:1)*Mathf.Cos(phi),-Mathf.Cos(theta)*Mathf.Sin(phi));
                    v.Add(Vector3.Scale(d,radius));var normal=new Vector3(d.x/radius.x,d.y/radius.y,d.z/radius.z).normalized;
                    n.Add(layer==0?normal:-normal);uv.Add(new Vector2(i/(float)columns,phi/Mathf.PI));
                }
                int offset=layer*layerSize;bool reverse=bottom^(layer==1);
                for(int i=0;i<columns;i++){
                    int next=(i+1)%columns;LidTriangle(t,offset,offset+1+next,offset+1+i,reverse);
                    for(int r=0;r<rows-1;r++){
                        int a=offset+1+r*columns+i,a1=offset+1+r*columns+next,b=a+columns,b1=a1+columns;
                        LidTriangle(t,a,a1,b,reverse);LidTriangle(t,a1,b1,b,reverse);
                    }
                }
            }
            for(int i=0;i<columns;i++){
                int next=(i+1)%columns,a=1+(rows-1)*columns+i,a1=1+(rows-1)*columns+next,b=a+layerSize,b1=a1+layerSize;
                LidTriangle(t,a,a1,b,bottom);LidTriangle(t,a1,b1,b,bottom);
            }
            var mesh=bottom?lowerLids[eye]:upperLids[eye];mesh.Clear();mesh.SetVertices(v);mesh.SetTriangles(t,0);mesh.SetNormals(n);mesh.SetUVs(0,uv);mesh.RecalculateBounds();
        }
        static void LidTriangle(List<int> triangles,int a,int b,int c,bool reverse)
        {triangles.Add(a);triangles.Add(reverse?c:b);triangles.Add(reverse?b:c);}
        void BuildFace()
        {
            face=new GameObject("Rosto — cabeça esférica").transform;face.SetParent(Avatar,false);
            skin.mainTexture=Resources.Load<Texture2D>("Painted/Skin");skin.SetFloat("_Motion",0);skin.SetFloat("_Outline",.001f);skin.SetFloat("_SmoothLight",1);
            ink.mainTexture=Texture2D.whiteTexture;ink.SetFloat("_Motion",0);
            Ellipsoid("Cabeça arredondada",new Vector3(0,2.8f,0),Vector3.one*(HeadRadius*2),skin,face,32,16);
            var dark=FaceInk(new Color(.045f,.03f,.03f));var white=FaceInk(new Color(.96f,.94f,.85f));
            for(int i=0;i<2;i++){
                float side=i==0?-1:1;Ellipsoid("Orelha",new Vector3(side*.545f,2.79f,0),new Vector3(.20f,.26f,.18f),skin,face,16,8);
                var pivot=new GameObject("Olho expressivo").transform;pivot.SetParent(face,false);pivot.localPosition=new Vector3(side*.23f,2.88f,-.50f);eyes[i]=pivot;
                Ellipsoid("Branco do olho",Vector3.zero,new Vector3(.34f,.36f,.21f),white,pivot,20,10);
                BuildPupil(i,dark);
                BuildLid(i,false);BuildLid(i,true);
                brows[i]=BuildBrow(i,dark);
                BuildOuterLashes(i,dark);
            }
            Ellipsoid("Nariz",new Vector3(0,2.70f,-.551f),new Vector3(.105f,.13f,.12f),skin,face,16,8);
            smile=Arc("Boca",new Vector3(0,2.50f,0),.20f,0,.02f,dark);mouthLineMesh=smile.GetComponent<MeshFilter>().sharedMesh;
            openMouth=Disc("Boca animada",new Vector3(0,2.51f,0),.28f,.145f,dark,true);
            var teeth=Disc("Dentes",new Vector3(0,2.545f,-.002f),.19f,.03f,white,true);teeth.SetParent(openMouth,true);
            surprisedMouth=Disc("Boca surpresa",new Vector3(0,2.51f,0),.105f,.135f,dark,true);
            foreach(Transform part in face)part.localPosition-=new Vector3(0,2.8f,0);face.localPosition=new Vector3(0,2.8f,0);
            ApplyExpression(AvatarExpression.Neutral);
        }
        public void SetExpression(AvatarExpression value){ExpressionOverride=(int)value;ApplyExpression(value);}
        public void CycleExpression(){SetExpression((AvatarExpression)(((int)expression+1)%6));}
        void ApplyExpression(AvatarExpression value)
        {
            if(!EnsureFaceBindings())return;faceExpressionNeedsRefresh=false;
            expression=value;expressionStart=idle;bool happy=value==AvatarExpression.Happy,surprised=value==AvatarExpression.Surprised;
            smile.gameObject.SetActive(!happy&&!surprised);openMouth.gameObject.SetActive(happy);surprisedMouth.gameObject.SetActive(surprised);
            float curve=value==AvatarExpression.Curious?.018f:value==AvatarExpression.Skeptical?-.012f:value==AvatarExpression.Sleepy?-.008f:0;
            mouthLineMesh.vertices=ArcVertices(new Vector3(0,2.50f,0),value==AvatarExpression.Sleepy?.15f:.20f,curve,.020f,value==AvatarExpression.Skeptical?.014f:0);mouthLineMesh.RecalculateNormals();mouthLineMesh.RecalculateBounds();
            for(int i=0;i<2;i++){
                var lashes=eyes[i].Find("Cílios externos");
                if(lashes!=null)lashes.gameObject.SetActive(feminineAppearance);
                float side=i==0?-1:1;
                float width=surprised?.055f:happy?.095f:value==AvatarExpression.Skeptical?.065f:.08f;
                float height=surprised?.06f:value==AvatarExpression.Sleepy?.055f:.10f;
                UpdatePupil(i,width,height,value==AvatarExpression.Skeptical?.055f:value==AvatarExpression.Curious?.025f:0,value==AvatarExpression.Curious?.025f:value==AvatarExpression.Sleepy?-.065f:0);
                upperCover[i]=value==AvatarExpression.Skeptical?(i==0?.48f:.35f):value==AvatarExpression.Sleepy?.64f:value==AvatarExpression.Curious?(i==0?.07f:.19f):surprised?0:.10f;
                lowerCover[i]=happy?.30f:value==AvatarExpression.Sleepy?.08f:0;
                UpdateBrow(i,value);
                UpdateLid(i,false,upperCover[i]);UpdateLid(i,true,lowerCover[i]);
            }
        }
        void SetFaceProfile(int profile)
        {
            if(!EnsureFaceBindings())return;
            faceProfile=profile;bodyScale=BodyProportion(profile);Avatar.localScale=bodyScale;
            face.localScale=HeadProportion(profile);face.localPosition=new Vector3(0,2.8f+HeadLift(profile),0);
            var neck=Avatar.Find("Pescoço");neck.localPosition=new Vector3(0,2.31f+HeadLift(profile)*.5f,.025f);neck.localScale=new Vector3(.30f,.32f+HeadLift(profile),.29f);
            var defaults=new[]{AvatarExpression.Curious,AvatarExpression.Happy,AvatarExpression.Skeptical,AvatarExpression.Sleepy,AvatarExpression.Neutral,AvatarExpression.Surprised,AvatarExpression.Neutral};
            ApplyExpression(ExpressionOverride>=0?(AvatarExpression)ExpressionOverride:defaults[Mathf.Min(profile,defaults.Length-1)]);
        }
        void AnimateFace()
        {
            if(!EnsureFaceBindings())return;if(faceExpressionNeedsRefresh)ApplyExpression(expression);float phase=Mathf.Repeat(blinkClock+faceProfile*.43f,4.3f);
            float blink=phase<.16f?Mathf.Sin(phase/.16f*Mathf.PI):0;
            for(int i=0;i<2;i++){UpdateLid(i,false,Mathf.Max(upperCover[i],blink));UpdateLid(i,true,lowerCover[i]);pupils[i].gameObject.SetActive(true);}
            AnimateMouth();
        }
        public bool Talking;
        void AnimateMouth()
        {
            if(ReducedMotion||mouthLineMesh==null)return;
            if(expression==AvatarExpression.Surprised){float phase=Mathf.Repeat(idle-expressionStart,8);bool gasp=phase>.05f&&phase<.75f;surprisedMouth.gameObject.SetActive(gasp);smile.gameObject.SetActive(!gasp);surprisedMouth.localScale=new Vector3(1,.35f+.65f*Mathf.Sin(Mathf.Clamp01(phase/.8f)*Mathf.PI),1);}
            float syllable=Talking?Mathf.Pow(Mathf.Max(0,Mathf.Sin(idle*12)*Mathf.Sin(idle*3.7f)),1.4f):Mathf.Pow(Mathf.Max(0,Mathf.Sin(idle*.7f+faceProfile)),10)*.12f;
            float curve=expression==AvatarExpression.Curious?.018f:expression==AvatarExpression.Skeptical?-.012f:expression==AvatarExpression.Sleepy?-.008f:0;
            var vertices=ArcVertices(new Vector3(0,2.50f,0),.20f,curve,.02f+syllable*.05f,expression==AvatarExpression.Skeptical?.014f:0);
            mouthLineMesh.vertices=vertices;mouthLineMesh.RecalculateNormals();mouthLineMesh.RecalculateBounds();
            if(openMouth!=null)openMouth.localScale=new Vector3(1,1+syllable*.15f,1);
        }
    }
}
