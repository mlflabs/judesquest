using System.Collections.Generic;
using Mlf.InventorySystem;
using Mlf.InventorySystem.GameObjects;
using Mlf.InventorySystem.Items;
using UnityEngine;


namespace Mlf.Gm
{


    public class GameResourceManager : MonoBehaviour
    {
        public static GameResourceManager instance;

        [SerializeField] public Dictionary<HarvestTypes, List<GameInventoryObject>> harvestItems = new Dictionary<HarvestTypes, List<GameInventoryObject>>();
        [SerializeField] public List<StoragePlaceComp> storagePlaces = new List<StoragePlaceComp>();








        public BaseInventoryComp getClosestStoragePlace(Vector3 position,
                                  ItemType type, bool notMaxedOut = false)
        {
            float distance = 9999f;
            int selectedIndex = 9999;

            float newDistance;
            for (int i = 0; i < storagePlaces.Count; i++)
            {
                if (storagePlaces[i].inventory.canAcceptType(type) == false)
                    continue;
                if (notMaxedOut)
                    if (storagePlaces[i].inventory.MaxInventoryReached())
                        continue;

                newDistance = (position - storagePlaces[i].transform.position).sqrMagnitude;

                if (newDistance < distance)
                {
                    distance = newDistance;
                    selectedIndex = i;
                }
            }

            if (selectedIndex == 9999)
                return null;

            Debug.Log(selectedIndex);
            return storagePlaces[selectedIndex];
        }

        /*
            public HarvestItemComp getClosestHarvestItem(HarvestTypes type, BasicSm worker) {
              float distance = 9999f;
              int selectedIndex = 999;

              float newDistance;
              for(int i = 0; i < harvestItems[type].Count; i++) {
                if(harvestItems[type][i].worker != null) continue;

                newDistance = (worker.transform.position -  harvestItems[type][i].inventory.transform.position).sqrMagnitude;

                if(newDistance < distance){
                  distance = newDistance;
                  selectedIndex = i;
                }
              }
              Debug.Log("Did we find harvest target::::::: " +  selectedIndex);
              if(selectedIndex == 999)
                return null;

              harvestItems[type][selectedIndex].worker = worker;
              return harvestItems[type][selectedIndex].inventory;

            }

            public void removeHarvestItemReservation(HarvestItemComp item) {
              for(int i = 0; i < harvestItems[item.harvestType].Count; i++) {
                if(harvestItems[item.harvestType][i].inventory = item) {
                  Debug.Log("-------- Item Reservation Removed");
                  harvestItems[item.harvestType][i].worker = null;
                  return;
                }
              }
            }
        */
        public void addStoragePlace(StoragePlaceComp i)
        {
            this.storagePlaces.Add(i);
            i.onItemDestroyed += onStorageItemDestroyed;
        }

        public void addHarvestItem(HarvestItemComp item)
        {
            //Debug.Log(item);
            item.onItemDestroyed += onHarvestItemDestroyed;
            harvestItems[item.harvestType].Add(new GameInventoryObject(item));
        }

        protected void onHarvestItemDestroyed(HarvestItemComp item)
        {
            for (int i = 0; i < harvestItems[item.harvestType].Count; i++)
            {
                if (harvestItems[item.harvestType][i].inventory == item)
                {
                    harvestItems[item.harvestType].RemoveAt(i);
                    return;
                }
            }
        }

        protected void onStorageItemDestroyed(StoragePlaceComp item)
        {
            for (int i = 0; i < harvestItems[item.harvestType].Count; i++)
            {
                if (harvestItems[item.harvestType][i].inventory == item)
                {
                    harvestItems[item.harvestType].RemoveAt(i);
                    return;
                }
            }
        }


        private void Awake()
        {
            if (GameResourceManager.instance == null) GameResourceManager.instance = this;

            harvestItems[HarvestTypes.cutGrass] = new List<GameInventoryObject>();
            harvestItems[HarvestTypes.harvestWildFruit] = new List<GameInventoryObject>();
            harvestItems[HarvestTypes.farm] = new List<GameInventoryObject>();
            harvestItems[HarvestTypes.water] = new List<GameInventoryObject>();
            // lets find all our waters
        }



    }

    public enum HarvestTypes
    {
        cutGrass,
        harvestWildFruit,
        farm,
        water,
    }

    [System.Serializable]
    public class GameInventoryObject
    {
        public HarvestItemComp inventory;
        //public BasicSm worker;

        public GameInventoryObject(HarvestItemComp inventory)
        {
            this.inventory = inventory;
        }
    }
}



