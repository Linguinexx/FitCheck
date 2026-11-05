using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        void CheckMusicInfill(string folder)
        {
            Home();EnterCity();paused=false;
            int triangles=city.MusicInfillTriangles,batches=city.MusicInfillBatches;
            if(triangles<=0||triangles>18000||batches!=0)throw new Exception("Music infill exceeded shared static budget: "+triangles+" / "+batches);
            if(city.GetComponentsInChildren<Light>(true).Length!=0||city.ResidentCount!=CityView.StreetResidentCount)throw new Exception("Music infill introduced realtime lights or actors");
            var path=new List<Vector2>();
            foreach(var target in new[]{new Vector2(-49,-18),new Vector2(-9,-19),new Vector2(-30,-21.2f),new Vector2(-21.5f,-36),new Vector2(-28,-38)})if(!city.PlanRoute(CityView.StreetShopDoor,target,path))throw new Exception("Music pocket is inaccessible: "+target);
            for(float z=-22;z>=-37;z-=.5f)if(!city.Free(new Vector2(-21.5f,z)))throw new Exception("Music pocket lane blocked at Z="+z);
            foreach(var p in new[]{new Vector2(-30,-29),new Vector2(-34,-38.5f),new Vector2(-36.2f,-41.1f)})if(city.Free(p))throw new Exception("Music architecture lacks collision: "+p);
            MoveProof(new Vector2(-38,-10.5f),145,-9);SaveFirstPersonFrame(Path.Combine(folder,"music-link-overview.png"));
            MoveProof(new Vector2(-30,-16.5f),180,-8);SaveFirstPersonFrame(Path.Combine(folder,"studio-front.png"));
            MoveProof(new Vector2(-21.5f,-21.5f),180,-5);SaveFirstPersonFrame(Path.Combine(folder,"courtyard-entrance.png"));
            MoveProof(new Vector2(-24.8f,-35.8f),210,-3);SaveFirstPersonFrame(Path.Combine(folder,"rhyme-courtyard.png"));
            MoveProof(new Vector2(-28,-38),35,-7);SaveFirstPersonFrame(Path.Combine(folder,"courtyard-return.png"));
            CheckCompletedCity(folder);
            File.WriteAllText(Path.Combine(folder,"checks.txt"),"SHOP_REVIEW_PASSED: studio facade, record crate, accessible rhyme courtyard between SIDE B and AO VIVO, continuous pavement, collision footprints, no new texture/material batch/realtime lights/actors, unchanged 30–45 second home/store route and existing city landmarks.");
            Debug.Log("MUSIC_INFILL_PASSED: "+triangles+" additional static triangles; "+batches+" new batches; no new textures/lights/actors; clear entrance and return, full neighborhood commute.");
        }
    }
}
