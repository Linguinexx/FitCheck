using System;
using System.Linq;
using UnityEngine;

namespace FitCheck
{
    [Serializable] public class Garment
    {
        public string id, name, drop; public int slot; public Color color;
        public bool neutral, loose, discreet;
        public bool light, shorts, cap, vibrant;
        public int unlockAt;
        public Garment(string id, string name, string drop, int slot, Color color, bool neutral, bool loose = false, bool discreet = true)
        { this.id=id; this.name=name; this.drop=drop; this.slot=slot; this.color=color; this.neutral=neutral; this.loose=loose; this.discreet=discreet; }
    }
    public static class Catalog
    {
        public static readonly Garment[] Items = {
            new Garment("AH_TOP_01", "Last Stop", "AFTER HOURS", 0, new Color(.88f,.86f,.79f), true, true),
            new Garment("CG_TOP_01", "Local Team", "COMMON GROUND", 0, new Color(.9f,.83f,.65f), false),
            new Garment("SB_TOP_01", "Demo Tape", "SIDE B", 0, Color.gray, true),
            new Garment("AH_BOTTOM_01", "Late Shift", "AFTER HOURS", 1, new Color(.15f,.16f,.18f), true),
            new Garment("CG_BOTTOM_01", "Open Court", "COMMON GROUND", 1, new Color(.15f,.3f,.65f), false),
            new Garment("CG_BOTTOM_02", "Home Turf", "COMMON GROUND", 1, new Color(.85f,.28f,.12f), false, true),
            new Garment("SB_BOTTOM_01", "Backstage", "SIDE B", 1, Color.black, true),
            new Garment("SB_SHOES_01", "Garage Session", "SIDE B", 2, new Color(.12f,.12f,.14f), true),
            new Garment("AH_SHOES_01", "Sidewalk", "AFTER HOURS", 2, new Color(.88f,.86f,.79f), true),
            new Garment("AH_HEAD_01", "After Dark", "AFTER HOURS", 3, new Color(.08f,.12f,.3f), false),
            new Garment("CG_HEAD_01", "Neighborhood", "COMMON GROUND", 3, new Color(.9f,.83f,.65f), false)
        };
        public static readonly Garment[] All = Items.Concat(new[]{
            new Garment("AH_TOP_02","Night Route","AFTER HOURS",0,new Color(.08f,.12f,.3f),false,true,false){unlockAt=100},
            new Garment("SB_BOTTOM_02","Remix","SIDE B",1,Color.gray,true,true){unlockAt=100,shorts=true},
            new Garment("CG_TOP_02","Court Lines","COMMON GROUND",0,new Color(.12f,.5f,.25f),false,true,false){unlockAt=200},
            new Garment("CG_SHOES_01","Daily Steps","COMMON GROUND",2,new Color(.2f,.4f,.2f),false){unlockAt=200},
            new Garment("AH_TOP_03","Closing Time","AFTER HOURS",0,new Color(.15f,.15f,.18f),true,false,false){unlockAt=300},
            new Garment("SB_HEAD_01","Independent","SIDE B",3,Color.black,true,false,false){unlockAt=300,cap=true},
            new Garment("SB_TOP_02","Cut & Paste","SIDE B",0,Color.white,true,true,false){unlockAt=450,light=true},
            new Garment("AH_BOTTOM_02","City Glow","AFTER HOURS",1,new Color(.6f,.15f,.85f),false,true){unlockAt=450,shorts=true,vibrant=true},
            new Garment("CG_TOP_03","Meeting Point","COMMON GROUND",0,new Color(.15f,.3f,.65f),false,false,false){unlockAt=600},
            new Garment("SB_TOP_03","Unreleased","SIDE B",0,Color.black,true,false,false){unlockAt=600}
        }).ToArray();
        static readonly Garment[] ArrivalPieces={
            new Garment("CIV_ARRIVAL_TOP","Parte de cima original","CIVIL",0,Color.gray,true),
            new Garment("CIV_ARRIVAL_BOTTOM","Parte de baixo original","CIVIL",1,Color.gray,true),
            new Garment("CIV_ARRIVAL_SHOES","Tênis original","CIVIL",2,Color.gray,true)
        };
        public static Garment ArrivalPiece(int slot)=>ArrivalPieces[slot];
        public static bool IsArrivalPiece(string id)=>id==ArrivalPieces[0].id||id==ArrivalPieces[1].id||id==ArrivalPieces[2].id;
        public static string[] ArrivalOutfit=>new[]{ArrivalPieces[0].id,ArrivalPieces[1].id,ArrivalPieces[2].id,""};
        public static Garment Get(string id) => All.FirstOrDefault(x=>x.id==id)??Array.Find(ArrivalPieces,x=>x.id==id);
        static Catalog()
        {
            foreach(var g in All.Where(g=>g.id.EndsWith("TOP_03")))g.loose=true;
            foreach(var id in new[]{"AH_TOP_01","CG_TOP_01","SB_TOP_01","AH_SHOES_01","CG_HEAD_01"}) Get(id).light=true;
            Get("CG_BOTTOM_02").shorts=true; Get("CG_BOTTOM_02").vibrant=true;
            Get("CG_BOTTOM_02").color=new Color(.94f,.29f,.09f);
            Get("CG_HEAD_01").cap=true;
            Get("SB_BOTTOM_02").light=true;
        }
        public static string[] Default => new[]{"CG_TOP_01","CG_BOTTOM_01","AH_SHOES_01",""};
    }
    [Serializable] public class Attempt
    {
        public int version=1, question=-1, score, reputation; public bool completed;
        public string[] outfit=Catalog.Default; public string photo="";
        public string[] available;
        public int photoStyle;
    }
    public static class Rules
    {
        // Temporary design convention: majority of top/bottom/shoes; headwear excluded.
        // Discreet print applies to every worn piece. Documented in README for review.
        public static bool[] Evaluate(string[] ids)
        {
            if(ids==null || ids.Length!=4) throw new ArgumentException("Conjunto inválido");
            for(int i=0;i<3;i++) if(Catalog.Get(ids[i])?.slot!=i) throw new ArgumentException("Categoria obrigatória inválida");
            if(!string.IsNullOrEmpty(ids[3]) && Catalog.Get(ids[3])?.slot!=3) throw new ArgumentException("Acessório inválido");
            return EvaluatePieces(ids.Select(Catalog.Get).ToArray());
        }
        internal static bool[] EvaluatePieces(Garment[] pieces)=>new[]{pieces[0].loose,
            pieces.Take(3).Count(piece=>piece.neutral)>=2,pieces.All(piece=>piece==null||piece.discreet)};
        public static int Score(string[] ids) { var c=Evaluate(ids); return (c[0]?4:0)+(c[1]?3:0)+(c[2]?3:0); }
        public static int TutorialReputation(int score) => score==10?100:score==7?60:score==6?30:0;
    }
}
