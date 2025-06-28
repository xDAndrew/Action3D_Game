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

            return true;
        }

        public bool TryRemoveItem(Guid index, int amount)
        {
            var slot = _storage.FirstOrDefault(x => x.Id == index);
            if (slot?.Item is null || slot.Amount < amount)
            {
                return false;
            }

            return true;
        }

        public void AddItem(PickUpModel item)
        {
            if (item is null)
            {
                return;
            }
            
            var emptySlot = _storage.FirstOrDefault(x => x.Item?.id == item.id && item.stackable && x.Amount < item.maxStackAmount) 
                ?? _storage.FirstOrDefault(x => x.Item is null);
                
            if (emptySlot is null) return;
                
            emptySlot.Item = item;
            emptySlot.Amount++;
        }

        public void RemoveItem(Guid index, int amount)
        {
            var slot = _storage.FirstOrDefault(x => x.Id == index);
            if (slot?.Item is null || slot.Amount < amount)
            {
                return;
            }
            
            slot.Amount -= amount;
            if (slot.Amount == 0)
            {
                slot.Item = null;
            }
        }

        public bool IsItemExist(string index, int amount)
        {
            var itemsCount = _storage.Sum(x => x?.Item?.id == index ? x.Amount : 0);
            return itemsCount >= amount;
        }
        
        public IReadOnlyList<InventorySlot> GetItems()
        {
            return _storage;
        }
    }
}