using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        void HandleBrowserBackAction()
        {
            if(loadingActive)return;
            // Browser Escape may release pointer lock or fullscreen. It only
            // navigates existing panels; P alone controls the web pause state.
            if(settingsOpen||showShopDecor||shopGuideOpen||pendingDeleteSlot>=0||
                (!paused&&(stage==Stage.Title||stage==Stage.CharacterCreator||stage==Stage.Confirm)))
                HandleEscapeAction();
            else if(stage==Stage.City&&!paused)cityFreeCursor=true;
        }
        void HandleEscapeAction()
        {
            if(loadingActive)return;
            if(stage==Stage.Title&&titleIntroPlaying&&!reducedMotion){titleIntroPlaying=false;return;}
            if(settingsOpen&&controlsOpen){controlsOpen=false;return;}
            if(settingsOpen&&audioSettingsOpen){audioSettingsOpen=false;return;}
            if(settingsOpen){CloseSettings();return;}
            if(stage==Stage.Title)return;
            if(showShopDecor){CloseShopDecor(false);return;}
            if(shopGuideOpen){CloseShopGuide();return;}
            if(pendingDeleteSlot>=0)CancelSaveDeletion();
            else if(stage==Stage.CharacterCreator&&!paused)CancelCreator();
            else if(stage==Stage.Confirm&&!paused)stage=Stage.Dressing;
            else {paused=!paused;if(!paused)cityFreeCursor=false;}
            dragging=false;
        }
        void LoadMouseSensitivity()
        {
            float value=SensitivityDefault;
            if(ProofMode)value=1;
            else if(PlayerPrefs.HasKey(SensitivityPreference))value=PlayerPrefs.GetFloat(SensitivityPreference,SensitivityDefault);
            else if(PlayerPrefs.HasKey("FitCheck.MouseSensitivity")){
                float legacy=PlayerPrefs.GetFloat("FitCheck.MouseSensitivity",1);
                // Migrate the old default to the calmer new default; retain custom values.
                if(!Mathf.Approximately(legacy,1))value=legacy;
            }
            SetMouseSensitivity(value);
        }
    }
}
