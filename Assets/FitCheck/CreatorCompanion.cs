using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        bool creatorPetTab;ShopCat creatorPetPreview;float creatorNormalFov=45;
        void RefreshCreatorPet(){
            if(creatorDraft.pet==null)creatorDraft.pet=new PetAppearance();
            if(creatorPetPreview!=null){creatorPetPreview.gameObject.SetActive(false);Destroy(creatorPetPreview.gameObject);}
            var root=new GameObject("Prévia do companheiro");root.transform.SetParent(studio.transform,false);root.transform.localPosition=new Vector3(0,.14f,0);root.transform.localScale=Vector3.one*1.15f;
            creatorPetPreview=root.AddComponent<ShopCat>();var choice=creatorDraft.pet.Copy();if(string.IsNullOrWhiteSpace(choice.name))choice.name="Companheiro";creatorPetPreview.Build(choice);creatorPetPreview.gameObject.SetActive(creatorPetTab);FrameCreatorPet();
        }
        void FrameCreatorPet(){
            mannequin.gameObject.SetActive(!creatorPetTab);
            if(creatorPetPreview!=null)creatorPetPreview.gameObject.SetActive(creatorPetTab);
            if(!creatorPetTab){cam.fieldOfView=creatorNormalFov;return;}
            if(creatorPetPreview==null){RefreshCreatorPet();return;}
            creatorPetPreview.transform.localRotation=Quaternion.Euler(0,rotation,0);cam.fieldOfView=48;
            cam.transform.position=studio.transform.position+new Vector3(.15f,1.1f,-Mathf.Clamp(distance*.48f,2.6f,3.95f));cam.transform.LookAt(studio.transform.position+new Vector3(0,.48f,0));
        }
        void ClearCreatorPet(){if(creatorPetPreview!=null){creatorPetPreview.gameObject.SetActive(false);Destroy(creatorPetPreview.gameObject);}creatorPetPreview=null;creatorPetTab=false;mannequin.gameObject.SetActive(true);cam.fieldOfView=creatorNormalFov;}
        void CreatorPetUI(){
            var pet=creatorDraft.pet;
            Fill(new Rect(0,88,375,812),KaosPanel);Fill(new Rect(1050,88,390,812),KaosPanel);
            Txt(new Rect(28,118,320,28),"SEU COMPANHEIRO",tiny,CreatorRed);Txt(new Rect(28,160,320,90),"Quem vai\ncom você?",heading);
            Txt(new Rect(28,264,320,28),"Nome do pet",caption);pet.name=GUI.TextField(new Rect(28,304,320,40),pet.name,20,nameEntry);
            Txt(new Rect(28,378,320,28),"Gato ou cachorro?",caption);
            for(int i=0;i<2;i++)if(CreatorButton(new Rect(28+i*165,420,155,48),i==0?"Gato":"Cachorro",pet.species==i)){pet.species=i;pet.breed=0;RefreshCreatorPet();}
            Txt(new Rect(28,500,320,28),"Raça / tipo",caption);
            for(int i=0;i<pet.Breeds.Length;i++)if(CreatorButton(new Rect(28,542+i*58,320,48),pet.Breeds[i],pet.breed==i)){pet.breed=i;RefreshCreatorPet();}
            Txt(new Rect(1074,126,330,32),"Pelagem",caption);
            for(int i=0;i<PetAppearance.Coats.Length;i++){
                var r=new Rect(1074+i%3*110,178+i/3*58,100,48);Fill(r,pet.coat==i?CreatorRed:line);Fill(new Rect(r.x+4,r.y+4,r.width-8,r.height-8),PetAppearance.Coats[i]);
                if(GUI.Button(r,GUIContent.none,GUIStyle.none)){pet.coat=i;RefreshCreatorPet();Sound(0);}
            }
            Txt(new Rect(1074,319,330,30),PetAppearance.CoatNames[pet.coat],caption,paper);
            Txt(new Rect(1074,402,330,160),"Seu companheiro descansa em casa e na loja.\n\nO Fubá continua sendo o gato do bairro.",copy,muted);
            if(CreatorButton(new Rect(1074,809,330,58),"Salvar personagem",true,!string.IsNullOrWhiteSpace(pet.name)&&!string.IsNullOrWhiteSpace(creatorDraft.name)))BeginLoading(()=>{SaveCreator();});
            if(CreatorButton(new Rect(28,808,320,54),"Voltar sem salvar"))CancelCreator();
            if(CreatorButton(new Rect(420,807,200,45),"↶ Girar")){rotation-=45;UpdateCamera();}
            if(CreatorButton(new Rect(640,807,200,45),"Girar ↷")){rotation+=45;UpdateCamera();}
            if(CreatorButton(new Rect(860,807,145,45),"Restaurar")){rotation=-15;distance=6.8f;UpdateCamera();}
            Txt(new Rect(422,862,580,28),message.Length>0?message:"Arraste para girar  /  Role para aproximar",tiny,muted);ProcessCreatorPointer(Event.current);
        }
    }
}
