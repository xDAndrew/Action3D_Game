using System;
using System.Linq;
using Core.InventoryService;
using Items;
using Player.Interfaces;
using UI.InventoryUI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InventoryController : MonoBehaviour, IInventoryController
    {
        public Canvas inventoryCanvas;
        
        private PlayerInventoryUI _inventoryUI;
        
        private Inventory _playerInventory;
        private bool IsInventoryOpen { get; set; }
        
        private IAppliable _needs;
        private EquipManager _equipManager;

        private bool _inventoryLocked;
        
        private void Awake()
        {
            _playerInventory = new Inventory(12);
            _inventoryUI = inventoryCanvas.transform.Find("Inventory").GetComponent<PlayerInventoryUI>();
            _inventoryUI.SetInventory(_playerInventory);
        }

        private void Start()
        {
            _inventoryUI.onUseBtnClick.AddListener(OnUseBtnClick);
            _inventoryUI.onDropBtnClick.AddListener(OnDropBtnClick);
            _inventoryUI.onEquipBtnClick.AddListener(OnEquipBtnClick);
            
            _needs = GetComponent<IAppliable>();
            _equipManager = GetComponent<EquipManager>();
        }

        public void OnInventoryOpen(InputAction.CallbackContext context)
        {
            if (!context.started || _inventoryLocked)
            {
                return;
            }

            IsInventoryOpen = !IsInventoryOpen;
            inventoryCanvas.gameObject.SetActive(IsInventoryOpen);
            if (IsInventoryOpen)
            {
                _inventoryUI.UpdateInventory();
            }
            else
            {
                _inventoryUI.ResetSelect();
            }
            
            Cursor.lockState = IsInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = IsInventoryOpen;
        }

        public bool TryPickUpItem(PickUpModel item)
        {
            var canPickUp = _playerInventory.TryAddItem(item);
            if (canPickUp)
            {
                _playerInventory.AddItem(item);
            }
            _inventoryUI.UpdateInventory();
            return canPickUp;
        }

        public Inventory GetPlayerInventory() => _playerInventory;
        
        public void LockInventory(bool locked)
        {
            _inventoryLocked = locked;
        }

        private void OnEquipBtnClick(Guid slotId)
        {
            Debug.Log($"Equip {slotId}");
            var slot = _playerInventory.GetItems().FirstOrDefault(x => x.Id == slotId);
            if (slot?.Item is not null)
            {
                if (slot.IsEquip)
                {
                    _equipManager.Unequip(slot);
                    Debug.Log($"Unequip {slotId}");
                }
                else
                {
                    _equipManager.Equip(slot);
                    Debug.Log($"Equip {slotId}");
                }
            }
        }
        
        private void OnDropBtnClick(Guid slotId)
        {
            Debug.Log($"Drop {slotId}");
            var slot = _playerInventory.GetItems().FirstOrDefault(x => x.Id == slotId);
            if (slot?.Item is not null)
            {
                var canRemove = _playerInventory.TryRemoveItem(slotId, 1);
                if (!canRemove)
                {
                    return;
                }
                
                var spawnOffset = transform.forward * 1.0f + transform.up * 1f;
                var spawnPosition = transform.position + spawnOffset;
                
                var obj = Instantiate(slot.Item.prefab, spawnPosition, Quaternion.identity, null);
                var rb = obj.GetComponent<Rigidbody>();
                rb?.AddForce((transform.forward + transform.up * 0.2f) * 10f, ForceMode.Impulse);

                if (slot.IsEquip)
                {
                    _equipManager.Unequip(slot);
                }
                
                _playerInventory.RemoveItem(slotId, 1);
            }
        }
        
        private void OnUseBtnClick(Guid slotId)
        {
            Debug.Log($"Use {slotId}");
            var slot = _playerInventory.GetItems().FirstOrDefault(x => x.Id == slotId);
            if (slot?.Item is not null)
            {
                var canRemove = _playerInventory.TryRemoveItem(slotId, 1);
                if (!canRemove)
                {
                    return;
                }
                
                foreach (var effect in slot.Item.applyEffects)
                {
                    _needs.Apply(effect);
                }
                _playerInventory.RemoveItem(slotId, 1);
            }
        }
    }
}