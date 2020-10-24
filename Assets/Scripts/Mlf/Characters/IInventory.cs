using Mlf.InventorySystem;
using Mlf.Traffic;

namespace Mlf.Characters
{
    public interface IInventory
    {
        InventoryData Inventory { get; set; }
    }
}