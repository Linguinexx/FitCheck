using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        void CheckBaseSneaker(string folder)
        {
            var json=Resources.Load<TextAsset>("Garments/Civilian/CIV_SHOES_BASE");
            if(json==null)throw new Exception("Everyday sneaker missing");
            var model=JsonUtility.FromJson<BlenderGarmentAsset>(json.text);int triangles=0;
            var bounds=new Bounds();bool first=true;
            foreach(var part in model.parts){triangles+=part.triangles.Length/3;foreach(var p in part.vertices){if(p.y<.09f||p.y>.49f||p.z<-.51f||p.z>.31f)throw new Exception("Everyday sneaker axes/scale invalid");if(first){bounds=new Bounds(p,Vector3.zero);first=false;}else bounds.Encapsulate(p);}}
            if(Mathf.Abs(bounds.size.x/bounds.size.z-.40146f)>.015f||Mathf.Abs(bounds.size.y/bounds.size.z-.47678f)>.015f)throw new Exception("Everyday sneaker original proportions changed");
            var texture=Resources.Load<Texture2D>("Garments/Civilian/CIV_SHOES_BASE");
            if(triangles>2400||triangles<100||texture==null||texture.width!=512||texture.mipmapCount<2)throw new Exception("Everyday sneaker exceeded budget or lost texture");
            var look=studio.PlayerLook;var tiles=new List<string>();
            try{
                for(int id=0;id<9;id++){
                    studio.PlayerLook=new PlayerAppearance{created=true,skin=id%6,hair=id%7};studio.ApplyCivilian(id);studio.ValidateArmFit();int shoes=0;
                    foreach(var renderer in mannequin.GetComponentsInChildren<MeshRenderer>())if(renderer.name.StartsWith("Tênis")){
                        shoes++;if(renderer.sharedMaterials.Length!=1||renderer.sharedMaterial.mainTexture!=texture||renderer.GetComponent<MeshFilter>().sharedMesh.triangles.Length/3!=triangles)throw new Exception("Civilian footwear not using supplied base");
                    }
                    if(shoes!=2)throw new Exception("Everyday pair must have two renderers");
                    if(id<3){FrameCollection(new Vector3(.8f,.62f,-1.65f),new Vector3(0,.27f,-.08f),40);string path=Path.Combine(folder,"base-shoe-"+id+".png");SaveFirstPersonFrame(path,480,360);tiles.Add(path);}
                }
                CollectionSheet(tiles,3,480,360,Path.Combine(folder,"base-shoes.png"));
                Home();EnterCity();city.ValidateFullBodyMotion();city.CheckAllBlinks();city.Frame(cam,true,0);SaveFirstPersonFrame(Path.Combine(folder,"civilian-street.png"));Home();
                Debug.Log("BASE_SNEAKER_PASSED: nine civilian looks; "+triangles+" triangles per foot; one 512 atlas; two single-material renderers per pair; independent from shop catalog.");
            }finally{studio.PlayerLook=look;}
        }
    }
}
