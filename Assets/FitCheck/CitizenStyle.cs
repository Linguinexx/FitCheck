using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        static PlayerAppearance CitizenAppearance(int id)
        {
            return new PlayerAppearance{created=true,name="Morador "+id,skin=id%6,hair=new[]{2,6,1,0,4,5,3,0,6}[id%9],hairColor=new[]{4,1,2,3,0,4,1,2,0}[id%9],expression=(id+2)%6};
        }
    }
}
