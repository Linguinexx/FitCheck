using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class StudioView
    {
        void BuildAbstractBackdrop()
        {
            var material=new Material(Resources.Load<Shader>("AbstractBackdrop"));Own(material);
            material.mainTexture=Resources.Load<Texture2D>("Painted/AbstractFlow");
            Surface("Fundo abstrato — fluxo descendente",
                new List<Vector3>{new Vector3(-.5f,-.5f,0),new Vector3(.5f,-.5f,0),new Vector3(.5f,.5f,0),new Vector3(-.5f,.5f,0)},
                new List<int>{0,2,1,0,3,2},new List<Vector2>{Vector2.zero,Vector2.right,Vector2.one,Vector2.up},null,
                new Vector3(0,2.2f,2.51f),new Vector3(3.65f,4.4f,1),material,transform);
        }
    }
}
