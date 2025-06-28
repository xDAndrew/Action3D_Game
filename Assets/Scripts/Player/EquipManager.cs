using System.Collections;
using Core.InventoryService;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class EquipManager : MonoBehaviour
    {
        public Transform equipParent;
        public Equip currentEquip;

        private Coroutine _attackCoroutine;
        
        public void OnAttackInput(InputAction.CallbackContext context)
        {
            if (currentEquip is null || Cursor.lockState != CursorLockMode.Locked)
                return;

            if (context.started)
            {
                _attackCoroutine = StartCoroutine(AttackLoop());
            }
            else if (context.canceled)
            {
                if (_attackCoroutine == null) return;
                StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }
        }
        
        public void OnAltAttackInput(InputAction.CallbackContext context)
        {
            if (context.performed && currentEquip is not null && Cursor.lockState == CursorLockMode.Locked)
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
        
        private IEnumerator AttackLoop()
        {
            while (true)
            {
                currentEquip.OnAttackInput();
                yield return new WaitForSeconds(0.4f);
            }
        }
    }
}
