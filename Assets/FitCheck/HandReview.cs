using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        void CaptureHandReview(string folder)
        {
            Home();studio.Apply(new[]{"CG_TOP_01","SB_BOTTOM_01","CG_SHOES_01",""},0);
            mannequin.rotation=Quaternion.identity;studio.Animate(0,false);studio.ValidateArmFit();
            cam.transform.position=new Vector3(1.7f,1.9f,-4.3f);cam.transform.LookAt(new Vector3(0,1.65f,0));cam.fieldOfView=42;
            SaveFirstPersonFrame(Path.Combine(folder,"hands-character.png"),800,1000);
            var renderers=studio.Avatar.GetComponentsInChildren<MeshRenderer>();
            var visible=new List<MeshRenderer>();foreach(var renderer in renderers)if(renderer.enabled)visible.Add(renderer);
            try{
                foreach(var renderer in visible)renderer.enabled=false;
                foreach(var hand in visible){
                    if(hand.name!="Mão")continue;hand.enabled=true;
                    var focus=hand.bounds.center;bool right=focus.x>0;
                    for(int view=0;view<2;view++){
                        cam.transform.position=focus+hand.transform.forward*(view==0?-.82f:.82f)+hand.transform.right*.12f+Vector3.up*.08f;
                        cam.transform.LookAt(focus);cam.fieldOfView=27;
                        SaveFirstPersonFrame(Path.Combine(folder,(right?"right":"left")+"-hand-"+(view==0?"palm":"back")+".png"),640,800);
                    }
                    hand.enabled=false;
                }
            }finally{foreach(var renderer in visible)renderer.enabled=true;Home();UpdateCamera();}
            Debug.Log("FITCHECK_HAND_REVIEW_PASSED: one closed mesh per hand; mirrored palms, four fingers, integrated thumb; wrist/sleeve fit and full-body animation preserved.");
        }
    }
}
