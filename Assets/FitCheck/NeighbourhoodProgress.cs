using System;
namespace FitCheck
{
    [Serializable] public sealed class NeighbourhoodProgress
    {
        public int emoteDays,stickers,placed,conversations,supported;
        public int shopQuizDays,shopChatDays,musicChoice,shopPhoto;
        public int[] courtBest=new int[7];
        public bool HasSticker(int id)=>id>=0&&id<7&&(stickers&(1<<id))!=0;
        public bool Collect(int id){if(id<0||id>=7||HasSticker(id))return false;stickers|=1<<id;return true;}
        public bool ChallengeDone(int day)=>(emoteDays&(1<<(day-1)))!=0;
        public bool CompleteChallenge(int day){if(day<1||day>7||ChallengeDone(day))return false;emoteDays|=1<<(day-1);Collect((day-1)%7);return true;}
        public bool Talked(int day,int story)=>(conversations&(1<<((day-1)*3+story)))!=0;
        public void Talk(int day,int story,bool help){if(day<1||day>7||story<0||story>2)return;int bit=1<<((day-1)*3+story);conversations|=bit;if(help)supported|=bit;if(day>=3)Collect(5);}
        public void Court(int day,int made){courtBest[day-1]=Math.Max(courtBest[day-1],Math.Min(3,Math.Max(0,made)));if(made>=2)Collect(6);}
        public void Validate(){
            if(shopQuizDays<0||shopQuizDays>127||shopChatDays<0||shopChatDays>127||musicChoice<0||musicChoice>2||shopPhoto<0||shopPhoto>21)throw new ArgumentException("Lembranças da loja inválidas");
            if(courtBest==null)courtBest=new int[7];
            if(emoteDays<0||emoteDays>127||stickers<0||stickers>127||placed<0||(placed&~stickers)!=0||conversations<0||conversations>2097151||supported<0||(supported&~conversations)!=0||courtBest.Length!=7)throw new ArgumentException("Memórias do bairro inválidas");
            foreach(int best in courtBest)if(best<0||best>3)throw new ArgumentException("Recorde da quadra inválido");
        }
    }
}
