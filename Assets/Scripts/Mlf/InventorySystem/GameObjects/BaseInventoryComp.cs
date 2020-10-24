using System.Collections.Generic;
using Mlf.InventorySystem.Items;
using Mlf.Gm;
using UnityEngine;
using Mlf.InventorySystem.Base;

namespace Mlf.InventorySystem.GameObjects
{


    public abstract class BaseInventoryComp : MonoBehaviour, IInventoryGameObject
    {

        public List<ItemType> canHoldItemTypes = new List<ItemType>();

        public bool canHoldAllTypes = false;

        [SerializeField] private InventoryData _inventory;

        public InventoryData inventory { get => _inventory; set => _inventory = value; }









    }
}
