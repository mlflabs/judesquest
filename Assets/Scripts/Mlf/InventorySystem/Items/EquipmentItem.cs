using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mlf.InventorySystem.Items
{

    [CreateAssetMenu(fileName = "New Default Object", menuName = "Mlf/Inventory System/Items/Equipment")]
    public class EquipmentItem : BaseItem
    {

        public int attactBonus;
        public int defenceBonus;
        public void Awake()
        {
            type = ItemType.Equipment;
        }
    }
}

