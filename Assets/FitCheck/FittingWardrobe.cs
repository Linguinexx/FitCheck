using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        bool wardrobeByDrop;
        int wardrobeDrop;
        static readonly string[] WardrobeSlots={"Cima","Baixo","Calçados","Cabeça"};
        static readonly string[] WardrobeTypes={"CIMA","BAIXO","CALÇADO","CABEÇA"};
        static readonly Color WardrobeCard=new Color32(29,27,30,255),WardrobeCardHover=new Color32(43,39,42,255);
        GUIStyle wardrobeName,wardrobeCompactName,wardrobeSmall,wardrobeTab;

        void WardrobeStyles()
        {
            if(wardrobeName!=null)return;
            wardrobeName=new GUIStyle(caption){font=uiStrong,fontSize=17,wordWrap=true,alignment=TextAnchor.UpperLeft,padding=new RectOffset()};
            wardrobeCompactName=new GUIStyle(wardrobeName){fontSize=15};
            wardrobeSmall=new GUIStyle(tiny){wordWrap=false,fontSize=13,padding=new RectOffset()};
            wardrobeTab=new GUIStyle(action){fontSize=14,wordWrap=false};
        }
        GUIStyle WardrobeNameStyle(string name)=>wardrobeName.CalcHeight(new GUIContent(name),152)<=37?wardrobeName:wardrobeCompactName;
        bool WardrobeTab(Rect r,string label,bool selected)
        {
            bool hot=GUI.enabled&&r.Contains(Event.current.mousePosition);
            RoundedPanel(new Rect(r.x,r.y+3,r.width,r.height),new Color(0,0,0,.4f),8);
            RoundedPanel(r,selected?CreatorRed:hot?KaosHover:KaosControl,8);
            Txt(new Rect(r.x+4,r.y+1,r.width-8,r.height-2),label,wardrobeTab,paper);
            if(!GUI.Button(r,GUIContent.none,GUIStyle.none))return false;
            Sound(0);return true;
        }
        void SetWardrobeGrouping(bool byDrop)
        {
            if(wardrobeByDrop==byDrop)return;
            wardrobeByDrop=byDrop;scroll=Vector2.zero;message="";
            if(byDrop){var worn=Catalog.Get(outfit[category]);int index=Array.FindIndex(DropCatalog.All,drop=>drop.name==worn?.drop);if(index>=0)wardrobeDrop=index;}
        }
        void ChooseWardrobePiece(Garment item,bool locked)
        {
            if(stage!=Stage.Dressing)return;
            if(locked){PlayClip(Clip("ui_unavailable"),.55f);message=item.name+" • "+item.drop+". "+(replayIndex>=0?"Peça fora do catálogo deste atendimento.":progress.reached.Contains(item.unlockAt)?"Entrega ao fechar o expediente.":"Alcance "+item.unlockAt+" RP; entrega ao fechar o expediente.");return;}
            if(outfit[item.slot]!=item.id)PlayWardrobeSound();
            outfit[item.slot]=item.id;category=item.slot;message="";Apply();
        }
        void DrawWardrobeShelf()
        {
            WardrobeStyles();
            Fill(new Rect(1000,88,440,812),surface);
            Txt(new Rect(1032,112,370,41),"Monte o fit.",heading);
            if(WardrobeTab(new Rect(1032,167,179,36),"Por peça",!wardrobeByDrop))SetWardrobeGrouping(false);
            if(WardrobeTab(new Rect(1221,167,179,36),"Por drop",wardrobeByDrop))SetWardrobeGrouping(true);
            if(wardrobeByDrop){
                for(int i=0;i<DropCatalog.All.Length;i++)if(WardrobeTab(new Rect(1032+i*125,219,118,39),i==0?"After Hours":i==1?"Common Ground":"Side B",wardrobeDrop==i)){wardrobeDrop=i;scroll=Vector2.zero;message="";}
            }else{
                for(int i=0;i<4;i++)if(WardrobeTab(new Rect(1032+i*94,219,87,39),WardrobeSlots[i],category==i)){category=i;scroll=Vector2.zero;message="";}
            }
            var items=DropCatalog.Pieces(wardrobeByDrop,wardrobeByDrop?wardrobeDrop:category);
            var available=new HashSet<string>(ActiveInventory);
            bool originals=shopDressingInStore||replayIndex>=0;
            float gridTop;
            if(wardrobeByDrop){
                DrawDropShelfHeader(new Rect(1032,273,368,67),DropCatalog.All[wardrobeDrop],items.Count(item=>available.Contains(item.id)),items.Length);
                gridTop=353;
                if(originals){
                    for(int slot=0;slot<3;slot++){
                        var original=Catalog.ArrivalPiece(slot);
                        if(WardrobeTab(new Rect(1032+slot*125,353,118,32),slot==0?"Cima original":slot==1?"Baixo original":"Calçado original",outfit[slot]==original.id))ChooseWardrobePiece(original,false);
                    }
                    gridTop=397;
                }
            }else{
                Txt(new Rect(1032,274,368,23),items.Count(item=>available.Contains(item.id))+" / "+items.Length+" peças disponíveis • 3 drops",wardrobeSmall,muted);
                gridTop=311;
                if(originals&&category<3){
                    var original=Catalog.ArrivalPiece(category);
                    if(WardrobeTab(new Rect(1032,306,368,32),original.name,outfit[category]==original.id))ChooseWardrobePiece(original,false);
                    gridTop=351;
                }
            }
            bool emptyAccessory=wardrobeByDrop||category==3;
            int count=items.Length+(emptyAccessory?1:0),rows=(count+1)/2;
            var viewport=new Rect(1026,gridTop,392,721-gridTop);
            scroll=GUI.BeginScrollView(viewport,scroll,new Rect(0,0,366,rows*186));
            for(int i=0;i<count;i++){
                var r=new Rect(6+(i%2)*184,(i/2)*186,174,176);
                if(r.yMax<scroll.y||r.y>scroll.y+viewport.height)continue;
                if(i==items.Length){DrawNoAccessory(r);continue;}
                var item=items[i];bool locked=!available.Contains(item.id),selected=outfit[item.slot]==item.id;
                DrawWardrobeCard(r,item,locked,selected);
                if(GUI.Button(r,GUIContent.none,GUIStyle.none))ChooseWardrobePiece(item,locked);
            }
            GUI.EndScrollView();
            Rule(1032,734,366);
            Txt(new Rect(1032,745,366,42),message.Length>0?message:CatalogTesting.Enabled&&replayIndex<0?"Catálogo de teste liberado. O progresso do save é preservado.":"Misture os drops. O visual só é enviado quando você finalizar.",tiny,muted);
            if(Tap(new Rect(1032,799,366,62),"Finalizar visual  →",true))stage=Stage.Confirm;
        }
        void DrawDropShelfHeader(Rect r,ClothingDrop drop,int available,int total)
        {
            RoundedPanel(r,drop.backdrop,9);
            DrawDropPattern(new Rect(r.xMax-108,r.y+5,100,r.height-10),drop,.28f);
            Fill(new Rect(r.x+12,r.y+12,3,r.height-24),CreatorRed);
            Txt(new Rect(r.x+25,r.y+7,235,21),drop.name,wardrobeTab,drop.ink);
            Txt(new Rect(r.x+25,r.y+28,235,20),drop.label,wardrobeSmall,paper);
            Txt(new Rect(r.x+25,r.y+47,235,17),available+" / "+total+" peças disponíveis",wardrobeSmall,muted);
        }
        void DrawDropPattern(Rect r,ClothingDrop drop,float opacity)
        {
            GUI.BeginGroup(r);
            var ink=new Color(drop.ink.r,drop.ink.g,drop.ink.b,opacity);
            if(drop.code=="AH"){
                for(int i=0;i<6;i++)Fill(new Rect(9+i*24,6,2,r.height-12),ink);
                Fill(new Rect(0,r.height*.57f,r.width,2),ink);Fill(new Rect(0,r.height*.65f,r.width,1),ink);
            }else if(drop.code=="CG"){
                RoundedPanel(new Rect(8,7,r.width-16,r.height-14),ink,3,1);
                Fill(new Rect(r.width*.5f,7,1,r.height-14),ink);
                RoundedPanel(new Rect(r.width*.5f-15,r.height*.5f-15,30,30),ink,15,1);
                Fill(new Rect(8,r.height*.28f,18,1),ink);Fill(new Rect(8,r.height*.72f,18,1),ink);
            }else{
                for(int i=0;i<4;i++){float size=34+i*18;RoundedPanel(new Rect(r.width*.65f-size/2,r.height*.5f-size/2,size,size),ink,size/2,1);}
                RoundedPanel(new Rect(r.width*.65f-6,r.height*.5f-6,12,12),ink,6);
            }
            GUI.EndGroup();
        }
        void DrawWardrobeCard(Rect r,Garment item,bool locked,bool selected)
        {
            var drop=DropCatalog.Get(item.drop);
            bool hot=GUI.enabled&&r.Contains(Event.current.mousePosition);
            RoundedPanel(new Rect(r.x,r.y+4,r.width,r.height),new Color(0,0,0,.55f),10);
            RoundedPanel(r,selected?CreatorRed:hot?KaosHover:KaosControl,10);
            RoundedPanel(new Rect(r.x+2,r.y+2,r.width-4,r.height-4),hot?WardrobeCardHover:WardrobeCard,8);
            var image=new Rect(r.x+8,r.y+8,r.width-16,88);
            RoundedPanel(image,drop.backdrop,6);
            DrawDropPattern(image,drop,locked?.10f:.19f);
            RoundedPanel(new Rect(image.x+16,image.yMax-10,image.width-32,4),new Color(0,0,0,.4f),2);
            var color=GUI.color;GUI.color=new Color(1,1,1,locked?.65f:1);
            ClothingIcon(new Rect(image.x+6,image.y+25,image.width-12,image.height-29),item,locked);GUI.color=color;
            RoundedPanel(new Rect(image.x+4,image.y+4,74,19),KaosPanel,4);
            Txt(new Rect(image.x+8,image.y+4,67,18),WardrobeTypes[item.slot],wardrobeSmall,muted);
            if(selected){RoundedPanel(new Rect(image.xMax-69,image.y+4,65,19),CreatorRed,4);Txt(new Rect(image.xMax-68,image.y+4,63,18),"NO FIT",wardrobeSmall,paper);}
            Txt(new Rect(r.x+11,r.y+103,r.width-22,37),item.name,WardrobeNameStyle(item.name),locked?muted:paper);
            Txt(new Rect(r.x+11,r.y+139,r.width-22,18),item.drop,wardrobeSmall,drop.ink);
            string state=locked?(replayIndex>=0?"FORA DESTE REPLAY":progress.reached.Contains(item.unlockAt)?"ENTREGA AO FECHAR":item.unlockAt+" RP • BLOQUEADA"):(item.unlockAt==0?"DESDE O INÍCIO":item.unlockAt+" RP • DISPONÍVEL");
            Txt(new Rect(r.x+11,r.y+157,r.width-22,17),state,wardrobeSmall,locked?muted:paper);
        }
        void DrawNoAccessory(Rect r)
        {
            bool selected=string.IsNullOrEmpty(outfit[3]);
            RoundedPanel(new Rect(r.x,r.y+4,r.width,r.height),new Color(0,0,0,.5f),10);
            RoundedPanel(r,selected?CreatorRed:KaosControl,10);
            RoundedPanel(new Rect(r.x+2,r.y+2,r.width-4,r.height-4),KaosPanel,8);
            Txt(new Rect(r.x+15,r.y+25,r.width-30,45),"—",heading,muted);
            Txt(new Rect(r.x+11,r.y+103,r.width-22,37),"Sem acessório",wardrobeName,paper);
            Txt(new Rect(r.x+11,r.y+139,r.width-22,18),"CABEÇA LIVRE",wardrobeSmall,muted);
            if(GUI.Button(r,GUIContent.none,GUIStyle.none)&&stage==Stage.Dressing){outfit[3]="";category=3;message="";Sound(0);Apply();}
        }
    }
}
