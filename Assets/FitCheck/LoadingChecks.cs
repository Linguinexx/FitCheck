using System;
using System.Collections;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        IEnumerator RunLoadingReview(string folder)
        {
            var review=CheckLoadingReview(folder);
            while(true){
                bool next=false;object current=null;Exception error=null;
                try{next=review.MoveNext();if(next)current=review.Current;}catch(Exception e){error=e;}
                if(error!=null){Debug.LogException(error);Application.Quit(1);yield break;}
                if(!next)break;yield return current;
            }
            Application.Quit(0);
        }
        IEnumerator CheckLoadingReview(string folder)
        {
            var oldProgress=progress;var oldLook=playerAppearance;string oldStorage=storageOverride,oldLibrary=saveLibraryOverride;bool oldReduced=reducedMotion;
            try{
                if(Array.IndexOf(Environment.GetCommandLineArgs(),"-sneaker-v2-review")>=0)CheckSneakerUpgrade(folder);
                if(Array.IndexOf(Environment.GetCommandLineArgs(),"-night-lighting-review")>=0)CheckNightLighting(folder);
                if(Array.IndexOf(Environment.GetCommandLineArgs(),"-road-polish-review")>=0)CheckRoadPolish(folder);
                Home();progress=new DayProgress();playerAppearance=new PlayerAppearance();
                saveLibraryOverride=Path.Combine(folder,"loading-saves-"+Guid.NewGuid().ToString("N"));storageOverride=Path.Combine(folder,"loading-session");Directory.CreateDirectory(storageOverride);
                loadingTestEnabled=true;int calls=0;float loadingStarted=Time.realtimeSinceStartup;
                BeginLoading(()=>{calls++;OpenSaveSelection();});BeginLoading(()=>{calls+=100;});
                if(!loadingActive||calls!=0)throw new Exception("Loading did not cover destination before work");
                for(int i=0;i<6;i++)if(LoadingDotCount(i*.14f+.001f)!=new[]{0,1,2,3,2,1}[i])throw new Exception("Loading dots do not reverse");
                yield return null;
                if(calls!=0)throw new Exception("Destination ran before a covered frame");
                foreach(float clock in new[]{.05f,.45f,.74f}){loadingClock=clock;CaptureInterface(Path.Combine(folder,"loading-"+Mathf.RoundToInt(clock*100)+".png"));}
                if(!reducedMotion&&LoadingDiagonalOffset(1.05f)<=LoadingDiagonalOffset(.05f)+100)throw new Exception("Loading diagonals do not descend visibly");
                loadingClock=0;
                while(loadingActive)yield return null;
                if(Time.realtimeSinceStartup-loadingStarted<LoadingMinimumSeconds)throw new Exception("Loading disappeared before its minimum readable time");
                if(calls!=1||stage!=Stage.Welcome)throw new Exception("Play loading skipped or duplicated save selection");
                PreviewSave(0);BeginLoading(()=>{OpenSelectedSave();});while(loadingActive)yield return null;
                if(stage!=Stage.CharacterCreator)throw new Exception("Empty save bypassed creator");
                creatorDraft.name="Loading test";BeginLoading(()=>{SaveCreator();});while(loadingActive)yield return null;
                if(stage!=Stage.City||!city.IsHome)throw new Exception("Creator loading did not start at home");
                WalkWorldProof(CityView.HomeDoor);var source=city;var position=city.Position;
                if(!InteractHome()||!loadingActive||city!=source)throw new Exception("Home exit did not defer destination");
                if(InteractHome())throw new Exception("Loading accepted repeated door interaction");
                ProcessCityEvent(new Event{type=EventType.KeyDown,keyCode=KeyCode.E});
                yield return null;if(city!=source||city.Position!=position)throw new Exception("Loading allowed movement before destination");
                while(loadingActive)yield return null;
                if(city.IsInterior)throw new Exception("Home exit did not reach street");
                for(int i=0;i<20;i++)UpdateCity(.05f);
                WalkWorldProof(CityView.StreetShopDoor);city.SetView(0,0);if(!InteractStreet()||!loadingActive)throw new Exception("Shop entrance lacks loading");
                while(loadingActive)yield return null;
                if(!city.IsInterior||city.IsHome||ShopOpen||city.CustomerVisible)throw new Exception("Loading started shop service automatically");
                WalkWorldProof(CityView.ShopSpawn);city.SetView(180,0);
                if(!InteractShop()||!loadingActive)throw new Exception("Shop exit lacks loading");while(loadingActive)yield return null;
                if(city.IsInterior)throw new Exception("Shop exit did not reach street");
                WalkWorldProof(CityView.StreetHomeDoor);city.SetView(0,0);reducedMotion=true;
                if(!InteractStreet()||!loadingActive)throw new Exception("Home entrance lacks loading");
                CaptureInterface(Path.Combine(folder,"loading-reduced-motion.png"));while(loadingActive)yield return null;
                if(!city.IsHome||stage!=Stage.City)throw new Exception("Home entrance failed");
                Home();OpenSaveSelection();PreviewSave(0);BeginLoading(()=>{OpenSelectedSave();});while(loadingActive)yield return null;
                if(!city.IsHome||playerAppearance.name!="Loading test")throw new Exception("Existing-save loading lost home or appearance");
                loadingTestEnabled=false;reducedMotion=oldReduced;
                CheckHomeCommute(folder);CheckShopServiceFlow(folder);CheckSaveSlots(folder);
                File.WriteAllText(Path.Combine(folder,"checks.txt"),"SHOP_REVIEW_PASSED: branded native loading, animated menu logo, reversible dots, duplicate/input blocking, frame before destination construction, Play/save/creator/home/street/shop round trip, reduced motion, full home/service/save regression.");
                Debug.Log("LOADING_REVIEW_PASSED: all eight transitions, input gating, dots, reduced motion, home/service/save regression.");
            }finally{loadingTestEnabled=false;Home();progress=oldProgress;playerAppearance=oldLook;storageOverride=oldStorage;saveLibraryOverride=oldLibrary;reducedMotion=oldReduced;}
        }
    }
}
