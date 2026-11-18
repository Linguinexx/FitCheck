using System;
namespace FitCheck
{
    [Serializable] public sealed class NeighbourhoodProgress
    {
        public int emoteDays,stickers,placed,conversations,supported;
        public int shopQuizDays,shopChatDays,musicChoice,shopPhoto;
        public bool musicStopped;
        public int[] courtBest=new int[7];
        public float[] muralX,muralY,muralAngle;
        public int[] muralLayer;
        public void EnsureMuralLayout(){
            if(muralX==null||muralX.Length==0)muralX=new[]{-.96f,-.26f,.61f,-.83f,-.18f,.47f,1.02f};
            if(muralY==null||muralY.Length==0)muralY=new[]{.29f,.24f,.23f,-.42f,-.40f,-.43f,-.35f};
            if(muralAngle==null||muralAngle.Length==0)muralAngle=new[]{-8f,7f,-5f,9f,-10f,6f,-7f};
            if(muralLayer==null||muralLayer.Length==0)muralLayer=new[]{0,1,2,3,4,5,6};
        }
        public bool HasSticker(int id)=>id>=0&&id<7&&(stickers&(1<<id))!=0;
        public bool Collect(int id){if(id<0||id>=7||HasSticker(id))return false;stickers|=1<<id;return true;}
        public bool ChallengeDone(int day)=>(emoteDays&(1<<(day-1)))!=0;
        public bool CompleteChallenge(int day){if(day<1||day>7||ChallengeDone(day))return false;emoteDays|=1<<(day-1);Collect((day-1)%7);return true;}
        public bool Talked(int day,int story)=>(conversations&(1<<((day-1)*3+story)))!=0;
        public void Talk(int day,int story,bool help){if(day<1||day>7||story<0||story>2)return;int bit=1<<((day-1)*3+story);conversations|=bit;if(help)supported|=bit;if(day>=3)Collect(5);}
        public void Court(int day,int made){courtBest[day-1]=Math.Max(courtBest[day-1],Math.Min(3,Math.Max(0,made)));if(made>=2)Collect(6);}
        public void Validate(){
            EnsureMuralLayout();
            if(muralX.Length!=7||muralY.Length!=7||muralAngle.Length!=7||muralLayer.Length!=7)throw new ArgumentException("Composição do mural inválida");
            int layers=0;foreach(int layer in muralLayer){if(layer<0||layer>6||(layers&(1<<layer))!=0)throw new ArgumentException("Ordem dos lambe-lambes inválida");layers|=1<<layer;}
            for(int i=0;i<7;i++)if(float.IsNaN(muralX[i])||float.IsInfinity(muralX[i])||Math.Abs(muralX[i])>1.5f||float.IsNaN(muralY[i])||float.IsInfinity(muralY[i])||Math.Abs(muralY[i])>1.1f||float.IsNaN(muralAngle[i])||float.IsInfinity(muralAngle[i])||Math.Abs(muralAngle[i])>30)throw new ArgumentException("Posição do lambe-lambe inválida");
            if(shopQuizDays<0||shopQuizDays>127||shopChatDays<0||shopChatDays>127||musicChoice<0||musicChoice>2||shopPhoto<0||shopPhoto>21)throw new ArgumentException("Lembranças da loja inválidas");
            if(courtBest==null)courtBest=new int[7];
            if(emoteDays<0||emoteDays>127||stickers<0||stickers>127||placed<0||(placed&~stickers)!=0||conversations<0||conversations>2097151||supported<0||(supported&~conversations)!=0||courtBest.Length!=7)throw new ArgumentException("Memórias do bairro inválidas");
            foreach(int best in courtBest)if(best<0||best>3)throw new ArgumentException("Recorde da quadra inválido");
        }
    }
}
