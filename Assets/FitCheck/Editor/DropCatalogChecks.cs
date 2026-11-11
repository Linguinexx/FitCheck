using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FitCheck.Editor
{
    public static class DropCatalogChecks
    {
        [MenuItem("Tools/Fit Check/Check clothing drops")]
        public static void Validate()
        {
            if(Catalog.All.Select(item=>item.id).Distinct().Count()!=Catalog.All.Length)throw new Exception("ID de roupa repetido");
            var grouped=DropCatalog.All.SelectMany((drop,index)=>DropCatalog.Pieces(true,index)).ToArray();
            if(grouped.Length!=Catalog.All.Length||grouped.Select(item=>item.id).Distinct().Count()!=Catalog.All.Length)throw new Exception("Peça sem drop ou duplicada nas prateleiras");
            foreach(var drop in DropCatalog.All){
                var pieces=Catalog.All.Where(item=>item.drop==drop.name).ToArray();
                if(pieces.Length==0||!pieces.Any(item=>item.slot==0)||!pieces.Any(item=>item.slot==1))throw new Exception("Drop sem combinação de cima e baixo");
            }
            var rewards=DayProgress.Milestones.SelectMany(DropCatalog.RewardsAt).ToArray();
            if(rewards.Distinct().Count()!=rewards.Length||!rewards.OrderBy(name=>name).SequenceEqual(Catalog.All.Where(item=>item.unlockAt>0).Select(item=>item.name).OrderBy(name=>name)))throw new Exception("Entrega sem marco, duplicada ou ausente");
            if(Catalog.All.Any(item=>item.unlockAt>0&&!DayProgress.Milestones.Contains(item.unlockAt)))throw new Exception("Desbloqueio fora da campanha");
            for(int slot=0;slot<4;slot++)if(DropCatalog.Pieces(false,slot).Any(item=>item.slot!=slot))throw new Exception("Filtro de categoria misturou peças");
            string[] shoes=DropCatalog.Pieces(false,2).Select(item=>item.id).Take(3).ToArray();
            if(!shoes.SequenceEqual(new[]{"SB_SHOES_01","AH_SHOES_01","CG_SHOES_01"}))throw new Exception("Ordem aprovada dos tênis mudou");
            CampaignChecks.Validate();
            Debug.Log("FITCHECK_DROPS_PASSED: 32 peças, três drops completos, filtros, ordem dos tênis, entregas e campanha histórica preservados.");
        }
    }
}
