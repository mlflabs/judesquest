using UnityEngine;
using System.Collections.Generic;
using Mlf.InventorySystem.Items;
using Mlf.InventorySystem.Base;

namespace Mlf.InventorySystem
{

    [System.Serializable]
    public class InventoryData : BaseInventory, IInventory
    {
        public ItemType[] canHoldItemTypes;

        public bool canHoldAllTypes = false;

        public virtual bool canAcceptItem(BaseItem item)
        {
            Debug.Log("ITEM TYPE::::: " + item);
            return canAcceptType(item.type);
        }

        public bool canAcceptType(ItemType type)
        {
            if (canHoldAllTypes) return true;

            for (int i = 0; i < canHoldItemTypes.Length; i++)
            {
                if (canHoldItemTypes[i] == type)
                    return true;
            }

            return false;
        }


    }
}