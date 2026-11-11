using System;
using System.Collections;
using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        bool loadingActive,loadingExecuting,loadingTestEnabled;
        float loadingClock,loadingOpacity=1;
        const float LoadingDurationSeconds=2f,LoadingRevealSeconds=.18f;
        const float LoadingMinimumSeconds=LoadingDurationSeconds-LoadingRevealSeconds;
        GUIStyle loadingLabel;
        static readonly string[] LoadingLabels={"Loading","Loading.","Loading..","Loading..."};
        bool DeferLoading(Action destination)
        {
            if(loadingExecuting||(ProofMode&&!loadingTestEnabled))return false;
            BeginLoading(destination);return true;
        }
        void BeginLoading(Action destination)
        {
            if(loadingActive)return;
            if(ProofMode&&!loadingTestEnabled){destination();return;}
            loadingActive=true;loadingClock=0;loadingOpacity=1;paused=false;dragging=false;
            cityDragLook=false;cityLookCaptured=false;
#if !UNITY_WEBGL || UNITY_EDITOR
            if(!ProofMode){Cursor.lockState=CursorLockMode.None;Cursor.visible=false;}
#endif
            StartCoroutine(LoadDestination(destination));
        }
        IEnumerator LoadDestination(Action destination)
        {
            try{
                // Paint the cover before any synchronous construction or save IO.
                yield return null;yield return null;
                loadingExecuting=true;
                try{destination();}
                catch(Exception error){message="Não foi possível abrir o destino. Tente novamente.";Debug.LogException(error);}
                finally{loadingExecuting=false;}
                arrivalFade=0;
#if !UNITY_WEBGL || UNITY_EDITOR
                if(!ProofMode){Cursor.lockState=CursorLockMode.None;Cursor.visible=false;}
#endif
                // Brief readable transition, never a simulated progress percentage.
                while(loadingClock<LoadingMinimumSeconds)yield return null;
                float fade=0;
                while(fade<LoadingRevealSeconds){fade+=Time.unscaledDeltaTime;loadingOpacity=1-Mathf.Clamp01(fade/LoadingRevealSeconds);yield return null;}
            }finally{
                loadingExecuting=false;loadingActive=false;loadingOpacity=1;
                if(!ProofMode){if(stage==Stage.City)SyncCityCursor(true);else{
#if UNITY_WEBGL && !UNITY_EDITOR
                    BrowserPointer.Capture(false);
#else
                    Cursor.lockState=CursorLockMode.None;Cursor.visible=true;
#endif
                }}
            }
        }
        int LoadingDotCount(float clock)
        {int phase=Mathf.FloorToInt(clock/.14f)%6;return phase<=3?phase:6-phase;}
        float LoadingDiagonalOffset(float clock)=>reducedMotion?0:Mathf.Repeat(clock*24,400);
        void DrawLoadingScreen()
        {
            var matrix=GUI.matrix;var color=GUI.color;var content=GUI.contentColor;bool enabled=GUI.enabled;
            try{
                GUI.enabled=true;GUI.matrix=Matrix4x4.identity;cam.GetComponent<RetroCamera>().DrawWorld();GUI.color=new Color(1,1,1,loadingOpacity);GUI.contentColor=Color.white;
                // Cover letterbox margins as well as the logical 1440x900 canvas.
                Fill(new Rect(0,0,Screen.width,Screen.height),new Color(.012f,.012f,.015f,loadingOpacity));
                float scale=Mathf.Min(Screen.width/1440f,Screen.height/900f);
                GUI.matrix=Matrix4x4.TRS(new Vector3((Screen.width-1440*scale)/2,(Screen.height-900*scale)/2,0),Quaternion.identity,Vector3.one*scale);
                var basis=GUI.matrix;
                float drift=LoadingDiagonalOffset(loadingClock);
                for(int stripe=-4;stripe<8;stripe++){
                    var r=new Rect(stripe*400+drift-180,-650,155,2200);
                    GUIUtility.RotateAroundPivot(26,new Vector2(720,450));
                    Fill(r,new Color(227/255f,6/255f,19/255f,.12f*loadingOpacity));GUI.matrix=basis;
                }
                if(titleLogo==null)titleLogo=Resources.Load<Texture2D>("Brand/FitCheck-Selo-Kaos-Red");
                GUI.color=new Color(1,1,1,loadingOpacity);
                if(titleLogo!=null)DrawAnimatedTitleLogo(new Rect(485,235,470,320),loadingClock);
                else Txt(new Rect(470,300,500,150),"FIT CHECK",heading,paper);
                InterfaceStyles();
                if(loadingLabel==null)loadingLabel=new GUIStyle(action){alignment=TextAnchor.MiddleCenter,fontSize=23};
                Txt(new Rect(550,590,340,55),LoadingLabels[reducedMotion?3:LoadingDotCount(loadingClock)],loadingLabel,paper);
            }finally{GUI.matrix=matrix;GUI.color=color;GUI.contentColor=content;GUI.enabled=enabled;}
        }
    }
}
