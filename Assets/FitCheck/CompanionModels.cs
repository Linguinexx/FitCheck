using UnityEngine;
namespace FitCheck
{
    public sealed partial class ShopCat
    {
        void CompletePetVoice(bool dog){
            if(InkOutlinePreview.GameplayEnabled)InkOutlinePreview.Apply(transform);
            BuildPetExpression(dog);headRest=head.localPosition;voice=gameObject.AddComponent<AudioSource>();voice.playOnAwake=false;voice.clip=Resources.Load<AudioClip>(dog?"Audio/PetBark":"Audio/CatMeow");voice.pitch=dog?(body.localScale.x>1.3f?.9f:1.04f):1;voice.dopplerLevel=0;voice.spatialBlend=.65f;voice.minDistance=2;voice.maxDistance=10;voice.rolloffMode=AudioRolloffMode.Linear;voice.volume=.60f;
            if(!dog){
                if(!IsFuba)meowVariants=new[]{Resources.Load<AudioClip>("Audio/Game/cat_meow_alt_01"),Resources.Load<AudioClip>("Audio/Game/cat_meow_alt_02")};
                purr=gameObject.AddComponent<AudioSource>();purr.playOnAwake=false;purr.clip=Resources.Load<AudioClip>("Audio/Game/cat_purr");purr.volume=.16f;purr.spatialBlend=1;purr.minDistance=.7f;purr.maxDistance=4;purr.rolloffMode=AudioRolloffMode.Linear;purr.dopplerLevel=0;
            }
        }
        void BuildDog(PetAppearance choice){
            bool sausage=choice.breed==2,pinscher=choice.breed==3,shih=choice.breed==4,golden=choice.breed==5;
            float length=sausage?1.48f:pinscher?.91f:shih?1.01f:golden?1.38f:1.14f;
            Form("Sombra de contato",new Vector3(0,.008f,0),new Vector3(length+.10f,.014f,.85f),dark,transform);
            body=Form("Corpo — cachorro deitado",new Vector3(.08f,.28f,.08f),new Vector3(length,pinscher?.40f:golden?.60f:.52f,pinscher?.53f:golden?.77f:.69f),fur,transform);
            head=new GameObject("Cabeça sonolenta").transform;head.SetParent(transform,false);head.localPosition=new Vector3(sausage?-.61f:pinscher?-.34f:golden?-.53f:-.43f,pinscher?.33f:golden?.44f:.38f,-.15f);
            if(pinscher)head.localScale=Vector3.one*.88f;
            if(golden)head.localScale=new Vector3(1.10f,1.04f,1.08f);
            Form("Cabeça do cachorro",Vector3.zero,new Vector3(.53f,.49f,.52f),fur,head);
            float muzzle=sausage?-.31f:pinscher?-.30f:shih?-.235f:golden?-.30f:-.27f;
            float nose=sausage?-.493f:pinscher?-.44f:shih?-.32f:golden?-.45f:-.393f;
            var tan=pinscher?Paint(new Color(.66f,.36f,.17f)):fur;
            Form("Focinho",new Vector3(0,-.09f,muzzle),new Vector3(shih?.35f:pinscher?.24f:.33f,shih?.18f:.22f,sausage?.36f:pinscher?.29f:shih?.16f:golden?.32f:.26f),choice.breed==1||shih?cream:tan,head);
            Form("Nariz úmido",new Vector3(0,-.06f,nose),new Vector3(pinscher?.115f:.15f,.095f,.075f),dark,head);
            foreach(int side in new[]{-1,1}){
                if(choice.breed==1||pinscher)Ear(side);
                else Form("Orelha caída",new Vector3(side*(golden?.26f:.247f),-.045f,.012f),new Vector3(shih?.23f:golden?.22f:.165f,sausage?.42f:golden?.42f:shih?.39f:.34f,.245f),choice.coat==4?dark:fur,head);
                Curve("Olho fechado",new[]{new Vector3(side*.18f,.036f,-.237f),new Vector3(side*.127f,.015f,-.263f),new Vector3(side*.075f,.035f,-.249f)},.009f,choice.coat==1?cream:dark,head);
                Form("Patinha dianteira",new Vector3(pinscher?-.37f:golden?-.57f:-.47f,.067f,side*(pinscher?.185f:.235f)),new Vector3(golden?.35f:pinscher?.22f:.28f,.13f,pinscher?.17f:.23f),pinscher?tan:choice.breed==1||choice.coat==4||shih?cream:fur,transform);
                Form("Patinha traseira",new Vector3(sausage?.57f:golden?.55f:pinscher?.34f:.43f,.07f,side*(pinscher?.19f:.24f)),new Vector3(pinscher?.20f:.26f,.135f,pinscher?.18f:.24f),pinscher?tan:fur,transform);
                if(pinscher)Form("Sobrancelha caramelo",new Vector3(side*.13f,.09f,-.229f),new Vector3(.09f,.045f,.025f),tan,head);
                if(shih){
                    Form("Bochecha peluda",new Vector3(side*.18f,-.105f,-.19f),new Vector3(.22f,.22f,.25f),cream,head);
                    Form("Bigode do Shih-tzu",new Vector3(side*.09f,-.12f,-.29f),new Vector3(.20f,.14f,.10f),cream,head);
                    for(int tuft=0;tuft<3;tuft++)Form("Mecha da orelha",new Vector3(side*(.25f+tuft*.022f),-.11f+tuft*.07f,-.025f),new Vector3(.13f,.20f,.19f),fur,head);
                }
                if(golden)for(int tuft=0;tuft<3;tuft++)Form("Franja de pelagem Golden",new Vector3(-.28f+tuft*.27f,.17f,side*.32f),new Vector3(.35f,.22f,.20f),fur,transform);
            }
            Curve("Boca do cachorro",new[]{new Vector3(-.075f,-.143f,nose+.015f),new Vector3(0,-.158f,nose-.005f),new Vector3(.075f,-.143f,nose+.015f)},.006f,dark,head);
            if(shih){Form("Franja repartida",new Vector3(0,.18f,-.065f),new Vector3(.34f,.19f,.30f),fur,head);Form("Barbinha do Shih-tzu",new Vector3(0,-.20f,-.215f),new Vector3(.32f,.17f,.20f),cream,head);}
            if(golden)Form("Juba do Golden",new Vector3(-.30f,.31f,-.04f),new Vector3(.55f,.61f,.63f),fur,transform);
            if(pinscher)Form("Peito caramelo",new Vector3(-.20f,.23f,-.16f),new Vector3(.29f,.28f,.32f),tan,transform);
            if(choice.breed==1){var tail=new Vector3[18];for(int i=0;i<tail.Length;i++){float a=i/(float)(tail.Length-1)*Mathf.PI*1.7f;tail[i]=new Vector3(.58f+Mathf.Cos(a)*.13f,.34f+Mathf.Sin(a)*.15f,.15f);}Curve("Cauda enrolada",tail,.065f,fur,transform,.04f);}
            else Curve("Cauda do cachorro",new[]{new Vector3(length*.45f,.27f,.13f),new Vector3(length*.59f,.20f,.16f),new Vector3(length*.63f,.115f,.23f)},golden?.10f:shih?.075f:.053f,fur,transform,golden?.06f:.026f);
            if(shih)Form("Cauda pompom",new Vector3(.59f,.31f,.13f),new Vector3(.30f,.24f,.29f),cream,transform);
            if(pinscher)transform.localScale*=.87f;
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
