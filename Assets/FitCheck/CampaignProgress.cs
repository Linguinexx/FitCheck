using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FitCheck
{
    [Serializable] public class DayProgress
    {
        public const int TotalDays=7, CustomersPerDay=3;
        public const int TotalCustomers=TotalDays*CustomersPerDay;
        public int version=2, day=1, reputation, uniform;
        public int FirstCustomerOfDay=>(day-1)*CustomersPerDay;
        public bool closed;
        public List<Attempt> posts=new List<Attempt>();
        public List<int> reached=new List<int>();
        public List<string> rewards=new List<string>();
        public List<string> achievements=new List<string>();
        public const int FubaUniform=4,FubaPetGoal=8;
        public const string FubaAward="Amigo do Fubá";
        public int fubaPets;
        public const int NeighbourhoodUniform=5;
        public const string NeighbourhoodAward="Figurinha do Bairro";
        public int greetedCustomerProfiles;
        public List<int> greetedCustomerEncounters=new List<int>();
        public const int GarageDjUniform=6, GarageDjRecordGoal=3;
        public const string GarageDjAward="DJ de Garagem";
        public int garageDjDecks;
        public bool HasGarageRecord(int deck)=>(garageDjDecks&(1<<deck))!=0;
        public int GarageRecordsFound=>
            (HasGarageRecord(0)?1:0)+(HasGarageRecord(1)?1:0)+(HasGarageRecord(2)?1:0);
        public const int FittingGhostUniform=7;
        public const string FittingGhostAward="Fantasma do Provador";
        static readonly string[] UniformAwards={"","Primeiro Expediente","Visual Perfeito","Sintonia com os Clientes",FubaAward,NeighbourhoodAward,GarageDjAward,FittingGhostAward};
        public bool DiscoverFittingGhost()
        {
            if(!closed||UniformUnlocked(FittingGhostUniform))return false;
            Unlock(FittingGhostAward);
            return true;
        }
        public bool RegisterGarageRecord(int deck)
        {
            if(deck<0||deck>=GarageDjRecordGoal||HasGarageRecord(deck)||UniformUnlocked(GarageDjUniform))return false;
            garageDjDecks|=1<<deck;
            if(GarageRecordsFound==GarageDjRecordGoal)Unlock(GarageDjAward);
            return true;
        }
        public bool UniformUnlocked(int index)=>index>=0&&index<UniformAwards.Length&&(index==0||achievements.Contains(UniformAwards[index]));
        public int EquippedUniform=>UniformUnlocked(uniform)?uniform:0;
        public bool CanGreetCustomer(int index)
        {
            return index>=0&&index>=FirstCustomerOfDay&&index<posts.Count&&greetedCustomerEncounters?.Contains(index)!=true;
        }
        public bool RegisterCustomerGreeting(int index)
        {
            if(!CanGreetCustomer(index))return false;
            greetedCustomerEncounters??=new List<int>();
            greetedCustomerEncounters.Add(index);
            int profileFlag=1<<DayOne.CustomerProfile(index);
            if((greetedCustomerProfiles&profileFlag)!=0)return true;
            greetedCustomerProfiles|=profileFlag;
            int count=0;
            for(int flags=greetedCustomerProfiles;flags!=0;flags&=flags-1)count++;
            if(count>=3)Unlock(NeighbourhoodAward);
            return true;
        }
        public bool RegisterFubaPet()
        {
            if(UniformUnlocked(FubaUniform)||fubaPets>=FubaPetGoal)return false;
            if(++fubaPets==FubaPetGoal)Unlock(FubaAward);
            return true;
        }
        public static readonly int[] Milestones={100,200,300,450,600,750,1050,1400};
        public static readonly string[][] RewardNames={
            new[]{"Night Route","Remix"},new[]{"Court Lines","Daily Steps"},new[]{"Closing Time","Independent"},
            new[]{"Cut & Paste","City Glow"},new[]{"Meeting Point","Unreleased"},new[]{"Pista Aberta","Cargo das Lajes"},
            new[]{"Fita Local","Bermuda de Pista"},new[]{"Luz de Poste","Aba de Tijolo"}
        };
        public bool DayComplete=>posts.Count==FirstCustomerOfDay+CustomersPerDay;
        public bool Complete=>day==TotalDays&&DayComplete&&closed;
        public string[] Available=>Catalog.All.Where(g=>g.unlockAt==0||rewards.Contains(g.name)).Select(g=>g.id).ToArray();
        public static int Credit(int score,int index)=>index==0?Rules.TutorialReputation(score):score==10?100:score==7?60:score==6?30:-30;
        public static void CheckOutfit(string[] outfit,string[] inventory)
        {
            Rules.Evaluate(outfit);
            if(inventory==null||outfit.Any(id=>!string.IsNullOrEmpty(id)&&!Catalog.IsArrivalPiece(id)&&!inventory.Contains(id)))throw new ArgumentException("Peça indisponível neste atendimento");
        }
        public void Add(Attempt attempt)
        {
            if(closed||DayComplete||posts.Count>=TotalCustomers)throw new InvalidOperationException("Feche o expediente antes de avançar");
            CheckOutfit(attempt.outfit,Available);
            attempt.available=Available;
            attempt.score=DayOne.Score(posts.Count,attempt.outfit);
            attempt.reputation=Math.Max(0,reputation+Credit(attempt.score,posts.Count))-reputation;
            reputation+=attempt.reputation;attempt.completed=true;posts.Add(attempt);UpdateMilestones();UpdateAchievements();
        }
        public bool Improve(int index,string[] outfit)
        {
            if(index<0||index>=posts.Count)throw new ArgumentOutOfRangeException(nameof(index));
            var post=posts[index];CheckOutfit(outfit,post.available);
            int score=DayOne.Score(index,outfit);if(score<=post.score)return false;
            int newCredit=Credit(score,index);
            reputation+=Math.Max(0,newCredit-post.reputation);
            if(newCredit>=0)post.reputation=newCredit;post.score=score;post.outfit=(string[])outfit.Clone();
            UpdateMilestones();UpdateAchievements();return true;
        }
        void UpdateMilestones(){foreach(int m in Milestones)if(reputation>=m&&!reached.Contains(m))reached.Add(m);}
        void Unlock(string id){if(!achievements.Contains(id))achievements.Add(id);}
        void UpdateAchievements()
        {
            if(posts.Any(p=>p.score==10))Unlock("Visual Perfeito");
            if(closed&&day>=1)Unlock("Primeiro Expediente");
            for(int start=0;start+CustomersPerDay<=posts.Count;start+=CustomersPerDay)
                if(posts.Skip(start).Take(CustomersPerDay).All(p=>p.score>=7))Unlock("Sintonia com os Clientes");
        }
        public void Deliver()
        {
            foreach(int m in reached)foreach(string name in RewardNames[Array.IndexOf(Milestones,m)])if(!rewards.Contains(name))rewards.Add(name);
        }
        public void Close()
        {
            if(!DayComplete)throw new InvalidOperationException("Conclua os três atendimentos do dia");
            if(closed)return;closed=true;Deliver();UpdateAchievements();
        }
        public void Advance()
        {
            if(!closed||day>=TotalDays)throw new InvalidOperationException("Não há outro dia disponível");
            // Entrega os novos marcos já alcançados por saves da campanha antiga.
            UpdateMilestones();
            Deliver();
            day++;closed=false;
        }
        public void Validate()
        {
            if(version==1){version=2;day=1;achievements=new List<string>();foreach(var post in posts)post.available=Catalog.Items.Select(g=>g.id).ToArray();UpdateAchievements();}
            if(version!=2||day<1||day>TotalDays||posts==null||posts.Count<FirstCustomerOfDay||posts.Count>FirstCustomerOfDay+CustomersPerDay||reached==null||rewards==null||achievements==null||closed&&!DayComplete)
                throw new ArgumentException("Progresso inválido");
            for(int i=0;i<posts.Count;i++){
                var post=posts[i];if(post==null||!post.completed||post.question< -1||post.question>1)throw new ArgumentException("Publicação inválida");
                CheckOutfit(post.outfit,post.available);
                if(post.available.Distinct().Count()!=post.available.Length||post.available.Any(id=>Catalog.Get(id)==null))throw new ArgumentException("Inventário salvo inválido");
                int credit=Credit(post.score,i);
                if(DayOne.Score(i,post.outfit)!=post.score||credit>=0&&post.reputation!=credit||credit<0&&(post.reputation<credit||post.reputation>0))throw new ArgumentException("Avaliação inconsistente");
            }
            if(posts.Sum(p=>p.reputation)!=reputation||reputation<0||reached.Distinct().Count()!=reached.Count||reached.Any(m=>!Milestones.Contains(m))||rewards.Distinct().Count()!=rewards.Count||rewards.Any(n=>!Catalog.All.Any(g=>g.name==n&&reached.Contains(g.unlockAt))))throw new ArgumentException("Reputação ou recompensas inconsistentes");
        }
    }
    public static class ProgressStore
    {
        public static DayProgress Load(string path){var p=JsonUtility.FromJson<DayProgress>(File.ReadAllText(path));if(p==null)throw new ArgumentException("Save vazio");p.Validate();return p;}
        public static void Save(string path,DayProgress progress)
        {
            progress.Validate();Directory.CreateDirectory(Path.GetDirectoryName(path));
            string tmp=path+".tmp";File.WriteAllText(tmp,JsonUtility.ToJson(progress,true));Load(tmp);
            if(File.Exists(path)){
                bool valid=true;try{Load(path);}catch{valid=false;}
                File.Replace(tmp,path,valid?path+".bak":null);
            }else File.Move(tmp,path);
        }
    }
}
