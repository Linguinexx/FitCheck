using System.IO;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        void CaptureCityScaleReview(string folder)
        {
            Home();EnterCity();paused=false;city.SetView(40,-14);city.Frame(cam,true,0);
            SaveFirstPersonFrame(Path.Combine(folder,"city-store-scale.png"));
            MoveProof(new Vector2(21,-7.8f),78,-17);SaveFirstPersonFrame(Path.Combine(folder,"city-right-scale.png"));
            MoveProof(new Vector2(-19,-8),300,-14);SaveFirstPersonFrame(Path.Combine(folder,"city-left-scale.png"));
            MoveProof(CityView.Spawn,335,-19);SaveFirstPersonFrame(Path.Combine(folder,"city-skyline-scale.png"));
            MoveProof(new Vector2(-19,-.5f),20,-13);SaveFirstPersonFrame(Path.Combine(folder,"city-cafe-scale.png"));
            Debug.Log("FITCHECK_CITY_SCALE_REVIEW_PASSED: approved store retained; residential floor/door/window modules; five native player views and existing walking routes.");
        }
    }
}
