using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mlf.InventorySystem.Items
{

    [CreateAssetMenu(fileName = "New Default Object", menuName = "Mlf/Inventory System/Items/Food")]
    public class FoodItem : BaseItem
    {

        public int restoreHealthValue;
        public void Awake()
        {
            type = ItemType.Food;
        }
    }
}

