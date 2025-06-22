using Core.InventoryService;
using Items;

namespace Player.Interfaces
{
    public interface IInventoryController
    {
        bool TryPickUpItem(PickUpModel pickUp);
        
        Inventory GetPlayerInventory();
        
        void LockInventory(bool locked);
    }
}