using UnityEngine;
namespace FitCheck
{
    public static class GalleryArt
    {
        public static readonly string[] Names={
            "Rua","Música","C.R.E.A.M.","Risco","Crie","Sol","Fortuna","Som",
            "Fade","Pausa","Órbita","Nuvens","Bliss","Impulso","Mundo","Superstar",
            "Temporal","Origem","Calma","Arte","Estrela","Miragem","Contato","Sonho","Mengão","Estranho"
        };
        public static Texture2D Load(int index){return Resources.Load<Texture2D>("Artwork/Poster"+index.ToString("D2"));}
    }
    public partial class CityView
    {
        Transform FramedPrint(string name,Vector3 position,float yaw,int index,Vector2 maximum)
        {
            var root=new GameObject(name).transform;root.SetParent(transform,false);root.localPosition=position;root.localRotation=Quaternion.Euler(0,yaw,0);
            LiveBox("Moldura",new Vector3(0,0,.04f),Vector3.one,Ink,"Wood",root);
            LiveBox("Passe-partout",new Vector3(0,0,-.009f),Vector3.one,Cream,"Paper",root);
            var mesh=new Mesh{name="Impressão — UV inteira",vertices=new[]{new Vector3(-.5f,-.5f,0),new Vector3(-.5f,.5f,0),new Vector3(.5f,.5f,0),new Vector3(.5f,-.5f,0)},uv=new[]{Vector2.zero,Vector2.up,Vector2.one,Vector2.right},triangles=new[]{0,1,2,0,2,3}};mesh.RecalculateNormals();mesh.RecalculateBounds();assets.Add(mesh);
            var image=new GameObject("Arte fornecida").transform;image.SetParent(root,false);image.localPosition=new Vector3(0,0,-.041f);image.gameObject.AddComponent<MeshFilter>().sharedMesh=mesh;
            var material=new Material(Resources.Load<Shader>("Artwork"));assets.Add(material);image.gameObject.AddComponent<MeshRenderer>().sharedMaterial=material;
            SetFramedPrint(root,index,maximum);return root;
        }
        void SetFramedPrint(Transform root,int index,Vector2 maximum)
        {
            var texture=GalleryArt.Load(index);if(texture==null)throw new System.InvalidOperationException("Arte ausente: "+index);
            float aspect=texture.width/(float)texture.height,h=Mathf.Min(maximum.y,maximum.x/aspect),w=h*aspect;
            root.Find("Moldura").localScale=new Vector3(w+.14f,h+.14f,.08f);
            root.Find("Passe-partout").localScale=new Vector3(w+.045f,h+.045f,.04f);
            var image=root.Find("Arte fornecida");image.localScale=new Vector3(w,h,1);image.GetComponent<MeshRenderer>().sharedMaterial.mainTexture=texture;
        }
        public void CheckPrints()
        {
            int count=0;foreach(var renderer in GetComponentsInChildren<MeshRenderer>(true))if(renderer.name=="Arte fornecida"){
                count++;var texture=renderer.sharedMaterial.mainTexture;var size=renderer.transform.localScale;
                if(texture==null||Mathf.Abs(size.x/size.y-texture.width/(float)texture.height)>.001f||!renderer.sharedMaterial.shader.isSupported)throw new System.Exception("Quadro distorcido ou sem textura");
            }
            if(count<(IsHome?2:6))throw new System.Exception("Quadros fornecidos não foram aplicados");
        }
    }
}
