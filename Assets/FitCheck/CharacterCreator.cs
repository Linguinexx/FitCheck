using System;
using System.IO;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        PlayerAppearance playerAppearance=new PlayerAppearance(),creatorDraft;
        Stage creatorReturn,creatorDestination;
        string AppearancePath=>Path.Combine(SessionFolder,"player-appearance.json");
        static readonly Color CreatorRed=new Color(227/255f,6/255f,19/255f);
        void LoadAppearance()
        {try{playerAppearance=AppearanceStore.Load(AppearancePath);}catch{playerAppearance=new PlayerAppearance();message="Não foi possível ler a aparência. Você pode criar outra.";}}
        void ShowPlayer(){studio.PlayerLook=playerAppearance;studio.ApplyUniform(progress.uniform);}
        void OpenCreator(Stage destination=Stage.Welcome)
        {
            bool wasHome=stage==Stage.Home||(stage==Stage.City&&city.IsHome);
            if(stage==Stage.City||(stage==Stage.Home&&city!=null&&city.IsHome&&city.gameObject.activeSelf))ExitCity();
            creatorReturn=wasHome?Stage.Home:Stage.Welcome;creatorDestination=destination;
            creatorDraft=playerAppearance.Copy();studio.PlayerLook=creatorDraft;studio.SetHome(false);
            stage=Stage.CharacterCreator;message="";paused=false;dragging=false;rotation=-15;distance=6.8f;
            studio.ApplyUniform(progress.uniform);UpdateCamera();
        }
        void RefreshCreator(){studio.PlayerLook=creatorDraft;studio.ApplyUniform(progress.uniform);}
        bool SaveCreator()
        {
            var saved=creatorDraft.Copy();saved.name=saved.name.Trim();saved.created=true;
            try{AppearanceStore.Save(AppearancePath,saved);SaveDay();}catch(Exception error){message="Não foi possível salvar: "+error.Message;return false;}
            playerAppearance=saved;LeaveCreator(creatorDestination);return true;
        }
        void CancelCreator(){LeaveCreator(creatorReturn);}
        void LeaveCreator(Stage destination)
        {
            creatorDraft=null;studio.PlayerLook=playerAppearance;dragging=false;
            if(destination==Stage.Home)GoHome();else{Home();ShowPlayer();}
            UpdateCamera();
        }
        bool CreatorButton(Rect r,string label,bool selected=false,bool enabled=true)
        {
            KaosButtonFace(r,label,selected,enabled);
            bool old=GUI.enabled;GUI.enabled=old&&enabled;bool clicked=GUI.Button(r,GUIContent.none,GUIStyle.none);GUI.enabled=old;
            if(clicked)Sound(0);return clicked;
        }
        void CreatorUI()
        {
            Fill(new Rect(0,88,375,812),new Color(.045f,.045f,.05f,.98f));
            Fill(new Rect(1050,88,390,812),new Color(.045f,.045f,.05f,.98f));
            Txt(new Rect(28,118,320,28),"SEU PERSONAGEM",tiny,CreatorRed);
            Txt(new Rect(28,160,320,52),"Seu jeito.",heading);
            Txt(new Rect(28,224,320,28),"Como você se chama?",caption);
            creatorDraft.name=GUI.TextField(new Rect(28,264,320,40),creatorDraft.name,24,nameEntry);
            Txt(new Rect(28,330,320,32),"Corte de cabelo",caption);
            for(int i=0;i<PlayerAppearance.HairNames.Length;i++)if(CreatorButton(new Rect(28+i%2*165,376+i/2*60,155,48),PlayerAppearance.HairNames[i],creatorDraft.hair==i)){creatorDraft.hair=i;RefreshCreator();}
            Txt(new Rect(28,640,320,58),"Gire para ver o volume\ne o encaixe por todos os lados.",caption,muted);
            Txt(new Rect(1074,126,330,32),"Tom de pele",caption);
            CreatorSwatches(new Rect(1074,178,330,120),PlayerAppearance.SkinColors,true);
            Txt(new Rect(1074,324,330,32),"Cor do cabelo",caption);
            CreatorSwatches(new Rect(1074,373,330,120),PlayerAppearance.HairColors,false);
            Txt(new Rect(1074,506,330,25),PlayerAppearance.ColorNames[creatorDraft.hairColor],tiny,muted);
            Txt(new Rect(1074,559,330,32),"Expressão",caption);
            for(int i=0;i<6;i++)if(CreatorButton(new Rect(1074+i%2*166,607+i/2*57,156,45),PlayerAppearance.FaceNames[i],creatorDraft.expression==i)){creatorDraft.expression=i;RefreshCreator();}
            if(CreatorButton(new Rect(1074,809,330,58),"Salvar personagem",true,!string.IsNullOrWhiteSpace(creatorDraft.name)))BeginLoading(()=>{SaveCreator();});
            if(CreatorButton(new Rect(28,808,320,54),"Voltar sem salvar"))CancelCreator();
            if(CreatorButton(new Rect(420,807,200,45),"↶ Girar")){rotation-=45;UpdateCamera();}
            if(CreatorButton(new Rect(640,807,200,45),"Girar ↷")){rotation+=45;UpdateCamera();}
            if(CreatorButton(new Rect(860,807,145,45),"Restaurar")){rotation=-15;distance=6.8f;UpdateCamera();}
            Txt(new Rect(422,862,580,28),message.Length>0?message:"Arraste para girar  /  Role para aproximar",tiny,muted);
            ProcessCreatorPointer(Event.current);
        }
        void CreatorSwatches(Rect area,Color[] colors,bool skin)
        {
            for(int i=0;i<colors.Length;i++){
                var r=new Rect(area.x+i%3*110,area.y+i/3*58,100,48);bool selected=(skin?creatorDraft.skin:creatorDraft.hairColor)==i;
                Fill(r,selected?CreatorRed:line);Fill(new Rect(r.x+4,r.y+4,r.width-8,r.height-8),colors[i]);
                if(GUI.Button(r,GUIContent.none,GUIStyle.none)){if(skin)creatorDraft.skin=i;else creatorDraft.hairColor=i;RefreshCreator();Sound(0);}
            }
        }
        void ProcessCreatorPointer(Event e)
        {
            if(stage!=Stage.CharacterCreator||paused)return;var area=new Rect(385,110,650,680);
            if(e.type==EventType.MouseDown&&e.button==0&&area.Contains(e.mousePosition)){dragging=true;e.Use();}
            if(dragging&&e.type==EventType.MouseDrag){rotation-=e.delta.x*.6f;UpdateCamera();e.Use();}
            if(dragging&&e.type==EventType.MouseUp){dragging=false;e.Use();}
            if(e.type==EventType.ScrollWheel&&area.Contains(e.mousePosition)){distance=Mathf.Clamp(distance+e.delta.y*.15f,5.2f,8.2f);UpdateCamera();e.Use();}
        }
    }
}
