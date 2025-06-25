using Core.InventoryService;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class EquipManager : MonoBehaviour
    {
        public Transform equipParent;
        public Equip currentEquip;

        public void OnAttackInput(InputAction.CallbackContext context)
        {
            if (context.performed && currentEquip is not null)
            {
                currentEquip.OnAttackInput();
            }
        }
        
        public void OnAltAttackInput(InputAction.CallbackContext context)
        {
            if (context.performed && currentEquip is not null)
            {
                currentEquip.OnAltAttackInput();
            }
        }
        
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
