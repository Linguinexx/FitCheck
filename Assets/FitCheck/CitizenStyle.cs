using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        static PlayerAppearance CitizenAppearance(int id)
        {
            return new PlayerAppearance{created=true,name="Morador "+id,skin=id%6,hair=new[]{2,6,1,0,4,5,3,0,6}[id%9],hairColor=new[]{4,1,2,3,0,4,1,2,0}[id%9],expression=(id+2)%6};
        }
        void CivilianClothes(StudioView studio,int id)
        {
            var tops=new[]{new Color(.90f,.52f,.07f),new Color(.09f,.62f,.36f),new Color(.91f,.85f,.63f),new Color(.43f,.13f,.75f),new Color(.86f,.24f,.11f),new Color(.04f,.52f,.58f),new Color(.97f,.67f,.06f),new Color(.18f,.31f,.75f),new Color(.82f,.12f,.43f)};
            foreach(var r in studio.Avatar.GetComponentsInChildren<MeshRenderer>()){
                string n=r.name;
                if(n.Contains("Estampa")||n.Contains("Grafismo")||n.Contains("Identificação")||n.Contains("Carretel")||n.Contains("Etiqueta")){r.gameObject.SetActive(false);continue;}
                Color? tint=null;
                if(n.Contains("Camiseta")||n.Contains("Manga")||n=="Gola"||n=="Capuz"||n.Contains("Barra"))tint=tops[id%tops.Length];
                else if(n=="Calça"||n=="Bermuda"||n=="Cintura"||n=="Bolso")tint=id%2==0?new Color(.30f,.33f,.29f):new Color(.26f,.30f,.36f);
                else if(n.StartsWith("Tênis"))tint=id%3==0?new Color(.44f,.28f,.20f):new Color(.66f,.64f,.55f);
                if(tint.HasValue)foreach(var mat in r.sharedMaterials)mat.color=tint.Value;
            }
        }
    }
}
