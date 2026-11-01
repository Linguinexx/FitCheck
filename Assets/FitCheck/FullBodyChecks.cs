using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FitCheck
{
    public partial class CityView
    {
        public void ValidateFullBodyMotion()
        {
            var actor=residents[2];float oldPhase=actor.phase,oldBlend=actor.blend;var oldPosition=actor.root.localPosition;
            var samples=new Dictionary<ResidentPart,Vector3[]>();var sums=new float[5];var counts=new int[5];
            try{
                actor.blend=1;actor.phase=.125f;float dip=.035f;
                actor.root.localPosition=new Vector3(oldPosition.x,actor.home.y-dip,oldPosition.z);PoseResident(actor,2,dip);
                foreach(var part in actor.parts)samples[part]=(Vector3[])part.vertices.Clone();
                actor.phase=.625f;PoseResident(actor,2,dip);
                foreach(var part in actor.parts){
                    int group=part.head?0:part.arm?2:part.foot?4:part.leg?3:1;
                    for(int i=0;i<part.vertices.Length;i++){sums[group]+=(part.vertices[i]-samples[part][i]).sqrMagnitude;counts[group]++;}
                }
                var names=new[]{"cabeça","tronco","braços","pernas","calçados"};var result="";
                for(int group=0;group<5;group++){
                    float movement=counts[group]>0?Mathf.Sqrt(sums[group]/counts[group]):0;
                    if(movement<(group<2?.003f:.025f))throw new Exception("Parte rígida na caminhada: "+names[group]+" / "+movement);
                    result+=names[group]+"="+movement.ToString("F3")+" ";
                }
                Debug.Log("FITCHECK_FULL_BODY_MOTION_PASSED: local vertex motion without root translation: "+result);
            }finally{
                actor.phase=oldPhase;actor.blend=oldBlend;actor.root.localPosition=oldPosition;
                PoseResident(actor,2,actor.home.y-oldPosition.y);
            }
        }
    }
    public partial class FirstCustomer
    {
        void CaptureBodyMotionReview(string folder)
        {
            ExpressionLifeReview(folder);EnterCity();paused=false;city.ValidateFullBodyMotion();
            city.SetView(46,88);city.Frame(cam,true,0);city.CheckPlayerBody(cam);SaveFirstPersonFrame(Path.Combine(folder,"player-looking-down.png"));
            city.SetView(46,65);city.Frame(cam,true,0);SaveFirstPersonFrame(Path.Combine(folder,"player-looking-down-65.png"));
            for(int i=0;i<12;i++)city.Step(Vector2.right,1/60f,false,mannequin);
            city.SetView(46,88);city.Frame(cam,true,0);city.CheckPlayerBody(cam);SaveFirstPersonFrame(Path.Combine(folder,"player-moving-down.png"));
            var motion=Path.Combine(folder,"close-motion");Directory.CreateDirectory(motion);
            for(int frame=0;frame<180;frame++){
                city.Frame(cam,false,1/30f);city.CheckResidents();
                var position=city.ResidentPosition(2);var focus=CityView.Origin+position+Vector3.up*1.5f;
                cam.transform.position=focus+new Vector3(2.2f,.2f,-3.3f);cam.transform.LookAt(focus);cam.fieldOfView=52;
                SaveFirstPersonFrame(Path.Combine(motion,frame.ToString("D3")+".png"),640,640);
                if(frame%30==0)SaveFirstPersonFrame(Path.Combine(folder,"walk-pose-"+(frame/30)+".png"),640,640);
            }
            Debug.Log("FITCHECK_BODY_REVIEW_PASSED: full-body gait, 180 close native frames, ankle contact and existing visibility/life checks.");Home();
        }
    }
}
