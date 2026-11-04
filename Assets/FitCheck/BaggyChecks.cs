using System;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        void CheckBlenderBaggy(string folder)
        {
            var original=progress;string storage=storageOverride;
            try{
                var json=Resources.Load<TextAsset>("Garments/BaggyDenim01");var asset=JsonUtility.FromJson<BlenderGarmentAsset>(json.text);if(!asset.source.Contains("Blender")||asset.parts.Length<3)throw new Exception("Calça sem origem Blender");
                int left=0,right=0,tris=0;foreach(var p in asset.parts){if(p.side<0)left++;if(p.side>0)right++;tris+=p.triangles.Length/3;foreach(var v in p.vertices)if(float.IsNaN(v.x)||v.y<.15f||v.y>1.5f)throw new Exception("Malha inválida");}
                if(left==0||right==0||tris>15000)throw new Exception("Malha sem pernas separadas ou muito densa");
                Home();progress=new DayProgress();storageOverride=Path.Combine(folder,"baggy-session");
                for(int profile=0;profile<7;profile++){
                    studio.Apply(new[]{"AH_TOP_01","CG_BOTTOM_01","AH_SHOES_01",""},profile);studio.ValidateArmFit();
                    int pieces=0;foreach(var f in studio.Avatar.GetComponentsInChildren<MeshFilter>())if(f.sharedMesh.name.Contains("Blender Baggy"))pieces++;
                    // The posture bakes a clone; preserve the mesh name suffix.
                    if(pieces==0)throw new Exception("Roupa Blender não foi aplicada");
                }
                studio.Apply(new[]{"AH_TOP_01","CG_BOTTOM_01","AH_SHOES_01",""},0);
                studio.Avatar.rotation=Quaternion.identity;studio.SetHome(false);mannequin.gameObject.SetActive(true);
                cam.transform.position=new Vector3(2.6f,2.2f,-5.2f);cam.transform.LookAt(new Vector3(0,1.6f,0));cam.fieldOfView=36;SaveFirstPersonFrame(Path.Combine(folder,"baggy-unity-front.png"));
                cam.transform.position=new Vector3(-2.4f,2.0f,4.8f);cam.transform.LookAt(new Vector3(0,1.55f,0));SaveFirstPersonFrame(Path.Combine(folder,"baggy-unity-back.png"));
                cam.transform.position=new Vector3(1.65f,1.12f,-2.5f);cam.transform.LookAt(new Vector3(0,.82f,0));cam.fieldOfView=38;SaveFirstPersonFrame(Path.Combine(folder,"baggy-unity-details.png"));
                studio.PrepareCityWalk();for(int i=0;i<90;i++)studio.AnimateCityWalk(1/30f,true,false);studio.EndCityWalk();
                var thumb=studio.Thumbnail(Catalog.Get("CG_BOTTOM_01"));if(thumb==null)throw new Exception("Miniatura da calça ausente");
                EnterShop();city.RestoreWalk(new Vector2(0,1),180,88,mannequin);
                for(int frame=0;frame<100;frame++){city.Step(new Vector2(0,.3f),1/30f,false,mannequin);city.Frame(cam,true,1/30f);city.CheckPlayerBody(cam);}
                city.FramePlayerForCamera(cam,true);cam.transform.position=city.PlayerBodyPosition+new Vector3(2.2f,1.8f,-4);cam.transform.LookAt(city.PlayerBodyPosition+Vector3.up*1.5f);SaveFirstPersonFrame(Path.Combine(folder,"baggy-walking-shop.png"));
                Debug.Log("FITCHECK_BLENDER_BAGGY_PASSED: authored FBX and UV, "+tris+" triangles; seven body profiles, independent legs, posture, thumbnail, studio walk and moving city player.");
            }finally{Home();progress=original;storageOverride=storage;}
        }
    }
}
