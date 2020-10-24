
using System.Collections.Generic;
using UnityEngine;


namespace Mlf.Gm.Spawners
{
    public class BaseSpawner : MonoBehaviour
    {

        public GameObject prefab;
        public string type;
        public float spawnRadiusDistance = 5f;
        public int maxObjectCount = 10;

        public virtual void spawn()
        {
            //if(spanwedList.Count >= maxObjectCount) return;

        }

        //protected void onItemDestroyed(HarvestItemComp item) {
        //  spanwedList.Remove(item);
        //}


        //public virtual void registerItem(HarvestItemComp item) {
        //  GameResourceManager.instance.addHarvestItem(item);
        //  spanwedList[spanwedList.Count-1].onItemDestroyed += onItemDestroyed;
        //}
    }
}



