using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        [Serializable] class DropShelfPerformanceReport {public int iterations,refreshes;public long steady_lookup_allocated_bytes;}
        void CheckDropShelfRelease(string folder)
        {
            Home();progress=new DayProgress();storageOverride=Path.Combine(folder,"drop-shelf-session");
            CatalogTesting.Enabled=false;inventory=null;customer=1;replayIndex=-1;stage=Stage.Dressing;
            outfit=new[]{"AH_TOP_01","AH_BOTTOM_01","AH_SHOES_01",""};mutedAudio=true;reducedMotion=true;
            studio.SetHome(false);Apply();UpdateCamera();wardrobeProgress=null;RefreshWardrobeCache();
            if(wardrobeAvailable.Count!=Catalog.Items.Length)throw new Exception("Catálogo inicial incorreto");
            var before=(string[])outfit.Clone();SetWardrobeGrouping(true);SetWardrobeGrouping(false);
            if(!outfit.SequenceEqual(before))throw new Exception("Filtro equipou roupas");
            ChooseWardrobePiece(Catalog.Get("CG_BOTTOM_04"),true);
            if(!outfit.SequenceEqual(before))throw new Exception("Roupa bloqueada equipada");
            progress.reached.Add(750);RefreshWardrobeCache();
            if(wardrobeStates["CG_BOTTOM_04"]!="ENTREGA AO FECHAR")throw new Exception("Marco não invalidou cache");
            progress.Deliver();RefreshWardrobeCache();
            if(!wardrobeAvailable.Contains("CG_BOTTOM_04"))throw new Exception("Entrega não invalidou cache");
            CatalogTesting.Enabled=true;RefreshWardrobeCache();
            if(wardrobeAvailable.Count!=Catalog.All.Length)throw new Exception("Teste não liberou catálogo");
            CatalogTesting.Enabled=false;RefreshWardrobeCache();
            if(wardrobeAvailable.Count==Catalog.All.Length)throw new Exception("Teste gravou liberação permanente");
            inventory=Catalog.All.Select(item=>item.id).ToArray();RefreshWardrobeCache();
            ChooseWardrobePiece(Catalog.Get("SB_HEAD_03"),false);
            if(outfit[3]!="SB_HEAD_03"||!outfit.Take(3).SequenceEqual(before.Take(3)))throw new Exception("Seleção alterou categorias vizinhas");
            outfit=before;Apply();message="";wardrobeByDrop=true;shopDressingInStore=true;
            for(int group=0;group<3;group++){wardrobeDrop=group;scroll=Vector2.zero;CaptureInterface(Path.Combine(folder,"drop-"+DropCatalog.All[group].code+".png"));}
            wardrobeByDrop=false;category=3;scroll=Vector2.zero;CaptureInterface(Path.Combine(folder,"by-category.png"));
            var report=new DropShelfPerformanceReport{iterations=1000};
            int refreshes=WardrobeCacheRefreshes;long allocated=GC.GetAllocatedBytesForCurrentThread();
            for(int i=0;i<report.iterations;i++){RefreshWardrobeCache();DropCatalog.Pieces(true,i%3);DropCatalog.Pieces(false,i%4);DropCatalog.Get("SIDE B");WardrobeNameStyle("Bermuda Comando");}
            report.steady_lookup_allocated_bytes=GC.GetAllocatedBytesForCurrentThread()-allocated;report.refreshes=WardrobeCacheRefreshes-refreshes;
            if(report.refreshes!=0||report.steady_lookup_allocated_bytes!=0)throw new Exception("Consulta estável do provador ainda recria dados: "+JsonUtility.ToJson(report));
            replayIndex=0;inventory=Catalog.Items.Select(item=>item.id).ToArray();RefreshWardrobeCache();
            if(wardrobeAvailable.Contains("CG_BOTTOM_04")||wardrobeStates["CG_BOTTOM_04"]!="FORA DESTE REPLAY")throw new Exception("Replay ganhou inventário futuro");
            wardrobeByDrop=true;wardrobeDrop=2;scroll=new Vector2(0,340);CaptureInterface(Path.Combine(folder,"replay-locked.png"));
            File.WriteAllText(Path.Combine(folder,"drop-shelf-performance.json"),JsonUtility.ToJson(report,true));
            Debug.Log("DROP_SHELF_RELEASE_PASSED: three vivid shelves, selection, delivery/testing/replay invalidation; "+report.iterations+" stable lookups allocate "+report.steady_lookup_allocated_bytes+" bytes.");
        }
    }
}
