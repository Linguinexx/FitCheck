using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        bool settingsOpen,settingsFromPause;float mouseSensitivity=1;
        const float SensitivityMin=.25f,SensitivityMax=2.5f;
        static readonly Color KaosControl=new Color32(43,38,43,255);
        static readonly Color KaosHover=new Color32(60,54,59,255);
        static readonly Color KaosPanel=new Color32(23,21,25,250);
        static readonly Color KaosTrack=new Color32(66,59,65,255);
        GUIStyle sensitivityValue;

        Color KaosControlColor(bool primary,bool hot,bool enabled)
        {
            if(!enabled)return new Color32(34,31,34,255);
            if(primary)return hot?new Color32(204,5,17,255):CreatorRed;
            return hot?KaosHover:KaosControl;
        }
        void RoundedPanel(Rect r,Color color,float radius=7,float border=0)
        {var old=GUI.color;GUI.color=Color.white;GUI.DrawTexture(r,Texture2D.whiteTexture,ScaleMode.StretchToFill,true,0,color,border,radius);GUI.color=old;}
        void KaosButtonFace(Rect r,string label,bool selected,bool enabled)
        {
            bool hot=enabled&&GUI.enabled&&r.Contains(Event.current.mousePosition);
            float pressed=hot&&!reducedMotion&&Input.GetMouseButton(0)?3:0;
            var face=new Rect(r.x,r.y+pressed,r.width,r.height-4);
            RoundedPanel(new Rect(r.x,r.y+4,r.width,r.height-4),new Color(0,0,0,.55f),10);
            RoundedPanel(face,KaosControlColor(selected,hot,enabled),10);
            Txt(new Rect(face.x+5,face.y,face.width-10,face.height),label,action,enabled?Color.white:muted);
        }
        void DrawKaosPanel(Rect area)
        {
            RoundedPanel(new Rect(area.x,area.y+7,area.width,area.height),new Color(0,0,0,.45f),16);
            RoundedPanel(area,KaosPanel,16);
        }
        void OpenSettings(bool fromPause){settingsFromPause=fromPause;settingsOpen=true;cityFreeCursor=true;SyncCityCursor(true);}
        void CloseSettings(){settingsOpen=false;if(!ProofMode){PlayerPrefs.SetFloat("FitCheck.MouseSensitivity",mouseSensitivity);PlayerPrefs.Save();}if(settingsFromPause)paused=true;else paused=false;}
        void SetMouseSensitivity(float value,bool save=false){mouseSensitivity=Mathf.Clamp(value,SensitivityMin,SensitivityMax);if(city!=null)city.MouseLookScale=mouseSensitivity;if(save&&!ProofMode){PlayerPrefs.SetFloat("FitCheck.MouseSensitivity",mouseSensitivity);PlayerPrefs.Save();}}
        void SensitivitySlider(Rect bounds)
        {
            const float knobSize=22;
            float left=bounds.x+knobSize/2,right=bounds.xMax-knobSize/2;
            int id=GUIUtility.GetControlID(FocusType.Keyboard,bounds);
            var input=Event.current;
            switch(input.GetTypeForControl(id)){
                case EventType.MouseDown:
                    if(!GUI.enabled||input.button!=0||!bounds.Contains(input.mousePosition))break;
                    GUIUtility.hotControl=id;
                    GUIUtility.keyboardControl=id;
                    SetMouseSensitivity(Mathf.Lerp(SensitivityMin,SensitivityMax,Mathf.InverseLerp(left,right,input.mousePosition.x)));
                    input.Use();
                    break;
                case EventType.MouseDrag:
                    if(GUIUtility.hotControl!=id)break;
                    SetMouseSensitivity(Mathf.Lerp(SensitivityMin,SensitivityMax,Mathf.InverseLerp(left,right,input.mousePosition.x)));
                    input.Use();
                    break;
                case EventType.MouseUp:
                    if(GUIUtility.hotControl!=id||input.button!=0)break;
                    GUIUtility.hotControl=0;
                    input.Use();
                    break;
                case EventType.KeyDown:
                    if(GUIUtility.keyboardControl!=id)break;
                    float step=input.shift?.10f:.01f;
                    if(input.keyCode==KeyCode.LeftArrow)SetMouseSensitivity(mouseSensitivity-step);
                    else if(input.keyCode==KeyCode.RightArrow)SetMouseSensitivity(mouseSensitivity+step);
                    else if(input.keyCode==KeyCode.Home)SetMouseSensitivity(SensitivityMin);
                    else if(input.keyCode==KeyCode.End)SetMouseSensitivity(SensitivityMax);
                    else break;
                    input.Use();
                    break;
            }
            float x=Mathf.Lerp(left,right,Mathf.InverseLerp(SensitivityMin,SensitivityMax,mouseSensitivity));
            var track=new Rect(left,bounds.center.y-4,right-left,8);
            RoundedPanel(track,KaosTrack,4);
            if(x-left>=1)RoundedPanel(new Rect(left,track.y,x-left,8),CreatorRed,4);
            var knob=new Rect(x-knobSize/2,bounds.center.y-knobSize/2,knobSize,knobSize);
            if(GUIUtility.keyboardControl==id||bounds.Contains(input.mousePosition)){
                var halo=new Rect(knob.x-4,knob.y-4,knob.width+8,knob.height+8);
                RoundedPanel(halo,GUIUtility.keyboardControl==id?CreatorRed:KaosHover,15);
            }
            RoundedPanel(knob,paper,knobSize/2);
        }
        void SettingsUI()
        {
            Fill(new Rect(0,0,1440,900),new Color(.02f,.018f,.022f,.88f));
            DrawKaosPanel(new Rect(390,116,660,670));
            Txt(new Rect(430,154,570,52),"Configurações",heading);
            Fill(new Rect(430,217,36,3),CreatorRed);
            RoundedPanel(new Rect(422,246,588,156),KaosControl,12);
            Txt(new Rect(444,262,400,32),"Sensibilidade do mouse",copy);
            if(sensitivityValue==null)sensitivityValue=new GUIStyle(action){alignment=TextAnchor.MiddleCenter};
            RoundedPanel(new Rect(892,261,94,34),KaosPanel,8);
            Txt(new Rect(892,261,94,34),Mathf.RoundToInt(mouseSensitivity*100)+"%",sensitivityValue,paper);
            SensitivitySlider(new Rect(442,298,544,44));
            Txt(new Rect(450,343,60,24),"25%",tiny,muted);
            Txt(new Rect(934,343,52,24),"250%",tiny,muted);
            Txt(new Rect(444,373,540,23),"Velocidade do olhar em primeira pessoa.",caption,muted);
            if(Tap(new Rect(430,423,570,48),mutedAudio?"Sons: desligados":"Sons: ligados")){mutedAudio=!mutedAudio;if(!ProofMode)PlayerPrefs.SetInt("FitCheck.Mute",mutedAudio?1:0);}
            if(Tap(new Rect(430,487,570,48),reducedMotion?"Movimento reduzido: sim":"Movimento reduzido: não")){reducedMotion=!reducedMotion;if(!ProofMode)PlayerPrefs.SetInt("FitCheck.ReducedMotion",reducedMotion?1:0);}
            if(Tap(new Rect(430,551,570,48),retroFilter?"Pixelização: leve":"Pixelização: desligada")){retroFilter=!retroFilter;if(!ProofMode)PlayerPrefs.SetInt("FitCheck.Retro",retroFilter?1:0);}
            if(Tap(new Rect(430,611,570,48),garmentFinishes?"Brilho das roupas: ligado":"Brilho das roupas: desligado"))SetGarmentFinishes(!garmentFinishes,true);
            if(Tap(new Rect(430,665,570,40),CatalogTesting.Enabled?"Catálogo de teste: liberado":"Catálogo de teste: desligado"))
                SetCatalogTesting(!CatalogTesting.Enabled);
            if(Tap(new Rect(430,714,570,52),"Voltar",true))CloseSettings();
        }
        void SetCatalogTesting(bool enabled)
        {
            CatalogTesting.Enabled=enabled;
            if(replayIndex<0&&campaignBeforePreview==null){
                inventory=progress.Available;
                bool changed=false;
                for(int slot=0;slot<outfit.Length;slot++){
                    string id=outfit[slot];
                    if(string.IsNullOrEmpty(id)||Catalog.IsArrivalPiece(id)||System.Array.IndexOf(inventory,id)>=0)continue;
                    outfit[slot]=slot==3?"":shopDressingInStore?Catalog.ArrivalPiece(slot).id:Catalog.Default[slot];
                    changed=true;
                }
                if(changed)Apply();
            }
            if(!ProofMode){PlayerPrefs.SetInt(CatalogTesting.PreferenceKey,enabled?1:0);PlayerPrefs.Save();}
        }
        void PauseMenuUI()
        {
            Fill(new Rect(0,0,1440,900),new Color(.025f,.02f,.025f,.90f));
            DrawKaosPanel(new Rect(440,202,560,520));
            Fill(new Rect(490,240,36,3),CreatorRed);
            Txt(new Rect(490,254,470,65),"Uma pausa.",heading);
            Txt(new Rect(490,330,460,45),"Seu atendimento continua daqui.",caption,muted);
            if(Tap(new Rect(490,398,460,58),"Retomar",true)){paused=false;cityFreeCursor=false;}
            if(Tap(new Rect(490,474,460,52),"Configurações"))OpenSettings(true);
            if(Tap(new Rect(490,544,460,52),"Voltar ao início")){paused=false;ReturnToTitle();}
        }
    }
}
