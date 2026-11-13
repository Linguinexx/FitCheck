using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class CityView
    {
        [SerializeField] List<Material> nightMaterials=new List<Material>();
        [SerializeField] List<Transform> radioBars=new List<Transform>();
        Transform traffic;float trafficX=-38;
        public float AmbientTime=>atmosphereTime;
        Material CityMaterial(string key)
        {
            if(key=="WeekSign"){var sign=new Material(Resources.Load<Shader>("CitySign")){name="Cartazes da semana — atlas compartilhado"};sign.mainTexture=Resources.Load<Texture2D>("WeekSigns");sign.SetFloat("_Emission",.18f);return sign;}
            if(key=="CommerceSign"){var sign=new Material(Resources.Load<Shader>("CitySign")){name="Comércio local — letreiros impressos"};sign.mainTexture=Resources.Load<Texture2D>("CommerceSigns");sign.SetFloat("_Emission",0);return sign;}
            if(key=="CityWindow"){
                var glass=new Material(Resources.Load<Shader>("WindowGlass")){name="Vidro texturizado — reflexos e desgaste"};glass.mainTexture=Resources.Load<Texture2D>("Surfaces/Metal032");glass.SetFloat("_Interior",IsInterior?1:0);return glass;
            }
            if(key=="CitySign"||key=="CitySignGlow"){
                var printed=new Material(Resources.Load<Shader>("CitySign")){name="Letreiro impresso"};printed.mainTexture=Resources.Load<Texture2D>("CitySigns");printed.SetFloat("_Emission",key=="CitySignGlow"?1:0);return printed;
            }
            var m=new Material(Resources.Load<Shader>("CityPaint")){name="Bairro noturno — "+key};
            int tile=key=="CityStucco"?1:key=="CityBrick"?2:key=="CityMetal"?3:key=="CityAsphalt"?4:0;
            string source=key=="CityBanners"||key=="CityLeaves"?"Cloth":key=="CityGlow"||key=="CityWindow"?"Leather":key;
            m.mainTexture=tile>0?Resources.Load<Texture2D>("CityNightAtlas"):Resources.Load<Texture2D>("Painted/"+source);
            m.SetFloat("_SurfaceTile",tile);m.SetFloat("_TexScale",tile==1?.33f:tile==2?.30f:tile==4?.5f:1);
            m.SetFloat("_TextureWeight",tile>0?.94f:key=="CityStone"?.88f:.68f);
            m.SetFloat("_Motion",0);m.SetFloat("_Interior",IsInterior?1:0);m.SetFloat("_Emission",key=="CityGlow"?1:key=="CityWindow"?.72f:0);
            if(IsHome){var origin=transform.position;m.SetVector("_HomeOrigin",new Vector4(origin.x,origin.y,origin.z,1));}
            m.SetFloat("_Wind",key=="CityBanners"?1:0);m.SetFloat("_LeafWind",key=="CityLeaves"?1:0);
            m.SetFloat("_Cull",key=="CityBanners"||key=="CityLeaves"?0:2);
            if(key=="CityBanners")bannerMaterial=m;
            if(key=="CityBanners"||key=="CityLeaves"||key=="CityGlow")nightMaterials.Add(m);
            AmbientSurfaces.Apply(m,key,IsInterior);
            return m;
        }
        Transform LiveBox(string name,Vector3 position,Vector3 size,Color color,string material,Transform parent=null)
        {
            var go=GameObject.CreatePrimitive(PrimitiveType.Cube);go.name=name;Destroy(go.GetComponent<Collider>());
            go.transform.SetParent(parent==null?transform:parent,false);go.transform.localPosition=position;go.transform.localScale=size;
            var mesh=Instantiate(go.GetComponent<MeshFilter>().sharedMesh);var colors=new Color[mesh.vertexCount];for(int i=0;i<colors.Length;i++)colors[i]=color;
            mesh.colors=colors;go.GetComponent<MeshFilter>().sharedMesh=mesh;assets.Add(mesh);
            var m=CityMaterial(material);assets.Add(m);go.GetComponent<MeshRenderer>().sharedMaterial=m;return go.transform;
        }
        void NightDetails()
        {
            FacadeSign("DRIPLAB",new Vector3(7.5f,4.98f,3.61f),8.4f,1.44f,.175f,Ink,Cream,true);
            FacadeSign("RADIO",new Vector3(22.1f,4.35f,5.08f),4.25f,.94f,.10f,Ink,Kaos,true);
            Words("ABERTO",new Vector3(-16.8f,2.05f,4.70f),.065f,new Color(1,.72f,.40f),"CityGlow");
            Block(new Vector3(7.5f,4.1f,3.16f),new Vector3(16,.065f,.07f),new Color(1,.64f,.30f),"CityGlow");
            Block(new Vector3(7.5f,5.73f,3.61f),new Vector3(17.1f,.055f,.07f),Kaos,"CityGlow");
            Block(new Vector3(22.1f,2.15f,5.03f),new Vector3(5.5f,3.1f,.12f),Ink,"CityMetal");
            for(int i=0;i<12;i++)radioBars.Add(LiveBox("Rádio — faixa do equalizador",new Vector3(19.7f+i*.43f,1.3f,4.90f),new Vector3(.21f,1,.07f),i%3==0?Cream:Kaos,"CityGlow"));
            // Warm lamps and strings make the square legible against a genuinely dark sky.
            for(int i=0;i<15;i++){
                float x=-21+i*2.8f,y=7.7f-Mathf.Sin(i/14f*Mathf.PI)*1.0f;
                Rod(new Vector3(x,y,-3),new Vector3(x+2.8f,y-.07f,-3),.012f,Ink,"Leather",5);
                Rod(new Vector3(x,y-.15f,-3),new Vector3(x,y-.32f,-3),.10f,new Color(1,.67f,.31f),"CityGlow",8);
            }
            // A short exposed masonry plinth makes the brick material readable up close.
            Block(new Vector3(22.1f,.45f,5.3f),new Vector3(7.5f,.8f,.18f),Color.white,"CityBrick");
            traffic=new GameObject("Carro — trânsito do bairro").transform;traffic.SetParent(transform,false);traffic.localPosition=new Vector3(trafficX,0,-13.8f);traffic.localScale=new Vector3(1.5f,1.42f,1.45f);
            BuildStreetCar();
        }
        bool VehicleBlocks(Vector2 point){return traffic!=null&&Mathf.Abs(point.x-trafficX)<3.2f&&Mathf.Abs(point.y+13.8f)<1.65f;}
        void AnimateNight(float delta)
        {
            foreach(var m in nightMaterials)m.SetFloat("_CityTime",atmosphereTime);
            for(int i=0;i<radioBars.Count;i++){
                float h=.35f+(Mathf.Sin(atmosphereTime*2.8f+i*1.3f)*.5f+.5f)*1.9f;
                radioBars[i].localScale=new Vector3(.21f,h,.07f);radioBars[i].localPosition=new Vector3(19.7f+i*.43f,1.0f+h*.5f,4.9f);
            }
        }
        void TickTraffic(float delta)
        {
            if(traffic!=null){
                bool stop=Mathf.Abs(Position.y+13.8f)<2&&Position.x>trafficX-2&&Position.x<trafficX+6.4f;
                if(!stop)trafficX+=delta*3.8f;
                if(trafficX>49){
                    trafficX=-153;
                    SetTrafficPaint((trafficPaintIndex+1)%TrafficPaints.Length);
                }
                traffic.localPosition=new Vector3(trafficX,0,-13.8f);traffic.gameObject.SetActive(trafficX<42);
                foreach(var wheel in trafficWheels)wheel.localRotation=Quaternion.Euler(90,0,0)*Quaternion.Euler(0,-trafficX*120,0);
            }
        }
    }
}
