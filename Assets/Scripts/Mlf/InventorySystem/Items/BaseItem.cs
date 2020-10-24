using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mlf.InventorySystem.Items
{

    public enum ItemType
    {
        Food,
        Grass,
        Equipment,

        Building,
        Default
    }
    public abstract class BaseItem : ScriptableObject
    {
        public Sprite icon;
        public ItemType type;
        public GameObject stagePrefab;

        new public string name;

        [TextArea(5, 20)] public string description;

        public virtual bool onItemUse()
        {
            return false;
        }


    }
}

