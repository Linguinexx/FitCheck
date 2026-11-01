using System;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        public void RoundedAvatarSmokeCheck()
        {
            int originalCustomer=customer;var originalOutfit=outfit;float originalRotation=rotation;
            bool originalTall=studio.TallPreview,originalPreview=studio.PreviewProportions;
            int originalExpression=studio.ExpressionOverride;
            var shoeShapes=new System.Collections.Generic.Dictionary<string,Vector3[]>();
            string originalProgress=JsonUtility.ToJson(progress);int cases=0,uvChecks=0;
            try{
                for(int shape=0;shape<2;shape++)foreach(var garment in Catalog.All){
                    studio.TallPreview=shape==1;studio.PreviewProportions=true;
                    customer=cases%9;outfit=Catalog.Default;outfit[garment.slot]=garment.id;
                    // Rebuilding while already rotated/scaled must not capture the
                    // old world transform in the clothes, hair or accessory meshes.
                    rotation=cases%2==0?57:-43;UpdateCamera();Apply();studio.Animate(0,false);
                    if((studio.Avatar.localScale-new Vector3(.98f,.94f,1)).sqrMagnitude>.0001f||studio.Avatar.Find("Pescoço").localScale.y>.321f)throw new Exception("Corpo alongado ou pescoço comprido voltou");
                    Transform clothing=null;foreach(Transform child in studio.Avatar)if(child.name=="Vestuário 3D"&&child.gameObject.activeSelf)clothing=child;
                    if(clothing==null||Quaternion.Angle(clothing.localRotation,Quaternion.identity)>.01f||(clothing.localScale-Vector3.one).sqrMagnitude>.0001f)
                        throw new Exception("Troca girada herdou rotação/escala do mundo");
                    Renderer head=null;var renderers=studio.Avatar.GetComponentsInChildren<Renderer>();
                    foreach(var r in renderers)if(r.name=="Cabeça arredondada")head=r;
                    if(head==null)throw new Exception("Cabeça arredondada ausente");
                    var skull=head.GetComponent<MeshFilter>().sharedMesh;
                    foreach(var vertex in skull.vertices){
                        float radius=Vector3.Distance(head.transform.TransformPoint(vertex),head.transform.position);
                        if(Mathf.Abs(radius-.56f)>.001f)throw new Exception("Cabeça deixou de ser esférica com a proporção do corpo");
                    }
                    int eyes=0;
                    foreach(var renderer in renderers){
                        string name=renderer.name;
                        if(name=="Mecha K7")throw new Exception("Bolinha/mecha removida voltou ao cabelo");
                        if(name=="Tênis"&&garment.slot==2)shoeShapes[garment.id]=(Vector3[])renderer.GetComponent<MeshFilter>().sharedMesh.vertices.Clone();
                        if(name=="Branco do olho"){
                            eyes++;if(renderer.bounds.size.magnitude>head.bounds.size.magnitude*.55f)throw new Exception("Expressão alterou o tamanho da malha do olho");
                        }
                        if(name=="Tênis"||name=="Cabelo ajustado à cabeça"||name=="Armação arredondada")
                            if(Quaternion.Angle(renderer.transform.localRotation,Quaternion.identity)>.01f)throw new Exception("Peça perdeu o encaixe após girar: "+name);
                        if(name!="Cabeça arredondada"&&name!="Tênis"&&name!="Sola"&&name!="Boné"&&name!="Gorro"&&name!="Cabelo ajustado à cabeça")continue;
                        var mesh=renderer.GetComponent<MeshFilter>().sharedMesh;var uv=mesh.uv;
                        if(uv.Length!=mesh.vertexCount||uv.Length==0)throw new Exception("UV ausente: "+name);
                        Vector2 lo=uv[0],hi=uv[0];
                        foreach(var point in uv){
                            if(float.IsNaN(point.x)||float.IsNaN(point.y)||float.IsInfinity(point.x)||float.IsInfinity(point.y))throw new Exception("UV inválido: "+name);
                            lo=Vector2.Min(lo,point);hi=Vector2.Max(hi,point);
                        }
                        if(hi.x-lo.x<.01f||hi.y-lo.y<.01f)throw new Exception("Textura colapsada numa linha/ponto: "+name);
                        uvChecks++;
                    }
                    if(eyes!=2)throw new Exception("Quantidade de olhos incorreta");cases++;
                }
                var shapes=new System.Collections.Generic.List<Vector3[]>(shoeShapes.Values);
                if(shapes.Count!=3)throw new Exception("Faltam construções de tênis");
                for(int a=0;a<shapes.Count;a++)for(int b=a+1;b<shapes.Count;b++){
                    int changed=0;for(int i=0;i<Mathf.Min(shapes[a].Length,shapes[b].Length);i++)if((shapes[a][i]-shapes[b][i]).sqrMagnitude>.0001f)changed++;
                    if(changed<shapes[a].Length/3)throw new Exception("Tênis continuam apenas variações de cor");
                }
                for(int e=0;e<6;e++){
                    studio.SetExpression((AvatarExpression)e);studio.Animate(0,false);
                    if((int)studio.CurrentExpression!=e)throw new Exception("Expressão não aplicada");
                    int lids=0,pupils=0,mouths=0;
                    foreach(var renderer in studio.Avatar.GetComponentsInChildren<Renderer>()){
                        if(renderer.name.StartsWith("Pálpebra"))lids++;if(renderer.name=="Pupila")pupils++;
                        if(renderer.name=="Boca"||renderer.name=="Boca animada"||renderer.name=="Boca surpresa")mouths++;
                    }
                    if(pupils!=2||mouths!=1||(e==3&&lids<2))throw new Exception("Componentes da expressão incompletos");
                }
                if(JsonUtility.ToJson(progress)!=originalProgress)throw new Exception("Teste de avatar alterou a campanha");
                Debug.Log("FITCHECK_ROUNDED_AVATAR_PASSED: "+cases+" trocas, cabeça esférica em duas proporções, "+uvChecks+" superfícies com UV válido, 3 construções de tênis, 6 expressões e campanha preservada.");
            }finally{
                customer=originalCustomer;outfit=originalOutfit;rotation=originalRotation;
                studio.TallPreview=originalTall;studio.PreviewProportions=originalPreview;studio.ExpressionOverride=originalExpression;Apply();UpdateCamera();
            }
        }
    }
}
