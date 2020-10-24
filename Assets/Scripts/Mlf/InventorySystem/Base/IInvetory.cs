using Mlf.InventorySystem.Items;
using UnityEngine;

namespace Mlf.InventorySystem.Base
{
    public interface IInventory
    {
        int AddItem(BaseItem slot, int ammount);
        BaseItem removeRandomItem();

        void removeOneItemAmountByIndex(int index);

        bool hasMoreItems();

    }
}