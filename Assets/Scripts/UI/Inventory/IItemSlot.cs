using Items;

namespace UI.Inventory
{
    public interface IItemSlot
    {
        void AddItem(ItemModel itemModel, int amount);
        
        ItemModel GetItem();

        void SetAmount(int amount);
        
        int GetAmount();
        
        void SetSelect(bool isSelected);
    }
}