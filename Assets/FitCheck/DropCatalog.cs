using System;
using System.Linq;
using UnityEngine;

namespace FitCheck
{
    public sealed class ClothingDrop
    {
        public readonly string code,name,label,story;
        public readonly Color ink,backdrop;
        public ClothingDrop(string code,string name,string label,string story,Color ink,Color backdrop)
        {this.code=code;this.name=name;this.label=label;this.story=story;this.ink=ink;this.backdrop=backdrop;}
    }
    public static class DropCatalog
    {
        public static readonly ClothingDrop[] All={
            new ClothingDrop("AH","AFTER HOURS","Depois do último ônibus.","Camadas noturnas, couro e peças para continuar na rua depois do expediente.",new Color32(250,218,203,255),new Color32(105,23,47,255)),
            new ClothingDrop("CG","COMMON GROUND","A rua é de geral.","Malhas de quadra, jeans largos e utilitários que circulam entre a pista e a praça.",new Color32(214,237,171,255),new Color32(15,87,69,255)),
            new ClothingDrop("SB","SIDE B","Fora do lado óbvio.","Camisetas autorais, camadas de backstage e acessórios com energia de música independente.",new Color32(250,216,145,255),new Color32(74,40,123,255))
        };
        static readonly System.Collections.Generic.Dictionary<string,ClothingDrop> ByName=All.ToDictionary(drop=>drop.name);
        static Garment[][] slotPieces,dropPieces;
        static string guide;
        public static ClothingDrop Get(string name)=>name!=null&&ByName.TryGetValue(name,out var drop)?drop:null;
        public static Garment[] Pieces(bool byDrop,int group)
        {
            if(slotPieces==null){
                slotPieces=Enumerable.Range(0,4).Select(slot=>Catalog.All.Where(item=>item.slot==slot).ToArray()).ToArray();
                // Stable ordering retains the approved black / blue / green shoe order.
                dropPieces=All.Select(drop=>Catalog.All.Where(item=>item.drop==drop.name).OrderBy(item=>item.slot).ThenBy(item=>item.unlockAt).ToArray()).ToArray();
            }
            return byDrop?dropPieces[Mathf.Clamp(group,0,All.Length-1)]:slotPieces[Mathf.Clamp(group,0,3)];
        }
        public static string[] RewardsAt(int reputation)=>Catalog.All.Where(item=>item.unlockAt==reputation).Select(item=>item.name).ToArray();
        public static string Guide=>guide??(guide=string.Join("\n\n",All.Select(drop=>drop.name+"\n"+drop.story+"\n"+Catalog.All.Count(item=>item.drop==drop.name)+" peças • as etiquetas mostram a Reputação necessária.\n"+drop.label))+"\n\nDESBLOQUEIOS\nAlcance a Reputação indicada na peça. As novas entregas chegam ao fechar o expediente. No replay, vale o catálogo daquele atendimento.");
    }
}
