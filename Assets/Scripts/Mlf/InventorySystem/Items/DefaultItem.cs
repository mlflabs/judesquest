using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mlf.InventorySystem.Items
{

    [CreateAssetMenu(fileName = "New Default Object", menuName = "Mlf/Inventory System/Items/Default")]
    public class DefaultItem : BaseItem
    {



        public void Awake()
        {
            type = ItemType.Default;
        }
    }
}

