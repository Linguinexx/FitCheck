using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class StudioView
    {
        sealed class WalkingLeg {public Mesh mesh;public Vector3[] rest,work;public int side;}
        readonly List<WalkingLeg> walkingLegs=new List<WalkingLeg>();
        readonly List<Transform> walkingFeet=new List<Transform>();readonly List<Vector3> walkingFootRest=new List<Vector3>();
        float walkPhase,walkAmount;
        public void PrepareCityWalk()
        {
            walkingLegs.Clear();walkingFeet.Clear();walkingFootRest.Clear();walkPhase=0;walkAmount=0;
            foreach(var filter in clothes.GetComponentsInChildren<MeshFilter>()){
                if(filter.name!="Calça"&&filter.name!="Bermuda"&&filter.name!="Bolso")continue;
                var mesh=filter.sharedMesh;walkingLegs.Add(new WalkingLeg{mesh=mesh,rest=mesh.vertices,work=new Vector3[mesh.vertexCount],side=mesh.bounds.center.x<0?-1:1});
            }
            foreach(Transform child in clothes)if(child.name.StartsWith("Apoio — pé")){walkingFeet.Add(child);walkingFootRest.Add(child.localPosition);}
        }
        public void AnimateCityWalk(float delta,bool moving,bool paused)
        {
            if(paused)return;walkAmount=Mathf.MoveTowards(walkAmount,moving?1:0,delta*6);if(moving)walkPhase+=delta*7.4f;
            foreach(var leg in walkingLegs){
                if(leg.mesh==null)continue;float stride=Mathf.Sin(walkPhase+(leg.side<0?0:Mathf.PI))*walkAmount;
                for(int i=0;i<leg.rest.Length;i++){var p=leg.rest[i];float weight=Mathf.Clamp01((1.25f-p.y)/.8f);p.z+=stride*.18f*weight;p.y+=Mathf.Max(0,stride)*.075f*weight;leg.work[i]=p;}
                leg.mesh.vertices=leg.work;leg.mesh.RecalculateBounds();
            }
            for(int i=0;i<walkingFeet.Count;i++){
                var foot=walkingFeet[i];if(foot==null)continue;float stride=Mathf.Sin(walkPhase+(walkingFootRest[i].x<0?0:Mathf.PI))*walkAmount;
                foot.localPosition=walkingFootRest[i]+new Vector3(0,Mathf.Max(0,stride)*.075f,stride*.18f);
            }
        }
        public void EndCityWalk(){AnimateCityWalk(1,false,false);walkingLegs.Clear();walkingFeet.Clear();walkingFootRest.Clear();}
    }
}
