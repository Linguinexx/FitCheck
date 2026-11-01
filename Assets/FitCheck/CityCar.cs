using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class CityView
    {
        [SerializeField] List<Transform> trafficWheels=new List<Transform>();
        void CarSurface(string name,Vector3[][] faces,Color color,string texture="Leather")
        {
            var v=new List<Vector3>();var uv=new List<Vector2>();var triangles=new List<int>();var colors=new List<Color>();
            foreach(var face in faces){
                int start=v.Count;v.AddRange(face);uv.AddRange(new[]{Vector2.zero,Vector2.up,Vector2.one,Vector2.right});
                for(int j=0;j<4;j++)colors.Add(color);
                triangles.AddRange(new[]{start,start+1,start+2,start,start+2,start+3});
            }
            var mesh=new Mesh{name=name};mesh.SetVertices(v);mesh.SetUVs(0,uv);mesh.SetColors(colors);mesh.SetTriangles(triangles,0);mesh.RecalculateNormals();mesh.RecalculateBounds();assets.Add(mesh);
            var go=new GameObject(name);go.transform.SetParent(traffic,false);go.AddComponent<MeshFilter>().sharedMesh=mesh;
            var material=CityMaterial(texture);material.SetFloat("_Cull",0);assets.Add(material);go.AddComponent<MeshRenderer>().sharedMaterial=material;
        }
        void BuildStreetCar()
        {
            // Chamfered hatchback body, sloped cabin and separated glass panels.
            var outline=new[]{new Vector2(-1.86f,-.57f),new Vector2(-1.61f,-.81f),new Vector2(1.49f,-.81f),new Vector2(1.87f,-.56f),new Vector2(1.87f,.56f),new Vector2(1.49f,.81f),new Vector2(-1.61f,.81f),new Vector2(-1.86f,.57f)};
            var faces=new List<Vector3[]>();
            for(int i=0;i<8;i++){
                var a=outline[i];var b=outline[(i+1)%8];
                faces.Add(new[]{new Vector3(a.x,.43f,a.y),new Vector3(a.x,.93f,a.y),new Vector3(b.x,.93f,b.y),new Vector3(b.x,.43f,b.y)});
                faces.Add(new[]{new Vector3(0,.99f,0),new Vector3(a.x,.93f,a.y),new Vector3(b.x,.93f,b.y),new Vector3(0,.99f,0)});
            }
            CarSurface("Carro — carroceria chanfrada",faces.ToArray(),Kaos);
            var cabin=new[]{new Vector3(-1.43f,1.00f,-.72f),new Vector3(-.88f,1.67f,-.57f),new Vector3(.27f,1.67f,-.57f),new Vector3(.98f,1.0f,-.72f)};
            faces.Clear();faces.Add(cabin);var back=new Vector3[4];for(int i=0;i<4;i++)back[i]=new Vector3(cabin[i].x,cabin[i].y,-cabin[i].z);faces.Add(back);
            for(int i=0;i<3;i++)faces.Add(new[]{cabin[i],back[i],back[i+1],cabin[i+1]});
            CarSurface("Carro — teto e colunas",faces.ToArray(),Kaos*.8f);
            var glass=new Color(.31f,.28f,.24f);
            for(int side=-1;side<=1;side+=2){
                CarSurface("Carro — vidro lateral",new[]{new[]{new Vector3(-1.25f,1.08f,side*.704f),new Vector3(-.83f,1.58f,side*.60f),new Vector3(.23f,1.58f,side*.60f),new Vector3(.79f,1.08f,side*.704f)}},glass);
                LiveBox("Carro — coluna do vidro",new Vector3(-.32f,1.33f,side*.666f),new Vector3(.08f,.52f,.035f),Ink,"Leather",traffic);
                LiveBox("Carro — friso",new Vector3(-.05f,.68f,side*.823f),new Vector3(2.65f,.055f,.035f),Ink,"Rubber",traffic);
                LiveBox("Carro — maçaneta",new Vector3(.05f,.88f,side*.83f),new Vector3(.22f,.045f,.04f),Cream,"Leather",traffic);
                foreach(float x in new[]{-1.15f,1.18f}){
                    var go=GameObject.CreatePrimitive(PrimitiveType.Cylinder);go.name="Carro — roda e calota";Destroy(go.GetComponent<Collider>());
                    go.transform.SetParent(traffic,false);go.transform.localPosition=new Vector3(x,.40f,side*.81f);go.transform.localScale=new Vector3(.67f,.13f,.67f);go.transform.localRotation=Quaternion.Euler(90,0,0);
                    var mesh=Instantiate(go.GetComponent<MeshFilter>().sharedMesh);var colors=new Color[mesh.vertexCount];for(int i=0;i<colors.Length;i++)colors[i]=new Color(.08f,.075f,.07f);mesh.colors=colors;assets.Add(mesh);go.GetComponent<MeshFilter>().sharedMesh=mesh;
                    var material=CityMaterial("Rubber");assets.Add(material);go.GetComponent<MeshRenderer>().sharedMaterial=material;trafficWheels.Add(go.transform);
                    LiveBox("Calota",new Vector3(0,side*.99f,0),new Vector3(.36f,.03f,.36f),new Color(.52f,.49f,.42f),"Leather",go.transform);
                    LiveBox("Raio da calota",new Vector3(0,side*1.02f,0),new Vector3(.11f,.04f,.60f),Cream,"Leather",go.transform);
                }
            }
            CarSurface("Carro — para-brisa",new[]{new[]{new Vector3(.91f,1.08f,-.65f),new Vector3(.32f,1.60f,-.53f),new Vector3(.32f,1.60f,.53f),new Vector3(.91f,1.08f,.65f)}},glass);
            CarSurface("Carro — vidro traseiro",new[]{new[]{new Vector3(-1.38f,1.08f,-.65f),new Vector3(-.91f,1.60f,-.53f),new Vector3(-.91f,1.60f,.53f),new Vector3(-1.38f,1.08f,.65f)}},glass*.75f);
            foreach(float z in new[]{-.49f,.49f}){
                LiveBox("Farol",new Vector3(1.884f,.78f,z),new Vector3(.04f,.20f,.30f),new Color(1,.82f,.45f),"CityGlow",traffic);
                LiveBox("Lanterna",new Vector3(-1.876f,.78f,z),new Vector3(.04f,.17f,.24f),Kaos,"CityGlow",traffic);
            }
            foreach(float x in new[]{-1.89f,1.90f})LiveBox("Para-choque",new Vector3(x,.49f,0),new Vector3(.12f,.13f,1.24f),Ink,"Rubber",traffic);
            LiveBox("Placa",new Vector3(1.972f,.59f,0),new Vector3(.025f,.13f,.36f),Cream,"Leather",traffic);
        }
    }
}
