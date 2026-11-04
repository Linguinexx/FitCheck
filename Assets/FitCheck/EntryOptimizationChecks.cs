using System;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        bool referenceSkin;
        public void CheckSkinEquivalence()
        {
            float maximum=0,normalMaximum=0;int checkedVertices=0;var actors=new System.Collections.Generic.List<Resident>(residents);if(playerBody!=null)actors.Add(playerBody);
            foreach(var actor in actors){
                float phase=actor.phase,blend=actor.blend,lean=actor.leanBlend;
                try{
                    foreach(float gait in new[]{0f,.125f,.625f})foreach(float amount in new[]{0f,.4f,1f}){
                        actor.phase=gait;actor.blend=amount;if(actor.wallLean)actor.leanBlend=amount;
                        var body=ResidentBody(actor,2);var left=ResidentLeg(actor,-1,.035f,body);var right=ResidentLeg(actor,1,.035f,body);denimLeft=left;denimRight=right;
                        foreach(var part in actor.parts){
                            if(part.head)continue;var leg=part.side<0?left:right;referenceSkin=false;PoseResidentPart(part,actor,2,body,leg);
                            var p=(Vector3[])part.vertices.Clone();var n=(Vector3[])part.posedNormals.Clone();referenceSkin=true;PoseResidentPart(part,actor,2,body,leg);
                            for(int i=0;i<p.Length;i++){maximum=Mathf.Max(maximum,Vector3.Distance(p[i],part.vertices[i]));normalMaximum=Mathf.Max(normalMaximum,Vector3.Distance(n[i],part.posedNormals[i]));checkedVertices++;}
                        }
                    }
                }finally{referenceSkin=false;actor.phase=phase;actor.blend=blend;actor.leanBlend=lean;PoseResident(actor,2,0);}
            }
            if(maximum>.00003f||normalMaximum>.00003f)throw new Exception("Skin cache changed motion: "+maximum+" / normals "+normalMaximum);
            Debug.Log("ENTRY_SKIN_EQUIVALENCE_PASSED: "+checkedVertices+" vertex poses; max error="+maximum+"; normal error="+normalMaximum);
        }
    }
    public partial class FirstCustomer
    {
        void CheckEntryOptimization(string folder)
        {
            CheckStreetwearCollection(folder);CheckShopServiceFlow(folder);CheckSaveSlots(folder);
            Home();EnterCity();city.CheckSkinEquivalence();city.ValidateFullBodyMotion();city.CheckAllBlinks();
            city.SetView(180,88);city.Frame(cam,true,0);city.CheckPlayerBody(cam);SaveFirstPersonFrame(Path.Combine(folder,"player-body.png"));
            Home();EnterShop();city.CheckSkinEquivalence();city.CheckRackFit();city.CheckAllBlinks();city.Frame(cam,true,0);SaveFirstPersonFrame(Path.Combine(folder,"shop.png"));
            Home();Debug.Log("ENTRY_REGRESSION_PASSED: exact cached skinning, full body gait, all blinks, 17 Blender garments, 63 fits, shop displays, service and save isolation.");
            File.WriteAllText(Path.Combine(folder,"checks.txt"),"SHOP_REVIEW_PASSED: entry optimization, cached skin/reference geometry equivalence, full body gait, blinks, 17 Blender garments, 63 fits, shop displays, service and save isolation.");
        }
    }
}
