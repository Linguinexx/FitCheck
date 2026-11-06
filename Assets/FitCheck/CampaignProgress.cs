using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FitCheck
{
    [Serializable] public class DayProgress
    {
        public int version=2, day=1, reputation, uniform;
        public bool closed;
        public List<Attempt> posts=new List<Attempt>();
        public List<int> reached=new List<int>();
        public List<string> rewards=new List<string>();
        public List<string> achievements=new List<string>();
        static readonly string[] UniformAwards={"","Primeiro Expediente","Visual Perfeito","Sintonia com os Clientes"};
        public bool UniformUnlocked(int index)=>index>=0&&index<UniformAwards.Length&&(index==0||achievements.Contains(UniformAwards[index]));
        public int EquippedUniform=>UniformUnlocked(uniform)?uniform:0;
        public static readonly int[] Milestones={100,200,300,450,600};
        public static readonly string[][] RewardNames={new[]{"Night Route","Remix"},new[]{"Court Lines","Daily Steps"},new[]{"Closing Time","Independent"},new[]{"Cut & Paste","City Glow"},new[]{"Meeting Point","Unreleased"}};
        public bool DayComplete=>posts.Count==day*3;
        public bool Complete=>posts.Count==9&&closed;
        public string[] Available=>Catalog.All.Where(g=>g.unlockAt==0||rewards.Contains(g.name)).Select(g=>g.id).ToArray();
        public static int Credit(int score,int index)=>index==0?Rules.TutorialReputation(score):score==10?100:score==7?60:score==6?30:-30;
        public static void CheckOutfit(string[] outfit,string[] inventory)
        {
            Rules.Evaluate(outfit);
            if(inventory==null||outfit.Any(id=>!string.IsNullOrEmpty(id)&&!Catalog.IsArrivalPiece(id)&&!inventory.Contains(id)))throw new ArgumentException("Peça indisponível neste atendimento");
        }
        public void Add(Attempt attempt)
        {
            if(closed||DayComplete||posts.Count>=9)throw new InvalidOperationException("Feche o expediente antes de avançar");
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
            for(int d=0;d<posts.Count/3;d++)if(posts.Skip(d*3).Take(3).All(p=>p.score>=7))Unlock("Sintonia com os Clientes");
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
            if(!closed||day>=3)throw new InvalidOperationException("Não há outro dia disponível");
            day++;closed=false;
        }
        public void Validate()
        {
            if(version==1){version=2;day=1;achievements=new List<string>();foreach(var post in posts)post.available=Catalog.Items.Select(g=>g.id).ToArray();UpdateAchievements();}
            if(version!=2||day<1||day>3||posts==null||posts.Count<(day-1)*3||posts.Count>day*3||reached==null||rewards==null||achievements==null||closed&&!DayComplete)
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
