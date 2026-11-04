using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.Profiling;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        [Serializable] class EntrySample {public string room;public double entry_ms,pose_ms;public int renderers,mesh_vertices,meshes,materials;public long allocated_bytes;}
        [Serializable] class EntryReport {public string unity;public List<EntrySample> rooms=new List<EntrySample>();}
        IEnumerator CheckEntryPerformance(string folder)
        {
            var original=progress;var appearance=playerAppearance;var storage=storageOverride;var report=new EntryReport{unity=Application.unityVersion};
            Home();progress=new DayProgress();playerAppearance=new PlayerAppearance{created=true,name="Desempenho"};storageOverride=Path.Combine(folder,"performance-session");
            string[] names={"home-cold","street-cold","shop-cold","home-return","street-return","shop-return"};
            for(int i=0;i<names.Length;i++){
                yield return null;
                var watch=Stopwatch.StartNew();if(i%3==0)GoHome();else if(i%3==1)EnterCity();else EnterShop();watch.Stop();
                var sample=new EntrySample{room=names[i],entry_ms=watch.Elapsed.TotalMilliseconds};
                yield return null;
                sample.renderers=city.GetComponentsInChildren<MeshRenderer>(true).Length;
                foreach(var filter in city.GetComponentsInChildren<MeshFilter>(true))if(filter.sharedMesh!=null){sample.meshes++;sample.mesh_vertices+=filter.sharedMesh.vertexCount;}
                sample.materials=Resources.FindObjectsOfTypeAll<Material>().Length;sample.allocated_bytes=Profiler.GetTotalAllocatedMemoryLong();
                watch.Restart();for(int frame=0;frame<30;frame++)city.Frame(cam,true,1/60f);watch.Stop();sample.pose_ms=watch.Elapsed.TotalMilliseconds/30;
                report.rooms.Add(sample);UnityEngine.Debug.Log("ENTRY_PERFORMANCE: "+sample.room+" "+sample.entry_ms.ToString("F2")+" ms; renderers="+sample.renderers+"; vertices="+sample.mesh_vertices+"; pose="+sample.pose_ms.ToString("F2")+" ms");
            }
            Home();progress=original;playerAppearance=appearance;storageOverride=storage;
            File.WriteAllText(Path.Combine(folder,"entry-performance.json"),JsonUtility.ToJson(report,true));
        }
    }
}
