using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using Player.Interfaces;
using TMPro;
using UI;
using UI.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InventoryController : MonoBehaviour, IInventoryController
    {
        private bool IsInventoryVisible { get; set; }

        public GameObject inventoryPrefab;
        public ItemSlot slotPrefab;
        public Canvas inventoryCanvas;

        private List<ItemSlot> slots = new(12);

        private GameObject _slotsGameObject;
        
        private TextMeshProUGUI _itemNameText;
        private TextMeshProUGUI _itemDescriptionText;

        private Transform _equipBtn;
        private Transform _useBtn;
        private Transform _dropBtn;

        private IAppliable _needs;
        
        private ItemSlot _currentSlot;

        private void Start()
        {
            var inventory = Instantiate(inventoryPrefab, inventoryCanvas.transform);
            inventory.name = "Inventory";

            _itemNameText = FindRecursive(inventory.transform, "ItemName").GetComponent<TextMeshProUGUI>();
            _itemDescriptionText = FindRecursive(inventory.transform, "Description").GetComponent<TextMeshProUGUI>();

            _useBtn = FindRecursive(inventory.transform, "UseBtn");
            _useBtn.GetComponent<ClickableImage>().onLeftClick.AddListener(OnUseBtnClick);
            
            _equipBtn = FindRecursive(inventory.transform, "EquipButton");
            //_UseBtn.onLeftClick.AddListener(OnDropBtnClick);
            
            _dropBtn = FindRecursive(inventory.transform, "DropButton");
            _dropBtn.GetComponent<ClickableImage>().onLeftClick.AddListener(OnDropBtnClick);

            _needs = GetComponent<IAppliable>();
            
            inventoryCanvas.gameObject.SetActive(false);
            _slotsGameObject = inventory.transform.Find("Slots").gameObject;

            for (var i = 0; i < slots.Capacity; i++)
            {
                var slot = Instantiate(slotPrefab, _slotsGameObject.transform);
                slots.Add(slot);

                slot.OnSlotClick.AddListener(OnSlotClick);
            }
        }

        public void OnInventoryOpen(InputAction.CallbackContext context)
        {
            if (!context.started)
            {
                return;
            }

            IsInventoryVisible = !IsInventoryVisible;
            inventoryCanvas.gameObject.SetActive(IsInventoryVisible);

            Cursor.lockState = IsInventoryVisible ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = IsInventoryVisible;

            var state = IsInventoryVisible ? "открыт" : "закрыт";
            Debug.Log($"Инвентарь {state}");
        }

        public bool TryPickUpItem(ItemModel item)
        {
            var emptySlot = slots.FirstOrDefault(x => x.GetItem()?.id == item.id && x.GetAmount() < x.GetItem().maxStackAmount)
                            ?? slots.FirstOrDefault(x => x.GetItem() is null);

            if (emptySlot is null)
            {
                return false;
            }

            emptySlot.AddItem(item, 1);

            return true;
        }

        public static GameObject FindInHierarchy(string name)
        {
            var rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var root in rootObjects)
            {
                var found = FindRecursive(root.transform, name);
                if (found is not null)
                {
                    return found.gameObject;
                }
            }

            return null;
        }

        private static Transform FindRecursive(Transform parent, string name)
        {
            if (parent.name == name)
            {
                return parent;
            }

            return (from Transform child in parent select FindRecursive(child, name))
                .FirstOrDefault(result => result is not null);
        }

        private void OnSlotClick(string slotId)
        {
            _currentSlot?.SetSelect(false);
            _currentSlot = slots.FirstOrDefault(x => x.Id == slotId);
            _currentSlot?.SetSelect(true);
            Update();
        }

        private void Update()
        {
            _itemNameText.text = _currentSlot?.GetItem() is not null ? _currentSlot.GetItem().name : string.Empty;
            _itemDescriptionText.text = _currentSlot?.GetItem() is not null ? _currentSlot.GetItem().description : string.Empty;

            if (_currentSlot is not null)
            {
                var item = _currentSlot.GetItem();
                if (item is not null)
                {
                    switch (item.type)
                    {
                        case ItemType.Resource:
                            _useBtn.gameObject.SetActive(false);
                            _equipBtn.gameObject.SetActive(false);
                            break;
                        case ItemType.Equipment:
                            _useBtn.gameObject.SetActive(false);
                            _equipBtn.gameObject.SetActive(true);
                            break;
                        case ItemType.Consumable:
                            _useBtn.gameObject.SetActive(true);
                            _equipBtn.gameObject.SetActive(false);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    _useBtn.gameObject.SetActive(false);
                    _equipBtn.gameObject.SetActive(false);
                }
            }
            else
            {
                _useBtn.gameObject.SetActive(false);
                _equipBtn.gameObject.SetActive(false);
            }
        }

        private void OnDropBtnClick()
        {
            if (_currentSlot?.GetItem() is not null)
            {
                var spawnOffset = transform.forward * 1.0f + transform.up * 1f;
                var spawnPosition = transform.position + spawnOffset;
                
                var obj = Instantiate(_currentSlot.GetItem().prefab, spawnPosition, Quaternion.identity, null);
                var rb = obj.GetComponent<Rigidbody>();
                rb?.AddForce((transform.forward + transform.up * 0.2f) * 10f, ForceMode.Impulse);

                _currentSlot.SetAmount(-1);
            }
        }

        private void OnUseBtnClick()
        {
            if (_currentSlot?.GetItem() is not null)
            {
                var item = _currentSlot.GetItem();
                foreach (var effect in item.ApplyEffects)
                {
                    _needs.Apply(effect);
                }
                
                _currentSlot.SetAmount(-1);
            }
        }
    }
}