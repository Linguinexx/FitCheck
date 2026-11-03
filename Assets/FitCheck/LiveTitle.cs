using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        bool titleSceneActive,titleStarting;float titleSceneTime;
        void StartLiveTitle()
        {
            if(titleSceneActive)return;
            titleStarting=true;try{EnterCity();}finally{titleStarting=false;}
            titleSceneActive=true;titleSceneTime=0;stage=Stage.Title;paused=false;city.PlayerBodyVisible=false;
            FrameLiveTitle(0);SyncCityCursor(true);
        }
        void EndLiveTitle(){if(!titleSceneActive)return;titleSceneActive=false;ExitCity();}
        void FrameLiveTitle(float delta)
        {
            if(city==null)return;if(!paused)titleSceneTime+=delta;
            city.Frame(cam,reducedMotion,paused?0:delta);city.PlayerBodyVisible=false;
            float orbit=reducedMotion?0:Mathf.Sin(titleSceneTime*.12f)*.65f;
            cam.transform.position=city.transform.position+new Vector3(-5.5f+orbit,3.1f,-12.5f);
            cam.transform.LookAt(city.transform.position+new Vector3(7.5f,4.2f,4));cam.fieldOfView=51;
        }
    }
}
