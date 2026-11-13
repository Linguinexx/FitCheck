using UnityEngine;
namespace FitCheck
{
    public sealed partial class ShopCat
    {
        void CompletePetVoice(bool dog){
            headRest=head.localPosition;voice=gameObject.AddComponent<AudioSource>();voice.playOnAwake=false;voice.clip=Resources.Load<AudioClip>(dog?"Audio/PetBark":"Audio/CatMeow");voice.pitch=dog?(body.localScale.x>1.3f?.9f:1.04f):1;voice.dopplerLevel=0;voice.spatialBlend=.65f;voice.minDistance=2;voice.maxDistance=10;voice.rolloffMode=AudioRolloffMode.Linear;voice.volume=.60f;
            if(!dog){
                if(!IsFuba)meowVariants=new[]{Resources.Load<AudioClip>("Audio/Game/cat_meow_alt_01"),Resources.Load<AudioClip>("Audio/Game/cat_meow_alt_02")};
                purr=gameObject.AddComponent<AudioSource>();purr.playOnAwake=false;purr.clip=Resources.Load<AudioClip>("Audio/Game/cat_purr");purr.volume=.16f;purr.spatialBlend=1;purr.minDistance=.7f;purr.maxDistance=4;purr.rolloffMode=AudioRolloffMode.Linear;purr.dopplerLevel=0;
            }
        }
        void BuildDog(PetAppearance choice){
            bool sausage=choice.breed==2;float length=sausage?1.48f:1.14f;
            Form("Sombra de contato",new Vector3(0,.008f,0),new Vector3(length+.10f,.014f,.85f),dark,transform);
            body=Form("Corpo — cachorro deitado",new Vector3(.08f,.28f,.08f),new Vector3(length,.52f,.69f),fur,transform);
            head=new GameObject("Cabeça sonolenta").transform;head.SetParent(transform,false);head.localPosition=new Vector3(sausage?-.61f:-.43f,.38f,-.15f);
            Form("Cabeça do cachorro",Vector3.zero,new Vector3(.53f,.49f,.52f),fur,head);
            Form("Focinho",new Vector3(0,-.09f,sausage?-.31f:-.27f),new Vector3(.33f,.22f,sausage?.36f:.26f),choice.breed==1?cream:fur,head);
            Form("Nariz úmido",new Vector3(0,-.06f,sausage?-.493f:-.393f),new Vector3(.15f,.095f,.075f),dark,head);
            foreach(int side in new[]{-1,1}){
                if(choice.breed==1)Ear(side);
                else Form("Orelha caída",new Vector3(side*.247f,-.045f,.012f),new Vector3(.165f,sausage?.42f:.34f,.245f),choice.coat==4?dark:fur,head);
                Curve("Olho fechado",new[]{new Vector3(side*.18f,.036f,-.237f),new Vector3(side*.127f,.015f,-.263f),new Vector3(side*.075f,.035f,-.249f)},.009f,choice.coat==1?cream:dark,head);
                Form("Patinha dianteira",new Vector3(-.47f,.067f,side*.235f),new Vector3(.28f,.13f,.23f),choice.breed==1||choice.coat==4?cream:fur,transform);
                Form("Patinha traseira",new Vector3(sausage?.57f:.43f,.07f,side*.24f),new Vector3(.26f,.135f,.24f),fur,transform);
            }
            Curve("Boca do cachorro",new[]{new Vector3(-.075f,-.143f,-.382f),new Vector3(0,-.158f,-.40f),new Vector3(.075f,-.143f,-.382f)},.006f,dark,head);
            if(choice.breed==1){var tail=new Vector3[18];for(int i=0;i<tail.Length;i++){float a=i/(float)(tail.Length-1)*Mathf.PI*1.7f;tail[i]=new Vector3(.58f+Mathf.Cos(a)*.13f,.34f+Mathf.Sin(a)*.15f,.15f);}Curve("Cauda enrolada",tail,.065f,fur,transform,.04f);}
            else Curve("Cauda do cachorro",new[]{new Vector3(length*.45f,.27f,.13f),new Vector3(length*.59f,.20f,.16f),new Vector3(length*.63f,.115f,.23f)},.053f,fur,transform,.026f);
            if(choice.coat==4)Form("Mancha bicolor",new Vector3(.16f,.44f,.08f),new Vector3(length*.59f,.24f,.55f),dark,transform);
        }
    }
    public partial class CityView
    {
        public Vector2 FubaStreetPosition=>new Vector2(cat.transform.localPosition.x,cat.transform.localPosition.z);
        void BuildStreetFuba(){
            Vector2 position=new Vector2(-11.5f,2.5f);bool found=false;
            foreach(var candidate in new[]{position,new Vector2(-11.5f,.8f),new Vector2(-8,-1.7f),new Vector2(-25,-7),new Vector2(-29,-4)})if(WorldFree(candidate,.65f)){position=candidate;found=true;break;}
            if(!found)throw new System.InvalidOperationException("Sem espaço livre para o canto de Fubá");
            Block(new Vector3(position.x,.23f,position.y),new Vector3(1.6f,.46f,1.10f),new Color(.43f,.24f,.13f),"Wood",.05f);
            Ground(position.x,position.y,1.44f,.98f,.468f,new Color(.43f,.09f,.12f),"Rug");
            obstacles.Add(new Rect(position.x-.85f,position.y-.6f,1.7f,1.2f));FinishGeometry();BuildCat(new Vector3(position.x,.48f,position.y),160);
        }
    }
}
