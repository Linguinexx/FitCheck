using System;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        public void CheckMapFinishing()
        {
            var floor=transform.Find("Arquitetura — CityStone").GetComponent<MeshFilter>().sharedMesh;var vertices=floor.vertices;var uv=floor.uv;int count=0;
            for(int i=0;i<vertices.Length;i++)if(Mathf.Abs(vertices[i].y+.04f)<.0001f){count++;if(Vector2.Distance(uv[i],new Vector2(vertices[i].x/4.7f,vertices[i].z/4.7f))>.001f)throw new Exception("Piso reiniciou a escala/posição da textura");}
            if(count!=4)throw new Exception("Camadas de calçada sobrepostas: "+count);
            var asphalt=transform.Find("Arquitetura — CityAsphalt").GetComponent<MeshFilter>().sharedMesh;var road=asphalt.vertices;
            for(int i=0;i<road.Length;i+=4){var center=(road[i]+road[i+1]+road[i+2]+road[i+3])*.25f;if(Mathf.Abs(center.y+.04f)<.0001f&&center.z>20&&center.z<28)throw new Exception("Faixa de asfalto sobreposta à praça");}
            var leaves=transform.Find("Arquitetura — CityLeaves").GetComponent<MeshFilter>().sharedMesh;foreach(var v in leaves.vertices)if(v.z< -45&&v.y<6.4f)throw new Exception("Folhagem terminal atravessa o muro");
            if(WorldFree(new Vector2(27,-13.8f))||WorldFree(new Vector2(-146,-13.8f)))throw new Exception("Jogador entra no túnel dos carros");
            float savedX=trafficX;try{
                trafficX=24;TickTraffic(1);if(trafficX<=26||traffic.localPosition.x<=26)throw new Exception("Carro não atravessa o portal");
                trafficX=48.9f;TickTraffic(1);if(trafficX!=-153)throw new Exception("Carro não reaparece pelo túnel de entrada");
                trafficX=-147;TickTraffic(1);if(trafficX<=-144.5f)throw new Exception("Carro não sai do túnel de entrada");
            }finally{trafficX=savedX;TickTraffic(0);}
        }
        public void CheckRackFit()
        {
            int pieces=0;foreach(Transform root in transform)if(root.name.StartsWith("Peça na arara")){
                pieces++;foreach(var renderer in root.GetComponentsInChildren<MeshRenderer>())if(renderer.bounds.max.z-transform.position.z>4.8f)throw new Exception("Roupa invade o provador: "+root.name+" / "+renderer.name);
            }
            if(pieces!=9)throw new Exception("Peças dos três drops foram perdidas");
        }
    }
    public partial class FirstCustomer
    {
        void CheckMapPolish(string folder)
        {
            Home();EnterCity();city.CheckMapFinishing();
            var positions=new[]{new Vector3(-93,2.78f,28),new Vector3(-124.8f,2.78f,17),new Vector3(-124.8f,2.78f,4),new Vector3(18,2.78f,-13.8f),new Vector3(-136,2.78f,-13.8f),new Vector3(-60,2.78f,-40)};
            var targets=new[]{new Vector3(-86,0,31),new Vector3(-124.8f,2.1f,13.57f),new Vector3(-124.8f,2.8f,9),new Vector3(26,2.5f,-13.8f),new Vector3(-144.5f,2.5f,-13.8f),new Vector3(-68,3.4f,-35)};
            for(int i=0;i<positions.Length;i++){cam.transform.position=city.transform.position+positions[i];cam.transform.LookAt(city.transform.position+targets[i]);cam.fieldOfView=68;SaveFirstPersonFrame(Path.Combine(folder,"map-fixed-"+i+".png"));}
            Home();EnterShop();city.CheckRackFit();
            cam.transform.position=city.transform.position+new Vector3(-4.2f,2,2.4f);cam.transform.LookAt(city.transform.position+new Vector3(-6.9f,.25f,2.8f));cam.fieldOfView=62;SaveFirstPersonFrame(Path.Combine(folder,"rack-feet-connected.png"));
            cam.transform.position=city.transform.position+new Vector3(-4,2.8f,2.9f);cam.transform.LookAt(city.transform.position+new Vector3(-6.9f,2.6f,4.4f));cam.fieldOfView=65;SaveFirstPersonFrame(Path.Combine(folder,"rack-curtain-clearance.png"));Home();
            Debug.Log("FITCHECK_MAP_POLISH_PASSED: one globally mapped pavement, boundary foliage clearance, vehicle-only tunnel passage/wrap, finished residential front/back and nine garments clear of fitting curtain.");
        }
    }
}
