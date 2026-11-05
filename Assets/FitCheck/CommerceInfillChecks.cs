using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        void CheckCommerceInfill(string folder)
        {
            Home();EnterCity();paused=false;
            if(city.CommerceInfillTriangles<=0||city.CommerceInfillTriangles>40000||city.CommerceInfillBatches>1)throw new Exception("Commerce infill exceeded static geometry/material budget: "+city.CommerceInfillTriangles+" / "+city.CommerceInfillBatches);
            if(city.GetComponentsInChildren<Light>(true).Length!=0)throw new Exception("Commerce introduced realtime lights");
            var atlas=Resources.Load<Texture2D>("CommerceSigns");if(atlas==null||atlas.width!=512||atlas.height!=256||atlas.mipmapCount<2)throw new Exception("Commerce lettering atlas invalid");
            var path=new List<Vector2>();
            foreach(var target in new[]{new Vector2(-108,21),new Vector2(-76,28),new Vector2(-61,21),new Vector2(-98,4.8f),new Vector2(-85,4.8f),new Vector2(-49,4.8f)})if(!city.PlanRoute(CityView.StreetShopDoor,target,path))throw new Exception("Commerce blocked neighborhood access: "+target);
            foreach(var center in new[]{new Vector2(-98,12),new Vector2(-85,12),new Vector2(-49,12)})if(city.Free(center))throw new Exception("Commerce building lacks collision: "+center);
            int triangles=city.CommerceInfillTriangles,batches=city.CommerceInfillBatches;
            MoveProof(new Vector2(-111,-5.5f),58,-8);SaveFirstPersonFrame(Path.Combine(folder,"commerce-west-overview.png"));
            MoveProof(new Vector2(-98,-1.5f),0,-7);SaveFirstPersonFrame(Path.Combine(folder,"barbershop.png"));
            MoveProof(new Vector2(-85,-1.5f),0,-7);SaveFirstPersonFrame(Path.Combine(folder,"laundry.png"));
            MoveProof(new Vector2(-49,-1.5f),0,-7);SaveFirstPersonFrame(Path.Combine(folder,"repairs.png"));
            MoveProof(new Vector2(-76,4.8f),-90,-5);SaveFirstPersonFrame(Path.Combine(folder,"north-passage.png"));
            CheckCompletedCity(folder);
            File.WriteAllText(Path.Combine(folder,"checks.txt"),"SHOP_REVIEW_PASSED: market/workshop infill, static geometry and one new sign batch, shared textures, three blocked building footprints, gallery/court/cross-street access, 30–45 second commute, residents and photo locations.");
            Debug.Log("COMMERCE_INFILL_PASSED: "+triangles+" added static triangles; "+batches+" new material batch; no realtime lights; passages, collisions and full neighborhood commute.");
        }
    }
}
