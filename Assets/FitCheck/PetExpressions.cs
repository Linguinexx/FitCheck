using UnityEngine;
namespace FitCheck
{
    public sealed partial class ShopCat
    {
        Transform[] openPetEyes,closedPetEyes,petEars,frontPaws;
        Vector3[] petPawRest;Quaternion[] petEarRest;
        float awakeTime;Vector3 bodyRestScale=Vector3.one;bool hairlessCat;
        public float EyeOpen {get;private set;}
        public Color EyeColor {get;private set;}
        void BuildPetExpression(bool dog){
            bodyRestScale=body.localScale;awakeTime=2.8f;
            var closed=new System.Collections.Generic.List<Transform>();var ears=new System.Collections.Generic.List<Transform>();var paws=new System.Collections.Generic.List<Transform>();
            foreach(var item in GetComponentsInChildren<MeshRenderer>()){
                if(item.name=="Olho fechado")closed.Add(item.transform);
                if(item.name=="Orelha triangular"||item.name=="Orelha caída")ears.Add(item.transform);
                if(item.name.StartsWith("Patinha dianteira"))paws.Add(item.transform);
            }
            closedPetEyes=closed.ToArray();petEars=ears.ToArray();frontPaws=paws.ToArray();petPawRest=new Vector3[frontPaws.Length];petEarRest=new Quaternion[petEars.Length];
            for(int i=0;i<frontPaws.Length;i++)petPawRest[i]=frontPaws[i].localPosition;for(int i=0;i<petEars.Length;i++)petEarRest[i]=petEars[i].localRotation;
            var look=IsFuba?null:JsonUtility.FromJson<PetAppearance>(ConfigurationKey);
            EyeColor=dog?new Color(.38f,.20f,.085f):look!=null&&(look.coat==4||look.breed==2)?new Color(.12f,.53f,.97f):look!=null&&look.coat==1?new Color(.32f,.75f,.34f):new Color(.78f,.56f,.13f);
            var iris=Paint(EyeColor,false);var shine=Paint(new Color(.99f,.98f,.91f),false);openPetEyes=new Transform[2];
            for(int i=0;i<2;i++){
                float side=i==0?-1:1;var root=new GameObject("Olho acordado").transform;root.SetParent(head,false);root.localPosition=new Vector3(side*.128f,.032f,0);openPetEyes[i]=root;
                PetEyePatch("Íris",.063f,.038f,Vector2.zero,.005f,iris,root,dog);
                PetEyePatch("Pupila",dog?.020f:.012f,.032f,Vector2.zero,.007f,dark,root,dog);
                PetEyePatch("Brilho do olho",.006f,.006f,new Vector2(-.014f,.016f),.010f,shine,root,dog);
            }
        }
        void PetEyePatch(string name,float width,float height,Vector2 offset,float depth,Material paint,Transform parent,bool dog){
            const int sides=20;var points=new Vector3[sides+1];var faces=new int[sides*3];
            for(int i=0;i<=sides;i++){float angle=(i-1)*Mathf.PI*2/sides;float x=offset.x+(i==0?0:Mathf.Cos(angle)*width),y=offset.y+(i==0?0:Mathf.Sin(angle)*height);float gx=parent.localPosition.x+x,gy=parent.localPosition.y+y;float z=-(dog?.26f:.245f)*Mathf.Sqrt(Mathf.Max(.02f,1-gx*gx/(dog?.070225f:.0729f)-gy*gy/(dog?.060025f:.055225f)))-depth;points[i]=new Vector3(x,y,z);if(i>0){faces[(i-1)*3]=0;faces[(i-1)*3+1]=i==sides?1:i+1;faces[(i-1)*3+2]=i;}}
            MeshPart(name,points,faces,paint,parent);
        }
        void AnimatePetExpression(float delta,bool reduced){
            awakeTime=Mathf.Max(0,awakeTime-delta);
            float cycle=Mathf.Repeat(age,18),peek=cycle>10&&cycle<14?Mathf.Sin((cycle-10)/4*Mathf.PI):0;
            float awake=Mathf.Max(Mathf.Clamp01(awakeTime),peek);float blink=Mathf.Repeat(age,3.7f);if(blink<.16f)awake*=Mathf.Abs(blink-.08f)/.08f;
            EyeOpen=Mathf.MoveTowards(EyeOpen,awake,delta*5);
            for(int i=0;i<openPetEyes.Length;i++){openPetEyes[i].gameObject.SetActive(EyeOpen>.035f);openPetEyes[i].localScale=new Vector3(1,Mathf.Max(.035f,EyeOpen),1);}
            foreach(var lid in closedPetEyes)lid.gameObject.SetActive(EyeOpen<.16f);
            if(reduced){head.localRotation=Quaternion.identity;for(int i=0;i<petEars.Length;i++)petEars[i].localRotation=petEarRest[i];for(int i=0;i<frontPaws.Length;i++)frontPaws[i].localPosition=petPawRest[i];return;}
            head.localPosition+=Vector3.up*(awake*.055f);head.localRotation=Quaternion.Euler(-awake*5,Mathf.Sin(age*.8f)*awake*7,Mathf.Sin(age*.55f)*awake*2);
            for(int i=0;i<petEars.Length;i++)petEars[i].localRotation=petEarRest[i]*Quaternion.Euler(0,0,Mathf.Sin(age*4+i)*awake*3);
            for(int i=0;i<frontPaws.Length;i++)frontPaws[i].localPosition=petPawRest[i]+new Vector3(-.025f*awake,0,-.025f*awake);
        }
        void BuildSoftEar(int side){
            float width=hairlessCat?.135f:.11f,height=hairlessCat?.29f:.21f;
            var center=new Vector3(side*.145f,.105f,.012f);
            var shape=new[]{new Vector2(-1,0),new Vector2(-.95f,.27f),new Vector2(-.66f,.67f),new Vector2(-.24f,.96f),new Vector2(.18f,1),new Vector2(.67f,.67f),new Vector2(.96f,.27f),new Vector2(1,0)};
            var vertices=new Vector3[18];var triangles=new System.Collections.Generic.List<int>();
            for(int i=0;i<8;i++){float rootWidth=Mathf.Lerp(.075f,width,Mathf.Clamp01(shape[i].y/.32f));var p=new Vector3(shape[i].x*rootWidth+side*shape[i].y*.025f,shape[i].y*height,-.045f);vertices[i]=p;vertices[i+8]=p+Vector3.forward*.085f;}
            vertices[16]=new Vector3(0,height*.36f,-.014f);vertices[17]=vertices[16]+Vector3.forward*.06f;
            for(int i=0;i<8;i++){int next=(i+1)%8;triangles.AddRange(new[]{16,i,next,17,next+8,i+8,i,next+8,next,i,i+8,next+8});}
            var ear=MeshPart("Orelha triangular",vertices,triangles.ToArray(),fur,head);ear.localPosition=center;
            var inner=new Vector3[9];inner[8]=vertices[16]-Vector3.forward*.014f;
            var faces=new System.Collections.Generic.List<int>();for(int i=0;i<8;i++){inner[i]=Vector3.Lerp(vertices[16],vertices[i],.72f)-Vector3.forward*.008f;faces.AddRange(new[]{8,i,(i+1)%8});}
            MeshPart("Interior da orelha",inner,faces.ToArray(),pink,ear);
        }
    }
}
