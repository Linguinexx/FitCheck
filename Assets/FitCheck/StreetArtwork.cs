using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        void StreetArtDisplays()
        {
            Billboard("Bliss — residencial",new Vector3(-137,11.5f,5.40f),0,12,new Vector2(7.7f,4.6f),9.08f);
            Billboard("Nuvens — mercearia",new Vector3(-112,11.25f,5.46f),0,11,new Vector2(5.8f,3.8f),9.08f);
            Billboard("Superstar — oficina",new Vector3(-31.5f,11.55f,5.40f),0,15,new Vector2(7.4f,4.2f),9.08f);
            Billboard("Not of this Earth — quadra",new Vector3(-68.5f,7.7f,50.08f),0,10,new Vector2(7,3.95f),2.72f);
            Billboard("Do More — casa de shows",new Vector3(-49,11.6f,-20.40f),180,13,new Vector2(7.8f,4.3f),9.08f);
            FramedPrint("Cartaz urbano — Fade Away",new Vector3(-87.85f,2.7f,41),-90,8,new Vector2(1.45f,2.1f));
            FramedPrint("Cartaz urbano — cadeira",new Vector3(-122.45f,2.2f,8.63f),0,9,new Vector2(.95f,1.55f));
            FramedPrint("Cartaz urbano — The World Is Yours",new Vector3(-59.14f,2.6f,-26),90,14,new Vector2(1.8f,1.8f));
            FramedPrint("Cartaz urbano — Street",new Vector3(-105.35f,2.7f,11),-90,0,new Vector2(1.5f,1.8f));
            FramedPrint("Cartaz urbano — Nuvem",new Vector3(-138.9f,2.6f,5.63f),0,16,new Vector2(1.15f,1.5f));
            FramedPrint("Cartaz urbano — Made in Brazil",new Vector3(-105.12f,2.5f,13.5f),90,17,new Vector2(1.3f,1.25f));
            Billboard("Don't Panic — pátio musical",new Vector3(-28.5f,5.8f,-43.80f),180,18,new Vector2(4.2f,2.8f),3.2f);
            FramedPrint("Cartaz urbano — Art",new Vector3(-96.5f,2.05f,35.20f),0,19,new Vector2(1.5f,2.3f));
            FramedPrint("Cartaz urbano — Estrela",new Vector3(15.68f,2.6f,7.3f),-90,20,new Vector2(1.25f,1.25f));
            FramedPrint("Cartaz urbano — Céu da cidade",new Vector3(-135.1f,2.6f,-18.83f),180,21,new Vector2(1.2f,1.6f));
            FramedPrint("Cartaz urbano — Disco voador",new Vector3(-23,2.6f,-43.80f),180,22,new Vector2(1.0f,1.8f));
            FramedPrint("Cartaz urbano — Sonho",new Vector3(-101,2.05f,35.20f),0,23,new Vector2(3.1f,2.3f));
            FramedPrint("Cartaz urbano — Flamengo",new Vector3(-79.6f,1.35f,50.03f),0,24,new Vector2(1.25f,1.25f));
            FramedPrint("Cartaz urbano — Weirdo",new Vector3(-24.94f,2.5f,12),-90,25,new Vector2(1.25f,1.25f));
        }
        void Billboard(string name,Vector3 position,float yaw,int index,Vector2 maximum,float baseHeight)
        {
            var root=FramedPrint("Outdoor — "+name,position,yaw,index,maximum);
            var size=root.Find("Arte fornecida").localScale;float length=position.y-baseHeight;
            foreach(float side in new[]{-1f,1f}){
                LiveBox("Pilar do outdoor",new Vector3(side*size.x*.34f,-length*.5f,.24f),new Vector3(.16f,length,.20f),Ink,"CityMetal",root);
                LiveBox("Braço do refletor",new Vector3(side*size.x*.28f,size.y*.5f+.12f,-.15f),new Vector3(.10f,.12f,.55f),Ink,"CityMetal",root);
                LiveBox("Refletor do outdoor",new Vector3(side*size.x*.28f,size.y*.5f+.07f,-.43f),new Vector3(.36f,.10f,.22f),Cream,"CityGlow",root);
            }
            LiveBox("Travessa do outdoor",new Vector3(0,-size.y*.35f,.22f),new Vector3(size.x*.80f,.12f,.14f),Ink,"CityMetal",root);
        }
        public void CheckStreetArt()
        {
            int boards=0,posters=0;
            foreach(Transform root in transform){
                if(root.name.StartsWith("Outdoor — ")){
                    boards++;var image=root.Find("Arte fornecida");var texture=image.GetComponent<MeshRenderer>().sharedMaterial.mainTexture;
                    if(texture.width<=texture.height||Mathf.Abs(image.localScale.x/image.localScale.y-texture.width/(float)texture.height)>.001f)throw new System.Exception("Outdoor esticou a arte horizontal");
                    int posts=0;foreach(Transform child in root)if(child.name=="Pilar do outdoor")posts++;if(posts!=2)throw new System.Exception("Outdoor sem suporte");
                }
                if(root.name.StartsWith("Cartaz urbano — "))posters++;
            }
            if(boards!=6||posters!=13)throw new System.Exception("Artes urbanas incompletas: "+boards+" / "+posters);
        }
    }
    public partial class FirstCustomer
    {
        void CheckStreetArtwork(string folder)
        {
            for(int i=0;i<GalleryArt.Names.Length;i++)if(GalleryArt.Load(i)==null)throw new System.Exception("Arte fornecida ausente: "+i);
            Home();EnterCity();city.CheckStreetArt();
            var route=new System.Collections.Generic.List<Vector2>();if(!city.PlanRoute(CityView.StreetHomeDoor,CityView.StreetShopDoor,route))throw new System.Exception("Arte bloqueou percurso");
            var cameras=new[]{new Vector3(-137,3,-3),new Vector3(-112,3,-3),new Vector3(-31.5f,3,-3),new Vector3(-76,3,39),new Vector3(-49,3,-10),new Vector3(-83.5f,2.8f,41)};
            var targets=new[]{new Vector3(-137,11.5f,5.4f),new Vector3(-112,11.25f,5.46f),new Vector3(-31.5f,11.55f,5.4f),new Vector3(-68.5f,7.7f,50.08f),new Vector3(-49,11.6f,-20.4f),new Vector3(-87.85f,2.7f,41)};
            for(int i=0;i<cameras.Length;i++){cam.transform.position=city.transform.position+cameras[i];cam.transform.LookAt(city.transform.position+targets[i]);cam.fieldOfView=i==3?62:68;SaveFirstPersonFrame(System.IO.Path.Combine(folder,"street-art-"+i+".png"));}
            Home();Debug.Log("FITCHECK_STREET_ARTWORK_PASSED: all artwork assets, six supported landscape billboards, thirteen facade posters, full artwork UV/aspect and residential/store route accessible.");
        }
    }
}
