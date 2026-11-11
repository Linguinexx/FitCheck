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
            new ClothingDrop("AH","AFTER HOURS","Depois do último ônibus.","Camadas noturnas, couro e peças para continuar na rua depois do expediente.",new Color32(230,209,191,255),new Color32(35,29,32,255)),
            new ClothingDrop("CG","COMMON GROUND","A rua é de geral.","Malhas de quadra, jeans largos e utilitários que circulam entre a pista e a praça.",new Color32(188,208,152,255),new Color32(28,36,31,255)),
            new ClothingDrop("SB","SIDE B","Fora do lado óbvio.","Camisetas autorais, camadas de backstage e acessórios com energia de música independente.",new Color32(235,195,124,255),new Color32(39,32,29,255))
        };
        public static ClothingDrop Get(string name)=>Array.Find(All,drop=>drop.name==name);
        public static Garment[] Pieces(bool byDrop,int group)
        {
            if(!byDrop)return Catalog.All.Where(item=>item.slot==Mathf.Clamp(group,0,3)).ToArray();
            string name=All[Mathf.Clamp(group,0,All.Length-1)].name;
            // Stable ordering retains the approved black / blue / green shoe order.
            return Catalog.All.Where(item=>item.drop==name).OrderBy(item=>item.slot).ThenBy(item=>item.unlockAt).ToArray();
        }
        public static string[] RewardsAt(int reputation)=>Catalog.All.Where(item=>item.unlockAt==reputation).Select(item=>item.name).ToArray();
        public static string Guide=>string.Join("\n\n",All.Select(drop=>drop.name+"\n"+drop.story+"\n"+Catalog.All.Count(item=>item.drop==drop.name)+" peças • as etiquetas mostram a Reputação necessária.\n"+drop.label))+"\n\nDESBLOQUEIOS\nAlcance a Reputação indicada na peça. As novas entregas chegam ao fechar o expediente. No replay, vale o catálogo daquele atendimento.";
    }
}
