using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        bool settingsOpen,settingsFromPause,audioSettingsOpen;
        const string MasterVolumePreference="FitCheck.MasterVolume", MusicVolumePreference="FitCheck.MusicVolume";
        float masterVolume=1,musicVolume=1;float mouseSensitivity=SensitivityDefault;
        const float SensitivityMin=0,SensitivityMax=1.5f,SensitivityDefault=.525f;
        const string SensitivityPreference="FitCheck.MouseSensitivityV2";
        const string FullscreenPreference="FitCheck.Fullscreen";
        static readonly Color KaosControl=new Color32(43,38,43,255);
        static readonly Color KaosHover=new Color32(60,54,59,255);
        static readonly Color KaosPanel=new Color32(23,21,25,250);
        static readonly Color KaosTrack=new Color32(66,59,65,255);
        GUIStyle sensitivityValue;

        Color KaosControlColor(bool primary,bool hot,bool enabled)
        {
            if(!enabled)return new Color32(34,31,34,255);
            if(primary)return hot?new Color32(175,0,14,255):CreatorRed;
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
        void OpenSettings(bool fromPause){settingsFromPause=fromPause;audioSettingsOpen=false;controlsOpen=false;settingsOpen=true;cityFreeCursor=true;SyncCityCursor(true);}
        void SetFullscreen(bool fullscreen)
        {
            if(ProofMode||Application.isEditor)return;
#if UNITY_WEBGL
            if(fullscreen)Screen.fullScreen=true;else BrowserPointer.Minimize();
#else
            Resolution desktop=Screen.currentResolution;
            if(fullscreen)Screen.SetResolution(desktop.width,desktop.height,FullScreenMode.FullScreenWindow);
            else{
                float scale=Mathf.Min(1f,desktop.width*.9f/1440f,desktop.height*.9f/900f);
                Screen.SetResolution(Mathf.RoundToInt(1440*scale),Mathf.RoundToInt(900*scale),FullScreenMode.Windowed);
            }
#endif
        }
        void CloseSettings(){settingsOpen=false;controlsOpen=false;if(!ProofMode){PlayerPrefs.SetFloat(SensitivityPreference,mouseSensitivity);SaveAudioSettings();}if(settingsFromPause)paused=true;else paused=false;}
        void SetMouseSensitivity(float value,bool save=false){mouseSensitivity=Mathf.Clamp(value,SensitivityMin,SensitivityMax);if(city!=null)city.MouseLookScale=mouseSensitivity;if(save&&!ProofMode){PlayerPrefs.SetFloat(SensitivityPreference,mouseSensitivity);SaveAudioSettings();}}
        void SensitivitySlider(Rect bounds)
        {SetMouseSensitivity(Mathf.Lerp(SensitivityMin,SensitivityMax,SettingsSlider(bounds,Mathf.InverseLerp(SensitivityMin,SensitivityMax,mouseSensitivity))));}
        float SettingsSlider(Rect bounds,float value)
        {
            value=Mathf.Clamp01(value);
            const float knobSize=22;
            float left=bounds.x+knobSize/2,right=bounds.xMax-knobSize/2;
            int id=GUIUtility.GetControlID(FocusType.Keyboard,bounds);
            var input=Event.current;
            switch(input.GetTypeForControl(id)){
                case EventType.MouseDown:
                    if(!GUI.enabled||input.button!=0||!bounds.Contains(input.mousePosition))break;
                    GUIUtility.hotControl=id;
                    GUIUtility.keyboardControl=id;
                    value=Mathf.InverseLerp(left,right,input.mousePosition.x);
                    input.Use();
                    break;
                case EventType.MouseDrag:
                    if(GUIUtility.hotControl!=id)break;
                    value=Mathf.InverseLerp(left,right,input.mousePosition.x);
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
                    if(input.keyCode==KeyCode.LeftArrow)value=Mathf.Clamp01(value-step);
                    else if(input.keyCode==KeyCode.RightArrow)value=Mathf.Clamp01(value+step);
                    else if(input.keyCode==KeyCode.Home)value=0;
                    else if(input.keyCode==KeyCode.End)value=1;
                    else break;
                    input.Use();
                    break;
            }
            float x=Mathf.Lerp(left,right,value);
            var track=new Rect(left,bounds.center.y-4,right-left,8);
            RoundedPanel(track,KaosTrack,4);
            if(x-left>=1)RoundedPanel(new Rect(left,track.y,x-left,8),CreatorRed,4);
            var knob=new Rect(x-knobSize/2,bounds.center.y-knobSize/2,knobSize,knobSize);
            if(GUIUtility.keyboardControl==id||bounds.Contains(input.mousePosition)){
                var halo=new Rect(knob.x-4,knob.y-4,knob.width+8,knob.height+8);
                RoundedPanel(halo,GUIUtility.keyboardControl==id?CreatorRed:KaosHover,15);
            }
            RoundedPanel(knob,paper,knobSize/2);
            return value;
        }
        void SettingsUI()
        {
            Fill(new Rect(0,0,1440,900),new Color(.02f,.018f,.022f,.88f));
            if(controlsOpen){ControlsUI();return;}
            if(audioSettingsOpen){AudioSettingsUI();return;}
            DrawKaosPanel(new Rect(390,116,660,670));
            Txt(new Rect(430,154,570,52),"Configurações",heading);
#if UNITY_WEBGL && !UNITY_EDITOR
            bool fullscreen=Screen.fullScreen;
            if(Tap(new Rect(795,154,205,48),fullscreen?"Tela cheia":"Modo janela",false,!Application.isEditor)){
                SetFullscreen(!fullscreen);
                if(!ProofMode){PlayerPrefs.SetInt(FullscreenPreference,fullscreen?0:1);PlayerPrefs.Save();}
            }
#endif
            Fill(new Rect(430,217,36,3),CreatorRed);
            RoundedPanel(new Rect(422,246,588,156),KaosControl,12);
            Txt(new Rect(444,262,400,32),"Sensibilidade do mouse",copy);
            if(sensitivityValue==null)sensitivityValue=new GUIStyle(action){alignment=TextAnchor.MiddleCenter};
            RoundedPanel(new Rect(892,261,94,34),KaosPanel,8);
            Txt(new Rect(892,261,94,34),Mathf.RoundToInt(Mathf.InverseLerp(SensitivityMin,SensitivityMax,mouseSensitivity)*100).ToString(),sensitivityValue,paper);
            SensitivitySlider(new Rect(442,298,544,44));
            Txt(new Rect(450,343,60,24),"0",tiny,muted);
            Txt(new Rect(934,343,52,24),"100",tiny,muted);
            Txt(new Rect(444,373,540,23),"Velocidade do olhar em primeira pessoa.",caption,muted);
            if(Tap(new Rect(430,423,278,48),"Áudio  →"))audioSettingsOpen=true;
            if(Tap(new Rect(722,423,278,48),"Controles  →"))OpenControls();
            if(Tap(new Rect(430,487,570,48),reducedMotion?"Movimento reduzido: sim":"Movimento reduzido: não")){reducedMotion=!reducedMotion;if(!ProofMode)PlayerPrefs.SetInt("FitCheck.ReducedMotion",reducedMotion?1:0);}
            if(Tap(new Rect(430,551,570,48),retroFilter?"Pixelização: leve":"Pixelização: desligada")){retroFilter=!retroFilter;if(!ProofMode)PlayerPrefs.SetInt("FitCheck.Retro",retroFilter?1:0);}
            if(Tap(new Rect(430,611,570,48),garmentFinishes?"Brilho das roupas: ligado":"Brilho das roupas: desligado"))SetGarmentFinishes(!garmentFinishes,true);
            if(Tap(new Rect(430,665,570,40),CatalogTesting.Enabled?"Catálogo de teste: liberado":"Catálogo de teste: desligado"))
                SetCatalogTesting(!CatalogTesting.Enabled);
            if(Tap(new Rect(430,714,570,52),"Voltar",true))CloseSettings();
        }
        void SaveAudioSettings()
        {
            if(ProofMode)return;
            PlayerPrefs.SetFloat(MasterVolumePreference,masterVolume);
            PlayerPrefs.SetFloat(MusicVolumePreference,musicVolume);
            PlayerPrefs.SetInt("FitCheck.Mute",mutedAudio?1:0);
            PlayerPrefs.Save();
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
        float AudioSlider(string label,float y,float value)
        {
            Txt(new Rect(444,y,420,32),label,copy,paper);
            Txt(new Rect(911,y,75,32),$"{Mathf.RoundToInt(value*100)}%",copy,paper);
            return SettingsSlider(new Rect(442,y+31,544,44),value);
        }
        void AudioSettingsUI()
        {
            DrawKaosPanel(new Rect(390,202,660,488));
            Txt(new Rect(430,235,570,52),"Áudio",heading);
            Fill(new Rect(430,296,36,3),CreatorRed);
            masterVolume=AudioSlider("Volume geral",321,masterVolume);
            musicVolume=AudioSlider("Volume da música",407,musicVolume);
            if(Tap(new Rect(430,501,570,48),mutedAudio?"Som desligado":"Som ligado"))mutedAudio=!mutedAudio;
            if(Tap(new Rect(430,601,570,52),"Voltar",true)){
                SaveAudioSettings();
                audioSettingsOpen=false;
            }
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
