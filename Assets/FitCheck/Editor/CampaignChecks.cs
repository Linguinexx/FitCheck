using System;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace FitCheck.Editor
{
    public static class CampaignChecks
    {
        public static readonly string[][] Solutions={
            new[]{"AH_TOP_01","AH_BOTTOM_01","AH_SHOES_01",""},
            new[]{"CG_TOP_01","SB_BOTTOM_01","SB_SHOES_01","CG_HEAD_01"},
            new[]{"AH_TOP_01","CG_BOTTOM_02","AH_SHOES_01",""},
            new[]{"AH_TOP_01","AH_BOTTOM_01","AH_SHOES_01",""},
            new[]{"SB_TOP_01","CG_BOTTOM_02","SB_SHOES_01",""},
            new[]{"CG_TOP_01","CG_BOTTOM_02","AH_SHOES_01","CG_HEAD_01"},
            new[]{"SB_TOP_01","SB_BOTTOM_01","AH_SHOES_01",""},
            new[]{"AH_TOP_01","AH_BOTTOM_01","SB_SHOES_01",""},
            new[]{"CG_TOP_01","SB_BOTTOM_01","AH_SHOES_01",""}
        };
        [MenuItem("Tools/Fit Check/Validate full campaign")]
        public static void Validate()
        {
            if(DayOne.Customers.Length!=9||Catalog.All.Length!=21||Catalog.All.Select(g=>g.id).Distinct().Count()!=21)throw new Exception("Conteúdo incompleto ou IDs duplicados");
            var campaign=new DayProgress();
            for(int i=0;i<9;i++){
                if(DayOne.Score(i,Solutions[i])!=10)throw new Exception("Solução inicial falhou: "+i);
                campaign.Add(new Attempt{outfit=(string[])Solutions[i].Clone(),question=i%2});
                if((i+1)%3==0){campaign.Close();campaign.Close();if(i<8)campaign.Advance();}
                campaign.Validate();
            }
            if(!campaign.Complete||campaign.reputation!=900||campaign.Available.Length!=21||campaign.achievements.Count!=3)throw new Exception("Conclusão/progressão inválida");
            var before=campaign.reputation;var question=campaign.posts[0].question;
            if(campaign.Improve(0,Solutions[0])||campaign.reputation!=before||campaign.posts[0].question!=question)throw new Exception("Replay duplicou crédito ou mudou pergunta");
            bool denied=false;try{campaign.Improve(0,new[]{"AH_TOP_02","AH_BOTTOM_01","AH_SHOES_01",""});}catch(ArgumentException){denied=true;}
            if(!denied)throw new Exception("Replay aceitou roupa futura");
            var floor=new DayProgress();floor.Add(new Attempt{outfit=Catalog.Default});floor.Add(new Attempt{outfit=Catalog.Default});
            floor.Improve(1,Solutions[1]);if(floor.reputation!=100)throw new Exception("Piso zero inflou melhoria");floor.Validate();
            int credit=floor.reputation;floor.Improve(1,Solutions[1]);if(floor.reputation!=credit)throw new Exception("Ganho infinito");
            var falling=new DayProgress();falling.Add(new Attempt{outfit=Solutions[0]});falling.Add(new Attempt{outfit=Catalog.Default});falling.Add(new Attempt{outfit=Catalog.Default});falling.Close();
            if(!falling.reached.Contains(100)||!falling.rewards.Contains("Night Route"))throw new Exception("Marco perdido após queda");
            var greetings=new DayProgress();
            if(greetings.RegisterCustomerGreeting(0))throw new Exception("Cumprimento antes do atendimento");
            for(int i=0;i<3;i++)greetings.Add(new Attempt{outfit=Catalog.Default});
            if(!greetings.RegisterCustomerGreeting(0)||greetings.RegisterCustomerGreeting(0))throw new Exception("Cumprimento duplicado");
            greetings.Close();greetings.Advance();
            greetings=JsonUtility.FromJson<DayProgress>(JsonUtility.ToJson(greetings));
            for(int i=3;i<6;i++)greetings.Add(new Attempt{outfit=Catalog.Default});
            if(greetings.RegisterCustomerGreeting(0)||greetings.RegisterCustomerGreeting(4)||!greetings.RegisterCustomerGreeting(3)||
                greetings.UniformUnlocked(DayProgress.NeighbourhoodUniform)||!greetings.RegisterCustomerGreeting(5)||
                !greetings.UniformUnlocked(DayProgress.NeighbourhoodUniform))throw new Exception("Figurinha do Bairro não contou três clientes distintos");
            var dj=new DayProgress();
            if(!dj.RegisterGarageRecord(2)||dj.UniformUnlocked(DayProgress.GarageDjUniform))throw new Exception("DJ liberado na primeira vitrola");
            dj=JsonUtility.FromJson<DayProgress>(JsonUtility.ToJson(dj));
            if(dj.RegisterGarageRecord(2)||!dj.RegisterGarageRecord(0)||dj.UniformUnlocked(DayProgress.GarageDjUniform)||
                !dj.RegisterGarageRecord(1)||!dj.UniformUnlocked(DayProgress.GarageDjUniform)||dj.RegisterGarageRecord(1))
                throw new Exception("Progressão da vitrola inválida após recarregar");
            var ghost=new DayProgress();
            if(ghost.DiscoverFittingGhost())throw new Exception("Fantasma liberado antes de fechar");
            for(int i=0;i<3;i++)ghost.Add(new Attempt{outfit=Catalog.Default});
            ghost.Close();
            if(!ghost.DiscoverFittingGhost()||ghost.DiscoverFittingGhost())throw new Exception("Descoberta do Fantasma inválida");
            ghost=JsonUtility.FromJson<DayProgress>(JsonUtility.ToJson(ghost));
            if(!ghost.UniformUnlocked(DayProgress.FittingGhostUniform))throw new Exception("Traje Fantasma perdido no save");
            var legacy=new DayProgress();legacy.Add(new Attempt{outfit=Solutions[0]});legacy.version=1;legacy.posts[0].available=null;
            var migrated=JsonUtility.FromJson<DayProgress>(JsonUtility.ToJson(legacy));migrated.Validate();if(migrated.version!=2||migrated.posts[0].available.Length!=11)throw new Exception("Migração inválida");
            foreach(var top in Catalog.Items.Where(g=>g.slot==0))foreach(var bottom in Catalog.Items.Where(g=>g.slot==1))foreach(var shoes in Catalog.Items.Where(g=>g.slot==2))foreach(var head in new[]{"","AH_HEAD_01","CG_HEAD_01"})for(int i=0;i<9;i++){
                int score=DayOne.Score(i,new[]{top.id,bottom.id,shoes.id,head});if(!new[]{0,3,4,6,7,10}.Contains(score))throw new Exception("Nota fora do modelo 4/3/3");
            }
            var path=Path.Combine(Application.temporaryCachePath,"FitCheck-save-test-"+Guid.NewGuid()+".json");
            try{
                var mixed=new DayProgress();var mixedOutfit=Catalog.ArrivalOutfit;
                mixedOutfit[0]="AH_TOP_01";mixed.Add(new Attempt{outfit=mixedOutfit});
                ProgressStore.Save(path,mixed);
                if(!ProgressStore.Load(path).posts[0].outfit.SequenceEqual(mixedOutfit))throw new Exception("Peças originais perdidas ao salvar");
                File.Delete(path);
                ProgressStore.Save(path,campaign);ProgressStore.Save(path,campaign);
                File.WriteAllText(path,"save corrompido");var recovered=ProgressStore.Load(path+".bak");if(!recovered.Complete)throw new Exception("Backup inválido");
                ProgressStore.Save(path,recovered);if(ProgressStore.Load(path).reputation!=900)throw new Exception("Regravação inválida");
            }finally{foreach(var suffix in new[]{"",".tmp",".bak"})if(File.Exists(path+suffix))File.Delete(path+suffix);}
            Debug.Log("FITCHECK_CAMPAIGN_PASSED: 9 pedidos, 900 RP, 21 peças, 3 conquistas, replays sem inflação, inventário congelado, migração e backup.");
        }
        [MenuItem("Tools/Fit Check/Run full campaign runtime check")]
        public static void Runtime()
        {
            var game=UnityEngine.Object.FindAnyObjectByType<FirstCustomer>();if(!Application.isPlaying||game==null)throw new Exception("Inicie Play Mode");
            game.CampaignSmokeCheck(Solutions);game.PreviewSubmissionSmokeCheck();
        }
    }
}
