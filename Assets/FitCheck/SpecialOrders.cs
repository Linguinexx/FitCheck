using System;
using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        bool SpecialOrderActive=>campaignBeforePreview==null&&replayIndex<0&&shopService&&(customer==10||customer==12||customer==19);
        string PartnerShoe=>progress.posts.Count>18?progress.posts[18].outfit[2]:Catalog.ArrivalPiece(2).id;
        string SpecialOrderText=>customer==10?"PEÇA DE CASA · Ravi quer manter a própria camiseta.":customer==12?"LOOK DE COLEÇÃO · Cima, baixo e calçado AFTER HOURS.":"EM SINTONIA · Joana quer o mesmo calçado do Ravi: "+Catalog.Get(PartnerShoe).name+".";
        bool SpecialOrderSatisfied(){
            if(!SpecialOrderActive)return true;
            if(customer==10)return outfit[0]==Catalog.ArrivalPiece(0).id;
            if(customer==12){for(int slot=0;slot<3;slot++)if(Catalog.Get(outfit[slot])?.drop!="AFTER HOURS")return false;return true;}
            return outfit[2]==PartnerShoe;
        }
        bool AllowSpecialOrderPiece(Garment item){
            if(!SpecialOrderActive||customer!=10||item.slot!=0||item.id==Catalog.ArrivalPiece(0).id)return true;
            message="Ravi trouxe esta camiseta e pediu para mantê-la. Escolha as outras peças.";PlayClip(Clip("ui_unavailable"),.55f);return false;
        }
        void DrawSpecialOrder(){
            if(!SpecialOrderActive||showDrops||showRequest)return;
            DrawKaosPanel(new Rect(38,365,260,215));
            Txt(new Rect(54,381,228,110),SpecialOrderText,caption,paper);
            Txt(new Rect(54,497,228,28),SpecialOrderSatisfied()?"✓ Pedido especial atendido":"Combine antes de finalizar",tiny,SpecialOrderSatisfied()?paper:FeedAccent);
            if(customer==19&&Tap(new Rect(54,535,228,32),"Usar calçado do Ravi"))ChooseWardrobePiece(Catalog.Get(PartnerShoe),false);
        }
    }
}
