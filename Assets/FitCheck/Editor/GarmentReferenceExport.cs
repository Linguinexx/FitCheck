using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace FitCheck.Editor
{
    public static class GarmentReferenceExport
    {
        [Serializable] public class Piece {public string name;public Vector3[] vertices;public int[] triangles;public Color color;}
        [Serializable] public class Snapshot {public List<Piece> pieces=new List<Piece>();}
        [MenuItem("Tools/Fit Check/Export garment fitting reference")]
        public static void Export()
        {
            var go=new GameObject("Referência de modelagem");var studio=go.AddComponent<StudioView>();studio.Build();studio.Apply(Catalog.Default,0);
            var snapshot=new Snapshot();
            foreach(var r in studio.Avatar.GetComponentsInChildren<MeshRenderer>()){
                var mesh=r.GetComponent<MeshFilter>().sharedMesh;var vertices=mesh.vertices;
                for(int i=0;i<vertices.Length;i++)vertices[i]=studio.Avatar.InverseTransformPoint(r.transform.TransformPoint(vertices[i]));
                snapshot.pieces.Add(new Piece{name=r.name,vertices=vertices,triangles=mesh.triangles,color=r.sharedMaterial.color});
            }
            var project=Directory.GetParent(Application.dataPath);var workspace=project.Name==".city-build-project"?project.Parent.Parent:project.Parent;
            string path=Path.Combine(workspace.FullName,"tools/garment-avatar-reference.json");File.WriteAllText(path,JsonUtility.ToJson(snapshot));Debug.Log("GARMENT_REFERENCE_EXPORTED: "+snapshot.pieces.Count+" meshes; "+path);
        }
    }
}
