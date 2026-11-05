using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
namespace FitCheck {
 public partial class FirstCustomer {
  bool shopProbeActive,shopProbeMoving;long shopProbeStamp;double[] shopProbeParts=new double[4];
  void ShopProbeMark(int point){if(!shopProbeActive)return;long now=Stopwatch.GetTimestamp();if(point>0)shopProbeParts[point-1]=(now-shopProbeStamp)*1000.0/Stopwatch.Frequency;shopProbeStamp=now;}
  [Serializable] class ShopGameplaySample {public string view;public double frame_median_ms,frame_p95_ms,update_median_ms,studio_ms,player_ms,actors_camera_ms,other_ms;}
  [Serializable] class ShopGameplayReport {public List<ShopGameplaySample> samples=new List<ShopGameplaySample>();}
  static double ProbeMedian(List<double> values,double percentile=.5){values.Sort();return values[Math.Min(values.Count-1,(int)(values.Count*percentile))];}
  IEnumerator CheckShopGameplayPerformance(string folder){
   Home();EnterShop();paused=false;retroFilter=false;var report=new ShopGameplayReport();
   for(int phase=0;phase<3;phase++){
    city.SetView(180,phase==2?70:0);shopProbeMoving=phase>0;shopProbeActive=true;
    for(int i=0;i<30;i++)yield return null;
    var frames=new List<double>();var totals=new List<double>();var parts=new[]{new List<double>(),new List<double>(),new List<double>(),new List<double>()};
    for(int i=0;i<180;i++){yield return null;frames.Add(Time.unscaledDeltaTime*1000);double total=0;for(int k=0;k<4;k++){parts[k].Add(shopProbeParts[k]);total+=shopProbeParts[k];}totals.Add(total);}
    var sample=new ShopGameplaySample{view=phase==0?"standing":phase==1?"walking-forward":"walking-look-down",frame_median_ms=ProbeMedian(frames),frame_p95_ms=ProbeMedian(frames,.95),update_median_ms=ProbeMedian(totals),studio_ms=ProbeMedian(parts[0]),player_ms=ProbeMedian(parts[1]),actors_camera_ms=ProbeMedian(parts[2]),other_ms=ProbeMedian(parts[3])};report.samples.Add(sample);UnityEngine.Debug.Log("SHOP_GAMEPLAY: "+JsonUtility.ToJson(sample));
   }
   shopProbeActive=false;city.CheckPlayerPoseRecovery(cam,mannequin);File.WriteAllText(Path.Combine(folder,"shop-gameplay-performance.json"),JsonUtility.ToJson(report,true));
  }
 }
 public partial class CityView {
  public void CheckPlayerPoseRecovery(Camera camera,Transform source){
   SetView(180,0);Frame(camera,true,0);FramePlayerForCamera(camera,false);
   float previous=playerPoseCycle;
   for(int i=0;i<20;i++)Step(new Vector2(0,.1f),1/60f,false,source);
   if(playerPoseCycle!=previous)throw new Exception("Invisible player was skinned unnecessarily");
   SetView(180,70);Frame(camera,true,0);FramePlayerForCamera(camera,false);
   if(playerPoseCycle!=playerBody.cycle)throw new Exception("Look-down pose not recovered immediately");CheckPlayerBody(camera);
   SetView(180,0);Frame(camera,true,0);FramePlayerForCamera(camera,false);
   for(int i=0;i<20;i++)Step(Vector2.zero,1/60f,false,source);
   camera.transform.position=playerBody.root.position+new Vector3(0,2,-5);camera.transform.LookAt(playerBody.root.position+Vector3.up*1.8f);FramePlayerForCamera(camera,true);
   if(playerPoseCycle!=playerBody.cycle||!PlayerHeadVisible)throw new Exception("External full-body pose not recovered immediately");
   CheckSkinEquivalence();UnityEngine.Debug.Log("PLAYER_POSE_RECOVERY_PASSED: deferred offscreen skin, immediate look-down and external full body, reference skin equivalence.");
  }
 }
}
