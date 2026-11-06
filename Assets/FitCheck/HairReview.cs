using System;
using System.IO;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        void CaptureHairReview(string folder)
        {
            Home();var old=studio.PlayerLook;int oldCut=studio.HairOverride;bool oldReduced=studio.ReducedMotion;
            try{
                studio.SetHome(false);studio.HairOverride=-1;studio.ReducedMotion=true;
                for(int cut=0;cut<PlayerAppearance.HairNames.Length;cut++){
                    studio.PlayerLook=new PlayerAppearance{hair=cut,hairColor=2,skin=3,expression=0};studio.ApplyUniform(progress.EquippedUniform);studio.Animate(0,false);
                    foreach(int angle in cut==6?new[]{0,90,180,270}:new[]{0,90,180}){
                        mannequin.localRotation=Quaternion.Euler(0,angle,0);cam.transform.position=new Vector3(0,2.86f,-2.65f);cam.transform.LookAt(new Vector3(0,2.72f,0));
                        cam.fieldOfView=36;cam.clearFlags=CameraClearFlags.SolidColor;
                        SaveFirstPersonFrame(Path.Combine(folder,"hair-"+cut+"-"+angle+".png"),640,640);
                    }
                    foreach(var renderer in studio.Avatar.GetComponentsInChildren<MeshRenderer>())
                        if(renderer.name=="Mecha volumétrica")throw new Exception("Mechas tubulares antigas voltaram");
                }
                Debug.Log("FITCHECK_HAIR_REVIEW_PASSED: all cuts, front/profile/back native renders; no detached tubular locks; creator and hat checks retained.");
            }finally{studio.PlayerLook=old;studio.HairOverride=oldCut;studio.ReducedMotion=oldReduced;mannequin.localRotation=Quaternion.identity;Home();UpdateCamera();}
        }
    }
}
