using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        bool settingsOpen,settingsFromPause;float mouseSensitivity=1;
        void RoundedPanel(Rect r,Color color,float radius=7,float border=0)
        {var old=GUI.color;GUI.color=Color.white;GUI.DrawTexture(r,Texture2D.whiteTexture,ScaleMode.StretchToFill,true,0,color,border,radius);GUI.color=old;}
        void KaosButtonFace(Rect r,string label,bool selected,bool enabled)
        {
            bool hot=enabled&&GUI.enabled&&r.Contains(Event.current.mousePosition);bool down=hot&&Input.GetMouseButton(0);
            float lift=down?3:0;var face=new Rect(r.x,r.y+lift,r.width,r.height-4);
            RoundedPanel(new Rect(r.x+2,r.y+5,r.width,r.height),new Color(0,0,0,.42f));
            RoundedPanel(new Rect(r.x,r.y+4,r.width,r.height-3),selected?new Color(.39f,.015f,.035f):new Color(.055f,.05f,.055f));
            Color fill=!enabled?new Color(.12f,.12f,.12f):selected?(hot?new Color(.98f,.07f,.11f):CreatorRed):hot?new Color(.18f,.16f,.17f):surface;
            RoundedPanel(face,fill);
            if(selected&&enabled)RoundedPanel(face,Color.Lerp(CreatorRed,Color.white,.18f),7,1.2f);
            Fill(new Rect(face.x+9,face.y+2,face.width-18,1),new Color(1,.87f,.8f,selected?.25f:.10f));
            if(selected){Fill(new Rect(face.x+7,face.y+face.height-7,face.width-14,1),new Color(.42f,0,.02f,.45f));}
            Txt(new Rect(face.x+5,face.y,face.width-10,face.height),label,action,enabled?paper:muted);
        }
        void OpenSettings(bool fromPause){settingsFromPause=fromPause;settingsOpen=true;cityFreeCursor=true;SyncCityCursor(true);}
        void CloseSettings(){settingsOpen=false;if(!ProofMode){PlayerPrefs.SetFloat("FitCheck.MouseSensitivity",mouseSensitivity);PlayerPrefs.Save();}if(settingsFromPause)paused=true;else paused=false;}
        void SetMouseSensitivity(float value,bool save=false){mouseSensitivity=Mathf.Clamp(value,.25f,2.5f);if(city!=null)city.MouseLookScale=mouseSensitivity;if(save&&!ProofMode){PlayerPrefs.SetFloat("FitCheck.MouseSensitivity",mouseSensitivity);PlayerPrefs.Save();}}
        void SettingsUI()
        {
            Fill(new Rect(0,0,1440,900),new Color(.02f,.018f,.022f,.88f));
            RoundedPanel(new Rect(390,116,660,670),surface,12);Fill(new Rect(410,136,620,4),CreatorRed);
            Txt(new Rect(430,158,570,52),"Do seu jeito.",heading);Txt(new Rect(430,220,570,34),"CONFIGURAÇÕES",caption,CreatorRed);
            Txt(new Rect(430,282,570,34),"Sensibilidade do mouse  /  "+Mathf.RoundToInt(mouseSensitivity*100)+"%",copy);
            Fill(new Rect(430,339,570,4),new Color(.34f,.10f,.12f));
            SetMouseSensitivity(GUI.HorizontalSlider(new Rect(430,329,570,28),mouseSensitivity,.25f,2.5f));
            Txt(new Rect(430,368,570,30),"Ajusta o olhar em primeira pessoa.",caption,muted);
            if(Tap(new Rect(430,423,570,48),mutedAudio?"Sons: desligados":"Sons: ligados")){mutedAudio=!mutedAudio;if(!ProofMode)PlayerPrefs.SetInt("FitCheck.Mute",mutedAudio?1:0);}
            if(Tap(new Rect(430,487,570,48),reducedMotion?"Movimento reduzido: sim":"Movimento reduzido: não")){reducedMotion=!reducedMotion;if(!ProofMode)PlayerPrefs.SetInt("FitCheck.ReducedMotion",reducedMotion?1:0);}
            if(Tap(new Rect(430,551,570,48),retroFilter?"Pixelização: leve":"Pixelização: desligada")){retroFilter=!retroFilter;if(!ProofMode)PlayerPrefs.SetInt("FitCheck.Retro",retroFilter?1:0);}
            Txt(new Rect(430,623,570,60),"WASD andar • Shift correr • E interagir\nF personalizar a loja fechada • Tab liberar o cursor",caption,muted);
            if(Tap(new Rect(430,714,570,52),"Voltar",true))CloseSettings();
        }
        void PauseMenuUI()
        {
            Fill(new Rect(0,0,1440,900),new Color(.025f,.02f,.025f,.90f));
            RoundedPanel(new Rect(440,202,560,520),surface,12);Fill(new Rect(460,222,520,4),CreatorRed);
            Txt(new Rect(490,254,470,65),"Uma pausa.",heading);
            Txt(new Rect(490,330,460,45),"Seu atendimento continua daqui.",caption,muted);
            if(Tap(new Rect(490,398,460,58),"Retomar",true)){paused=false;cityFreeCursor=false;}
            if(Tap(new Rect(490,474,460,52),"Configurações"))OpenSettings(true);
            if(Tap(new Rect(490,544,460,52),"Voltar ao início")){paused=false;ReturnToTitle();}
        }
    }
}
