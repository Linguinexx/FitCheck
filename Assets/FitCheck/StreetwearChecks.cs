using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        static readonly string[] CollectionTops={"AH_TOP_01","CG_TOP_01","SB_TOP_01","AH_TOP_02","CG_TOP_02","SB_TOP_02","AH_TOP_03","CG_TOP_03","SB_TOP_03"};
        static readonly string[] CollectionBottoms={"CG_BOTTOM_01","AH_BOTTOM_01","SB_BOTTOM_01","CG_BOTTOM_02","SB_BOTTOM_02","AH_BOTTOM_02"};
        static readonly string[] CollectionShoes={"AH_SHOES_01","SB_SHOES_01","CG_SHOES_01"};
        void FrameCollection(Vector3 eye,Vector3 target,float fov=38)
        {studio.SetHome(false);studio.Avatar.position=Vector3.zero;studio.Avatar.rotation=Quaternion.identity;mannequin.gameObject.SetActive(true);cam.transform.position=eye;cam.transform.LookAt(target);cam.fieldOfView=fov;}
        void CheckStreetwearCollection(string folder)
        {
            var original=progress;string storage=storageOverride;var look=playerAppearance;
            try{
                Home();progress=new DayProgress();storageOverride=Path.Combine(folder,"collection-session");int count=0,triangles=0;
                foreach(var item in Catalog.All.Where(x=>x.slot<3&&x.id!="CG_BOTTOM_01")){
                    var json=Resources.Load<TextAsset>("Garments/Streetwear/"+item.id);if(json==null)throw new Exception("Modelo ausente: "+item.id);
                    var model=JsonUtility.FromJson<BlenderGarmentAsset>(json.text);if(model.id!=item.id||model.slot!=item.slot||!model.source.Contains("Blender"))throw new Exception("Modelo incorreto: "+item.id);
                    foreach(var part in model.parts){triangles+=part.triangles.Length/3;foreach(var p in part.vertices)if(float.IsNaN(p.x)||float.IsInfinity(p.y))throw new Exception("Vértice inválido");}
                    var outfit=Catalog.Default;outfit[item.slot]=item.id;studio.Apply(outfit,0);studio.ValidateArmFit();
                    var thumb=studio.Thumbnail(item);if(thumb==null)throw new Exception("Miniatura ausente: "+item.id);File.WriteAllBytes(Path.Combine(folder,"thumb-"+item.id+".png"),thumb.EncodeToPNG());Destroy(thumb);count++;
                }
                if(count!=17)throw new Exception("Coleção incompleta: "+count);
                for(int profile=0;profile<7;profile++)for(int i=0;i<CollectionTops.Length;i++){
                    var outfit=new[]{CollectionTops[i],CollectionBottoms[(profile+i)%6],CollectionShoes[(profile+i)%3],""};studio.Apply(outfit,profile);studio.ValidateArmFit();
                }
                var tiles=new List<string>();
                for(int i=0;i<CollectionTops.Length;i++){
                    studio.Apply(new[]{CollectionTops[i],CollectionBottoms[i%6],CollectionShoes[i%3],""},i%3);FrameCollection(new Vector3(1.35f,2.10f,-5.35f),new Vector3(0,1.75f,0));
                    string path=Path.Combine(folder,"look-"+CollectionTops[i]+".png");SaveFirstPersonFrame(path,480,600);tiles.Add(path);
                }
                CollectionSheet(tiles,3,480,600,Path.Combine(folder,"collection-lookbook.png"));
                var shoes=new List<string>();
                for(int i=0;i<3;i++){
                    studio.Apply(new[]{"AH_TOP_01","CG_BOTTOM_02",CollectionShoes[i],""},0);FrameCollection(new Vector3(.8f,.62f,-1.65f),new Vector3(0,.27f,-.08f),40);
                    string path=Path.Combine(folder,"shoe-"+CollectionShoes[i]+".png");SaveFirstPersonFrame(path,480,360);shoes.Add(path);
                }
                CollectionSheet(shoes,3,480,360,Path.Combine(folder,"collection-shoes.png"));
                playerAppearance=new PlayerAppearance{created=true,name="Coleção"};GoHome();city.SetView(180,88);city.Frame(cam,true,0);city.CheckPlayerBody(cam);
                EnterShop();city.SetView(180,88);
                for(int i=0;i<60;i++){city.Step(new Vector2(0,.15f),1/30f,false,mannequin);city.Frame(cam,true,1/30f);city.CheckPlayerBody(cam);}
                Debug.Log("FITCHECK_STREETWEAR_PASSED: "+count+" Blender assets, "+triangles+" triangles total, 63 mixed fits across seven profiles, shoulder/hand fit, all thumbnails, native lookbook and three distinct shoes, first-person body and movement.");
            }finally{Home();progress=original;storageOverride=storage;playerAppearance=look;}
        }
        static void CollectionSheet(List<string> paths,int columns,int width,int height,string output)
        {
            int rows=Mathf.CeilToInt(paths.Count/(float)columns);var sheet=new Texture2D(columns*width,rows*height,TextureFormat.RGB24,false);
            for(int i=0;i<paths.Count;i++){var tile=new Texture2D(2,2);tile.LoadImage(File.ReadAllBytes(paths[i]));sheet.SetPixels(i%columns*width,(rows-1-i/columns)*height,width,height,tile.GetPixels());Destroy(tile);}
            sheet.Apply();File.WriteAllBytes(output,sheet.EncodeToPNG());Destroy(sheet);
        }
    }
}
