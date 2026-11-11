using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public class PhotoScenery : MonoBehaviour
    {
        readonly List<Material> materials=new List<Material>();
        Material Paint(Color color){var m=new Material(Resources.Load<Shader>("RetroPaint"));m.color=color;m.mainTexture=Resources.Load<Texture2D>("Painted/Plaster");m.SetFloat("_Motion",1);materials.Add(m);return m;}
        GameObject Piece(string name,Vector3 p,Vector3 scale,Color c,PrimitiveType kind=PrimitiveType.Cube)
        {
            var o=GameObject.CreatePrimitive(kind);o.name=name;o.transform.SetParent(transform);o.transform.localPosition=p;o.transform.localScale=scale;o.GetComponent<Renderer>().sharedMaterial=Paint(c);Destroy(o.GetComponent<Collider>());return o;
        }
        public static GameObject Build(int index)
        {var go=new GameObject("Cidade — "+DayOne.Customers[index].location);go.transform.position=new Vector3(30,0,0);
            int scenery=DayOne.CustomerScenery(index);
            if(scenery==1||scenery==2||scenery==3||scenery==5||scenery==6)go.AddComponent<CityView>().BuildPhotoLocation(scenery);
            else go.AddComponent<PhotoScenery>().Compose(scenery);return go;}
        void Compose(int index)
        {
            var sand=new Color(.66f,.6f,.48f);var stone=new Color(.38f,.4f,.38f);var green=new Color(.15f,.34f,.22f);var cream=new Color(.87f,.79f,.6f);
            Piece("Calçada",new Vector3(0,-.06f,0),new Vector3(18,.15f,18),sand);
            for(int row=0;row<4;row++)for(int i=-7;i<=7;i++)Piece("Pedra portuguesa",new Vector3(i*.45f,.025f,row*.7f-2),new Vector3(.18f,.012f,.18f),stone);
            for(int i=-3;i<=3;i++){
                float h=3.1f+(i*i%3)*.6f;var facade=i%2==0?new Color(.5f,.33f,.26f):new Color(.28f,.41f,.45f);
                Piece("Casario",new Vector3(i*2.7f,h/2,8),new Vector3(2.5f,h,2),facade);
                Piece("Marquise",new Vector3(i*2.7f,h-.2f,6.8f),new Vector3(2.7f,.14f,.7f),cream);
                for(int w=-1;w<=1;w+=2)Piece("Janela",new Vector3(i*2.7f+w*.55f,h-.9f,6.96f),new Vector3(.52f,.7f,.06f),new Color(.16f,.19f,.25f));
            }
            if(index==0){
                Piece("Banco da praça",new Vector3(1.6f,.55f,2),new Vector3(2.2f,.16f,.65f),green);
                for(int i=-1;i<=1;i+=2){Piece("Tronco",new Vector3(i*2.6f,1.4f,3),new Vector3(.23f,1.4f,.23f),new Color(.28f,.17f,.1f),PrimitiveType.Cylinder);Piece("Copa",new Vector3(i*2.6f,3.1f,3),new Vector3(2.1f,2.2f,1.8f),green,PrimitiveType.Sphere);}
            }
            if(index==1||index==6){
                Piece("Galeria de azulejos",new Vector3(0,2,3.6f),new Vector3(9,4,.3f),new Color(.25f,.48f,.53f));
                for(int x=-5;x<=5;x++)for(int y=0;y<5;y++)Piece("Azulejo",new Vector3(x*.7f,.4f+y*.7f,3.42f),new Vector3(.6f,.6f,.025f),(x+y)%2==0?cream:new Color(.19f,.37f,.44f));
                Piece("Entrada da galeria",new Vector3(-2,1.4f,3.25f),new Vector3(1.5f,2.8f,.05f),new Color(.09f,.12f,.16f));
                for(int i=0;i<3;i++){var art=Piece("Cartaz autoral",new Vector3(1.1f+i*.8f,2.0f,3.15f),new Vector3(.62f,1,.035f),index==6?new Color(.67f,.22f,.3f):new Color(.8f,.45f,.19f));art.transform.localRotation=Quaternion.Euler(0,0,(i-1)*7);}
            }
            if(index==2){
                Piece("Quadra",new Vector3(0,.03f,1),new Vector3(10,.04f,7),new Color(.28f,.38f,.28f));
                for(int i=-1;i<=1;i+=2){Piece("Linha de quadra",new Vector3(i*2.4f,.06f,.4f),new Vector3(.07f,.012f,5),cream);Piece("Linha de fundo",new Vector3(0,.06f,i*2.1f+.4f),new Vector3(4.8f,.012f,.07f),cream);}
                Piece("Muro do mural",new Vector3(0,1.7f,4),new Vector3(10,3.4f,.2f),new Color(.19f,.29f,.24f));
                for(int i=-4;i<=4;i++){var mural=Piece("Mural coletivo",new Vector3(i,1.7f,3.85f),new Vector3(.65f,2.2f,.02f),i%2==0?new Color(.78f,.35f,.16f):new Color(.58f,.44f,.7f));mural.transform.localRotation=Quaternion.Euler(0,0,i*8);}
                Piece("Tabela",new Vector3(3.1f,3.2f,3.6f),new Vector3(1.4f,.9f,.1f),cream);
            }
            if(index==3||index==5){
                Piece("Entrada do salão",new Vector3(0,2,3.6f),new Vector3(8,4,.25f),new Color(.13f,.1f,.2f));
                Piece("Porta",new Vector3(0,1.3f,3.4f),new Vector3(2.2f,2.6f,.1f),new Color(.08f,.055f,.12f));
                Piece("Letreiro luminoso",new Vector3(0,3.1f,3.25f),new Vector3(4,.55f,.18f),new Color(.65f,.28f,.75f));
                for(int i=-4;i<=4;i++)Piece("Luz de festa",new Vector3(i*.8f,3.6f,2.7f),Vector3.one*.13f,i%2==0?new Color(.93f,.52f,.18f):new Color(.5f,.36f,.9f),PrimitiveType.Sphere);
            }
            if(index==4||index==8){
                Piece("Parapeito do terraço",new Vector3(0,.7f,4),new Vector3(12,1.4f,.2f),new Color(.35f,.32f,.36f));
                if(index==4){Piece("Mesa de DJ",new Vector3(0,1.0f,-.7f),new Vector3(2.3f,.12f,.7f),new Color(.12f,.12f,.16f));for(int i=-1;i<=1;i+=2)Piece("Disco",new Vector3(i*.62f,1.08f,-.7f),new Vector3(.45f,.025f,.45f),new Color(.4f,.4f,.45f),PrimitiveType.Cylinder);}
                else for(int i=-3;i<=3;i++){Piece("Público",new Vector3(i*.8f,1,3),new Vector3(.45f,1,.35f),i%2==0?green:new Color(.5f,.28f,.2f));Piece("Público",new Vector3(i*.8f,1.75f,3),Vector3.one*.4f,new Color(.54f,.37f,.25f),PrimitiveType.Sphere);}
                for(int side=-1;side<=1;side+=2)Piece("Caixa de som",new Vector3(side*2.1f,.9f,1),new Vector3(.7f,1.6f,.65f),new Color(.07f,.07f,.1f));
            }
            if(index==7){
                Piece("Fachada DripLab",new Vector3(0,2.1f,3.6f),new Vector3(9,4.2f,.2f),new Color(.12f,.1f,.14f));
                Piece("Assinatura cromática da loja",new Vector3(0,3.1f,3.4f),new Vector3(5,.7f,.16f),new Color(.89f,.024f,.075f));
                for(int i=-1;i<=1;i+=2)Piece("Vitrine",new Vector3(i*2.2f,1.6f,3.4f),new Vector3(1.8f,2,.05f),new Color(.31f,.24f,.42f));
                for(int i=0;i<5;i++)Piece("Balão de lançamento",new Vector3(2.6f+(i%2)*.3f,1.7f+i*.3f,2.5f),Vector3.one*.45f,i%2==0?new Color(.75f,.1f,.14f):cream,PrimitiveType.Sphere);
            }
        }
        void OnDestroy(){foreach(var material in materials)Destroy(material);}
    }
}
