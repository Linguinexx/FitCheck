using System;
using UnityEngine;
namespace FitCheck
{
    [Serializable] public class PetAppearance
    {
        public int species,breed,coat=2;public string name="Pipoca";
        public static readonly string[] CatBreeds={"Sem raça definida","Persa","Siamês"},DogBreeds={"Vira-lata","Shiba","Dachshund"};
        public static readonly string[] CoatNames={"Caramelo","Preto","Cinza","Creme","Bicolor","Chocolate"};
        public static readonly Color[] Coats={new Color(.67f,.36f,.14f),new Color(.085f,.075f,.07f),new Color(.39f,.42f,.43f),new Color(.85f,.76f,.59f),new Color(.91f,.88f,.80f),new Color(.30f,.14f,.075f)};
        public string[] Breeds=>species==0?CatBreeds:DogBreeds;
        public string CoatName=>species==0&&coat==4?"Branco":CoatNames[coat];
        public Color CoatColor=>species==0&&coat==4?new Color(.97f,.97f,.94f):Coats[coat];
        public PetAppearance Copy()=>new PetAppearance{species=species,breed=breed,coat=coat,name=name};
        public void Validate(){if(species<0||species>1||breed<0||breed>2||coat<0||coat>=CoatNames.Length||string.IsNullOrWhiteSpace(name)||name.Length>20)throw new ArgumentException("Companheiro inválido");foreach(char c in name)if(char.IsControl(c))throw new ArgumentException("Nome do companheiro inválido");}
    }
}
