using System;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        public void CheckCivilianWardrobe()
        {
            var actors=new System.Collections.Generic.List<Resident>(residents);if(watcher!=null)actors.Add(watcher);
            foreach(var actor in actors){bool shirt=false;int vertices=0;
                foreach(var part in actor.parts){
                    if(part.head)continue;vertices+=part.rest.Length;
                    foreach(var material in part.node.GetComponent<MeshRenderer>().sharedMaterials){
                        string texture=material.mainTexture!=null?material.mainTexture.name:"";
                        if(texture.StartsWith("AH_")||texture.StartsWith("CG_")||texture.StartsWith("SB_")||texture=="BaggyDenim01"||material.name.Contains("Cached clothing"))throw new Exception("Morador usa roupa de catálogo: "+actor.root.name+" / "+texture);
                        if(part.node.name=="Camiseta com caimento"&&material.name.StartsWith("Civilian"))shirt=true;
                    }
                    if(part.node.name.Contains("Estampa")||part.node.name.Contains("Grafismo")||part.node.name.Contains("Identificação"))throw new Exception("Marca da loja no morador");
                }
                if(!shirt||vertices>30000)throw new Exception("Roupa civil ausente ou acima do orçamento: "+actor.root.name+" / "+vertices);
            }
            CheckSleeveSeams();CheckAllBlinks();ValidateFullBodyMotion();Debug.Log("CIVILIAN_WARDROBE_PASSED: seven residents and balcony observer use independent brand-free basics; <=30k body/clothing vertices each, no catalog texture or material, full body gait and spherical blinks.");
        }
        public void CheckSleeveSeams()
        {
            for(int index=0;index<residents.Count;index++){
                var actor=residents[index];float phase=actor.phase,blend=actor.blend;
                try{
                    foreach(float p in new[]{.125f,.625f}){actor.phase=p;actor.blend=1;PoseResident(actor,index,0);var body=ResidentBody(actor,index);
                        foreach(var part in actor.parts)if(part.node.name.Contains("Manga"))for(int v=part.rest.Length-21;v<part.rest.Length;v++)if(Vector3.Distance(part.vertices[v],body.TorsoPoint(part.rest[v]))>.00002f)throw new Exception("Costura da manga abriu na caminhada");
                    }
                }finally{actor.phase=phase;actor.blend=blend;PoseResident(actor,index,0);}
            }
            Debug.Log("SLEEVE_SEAMS_PASSED: shoulder border stays attached in both gait phases on every resident.");
        }
        public void FrameCivilian(Camera camera,int index)
        {
            var actor=residents[index];var root=actor.root;camera.fieldOfView=37;camera.transform.position=root.TransformPoint(new Vector3(.7f,1.95f,-5.4f));camera.transform.LookAt(root.TransformPoint(new Vector3(0,1.75f,0)));
        }
    }
    public partial class FirstCustomer
    {
        void CheckWardrobeFinish(string folder)
        {
            CheckStreetwearCollection(folder);Home();EnterCity();city.CheckCivilianWardrobe();
            for(int i=0;i<city.ResidentCount;i++){city.FrameCivilian(cam,i);SaveFirstPersonFrame(Path.Combine(folder,"civilian-"+i+".png"),480,600);}
            Home();EnterShop();city.CheckSleeveSeams();city.CheckRackFit();city.Frame(cam,true,0);SaveFirstPersonFrame(Path.Combine(folder,"shop-finish.png"));
            CheckShopServiceFlow(folder);CheckSaveSlots(folder);
            File.WriteAllText(Path.Combine(folder,"checks.txt"),"SHOP_REVIEW_PASSED: independent unbranded city basics; store fabric/print/hem and sleeve finishing; 17 Blender catalog assets, 63 fits, original characters and spherical blinks, low civilian geometry budget, racks, service and isolated saves.");
        }
    }
}
