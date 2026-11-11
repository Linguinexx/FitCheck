using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class CityView
    {
        public int ResidentCount=>residents.Count;
        public int GeometryBatches {get;private set;}
        // Street counterparts of the FitFeed locations, in campaign order.
        static readonly Vector3[] FitFeedSpots={
            new Vector3(-8,-.055f,-3.5f), new Vector3(-99,-.055f,33.9f), new Vector3(-79,-.055f,42),
            new Vector3(-48,-.055f,-18.1f), new Vector3(-31.5f,-.055f,-40.5f), new Vector3(-54,-.055f,-18.7f),
            new Vector3(-94,-.055f,33.9f), new Vector3(11.8f,-.055f,-2.5f), new Vector3(-34,.245f,-38.65f)
        };
        static readonly float[] FitFeedFacing={155,180,120,0,180,0,180,0,180};
        readonly List<(int index,string[] outfit,Resident actor,List<Object> owned)> fitFeedResidents=
            new List<(int,string[],Resident,List<Object>)>();
        DayProgress fitFeedCampaign;
        int fitFeedDay;

        public bool TryLookAtPostedCustomer(Camera camera,DayProgress campaign,out int index,out Vector3 target)
        {
            index=-1;target=Vector3.zero;
            float best=.91f;
            foreach(var entry in fitFeedResidents){
                if(!entry.actor.root.gameObject.activeInHierarchy||!campaign.CanGreetCustomer(entry.index))continue;
                var point=entry.actor.root.position+Vector3.up*2.35f;
                var offset=point-camera.transform.position;
                float alignment=Vector3.Dot(camera.transform.forward,offset.normalized);
                if(offset.sqrMagnitude>12.25f||alignment<=best)continue;
                index=entry.index;target=point;best=alignment;
            }
            return index>=0;
        }

        public void HideFitFeedResidents()
        {
            foreach(var entry in fitFeedResidents)entry.actor.root.gameObject.SetActive(false);
        }

        public void SyncFitFeedResidents(StudioView studio,DayProgress campaign)
        {
            int first=campaign.FirstCustomerOfDay;
            for(int i=fitFeedResidents.Count-1;i>=0;i--){
                var entry=fitFeedResidents[i];
                if(ReferenceEquals(fitFeedCampaign,campaign)&&fitFeedDay==campaign.day&&entry.index<campaign.posts.Count
                    &&ReferenceEquals(entry.outfit,campaign.posts[entry.index].outfit)){
                    entry.actor.root.gameObject.SetActive(true);
                    continue;
                }
                residents.Remove(entry.actor);
                entry.actor.root.gameObject.SetActive(false);Destroy(entry.actor.root.gameObject);
                foreach(var asset in entry.owned){assets.Remove(asset);Destroy(asset);}
                fitFeedResidents.RemoveAt(i);
            }
            fitFeedCampaign=campaign;fitFeedDay=campaign.day;
            var rotation=studio.Avatar.rotation;
            bool avatarActive=studio.Avatar.gameObject.activeSelf;
            try{
                studio.Avatar.gameObject.SetActive(true);
                studio.Avatar.rotation=Quaternion.identity;
                for(int index=first;index<campaign.posts.Count;index++){
                    if(fitFeedResidents.Exists(entry=>entry.index==index))continue;
                    var outfit=campaign.posts[index].outfit;
                    studio.Apply(outfit,index);
                    int assetStart=assets.Count;
                    int scenery=DayOne.CustomerScenery(index);
                    var actor=CopyAvatar(studio,"FitFeed — "+DayOne.Customers[index].name,FitFeedSpots[scenery],FitFeedFacing[scenery],false);
                    actor.idleStyle=3+index%3;
                    actor.homeFacing=actor.facing;
                    residents.Add(actor);
                    PoseResident(actor,residents.Count-1,0);
                    fitFeedResidents.Add((index,outfit,actor,assets.GetRange(assetStart,assets.Count-assetStart)));
                }
            }finally{
                studio.Avatar.rotation=rotation;
                studio.Avatar.gameObject.SetActive(avatarActive);
            }
        }

        public void Build(StudioView studio)
        {
            transform.position=Origin;
            BuildAtmosphere();Landscape();DripLabFacade();Neighbours();CityIdentity();WestNeighbourhood();StreetCulture();OppositeShopFrontage();NightDetails();StreetArtDisplays();NeighbourhoodPlanting();BuildNeighbourhoodCourtyards();BuildNightLighting();GeometryBatches=batches.Count;FinishGeometry();
            marker=Disc("Destino na calçada",.27f,.27f,.19f,new Color(.36f,.76f,.65f));marker.gameObject.SetActive(false);
            shadow=Disc("Sombra do funcionário",.65f,.36f,0,new Color(.43f,.40f,.39f));
            BakeResident(studio,1,new[]{"CG_TOP_01","SB_BOTTOM_01","SB_SHOES_01",""},new Vector3(-15.4f,-.055f,2.8f),-22);
            BakeResident(studio,8,new[]{"CG_TOP_02","SB_BOTTOM_02","CG_SHOES_01",""},new Vector3(15.7f,-.055f,-1.5f),32);
            BakeResident(studio,3,new[]{"SB_TOP_01","SB_BOTTOM_01","SB_SHOES_01",""},new Vector3(-20,-.055f,-8.5f),-90);
            BakeResident(studio,5,new[]{"CG_TOP_01","CG_BOTTOM_01","CG_SHOES_01",""},new Vector3(-22,-.055f,-2),180);
            residents[1].path=new[]{new Vector2(13.5f,-6.5f),new Vector2(4,-6.5f),new Vector2(4,.4f),new Vector2(13.5f,.4f),new Vector2(15.7f,-1.5f)};
            residents[2].path=new[]{new Vector2(-4,-8.5f),new Vector2(10,-8.5f),new Vector2(19,-8.5f),new Vector2(10,-8.5f),new Vector2(-4,-8.5f),new Vector2(-20,-8.5f)};
            motionRigVersion=ResidentArtRevision;
            residents[3].path=new[]{new Vector2(-22.3f,3.6f),new Vector2(-16.8f,3.6f),new Vector2(-22.3f,3.6f),new Vector2(-22,-2)};
            BakeResident(studio,2,new[]{"AH_TOP_01","SB_BOTTOM_01","SB_SHOES_01",""},new Vector3(-122,-.055f,1.8f),90);
            residents[4].path=new[]{new Vector2(-139,1.8f),new Vector2(-109,1.8f)};
            BakeResident(studio,4,new[]{"SB_TOP_02","CG_BOTTOM_01","CG_SHOES_01",""},new Vector3(-97,-.055f,31.5f),90);
            residents[5].path=new[]{new Vector2(-103,31.5f),new Vector2(-90,31.5f)};
            BakeResident(studio,6,new[]{"CG_TOP_01","SB_BOTTOM_01","SB_SHOES_01",""},new Vector3(-73,-.055f,49.25f),0);
            ConfigureShopLean(residents[6],false,50.10f);
            residents[0].idleStyle=2;
            var phone=new GameObject("Celular preso à mão").transform;phone.SetParent(residents[0].root,false);residents[0].prop=phone;
            LiveBox("Corpo do celular",Vector3.zero,new Vector3(.165f,.30f,.035f),Ink,"CityMetal",phone);
            LiveBox("Tela do celular",new Vector3(0,0,.021f),new Vector3(.135f,.255f,.008f),new Color(.12f,.38f,.36f),"CityGlow",phone);
            LiveBox("Câmera do celular",new Vector3(-.045f,.10f,-.022f),new Vector3(.035f,.04f,.012f),Cream,"CityMetal",phone);
            PoseResident(residents[0],0,0);BuildWatcher(studio);
        }
        Transform Disc(string name,float rx,float rz,float inner,Color color)
        {
            const int sides=40;var vertices=new List<Vector3>();var triangles=new List<int>();var colors=new List<Color>();var uv=new List<Vector2>();var normals=new List<Vector3>();
            for(int ring=0;ring<2;ring++)for(int i=0;i<sides;i++){
                float a=i*Mathf.PI*2/sides,f=ring==0?(rx>0?inner/rx:0):1;vertices.Add(new Vector3(Mathf.Cos(a)*rx*f,0,Mathf.Sin(a)*rz*f));colors.Add(color);uv.Add(Vector2.zero);normals.Add(Vector3.up);
            }
            for(int i=0;i<sides;i++){int next=(i+1)%sides;triangles.AddRange(new[]{i,next,i+sides,next,next+sides,i+sides});}
            var mesh=new Mesh{name=name};mesh.SetVertices(vertices);mesh.SetNormals(normals);mesh.SetColors(colors);mesh.SetUVs(0,uv);mesh.SetTriangles(triangles,0);mesh.RecalculateBounds();assets.Add(mesh);
            var material=new Material(Resources.Load<Shader>("CityPaint"));material.mainTexture=Resources.Load<Texture2D>("Painted/Leather");material.SetFloat("_Motion",0);assets.Add(material);
            var go=new GameObject(name);go.transform.SetParent(transform,false);go.AddComponent<MeshFilter>().sharedMesh=mesh;go.AddComponent<MeshRenderer>().sharedMaterial=material;return go.transform;
        }
        void BakeResident(StudioView studio,int customer,string[] outfit,Vector3 position,float yaw,bool wallLean=false,bool everyday=false)
        {
            int oldHair=studio.HairOverride,oldExpression=studio.ExpressionOverride;bool oldPreview=studio.PreviewProportions,oldTall=studio.TallPreview;var rotation=studio.Avatar.rotation;var oldLook=studio.PlayerLook;
            try{
                studio.HairOverride=-1;studio.ExpressionOverride=-1;studio.PreviewProportions=false;studio.TallPreview=false;studio.Avatar.rotation=Quaternion.identity;
                if(!IsInterior){studio.PlayerLook=CitizenAppearance(customer);studio.ApplyCivilian(customer);}else if(everyday)studio.ApplyCivilian(customer,true);else studio.Apply(outfit,customer);
                if(wallLean)studio.SetShopAttitude();
                var actor=CopyAvatar(studio,"Morador — "+DayOne.Customers[customer].name,position,yaw,false);residents.Add(actor);
                if(wallLean)ConfigureShopLean(actor);
                var blob=Disc("Sombra do morador",.63f,.35f,0,new Color(.43f,.40f,.39f));blob.localPosition=new Vector3(position.x,.005f,position.z);
                actor.shadow=blob;
            }finally{studio.PlayerLook=oldLook;studio.HairOverride=oldHair;studio.ExpressionOverride=oldExpression;studio.PreviewProportions=oldPreview;studio.TallPreview=oldTall;studio.Avatar.rotation=rotation;}
        }
        Resident CopyAvatar(StudioView studio,string name,Vector3 position,float yaw,bool hideHead)
        {
                var resident=new GameObject(name).transform;resident.SetParent(transform,false);resident.localPosition=position;resident.localRotation=Quaternion.Euler(0,yaw,0);
                var actor=new Resident{root=resident,home=position,facing=resident.localRotation,expression=studio.CurrentExpression};
                var materials=new Dictionary<Material,Material>();studio.PrepareResidentLids();
                foreach(var renderer in studio.Avatar.GetComponentsInChildren<MeshRenderer>(true)){
                    int mouth=renderer.name=="Boca"?1:renderer.name=="Boca animada"?2:renderer.name=="Boca surpresa"?3:renderer.name=="Dentes"?4:0;
                    if((!renderer.gameObject.activeInHierarchy&&mouth==0&&renderer.name!="Pálpebra superior")||hideHead&&(studio.IsHeadPart(renderer.transform)||renderer.name=="Pescoço"))continue;var filter=renderer.GetComponent<MeshFilter>();if(filter==null)continue;
                    var sourceMesh=filter.sharedMesh;var vertices=sourceMesh.vertices;var normals=sourceMesh.normals;
                    if(vertices.Length==0)throw new System.Exception("Fonte do morador vazia: "+renderer.name);
                    // Own CPU copies of every stream and submesh. Dynamic face
                    // changes in the studio must not invalidate a resident's buffers.
                    var mesh=new Mesh{name=renderer.name+" — morador",indexFormat=sourceMesh.indexFormat};
                    mesh.vertices=vertices;mesh.uv=sourceMesh.uv;if(sourceMesh.colors.Length==vertices.Length)mesh.colors=sourceMesh.colors;
                    mesh.subMeshCount=sourceMesh.subMeshCount;for(int sub=0;sub<sourceMesh.subMeshCount;sub++)mesh.SetTriangles(sourceMesh.GetTriangles(sub),sub);
                    var copyMatrix=Matrix4x4.Translate(-studio.Avatar.position)*renderer.transform.localToWorldMatrix;var copyRotation=renderer.transform.rotation;
                    for(int i=0;i<vertices.Length;i++){vertices[i]=copyMatrix.MultiplyPoint3x4(vertices[i]);normals[i]=copyRotation*normals[i];}
                    mesh.vertices=vertices;mesh.normals=normals;mesh.RecalculateBounds();assets.Add(mesh);RegisterResidentMesh(actor,mesh,renderer.name,studio.IsHeadPart(renderer.transform),sourceMesh.name.Contains("Blender Baggy"));
                    var piece=new GameObject(renderer.name);piece.transform.SetParent(resident,false);piece.AddComponent<MeshFilter>().sharedMesh=mesh;
                    if(renderer.name=="Pálpebra superior"){
                        var lidPart=actor.parts[actor.parts.Count-1];lidPart.lidToAvatar=Matrix4x4.Translate(-studio.Avatar.position)*renderer.transform.localToWorldMatrix;lidPart.lidRotation=renderer.transform.rotation;lidPart.lidBaseCover=studio.ResidentLidCover(renderer.transform);
                        Vector3[] closed,closedNormals;studio.ResidentClosedLid(renderer.transform,out closed,out closedNormals);
                        for(int i=0;i<closed.Length;i++){closed[i]=renderer.transform.TransformPoint(closed[i])-studio.Avatar.position;closedNormals[i]=renderer.transform.TransformDirection(closedNormals[i]);}
                        actor.parts[actor.parts.Count-1].closedLid=closed;actor.parts[actor.parts.Count-1].closedNormals=closedNormals;
                    }
                    actor.parts[actor.parts.Count-1].node=piece;actor.parts[actor.parts.Count-1].mouthKind=mouth;
                    var source=renderer.sharedMaterials;var copies=new Material[source.Length];for(int i=0;i<source.Length;i++){
                        Material copy;if(!materials.TryGetValue(source[i],out copy)){copy=Instantiate(source[i]);copy.SetFloat("_NightCity",IsInterior?0:1);materials[source[i]]=copy;assets.Add(copy);}copies[i]=copy;
                    }
                    piece.AddComponent<MeshRenderer>().sharedMaterials=copies;
                }
                studio.RestoreResidentLids();AnimateResidentExpression(actor,0);return actor;
        }
    }
}
