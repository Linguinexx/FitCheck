using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class CityView
    {
        [SerializeField] Transform[] fittingCurtains=new Transform[2];
        [SerializeField] float fittingProgress;[SerializeField] bool fittingOpening;
        public float FittingProgress=>fittingProgress;
        public bool FittingOpen=>fittingProgress>=1;
        void BuildFittingCurtain(int side)
        {
            const int columns=36,rows=12;var vertices=new List<Vector3>();var uv=new List<Vector2>();var colors=new List<Color>();
            var clothTriangles=new List<int>();var ringTriangles=new List<int>();
            for(int row=0;row<=rows;row++)for(int col=0;col<=columns;col++){
                float u=col/(float)columns,v=row/(float)rows;
                vertices.Add(new Vector3(u-.5f,.12f+v*3.38f,5.15f+Mathf.Cos(u*Mathf.PI*12)*(.05f+(1-v)*.045f)));
                uv.Add(new Vector2(u*6,v*8));colors.Add(FittingPurple);
            }
            for(int row=0;row<rows;row++)for(int col=0;col<columns;col++){
                int a=row*(columns+1)+col,b=a+columns+1;clothTriangles.AddRange(new[]{a,b,b+1,a,b+1,a+1});
            }
            for(int ring=0;ring<9;ring++){
                int first=vertices.Count;var center=new Vector3(-.46f+ring*.115f,3.58f,5.15f);
                for(int segment=0;segment<12;segment++)for(int tube=0;tube<4;tube++){
                    float a=segment*Mathf.PI/6,b=tube*Mathf.PI/2;var radial=new Vector3(0,Mathf.Cos(a),Mathf.Sin(a));
                    vertices.Add(center+radial*(.095f+Mathf.Cos(b)*.014f)+Vector3.right*(Mathf.Sin(b)*.014f));
                    uv.Add(Vector2.zero);colors.Add(Cream);
                }
                for(int segment=0;segment<12;segment++)for(int tube=0;tube<4;tube++){
                    int a=first+segment*4+tube,b=first+(segment+1)%12*4+tube,c=first+(segment+1)%12*4+(tube+1)%4,d=first+segment*4+(tube+1)%4;
                    ringTriangles.AddRange(new[]{a,b,c,a,c,d});
                }
            }
            var mesh=new Mesh{name="Cortina com pregas e argolas"};mesh.SetVertices(vertices);mesh.SetUVs(0,uv);mesh.SetColors(colors);
            mesh.subMeshCount=2;mesh.SetTriangles(clothTriangles,0);mesh.SetTriangles(ringTriangles,1);mesh.RecalculateNormals();mesh.RecalculateBounds();assets.Add(mesh);
            var cloth=CityMaterial("Cloth");cloth.SetFloat("_Cull",0);var metal=CityMaterial("CityMetal");assets.Add(cloth);assets.Add(metal);
            var panel=new GameObject(side<0?"Cortina esquerda — deslizante":"Cortina direita — deslizante");panel.transform.SetParent(transform,false);
            panel.AddComponent<MeshFilter>().sharedMesh=mesh;panel.AddComponent<MeshRenderer>().sharedMaterials=new[]{cloth,metal};
            fittingCurtains[side<0?0:1]=panel.transform;
        }
        public void ResetFittingCurtain(){fittingProgress=0;fittingOpening=false;PoseFittingCurtain();}
        public void OpenFittingCurtain(){fittingOpening=true;}
        void AdvanceFittingCurtain(float delta)
        {
            if(fittingOpening)fittingProgress=Mathf.MoveTowards(fittingProgress,1,delta/.72f);
            PoseFittingCurtain();
        }
        void PoseFittingCurtain()
        {
            if(!IsInterior)return;float t=Mathf.SmoothStep(0,1,fittingProgress);
            for(int i=0;i<2;i++)if(fittingCurtains[i]!=null){
                float side=i==0?-1:1;
                fittingCurtains[i].localPosition=new Vector3(-4.9f+side*Mathf.Lerp(1.2125f,1.75f,t),0,0);
                fittingCurtains[i].localScale=new Vector3(Mathf.Lerp(2.43f,.80f,t),1,1);
            }
        }
        bool FittingCurtainBlocks(Vector2 point)
        {
            float t=Mathf.SmoothStep(0,1,fittingProgress),width=Mathf.Lerp(2.43f,.80f,t);
            for(int side=-1;side<=1;side+=2){
                float x=-4.9f+side*Mathf.Lerp(1.2125f,1.75f,t);
                if(Mathf.Abs(point.x-x)<width*.5f+Radius&&Mathf.Abs(point.y-5.15f)<.10f+Radius)return true;
            }
            return false;
        }
    }
}
