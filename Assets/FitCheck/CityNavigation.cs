using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class CityView
    {
        public static readonly Vector3 Origin=new Vector3(200,0,0);
        public static readonly Vector2 Spawn=new Vector2(-3.5f,-7.1f);
        public static readonly Rect WalkBounds=new Rect(-25.5f,-15.5f,51,29);
        const float Radius=.40f,Cell=.5f;
        public const float PlayerSpeed=4.4f;
        [SerializeField] List<Rect> obstacles=new List<Rect>();
        [SerializeField] List<Vector2> route=new List<Vector2>();int waypoint;
        public Vector2 Position {get;private set;}
        public bool Walking {get;private set;}
        public const float EyeHeight=2.78f,ViewFieldOfView=68;
        public float ViewYaw {get;private set;}=46;
        public float ViewPitch {get;private set;}=-5;
        public const float LookSensitivity=2.8f;
        Transform marker,shadow;
        public string Place {get{return Position.y>1&&Position.x< -9?"CAFÉ DO LADO":Position.y>0&&Position.x>0?"DRIPLAB":"PRAÇA DO ENCONTRO";}}
        public bool Free(Vector2 p){return WorldFree(p)&&!ResidentBlocks(p)&&!VehicleBlocks(p);}
        bool WorldFree(Vector2 p)
        {
            var bounds=IsHome?HomeBounds:IsInterior?ShopBounds:WalkBounds;
            if(IsInterior&&!IsHome&&FittingCurtainBlocks(p))return false;
            if(p.x<bounds.xMin+Radius||p.x>bounds.xMax-Radius||p.y<bounds.yMin+Radius||p.y>bounds.yMax-Radius)return false;
            foreach(var box in obstacles)if(p.x>box.xMin-Radius&&p.x<box.xMax+Radius&&p.y>box.yMin-Radius&&p.y<box.yMax+Radius)return false;return true;
        }
        bool ClearLine(Vector2 a,Vector2 b)
        {int count=Mathf.CeilToInt(Vector2.Distance(a,b)/.13f);for(int i=0;i<=count;i++)if(!Free(Vector2.Lerp(a,b,count==0?0:i/(float)count)))return false;return true;}
        Vector2Int CellAt(Vector2 p){return new Vector2Int(Mathf.RoundToInt(p.x/Cell),Mathf.RoundToInt(p.y/Cell));}
        Vector2 AtCell(Vector2Int p){return new Vector2(p.x*Cell,p.y*Cell);}
        bool NearCell(Vector2 p,out Vector2Int result)
        {
            var center=CellAt(p);result=center;float best=float.MaxValue;
            for(int x=-2;x<=2;x++)for(int y=-2;y<=2;y++){var cell=center+new Vector2Int(x,y);var point=AtCell(cell);float distance=(p-point).sqrMagnitude;if(distance<best&&Free(point)&&ClearLine(p,point)){result=cell;best=distance;}}
            return best<float.MaxValue;
        }
        public bool PlanRoute(Vector2 start,Vector2 end,List<Vector2> output)
        {
            output.Clear();if(!Free(start)||!Free(end))return false;if(ClearLine(start,end)){output.Add(end);return true;}
            Vector2Int source,goal;if(!NearCell(start,out source)||!NearCell(end,out goal))return false;
            var queue=new Queue<Vector2Int>();var previous=new Dictionary<Vector2Int,Vector2Int>();queue.Enqueue(source);previous[source]=source;
            var directions=new[]{Vector2Int.up,Vector2Int.right,Vector2Int.down,Vector2Int.left};bool found=false;
            while(queue.Count>0){var cell=queue.Dequeue();if(cell==goal){found=true;break;}foreach(var d in directions){var next=cell+d;if(previous.ContainsKey(next)||!Free(AtCell(next)))continue;previous[next]=cell;queue.Enqueue(next);}}
            if(!found)return false;var raw=new List<Vector2>{end};var cursor=goal;
            while(true){raw.Add(AtCell(cursor));if(cursor==source)break;cursor=previous[cursor];}raw.Add(start);raw.Reverse();
            int index=0;while(index<raw.Count-1){int next=raw.Count-1;while(next>index+1&&!ClearLine(raw[index],raw[next]))next--;if(!ClearLine(raw[index],raw[next])){output.Clear();return false;}output.Add(raw[next]);index=next;}return output.Count>0;
        }
        public bool RequestWalk(Vector2 target)
        {
            var planned=new List<Vector2>();if(!PlanRoute(Position,target,planned))return false;route.Clear();route.AddRange(planned);waypoint=0;
            return true;
        }
        public void ResetWalk(Transform avatar)
        {
            Position=IsHome?HomeSpawn:IsInterior?ShopSpawn:Spawn;route.Clear();waypoint=0;Walking=false;ViewYaw=IsHome?310:IsInterior?0:46;ViewPitch=IsHome?12:-5;
            avatar.position=transform.position+new Vector3(Position.x,-.055f,Position.y);avatar.rotation=Quaternion.Euler(0,-22,0);
            if(marker!=null)marker.gameObject.SetActive(false);if(shadow!=null)shadow.gameObject.SetActive(false);
        }
        public void Step(Vector2 input,float delta,bool paused,Transform avatar)
        {
            if(paused){Walking=false;return;}delta=Mathf.Min(delta,.05f);Vector2 old=Position;float distance=PlayerSpeed*delta;
            if(input.sqrMagnitude>.001f){
                route.Clear();if(marker!=null)marker.gameObject.SetActive(false);input=Vector2.ClampMagnitude(input,1);var target=Position+input*distance;
                if(Free(target))Position=target;else{var x=new Vector2(target.x,Position.y);if(Free(x))Position=x;var z=new Vector2(Position.x,target.y);if(Free(z))Position=z;}
            }else if(waypoint<route.Count){
                var target=route[waypoint];var next=Vector2.MoveTowards(Position,target,distance);if(Free(next))Position=next;else route.Clear();if(Vector2.Distance(Position,target)<.025f)waypoint++;
            }
            var movement=Position-old;Walking=movement.sqrMagnitude>.000001f;avatar.position=transform.position+new Vector3(Position.x,-.055f,Position.y);
            if(Walking){var facing=Quaternion.LookRotation(new Vector3(-movement.x,0,-movement.y));avatar.rotation=Quaternion.Slerp(avatar.rotation,facing,1-Mathf.Exp(-delta*12));}
            if(waypoint>=route.Count&&marker!=null)marker.gameObject.SetActive(false);
            if(shadow!=null)shadow.localPosition=new Vector3(Position.x,.005f,Position.y);
            StepPlayerBody(movement.magnitude,delta);
        }
        public void Frame(Camera camera,bool reduced,float delta)
        {
            if(delta>0){float step=Mathf.Min(delta,.05f);TickResidents(step);if(IsInterior){if(!IsHome)AdvanceFittingCurtain(step);}else{TickTraffic(step);if(!reduced)AnimateAtmosphere(step);}}
            StepPlayerBody(0,0);
            camera.orthographic=false;camera.fieldOfView=ViewFieldOfView;camera.nearClipPlane=.08f;
            camera.clearFlags=IsInterior?CameraClearFlags.SolidColor:CameraClearFlags.Skybox;
            camera.backgroundColor=new Color(.025f,.02f,.025f);
            var sky=camera.GetComponent<Skybox>();if(sky==null)sky=camera.gameObject.AddComponent<Skybox>();sky.material=SkyMaterial;sky.enabled=!IsInterior;
            camera.transform.position=transform.position+new Vector3(Position.x,EyeHeight,Position.y);
            camera.transform.rotation=Quaternion.Euler(ViewPitch,ViewYaw,0);
            if(PlayerBodyReady)foreach(var piece in playerHeadPieces)if(piece!=null)piece.SetActive(false);
        }
        public void RestoreWalk(Vector2 position,float yaw,float pitch,Transform avatar)
        {ResetWalk(avatar);if(Free(position))Position=position;SetView(yaw,pitch);}
        public void Look(Vector2 delta)
        {ViewYaw=Mathf.Repeat(ViewYaw+delta.x*LookSensitivity,360);ViewPitch=Mathf.Clamp(ViewPitch-delta.y*LookSensitivity,-75,88);}
        public void SetView(float yaw,float pitch)
        {ViewYaw=Mathf.Repeat(yaw,360);ViewPitch=Mathf.Clamp(pitch,-75,88);}
        public Vector2 GroundPoint(Camera camera,Vector2 viewport)
        {
            var ray=camera.ViewportPointToRay(new Vector3(viewport.x,viewport.y,0));if(Mathf.Abs(ray.direction.y)<.0001f)return new Vector2(999,999);
            float distance=-ray.origin.y/ray.direction.y;if(distance<=0)return new Vector2(999,999);var p=ray.GetPoint(distance)-transform.position;return new Vector2(p.x,p.z);
        }
    }
}
