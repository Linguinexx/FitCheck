using System.Collections.Generic;
using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        [System.Serializable] struct NightSource{public Vector3 position;public Color color;public float radius,strength,halo;}
        [SerializeField] List<NightSource> nightSources=new List<NightSource>();
        [SerializeField] Texture2D nightLightField;
        [SerializeField] GameObject nightHalos;
        static readonly Vector4 LightFieldRect=new Vector4(-150,-50,1/180f,1/115f);
        void AddNightSource(Vector3 position,Color color,float radius,float strength,float halo)
        {if(!IsInterior)nightSources.Add(new NightSource{position=position,color=color,radius=radius,strength=strength,halo=halo});}
        void BuildNightLighting()
        {
            foreach(float x in new[]{1.2f,7.5f,14.8f})AddNightSource(new Vector3(x,4.03f,3.08f),new Color(1,.61f,.28f),7,.30f,.26f);
            AddNightSource(new Vector3(7.5f,5.72f,3.57f),new Color(1,.025f,.055f),12,.24f,.30f);
            AddNightSource(new Vector3(-16.8f,4.1f,3.5f),new Color(1,.66f,.32f),10,.55f,.45f);
            AddNightSource(new Vector3(22.1f,4.3f,4.9f),new Color(1,.08f,.04f),7,.32f,.35f);
            AddNightSource(new Vector3(-9,4,-22.5f),new Color(.65f,.15f,1),10,.58f,.45f);
            AddNightSource(new Vector3(12,4,-22.5f),new Color(.08f,1,.63f),10,.48f,.45f);
            AddNightSource(new Vector3(-49,4,-20.7f),new Color(.68f,.16f,1),12,.66f,.55f);
            AddNightSource(new Vector3(-97,3.5f,31),new Color(.12f,.73f,1),12,.48f,.50f);
            AddNightSource(new Vector3(-76,5,41),new Color(1,.72f,.40f),13,.50f,.5f);
            AddNightSource(new Vector3(-124.8f,3.8f,8.9f),new Color(1,.65f,.30f),7,.48f,.35f);
            // Visible LED housings are static members of existing geometry batches.
            foreach(var item in new[]{new Vector4(-9,4.04f,-22.55f,0),new Vector4(12,4.04f,-22.55f,1)})
                Block(new Vector3(item.x,item.y,item.z),new Vector3(15.6f,.06f,.08f),item.w==0?new Color(.65f,.15f,1):new Color(.08f,1,.63f),"CityGlow",.005f);
            const int width=256,height=160;var pixels=new Color[width*height];
            for(int y=0;y<height;y++)for(int x=0;x<width;x++){
                var point=new Vector2(-150+(x+.5f)*180/width,-50+(y+.5f)*115/height);var sum=Color.black;
                foreach(var light in nightSources){float d=(point-new Vector2(light.position.x,light.position.z)).sqrMagnitude;float f=Mathf.Clamp01(1-d/(light.radius*light.radius));sum+=light.color*(f*f*light.strength);}
                pixels[y*width+x]=new Color(Mathf.Min(sum.r,1),Mathf.Min(sum.g,1),Mathf.Min(sum.b,1),1);
            }
            nightLightField=new Texture2D(width,height,TextureFormat.RGBA32,false,true){name="Iluminação colorida do bairro",filterMode=FilterMode.Bilinear,wrapMode=TextureWrapMode.Clamp};
            nightLightField.SetPixels(pixels);nightLightField.Apply(false,false);assets.Add(nightLightField);
            var v=new List<Vector3>();var uv=new List<Vector2>();var normals=new List<Vector3>();var colors=new List<Color>();var triangles=new List<int>();
            foreach(var light in nightSources){
                int first=v.Count;v.AddRange(new[]{light.position,light.position,light.position,light.position});
                uv.AddRange(new[]{Vector2.zero,Vector2.up,Vector2.one,Vector2.right});
                for(int i=0;i<4;i++){normals.Add(new Vector3(light.halo,0,0));colors.Add(light.color);}
                triangles.AddRange(new[]{first,first+1,first+2,first,first+2,first+3});
            }
            var mesh=new Mesh{name="Halos agrupados dos postes e LEDs"};mesh.SetVertices(v);mesh.SetNormals(normals);mesh.SetUVs(0,uv);mesh.SetColors(colors);mesh.SetTriangles(triangles,0);mesh.RecalculateBounds();mesh.bounds=new Bounds(new Vector3(-60,5,7),new Vector3(190,30,130));assets.Add(mesh);
            var material=new Material(Resources.Load<Shader>("CityLightHalo"));assets.Add(material);
            nightHalos=new GameObject("Luzes — halos do bairro");nightHalos.transform.SetParent(transform,false);nightHalos.AddComponent<MeshFilter>().sharedMesh=mesh;nightHalos.AddComponent<MeshRenderer>().sharedMaterial=material;
        }
        void BindNightLighting()
        {
            bool enabled=!IsInterior&&nightLightField!=null;
            Shader.SetGlobalFloat("_CityLightActive",enabled?1:0);
            if(enabled){Shader.SetGlobalTexture("_CityLightField",nightLightField);Shader.SetGlobalVector("_CityLightRect",LightFieldRect);}
        }
        public void PreviewNightLighting(bool enabled)
        {Shader.SetGlobalFloat("_CityLightActive",enabled?1:0);if(nightHalos!=null)nightHalos.SetActive(enabled);}
    }
}
