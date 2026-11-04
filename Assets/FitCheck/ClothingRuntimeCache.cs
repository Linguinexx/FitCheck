using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
namespace FitCheck
{
    public partial class StudioView
    {
        struct ClothingVertex:IEquatable<ClothingVertex>
        {
            public Vector3 position,normal;public Vector2 uv;
            public bool Equals(ClothingVertex other)=>position.Equals(other.position)&&normal.Equals(other.normal)&&uv.Equals(other.uv);
            public override bool Equals(object value)=>value is ClothingVertex&&Equals((ClothingVertex)value);
            public override int GetHashCode(){unchecked{return (position.GetHashCode()*397^normal.GetHashCode())*397^uv.GetHashCode();}}
        }
        sealed class ClothingMeshBuilder
        {
            public readonly List<Vector3> vertices=new List<Vector3>(),normals=new List<Vector3>();public readonly List<Vector2> uv=new List<Vector2>();
            public readonly List<List<int>> triangles=new List<List<int>>();readonly Dictionary<ClothingVertex,int> indices=new Dictionary<ClothingVertex,int>();
            public void Add(Vector3 p,Vector3 n,Vector2 tex,int material)
            {
                while(triangles.Count<=material)triangles.Add(new List<int>());
                var key=new ClothingVertex{position=p,normal=n,uv=tex};int index;
                if(!indices.TryGetValue(key,out index)){index=vertices.Count;indices.Add(key,index);vertices.Add(p);normals.Add(n);uv.Add(tex);}
                triangles[material].Add(index);
            }
            public Mesh Build(string name)
            {
                var mesh=new Mesh{name=name,indexFormat=vertices.Count>65535?IndexFormat.UInt32:IndexFormat.UInt16};mesh.SetVertices(vertices);mesh.SetNormals(normals);mesh.SetUVs(0,uv);mesh.subMeshCount=triangles.Count;
                for(int i=0;i<triangles.Count;i++)mesh.SetTriangles(triangles[i],i,false);mesh.RecalculateBounds();return mesh;
            }
        }
        sealed class ClothingTemplate {public string name;public Mesh mesh;public Material[] materials;}
        readonly Dictionary<string,List<ClothingTemplate>> clothingTemplates=new Dictionary<string,List<ClothingTemplate>>();
        readonly Dictionary<ClothingTemplate,Material[]> clothingInstanceMaterials=new Dictionary<ClothingTemplate,Material[]>();
        readonly Dictionary<string,ClothingTemplate> shopDisplayTemplates=new Dictionary<string,ClothingTemplate>();
        static string ClothingBehavior(int slot,string name)
        {
            if(slot==1)return "Calça";
            if(slot==2)return name.StartsWith("Sola")?"Sola":name=="Cadarço"?"Cadarço":"Tênis";
            if(name=="Camiseta com caimento"||name=="Capuz")return name;
            if(name.Contains("Estampa")||name.Contains("Grafismo")||name.Contains("Identificação")||name.Contains("Carretel")||name.Contains("Etiqueta"))return "BlenderTopo Estampa";
            return name.Contains("Barra")?"BlenderTopo Barra":"BlenderTopo Recortes";
        }
        void BuildCachedClothing(BlenderGarmentAsset model,string key,string texture,int slot,Vector3 offset)
        {
            List<ClothingTemplate> templates;
            if(!clothingTemplates.TryGetValue(key,out templates)){
                templates=new List<ClothingTemplate>();var groups=new Dictionary<string,List<BlenderGarmentPart>>();
                foreach(var part in model.parts){string behavior=ClothingBehavior(slot,part.name);if(!groups.TryGetValue(behavior,out var parts)){parts=new List<BlenderGarmentPart>();groups.Add(behavior,parts);}parts.Add(part);}
                var shader=Resources.Load<Shader>("GarmentFabric");var image=Resources.Load<Texture2D>(texture);
                foreach(var group in groups){
                    var builder=new ClothingMeshBuilder();var materials=new List<Material>();var palette=new Dictionary<string,int>();
                    foreach(var part in group.Value){
                        bool textured=part.textured||part.denim;var color=textured?Color.white:part.color;
                        string signature=(textured?"T":"C")+color.r.ToString("R",System.Globalization.CultureInfo.InvariantCulture)+"/"+color.g.ToString("R",System.Globalization.CultureInfo.InvariantCulture)+"/"+color.b.ToString("R",System.Globalization.CultureInfo.InvariantCulture)+"/"+color.a.ToString("R",System.Globalization.CultureInfo.InvariantCulture);
                        if(!palette.TryGetValue(signature,out int index)){
                            index=materials.Count;palette.Add(signature,index);var material=new Material(shader){name="Cached clothing — "+key,color=color,mainTexture=textured?image:Texture2D.whiteTexture};material.SetFloat("_Motion",0);material.SetFloat("_FabricWeight",0);material.SetFloat("_SmoothLight",1);material.SetFloat("_Outline",0);materials.Add(material);owned.Add(material);
                        }
                        foreach(int i in part.triangles)builder.Add(part.vertices[i],part.normals[i],part.uv[i],index);
                    }
                    var mesh=builder.Build(group.Key+(slot==1?" — Blender Baggy":" — Blender Streetwear"));owned.Add(mesh);templates.Add(new ClothingTemplate{name=group.Key,mesh=mesh,materials=materials.ToArray()});
                }
                clothingTemplates.Add(key,templates);
            }
            foreach(var template in templates){
                if(!clothingInstanceMaterials.TryGetValue(template,out var materials)){
                    materials=new Material[template.materials.Length];for(int i=0;i<materials.Length;i++){materials[i]=Instantiate(template.materials[i]);Own(materials[i]);}clothingInstanceMaterials.Add(template,materials);
                }
                var node=new GameObject(template.name);node.transform.SetParent(clothes,false);node.transform.localPosition=offset;node.AddComponent<MeshFilter>().sharedMesh=template.mesh;node.AddComponent<MeshRenderer>().sharedMaterials=materials;
            }
        }
        static bool GarmentMatchesSlot(string n,int slot)
        {
            if(slot==2)return n.StartsWith("Tênis")||n.StartsWith("Sola")||n=="Cadarço";
            return n=="Camiseta com caimento"||n.Contains("Manga")||n=="Gola"||n.Contains("Barra")||n.Contains("Estampa")||n.Contains("Carretel")||n.Contains("Grafismo")||n=="Capuz"||n.StartsWith("BlenderTopo");
        }
        void CacheShopDisplay(Garment item)
        {
            if(item.slot!=0&&item.slot!=2||shopDisplayTemplates.ContainsKey(item.id))return;
            var builder=new ClothingMeshBuilder();var materials=new List<Material>();var map=new Dictionary<Material,int>();
            var avatarInverse=Avatar.worldToLocalMatrix;var scale=Matrix4x4.Scale(Avatar.lossyScale);var rotation=Quaternion.Inverse(Avatar.rotation);
            foreach(var renderer in clothes.GetComponentsInChildren<MeshRenderer>()){
                if(!GarmentMatchesSlot(renderer.name,item.slot))continue;var mesh=renderer.GetComponent<MeshFilter>().sharedMesh;var v=mesh.vertices;var n=mesh.normals;var uv=mesh.uv;var source=renderer.sharedMaterials;
                var matrix=scale*avatarInverse*renderer.transform.localToWorldMatrix;var normalRotation=rotation*renderer.transform.rotation;
                for(int sub=0;sub<mesh.subMeshCount;sub++){
                    var material=source[Mathf.Min(sub,source.Length-1)];if(!map.TryGetValue(material,out int index)){index=materials.Count;map.Add(material,index);var copy=Instantiate(material);materials.Add(copy);owned.Add(copy);}
                    foreach(int i in mesh.GetTriangles(sub))builder.Add(matrix.MultiplyPoint3x4(v[i])-(renderer.name=="Capuz"?Vector3.up*.32f:Vector3.zero),normalRotation*n[i],uv.Length>i?uv[i]:Vector2.zero,index);
                }
            }
            var combined=builder.Build("Shop display — "+item.id);owned.Add(combined);shopDisplayTemplates[item.id]=new ClothingTemplate{name=item.slot==0?"Camiseta com caimento":"Tênis",mesh=combined,materials=materials.ToArray()};
        }
        public Transform CreateShopDisplay(string id,Transform parent,Vector3 position,float yaw)
        {
            var item=Catalog.Get(id);if(!shopDisplayTemplates.TryGetValue(id,out var template)){
                bool proportions=PreviewProportions,tall=TallPreview;PreviewProportions=TallPreview=false;
                var outfit=Catalog.Default;outfit[item.slot]=id;Apply(outfit,0);CacheShopDisplay(item);PreviewProportions=proportions;TallPreview=tall;template=shopDisplayTemplates[id];
            }
            var root=new GameObject((item.slot==0?"Peça na arara — ":"Tênis em exposição — ")+id).transform;root.SetParent(parent,false);root.localPosition=position;root.localRotation=Quaternion.Euler(0,yaw,0);
            var display=new GameObject(template.name);display.transform.SetParent(root,false);display.AddComponent<MeshFilter>().sharedMesh=template.mesh;display.AddComponent<MeshRenderer>().sharedMaterials=template.materials;return root;
        }
    }
}
