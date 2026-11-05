using System;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        void CheckStreetFrontage(string folder)
        {
            Home();EnterCity();paused=false;
            var route=new System.Collections.Generic.List<Vector2>();
            foreach(var target in new[]{new Vector2(-9,-19),new Vector2(12,-19),new Vector2(1.5f,-30)})
                if(!city.PlanRoute(CityView.StreetShopDoor,target,route))throw new Exception("Opposite shops blocked pedestrian access: "+target);
            foreach(var target in new[]{new Vector2(-9,-28),new Vector2(12,-28)})if(city.Free(target))throw new Exception("New building lacks collision");
            MoveProof(new Vector2(8,-5),180,-9);SaveFirstPersonFrame(Path.Combine(folder,"opposite-overview.png"));
            MoveProof(new Vector2(-9,-17.5f),180,-5);SaveFirstPersonFrame(Path.Combine(folder,"records-front.png"));
            MoveProof(new Vector2(12,-17.5f),180,-5);SaveFirstPersonFrame(Path.Combine(folder,"workshop-front.png"));
            int renderers=city.GetComponentsInChildren<MeshRenderer>(true).Length;
            if(renderers>411)throw new Exception("Static frontage added renderer overhead: "+renderers);
            CheckCompletedCity(folder);CheckShopServiceFlow(folder);CheckSaveSlots(folder);
            Home();EnterShop();SettleShopArrival();MoveProof(CityView.ShopCounter,180,0);InteractShop();question=1;shopCameraTime=.5f;FrameShopDialogue();TickContext(1);TickContext(1);
            CaptureInterface(Path.Combine(folder,"client-bubble-normal.png"));
            var ink=new Color(.08f,.065f,.07f);SetLabelInk(caption,ink);
            foreach(var state in new[]{caption.normal,caption.hover,caption.active,caption.focused,caption.onNormal,caption.onHover,caption.onActive,caption.onFocused})
                if(state.textColor!=ink)throw new Exception("Speech label loses contrast in an interaction state");
            CaptureInterface(Path.Combine(folder,"client-bubble-hover.png"),new Vector2(1175,280));
            File.WriteAllText(Path.Combine(folder,"checks.txt"),"SHOP_REVIEW_PASSED: static opposite record/bike storefronts, shared material batches with no renderer increase, pedestrian/collision access, 30–45 second commute, animated residents, customer speech contrast in all eight GUI states, service and independent saves.");
            Debug.Log("STREET_FRONTAGE_PASSED: renderers="+renderers+"; full commute/service/saves and label hover states.");
        }
    }
}
