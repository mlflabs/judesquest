using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mlf.InventorySystem.Items
{

    [CreateAssetMenu(fileName = "New Default Object", menuName = "Mlf/Inventory System/Items/Grass")]
    public class GrassItem : BaseItem
    {

        public int restoreHealthValue;
        public int hungerRestorValue;
        public void Awake()
        {
            type = ItemType.Grass;
        }
    }
}

