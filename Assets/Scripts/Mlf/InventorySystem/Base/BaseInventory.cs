using UnityEngine;
using System.Collections.Generic;
using Mlf.InventorySystem.Items;

namespace Mlf.InventorySystem.Base
{


    public abstract class BaseInventory : IInventory
    {

        public event System.Action onInventoryUpdate;
        public int maxItemSlots = 4;

        public int maxItemCapacity = 16;
        public int visibleItemSlots = 4;
        public int currentItemUsedCapacity = 0;
        public List<InventorySlot> items = new List<InventorySlot>();


        // return ammount of items accepted;
        // TODO: now item ammount is unlimited, later we might add limits
        public int AddItem(BaseItem item, int ammount = 1)
        {
            bool hasItem = false;

            //can we add all the items


            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].item == item)
                {
                    hasItem = true;
                    items[i].AddAmount(ammount);
                    currentItemUsedCapacity++;
                    break;
                }
            }

            if (hasItem)
            {
                onInventoryUpdate?.Invoke();
                return ammount;
            }


            if (items.Count < maxItemSlots)
            {
                items.Add(new InventorySlot(item, ammount));
                currentItemUsedCapacity++;
            }
            else
            {
                return 0;
            }

            onInventoryUpdate?.Invoke();
            return ammount;
        }

        public BaseItem removeTopItem()
        {

            BaseItem item = items[0].item;
            removeOneItemAmountByIndex(0);
            return item;
        }

        public BaseItem removeRandomItem()
        {

            int randomIndex = Random.Range(0, items.Count - 1);
            Debug.Log("Count: " + items.Count + "Random index: " + randomIndex);
            BaseItem item = items[randomIndex].item;
            removeOneItemAmountByIndex(randomIndex);
            return item;
        }

        public void removeOneItemAmountByIndex(int index)
        {
            items[index].amount--;
            currentItemUsedCapacity--;
            if (items[index].amount <= 0)
            {
                items.RemoveAt(index);
            }
            onInventoryUpdate?.Invoke();
        }

        public void removeItemAmountByIndex(int index, int amount)
        {

            if (items[index].amount < amount)
            {
                Debug.LogError("Trying to remove more items then are found");
            }

            items[index].amount = -amount;
            currentItemUsedCapacity = -amount;
            if (items[index].amount <= 0)
            {
                items.RemoveAt(index);
            }
            onInventoryUpdate?.Invoke();
        }

        public bool MaxInventoryReached()
        {
            if (currentItemUsedCapacity >= maxItemCapacity) return true;

            return false;
        }

        public bool hasMoreItems()
        {
            return items.Count > 0;
        }
    }
}