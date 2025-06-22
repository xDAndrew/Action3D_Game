using System;
using System.Collections.Generic;
using System.Linq;
using Items;

namespace Core.InventoryService
{
    public class Inventory
    {
        private readonly List<InventorySlot> _storage;
        
        public Inventory(int capacity)
        {
            _storage = new List<InventorySlot>(capacity);
            for (var i = 0; i < capacity; i++)
            {
                _storage.Add(new InventorySlot());
            }
        }

        public bool TryAddItem(PickUpModel item)
        {
            if (item is null)
            {
                return false;
            }
            
            var emptySlot = _storage.FirstOrDefault(x => x.Item?.id == item.id && item.stackable && x.Amount < item.maxStackAmount) 
                ?? _storage.FirstOrDefault(x => x.Item is null);

            if (emptySlot is null)
            {
                return false;
            }
            
            emptySlot.Item = item;
            emptySlot.Amount++;

            return true;
        }

        public bool TryRemoveItem(Guid index, int amount)
        {
            var slot = _storage.FirstOrDefault(x => x.Id == index);
            if (slot?.Item is null || slot.Amount < amount)
            {
                return false;
            }
            
            slot.Amount -= amount;
            if (slot.Amount == 0)
            {
                slot.Item = null;
            }

            return true;
        }

        public IReadOnlyList<InventorySlot> GetItems()
        {
            return _storage;
        }
    }
}