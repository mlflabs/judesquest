using System.Collections.Generic;
using Mlf.InventorySystem.Items;
using Mlf.Gm;
using UnityEngine;


namespace Mlf.InventorySystem.GameObjects  {

//difference with Inventory Comp, this one registers itself 
//with resource manager
  
  public class StoragePlaceComp : BaseInventoryComp, IInventoryGameObject
  {

      
      public HarvestTypes harvestType = HarvestTypes.cutGrass;

      public event System.Action<StoragePlaceComp> onItemDestroyed;
     
     
      public void destroyItem() {
        onItemDestroyed?.Invoke(this);
        Destroy(this.gameObject);
      }

      private void Start() {
        GameResourceManager.instance.addStoragePlace(this);
      }

  }
}
