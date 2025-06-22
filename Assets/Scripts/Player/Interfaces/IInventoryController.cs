using Items;

namespace Player.Interfaces
{
    public interface IInventoryController
    {
        bool TryPickUpItem(PickUpModel pickUp);
    }
}