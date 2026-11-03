using UnityEngine;
namespace FitCheck
{
    public partial class StudioView
    {
        public bool ExplorationMode {get;private set;}
        public int VisibleStudioDecor {get{
            int count=0;foreach(var r in GetComponentsInChildren<MeshRenderer>(true))
                if(!r.transform.IsChildOf(Avatar)&&(house==null||!r.transform.IsChildOf(house))&&r.enabled&&r.gameObject.activeInHierarchy)count++;
            return count;
        }}
        public void SetExploration(bool world)
        {
            ExplorationMode=world;
            foreach(var r in GetComponentsInChildren<MeshRenderer>(true))
                if(!r.transform.IsChildOf(Avatar)&&(house==null||!r.transform.IsChildOf(house)))r.enabled=!world;
            if(world&&house!=null)house.gameObject.SetActive(false);
        }
    }
}
