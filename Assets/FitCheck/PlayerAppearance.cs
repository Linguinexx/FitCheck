using System;
using System.IO;
using UnityEngine;

namespace FitCheck
{
    [Serializable] public class PlayerAppearance
    {
        public int version=1,skin,hair,hairColor,expression,appearanceBase;public bool created;public string name="Você";
        public const int BlackPowerHair=7,ShortCoilsHair=8;
        public static readonly string[] HairNames={"Cachos","Coque","Raspado","Franja","Penteado","Assimétrico","Ondulado longo","Black Power","Crespo curto"};
        public static readonly string[] ColorNames={"Castanho","Preto","Ruivo","Loiro","Grisalho","Ameixa"};
        public static readonly string[] FaceNames={"Neutra","Curiosa","Desconfiada","Sonolenta","Animada","Surpresa"};
        public static readonly Color[] SkinColors={new Color(.67f,.40f,.23f),new Color(.94f,.72f,.58f),new Color(.25f,.135f,.085f),new Color(.76f,.60f,.40f),new Color(.53f,.33f,.23f),new Color(.38f,.205f,.13f)};
        public static readonly Color[] HairColors={new Color(.16f,.075f,.035f),new Color(.045f,.033f,.028f),new Color(.49f,.17f,.065f),new Color(.78f,.59f,.27f),new Color(.52f,.51f,.49f),new Color(.23f,.075f,.19f)};
        public PlayerAppearance Copy(){return new PlayerAppearance{version=version,name=name,created=created,skin=skin,hair=hair,hairColor=hairColor,expression=expression,appearanceBase=appearanceBase};}
        public void Validate()
        {
            if(version!=1||string.IsNullOrWhiteSpace(name)||name.Length>24||skin<0||skin>=SkinColors.Length||hair<0||hair>=HairNames.Length||hairColor<0||hairColor>=HairColors.Length||expression<0||expression>=FaceNames.Length||appearanceBase<0||appearanceBase>1)
                throw new ArgumentException("Aparência inválida");
            foreach(char c in name)if(char.IsControl(c))throw new ArgumentException("Nome inválido");
        }
    }
    public static class AppearanceStore
    {
        static PlayerAppearance Read(string path){var value=JsonUtility.FromJson<PlayerAppearance>(File.ReadAllText(path));if(value==null)throw new ArgumentException("Aparência vazia");value.Validate();return value;}
        public static PlayerAppearance Load(string path)
        {
            if(!File.Exists(path)&&!File.Exists(path+".bak"))return new PlayerAppearance();
            try{return Read(path);}catch{if(File.Exists(path+".bak"))return Read(path+".bak");throw;}
        }
        public static void Save(string path,PlayerAppearance value)
        {
            value.Validate();Directory.CreateDirectory(Path.GetDirectoryName(path));string temporary=path+".tmp";
            File.WriteAllText(temporary,JsonUtility.ToJson(value,true));Read(temporary);
            if(File.Exists(path)){
                bool valid=true;try{Read(path);}catch{valid=false;}
                File.Replace(temporary,path,valid?path+".bak":null);
            }else File.Move(temporary,path);
        }
    }
}
