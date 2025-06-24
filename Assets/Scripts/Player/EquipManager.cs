using Core.InventoryService;
using UnityEngine;

namespace Player
{
    public class EquipManager : MonoBehaviour
    {
        public Transform equipParent;
        public Equip currentEquip;
        
        public void Equip(InventorySlot slot)
        {
            Unequip(slot);
            currentEquip = Instantiate(slot.Item.equipmentPrefab, equipParent).GetComponent<Equip>();
            slot.IsEquip = true;
        }

        public void Unequip(InventorySlot slot)
        {
            if (currentEquip is null || slot.IsEquip == false)
            {
                return;
            }
        
            Destroy(currentEquip.gameObject);
            currentEquip = null;
            slot.IsEquip = false;
        }
    }
}
