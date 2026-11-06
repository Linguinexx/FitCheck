using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public sealed class ShopCat : MonoBehaviour
    {
        readonly List<Object> owned=new List<Object>();
        Transform body,head;AudioSource voice;float age,response,cooldown;bool wasPaused;
        public int MeowCount {get;private set;}
        public Vector3 PetTarget=>head.TransformPoint(new Vector3(0,.02f,-.10f));
        public float Age=>age;
        public AudioClip MeowClip=>voice.clip;
        public bool Ready=>voice!=null&&body!=null&&head!=null&&owned.Count>0;
        Material fur,cream,dark,pink;
        Material Paint(Color color,bool grain=true,bool tabby=false)
        {
            var m=new Material(Resources.Load<Shader>("CatFur")){color=color};m.mainTexture=grain?Resources.Load<Texture2D>("Surfaces/Fabric084"):Texture2D.whiteTexture;
            m.SetFloat("_FurMean",.518f);m.SetFloat("_TextureWeight",grain?.55f:0);m.SetFloat("_StripeWeight",tabby?1:0);owned.Add(m);return m;
        }
        public void Build()
        {
            fur=Paint(new Color(.78f,.39f,.14f),true,true);cream=Paint(new Color(.96f,.87f,.70f));dark=Paint(new Color(.20f,.105f,.055f),false);pink=Paint(new Color(.65f,.32f,.27f),false);
            Form("Sombra de contato",new Vector3(0,.006f,.05f),new Vector3(1.22f,.012f,.77f),dark,transform);
            body=Form("Corpo — gatinho deitado",new Vector3(.06f,.31f,.07f),new Vector3(1.18f,.60f,.74f),fur,transform);
            head=new GameObject("Cabeça sonolenta").transform;head.SetParent(transform,false);head.localPosition=new Vector3(-.43f,.36f,-.14f);
            Form("Cabeça arredondada",Vector3.zero,new Vector3(.54f,.47f,.49f),fur,head);
            foreach(int side in new[]{-1,1}){
                Ear(side);
                Form("Focinho creme",new Vector3(side*.055f,-.08f,-.232f),new Vector3(.155f,.105f,.09f),cream,head);
                var lid=new Vector3[6];for(int i=0;i<lid.Length;i++){
                    float f=i/(float)(lid.Length-1),x=side*.128f+(f-.5f)*.115f,y=.04f-.020f*Mathf.Sin(f*Mathf.PI);
                    float z=-.245f*Mathf.Sqrt(Mathf.Max(.01f,1-x*x/(.27f*.27f)-y*y/(.235f*.235f)))-.007f;lid[i]=new Vector3(x,y,z);
                }
                Curve("Olho fechado",lid,.010f,dark,head);
                for(int whisker=0;whisker<2;whisker++)Curve("Bigodes",new[]{new Vector3(side*.10f,-.08f+whisker*.018f,-.282f),new Vector3(side*.25f,-.07f+whisker*.07f,-.28f),new Vector3(side*.32f,-.07f+whisker*.10f,-.25f)},.004f,cream,head);
            }
            MeshPart("Nariz",new[]{new Vector3(-.034f,-.041f,-.285f),new Vector3(.034f,-.041f,-.285f),new Vector3(0,-.071f,-.29f)},new[]{0,1,2},pink,head);
            Curve("Boquinha",new[]{new Vector3(-.034f,-.113f,-.285f),new Vector3(0,-.098f,-.29f),new Vector3(.034f,-.113f,-.285f)},.006f,dark,head);
            Form("Patinha dianteira",new Vector3(-.46f,.058f,-.24f),new Vector3(.23f,.11f,.23f),cream,transform);
            Form("Patinha recolhida",new Vector3(-.20f,.058f,-.25f),new Vector3(.22f,.11f,.22f),cream,transform);
            var tail=new Vector3[22];tail[0]=new Vector3(.48f,.24f,.17f);tail[1]=new Vector3(.64f,.15f,.18f);
            for(int i=2;i<tail.Length;i++){float a=Mathf.Lerp(.25f,-3f,(i-2)/(float)(tail.Length-3));tail[i]=new Vector3(.15f+Mathf.Cos(a)*.55f,.105f,.06f+Mathf.Sin(a)*.38f);}
            Curve("Cauda enrolada",tail,.080f,fur,transform,.035f);
            voice=gameObject.AddComponent<AudioSource>();voice.playOnAwake=false;voice.clip=Resources.Load<AudioClip>("Audio/CatMeow");voice.pitch=1;voice.dopplerLevel=0;voice.spatialBlend=.65f;voice.minDistance=2;voice.maxDistance=10;voice.rolloffMode=AudioRolloffMode.Linear;voice.volume=.60f;
        }
        void Ear(int side)
        {
            var center=new Vector3(side*.19f,.18f,.01f);
            var a=center+new Vector3(-.11f,0,-.11f);var b=center+new Vector3(.11f,0,-.11f);var tip=center+new Vector3(side*.035f,.21f,0);var back=center+new Vector3(0,.015f,.13f);
            MeshPart("Orelha triangular",new[]{a,b,tip,back},new[]{0,2,1,0,3,2,1,2,3,0,1,3},fur,head);
            var c=(a+b+tip)/3;MeshPart("Interior da orelha",new[]{Vector3.Lerp(c,a,.65f)-Vector3.forward*.007f,Vector3.Lerp(c,b,.65f)-Vector3.forward*.007f,Vector3.Lerp(c,tip,.65f)-Vector3.forward*.007f},new[]{0,2,1},pink,head);
        }
        Transform Form(string name,Vector3 p,Vector3 size,Material material,Transform parent)
        {
            const int sides=16,rows=10;var v=new List<Vector3>();var uv=new List<Vector2>();var t=new List<int>();
            for(int r=0;r<=rows;r++)for(int i=0;i<=sides;i++){float a=i*Mathf.PI*2/sides,phi=r*Mathf.PI/rows;v.Add(Vector3.Scale(new Vector3(Mathf.Cos(a)*Mathf.Sin(phi),Mathf.Cos(phi),Mathf.Sin(a)*Mathf.Sin(phi)),size*.5f));uv.Add(new Vector2(i/(float)sides,r/(float)rows));}
            for(int r=0;r<rows;r++)for(int i=0;i<sides;i++){int a=r*(sides+1)+i,b=a+sides+1;t.AddRange(new[]{a,a+1,b,a+1,b+1,b});}
            var part=MeshPart(name,v.ToArray(),t.ToArray(),material,parent,uv.ToArray());part.localPosition=p;return part;
        }
        Transform MeshPart(string name,Vector3[] v,int[] t,Material material,Transform parent,Vector2[] uv=null)
        {
            var mesh=new Mesh{name=name};mesh.vertices=v;mesh.triangles=t;mesh.uv=uv??new Vector2[v.Length];mesh.RecalculateNormals();mesh.RecalculateBounds();owned.Add(mesh);
            var part=new GameObject(name).transform;part.SetParent(parent,false);part.gameObject.AddComponent<MeshFilter>().sharedMesh=mesh;part.gameObject.AddComponent<MeshRenderer>().sharedMaterial=material;return part;
        }
        void Curve(string name,Vector3[] line,float radius,Material material,Transform parent,float endRadius=-1)
        {
            const int sides=7;var v=new List<Vector3>();var uv=new List<Vector2>();var t=new List<int>();
            for(int r=0;r<line.Length;r++){
                var tangent=(line[Mathf.Min(r+1,line.Length-1)]-line[Mathf.Max(0,r-1)]).normalized;var x=Vector3.Cross(tangent,Vector3.up).normalized;if(x.sqrMagnitude<.1f)x=Vector3.right;var y=Vector3.Cross(tangent,x).normalized;
                float width=endRadius>0?Mathf.Lerp(radius,endRadius,r/(float)(line.Length-1)):radius;
                for(int i=0;i<sides;i++){float a=i*Mathf.PI*2/sides;v.Add(line[r]+(x*Mathf.Cos(a)+y*Mathf.Sin(a))*width);uv.Add(new Vector2(i/(float)sides,r/(float)(line.Length-1)));}
            }
            for(int r=0;r<line.Length-1;r++)for(int i=0;i<sides;i++){int a=r*sides+i,b=r*sides+(i+1)%sides;t.AddRange(new[]{a,b,a+sides,b,b+sides,a+sides});}
            for(int cap=0;cap<2;cap++){int row=cap==0?0:line.Length-1,center=v.Count;v.Add(line[row]);uv.Add(new Vector2(.5f,cap));for(int i=0;i<sides;i++){int a=row*sides+i,b=row*sides+(i+1)%sides;t.AddRange(cap==0?new[]{center,b,a}:new[]{center,a,b});}}
            MeshPart(name,v.ToArray(),t.ToArray(),material,parent,uv.ToArray());
        }
        public bool CanPet(Camera camera)
        {var d=PetTarget-camera.transform.position;return d.sqrMagnitude<3.6f*3.6f&&Vector3.Dot(camera.transform.forward,d.normalized)>.93f;}
        public bool Pet(bool muted)
        {if(cooldown>0||voice.clip==null)return false;voice.mute=muted;voice.Play();response=voice.clip.length;cooldown=voice.clip.length+.35f;MeowCount++;return true;}
        public void Tick(float delta,bool paused,bool reduced,bool muted)
        {
            voice.mute=muted;if(paused!=wasPaused){if(paused)voice.Pause();else voice.UnPause();wasPaused=paused;}if(paused)return;
            age+=delta;cooldown=Mathf.Max(0,cooldown-delta);response=Mathf.Max(0,response-delta);
            float breath=reduced?0:Mathf.Sin(age*1.8f)*.018f;body.localScale=new Vector3(1,1+breath,1);
            head.localPosition=new Vector3(-.43f,.36f+(reduced?0:Mathf.Sin(Mathf.Clamp01(response/Mathf.Max(.1f,voice.clip.length))*Mathf.PI)*.025f),-.14f);
        }
        void OnDisable(){if(voice!=null)voice.Stop();}
        void OnDestroy(){foreach(var value in owned)if(value!=null)Destroy(value);}
    }
    public partial class CityView
    {
        [SerializeField] ShopCat cat;
        public ShopCat Cat=>cat;
        public void BuildCat(Vector3 position,float yaw)
        {var root=new GameObject("Fubá — gato da casa e da DripLab");root.transform.SetParent(transform,false);root.transform.localPosition=position;root.transform.localRotation=Quaternion.Euler(0,yaw,0);cat=root.AddComponent<ShopCat>();cat.Build();}
        public bool CatInteraction(Camera camera)=>cat!=null&&cat.gameObject.activeInHierarchy&&cat.CanPet(camera);
        public void TickCat(float delta,bool paused,bool reduced,bool muted){if(cat!=null)cat.Tick(delta,paused,reduced,muted);}
    }
    public partial class FirstCustomer
    {
        bool TryPetCat()
        {
            if(paused||stage!=Stage.City||ShopSequenceActive||homeDoorExitRemaining>0||!city.CatInteraction(cam))return false;
            if(city.Cat.Pet(mutedAudio)&&progress.RegisterFubaPet()){
                if(progress.UniformUnlocked(DayProgress.FubaUniform))notices.Enqueue(DayProgress.FubaAward+" · "+FubaUniformName);
                TrySave();
            }
            return true;
        }
    }
}
