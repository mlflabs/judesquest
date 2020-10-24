using Mlf.Gm;
using UnityEngine;


namespace Mlf.InventorySystem.GameObjects
{

    //difference with Inventory Comp, this one registers itself 
    //with resource manager

    public class HarvestItemComp : BaseInventoryComp, IInventoryGameObject
    {


        public HarvestTypes harvestType = HarvestTypes.cutGrass;

        public event System.Action<HarvestItemComp> onItemDestroyed;


        public void destroyItem()
        {
            onItemDestroyed?.Invoke(this);
            if (this != null)
            {
                Debug.Log("Destroying item: " + this);
                Destroy(this.gameObject);
            }
        }

        private void Start()
        {
            //GameResourceManager.instance.addHarvestItem(this);
        }

    }
}
