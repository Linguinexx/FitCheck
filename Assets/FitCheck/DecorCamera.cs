using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        bool decorCameraReady;Vector3 decorCameraPosition;Quaternion decorCameraRotation;float decorCameraFov;
        void FrameDecorCamera(Vector3 position,Vector3 target,float fov){
            var rotation=Quaternion.LookRotation(target-position,Vector3.up);
            float blend=!decorCameraReady||reducedMotion?1:1-Mathf.Exp(-Time.unscaledDeltaTime*9);
            decorCameraPosition=Vector3.Lerp(decorCameraReady?decorCameraPosition:position,position,blend);
            decorCameraRotation=Quaternion.Slerp(decorCameraReady?decorCameraRotation:rotation,rotation,blend);
            decorCameraFov=Mathf.Lerp(decorCameraReady?decorCameraFov:fov,fov,blend);decorCameraReady=true;
            cam.transform.SetPositionAndRotation(decorCameraPosition,decorCameraRotation);cam.fieldOfView=decorCameraFov;
        }
        void FocusHomeDecor(int item){homeDecorView=item==0?4:item==1||item==3?1:item==2||item==4?5:item==6?3:homeDecorArt==0?1:homeDecorArt==1?4:3;FrameHomeDecor();}
        void FocusShopDecor(int item){decorView=item==1?4:item==2?1:item==3?2:item==4||item==5?3:item==6?5:0;FrameShopDecor();}
    }
}
