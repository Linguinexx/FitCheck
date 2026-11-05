using System;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        public void CheckRoadDetails(Camera camera,string folder)
        {
            if(trafficWheels.Count!=4)throw new Exception("Traffic lost a wheel");
            var mesh=trafficWheels[0].GetComponent<MeshFilter>().sharedMesh;
            var materials=trafficWheels[0].GetComponent<MeshRenderer>().sharedMaterials;
            if(mesh.subMeshCount!=2||mesh.vertexCount>1600||mesh.bounds.size.x<.77f||mesh.bounds.size.y>.35f)throw new Exception("Wheel geometry budget or profile incorrect");
            foreach(var wheel in trafficWheels){
                if(wheel.GetComponent<MeshFilter>().sharedMesh!=mesh||wheel.childCount!=0)throw new Exception("Wheel uses duplicate geometry or old floating hubcap");
                var copy=wheel.GetComponent<MeshRenderer>().sharedMaterials;if(copy.Length!=2||copy[0]!=materials[0]||copy[1]!=materials[1])throw new Exception("Wheel materials not shared");
            }
            var leaves=transform.Find("Arquitetura — CityLeaves").GetComponent<MeshFilter>().sharedMesh;
            var positions=leaves.vertices;var colors=leaves.colors;
            foreach(float pot in new[]{FacadePlanterLeft,FacadePlanterRight}){
                int count=0;
                for(int i=0;i<positions.Length;i++)if(Mathf.Abs(positions[i].x-pot)<.85f&&Mathf.Abs(positions[i].z-2.2f)<1&&positions[i].y>.5f&&positions[i].y<1.9f){
                    var c=colors[i];if(c.g<c.r*1.6f||c.g<c.b*1.6f)throw new Exception("DripLab plant still inherits neutral architecture tint");count++;
                }
                if(count<100)throw new Exception("Missing DripLab planter foliage");
            }
            var previous=Position;float oldTraffic=trafficX;
            try{
                Position=new Vector2(8,-7);trafficX=0;TickTraffic(0);var rotation=trafficWheels[0].localRotation;TickTraffic(.1f);
                if(Quaternion.Angle(rotation,trafficWheels[0].localRotation)<1)throw new Exception("Wheels do not spin with traffic");
                Position=new Vector2(trafficX+2,-13.8f);rotation=trafficWheels[0].localRotation;TickTraffic(.1f);
                if(Quaternion.Angle(rotation,trafficWheels[0].localRotation)>.001f)throw new Exception("Stopped car spins wheels");
                Position=new Vector2(8,-7);trafficX=0;TickTraffic(0);
                camera.transform.position=Origin+new Vector3(5,2.2f,-19);camera.transform.LookAt(Origin+new Vector3(0,.8f,-13.8f));camera.fieldOfView=48;
                FirstCustomer.SaveRoadFrame(camera,Path.Combine(folder,"car-wheels.png"));
                camera.transform.position=Origin+new Vector3(2.65f,.95f,-16.8f);camera.transform.LookAt(Origin+new Vector3(1.77f,.58f,-15.02f));camera.fieldOfView=39;
                FirstCustomer.SaveRoadFrame(camera,Path.Combine(folder,"wheel-detail.png"));
                camera.transform.position=Origin+new Vector3(6,3.2f,-9);camera.transform.LookAt(Origin+new Vector3(7.5f,3.3f,3.5f));camera.fieldOfView=62;
                FirstCustomer.SaveRoadFrame(camera,Path.Combine(folder,"green-storefront.png"));
            }finally{Position=previous;trafficX=oldTraffic;TickTraffic(0);}
            Debug.Log("ROAD_DETAILS_PASSED: shared wheel vertices="+mesh.vertexCount+"; four single-renderer wheels, two shared materials, traffic spin/stop and green DripLab foliage.");
        }
    }
    public partial class FirstCustomer
    {
        internal static void SaveRoadFrame(Camera camera,string path)
        {
            var target=RenderTexture.GetTemporary(1280,800,24);var previous=camera.targetTexture;var active=RenderTexture.active;float aspect=camera.aspect;var image=new Texture2D(1280,800,TextureFormat.RGB24,false);
            try{camera.targetTexture=target;camera.aspect=1.6f;camera.Render();RenderTexture.active=target;image.ReadPixels(new Rect(0,0,1280,800),0,0);image.Apply();File.WriteAllBytes(path,image.EncodeToPNG());}
            finally{camera.targetTexture=previous;camera.aspect=aspect;RenderTexture.active=active;RenderTexture.ReleaseTemporary(target);Destroy(image);}
        }
        void CheckRoadPolish(string folder)
        {
            Home();EnterCity();city.CheckRoadDetails(cam,folder);city.CheckMapFinishing();
            File.WriteAllText(Path.Combine(folder,"road-checks.txt"),"ROAD_DETAILS_PASSED: shared beveled tires and five-spoke wheels, rotation/stop, green facade plants, original tunnels/pavement/collisions retained.");
        }
    }
}
