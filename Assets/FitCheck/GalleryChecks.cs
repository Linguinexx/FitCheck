using System;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        void CheckGalleryFix(string folder)
        {
            for(int i=0;i<8;i++)if(GalleryArt.Load(i)==null)throw new Exception("Arte fornecida ausente: "+i);
            Home();EnterHome();if(StreetBubblesAllowed)throw new Exception("Balão de rua permitido em casa");city.CheckPrints();
            cam.transform.position=city.transform.position+new Vector3(.6f,2.8f,.2f);cam.transform.LookAt(city.transform.position+new Vector3(-3.4f,2.9f,4.37f));cam.fieldOfView=58;SaveFirstPersonFrame(Path.Combine(folder,"home-user-art.png"));
            Home();EnterCity();if(!StreetBubblesAllowed)throw new Exception("Balões de rua desativados no bairro");var outside=city;
            Home();EnterShop();if(StreetBubblesAllowed||outside.gameObject.activeInHierarchy)throw new Exception("Rua/balões vazaram para loja");
            for(int i=0;i<8;i++){city.ApplyShopDecor(new ShopDecor{poster=i});city.CheckPrints();}
            city.ApplyShopDecor(new ShopDecor());
            cam.transform.position=city.transform.position+new Vector3(1.3f,3.65f,1);cam.transform.LookAt(city.transform.position+new Vector3(-7.65f,3.92f,1));cam.fieldOfView=60;SaveFirstPersonFrame(Path.Combine(folder,"shop-user-art-gallery.png"));
            cam.transform.position=city.transform.position+new Vector3(4.5f,3.65f,1);cam.transform.LookAt(city.transform.position+new Vector3(7.65f,3.96f,1));cam.fieldOfView=62;SaveFirstPersonFrame(Path.Combine(folder,"shop-user-art-shoes.png"));
            Home();EnterCity();if(!StreetBubblesAllowed)throw new Exception("Retorno ao bairro não restaurou os balões");Home();
            Debug.Log("FITCHECK_GALLERY_FIX_PASSED: all eight supplied artworks, original aspect ratio/UV/shader, home/store placement, all decor choices, street balloons allowed only outdoors and outdoor scene hidden inside.");
        }
    }
}
