using System.Linq;
using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        static readonly string[] CommentNames={"Kaio","Lia","Breno","Julia","Seu Jota","Nina","","Dandara","Ravi","Joana","Teo","Iara","Dante","Cora"};
        static readonly int[] CommentProfiles={0,1,2,3,4,5,7,8,9,10,11,12,13};
        static readonly string[] WarmComments={"Esse look merece uma volta pelo bairro.","Passou na DripLab e saiu protagonista!","A combinação ficou muito sua cara.","Já quero ver esse fit de perto!","Estilo de quem conhece o próprio corre."};
        static readonly string[] SoftComments={"O importante é experimentar. O próximo vai longe!","Curti a ideia! Eu tentaria outra combinação também.","Cada atendimento é uma história. Bora pro próximo?"};
        void FeedAvatar(Rect r,int profile,string name){
            if(feedPortraits==null)feedPortraits=FeedPortraitNames.Select(file=>file==null?null:Resources.Load<Texture2D>("Artwork/PhoneAndProfiles/"+file)).ToArray();
            var portrait=profile>=0&&profile<feedPortraits.Length?feedPortraits[profile]:null;
            if(portrait!=null)GUI.DrawTexture(r,portrait,ScaleMode.ScaleToFit,true);
            else{RoundedPanel(r,FeedInset,r.width/2);Txt(r,string.IsNullOrEmpty(name)?"?":name.Substring(0,1),action,paper);}
        }
        int CommentAuthor(int index,int row){
            int profile=DayOne.CustomerProfile(index),pick=(index*3+row*5+2)%CommentProfiles.Length;
            if(CommentProfiles[pick]==profile)pick=(pick+1)%CommentProfiles.Length;
            return CommentProfiles[pick];
        }
        void DrawFeedComments(float x,float y,int index,int score){
            Fill(new Rect(x,y,512,1),FeedDivider);Txt(new Rect(x,y+15,512,18),"NA CONVERSA",feedTag,FeedMuted);
            for(int row=0;row<2;row++){
                int author=CommentAuthor(index,row);float top=y+45+row*84;
                FeedAvatar(new Rect(x,top,44,44),author,CommentNames[author]);
                Txt(new Rect(x+58,top,454,21),CommentNames[author],feedSmall,FeedPaper);
                string line=score>=7?WarmComments[(index+row*2)%WarmComments.Length]:SoftComments[(index+row)%SoftComments.Length];
                Txt(new Rect(x+58,top+24,454,50),line,feedCopy,FeedMuted);
            }
        }
    }
}
