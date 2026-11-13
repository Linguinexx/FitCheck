using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        [SerializeField] Transform homeMirrorTrim;
        Material homeMirrorTrimMaterial;
        public int HomeMirrorStyle {get;private set;}
        public static readonly string[] MirrorStyleNames={"Original","Paint 95","Fit Player"};
        void BuildHomeMirrorTrim(){
            var vertices=new System.Collections.Generic.List<Vector3>();var uv=new System.Collections.Generic.List<Vector2>();var triangles=new System.Collections.Generic.List<int>();
            void Strip(float x,float y,float right,float bottom){
                int a=vertices.Count;
                foreach(var p in new[]{new Vector2(x,y),new Vector2(right,y),new Vector2(right,bottom),new Vector2(x,bottom)}){
                    vertices.Add(new Vector3(-.026f,HomeMirrorHalfHeight-(p.y-70)*(2*HomeMirrorHalfHeight/878),HomeMirrorHalfWidth-(p.x-100)*(2*HomeMirrorHalfWidth/392)));
                    uv.Add(new Vector2(p.x/512,1-p.y/1024));
                }
                triangles.AddRange(new[]{a,a+1,a+2,a,a+2,a+3});
            }
            Strip(0,0,512,70);Strip(0,948,512,1024);Strip(0,70,100,948);Strip(492,70,512,948);
            var mesh=new Mesh{name="Moldura retro — vidro livre"};mesh.SetVertices(vertices);mesh.SetUVs(0,uv);mesh.SetTriangles(triangles,0);mesh.RecalculateNormals();mesh.RecalculateBounds();assets.Add(mesh);
            homeMirrorTrim=new GameObject("Moldura do espelho — programa retro").transform;homeMirrorTrim.SetParent(transform,false);homeMirrorTrim.localPosition=HomeMirrorCenter;
            homeMirrorTrim.gameObject.AddComponent<MeshFilter>().sharedMesh=mesh;homeMirrorTrimMaterial=new Material(Resources.Load<Shader>("Artwork"));assets.Add(homeMirrorTrimMaterial);
            homeMirrorTrim.gameObject.AddComponent<MeshRenderer>().sharedMaterial=homeMirrorTrimMaterial;homeMirrorTrim.gameObject.SetActive(false);
        }
        void ApplyHomeMirrorStyle(int style){
            HomeMirrorStyle=style;if(homeMirrorTrim==null)return;
            homeMirrorTrim.gameObject.SetActive(style!=0);
            if(style!=0){if(homeMirrorTrimMaterial==null)homeMirrorTrimMaterial=homeMirrorTrim.GetComponent<MeshRenderer>().sharedMaterial;
                homeMirrorTrimMaterial.mainTexture=Resources.Load<Texture2D>("Decor/"+(style==1?"Paint95":"FitPlayer"));}
        }
    }
}
