using System;
using Items;

namespace Core.InventoryService
{
    public class InventorySlot
    {
        public Guid Id { get; } = Guid.NewGuid();
        
        public PickUpModel Item { get; set; }
        
        public int Amount { get; set; }
        
        public bool IsEquip { get; set; }
    }
}