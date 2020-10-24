using System.Collections.Generic;
using Mlf.InventorySystem.Items;
using Mlf.Gm;
using UnityEngine;
using Mlf.InventorySystem.Base;

namespace Mlf.InventorySystem.GameObjects
{


    public class InventoryPickableComp : BaseInventoryComp, IInventoryGameObject
    {


        public void pickupItem()
        {

        }

        public List<InventorySlot> getAllItems()
        {
            return inventory.items;
        }

        public void destoryItem()
        {
            Destroy(this.gameObject);
        }

    }
}
