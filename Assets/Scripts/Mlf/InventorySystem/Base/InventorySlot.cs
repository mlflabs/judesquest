using System;
using UnityEngine;
using System.Collections.Generic;
using Mlf.InventorySystem.Items;

namespace Mlf.InventorySystem.Base
{


    [Serializable]
    public class InventorySlot
    {
        public BaseItem item;
        public int amount;

        public bool equipped;

        public InventorySlot(BaseItem item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }

        public void AddAmount(int value)
        {
            amount += value;
        }


    }


}