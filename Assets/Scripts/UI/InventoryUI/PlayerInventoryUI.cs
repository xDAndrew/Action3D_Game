using System;
using System.Collections.Generic;
using System.Linq;
using Core.InventoryService;
using Items.Types;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI.InventoryUI
{
    public class PlayerInventoryUI : MonoBehaviour
    {
        public ItemSlot slotPrefab;

        public UnityEvent<Guid> onUseBtnClick;
        public UnityEvent<Guid> onDropBtnClick;
        public UnityEvent<Guid> onEquipBtnClick;
        
        private Transform _slotContainer;
        private readonly List<ItemSlot> _itemSlots = new();

        private Inventory _inventory;
        private ItemSlot _selectSlot;
        private ItemSlot _equipSlot;
        
        private ClickableImage _equipBtn;
        private ClickableImage _useBtn;
        private ClickableImage _dropBtn;
        
        private TextMeshProUGUI _equipBtnLabel;

        private void Awake()
        {
            _equipBtnLabel = transform.Find("EquipBtn").Find("Label").GetComponent<TextMeshProUGUI>();
            
            _equipBtn = transform.Find("EquipBtn").GetComponent<ClickableImage>();
            _equipBtn.onLeftClick.AddListener(() =>
            {
                if (_selectSlot is not null)
                {
                    foreach (var itemSlot in _itemSlots)
                    {
                        itemSlot.SetEquip(false);
                    }
                    
                    if (_selectSlot == _equipSlot)
                    {
                        _equipSlot.SetEquip(false);
                        _equipSlot = null;
                    }
                    else
                    {
                        _equipSlot = _selectSlot;
                        _equipSlot.SetEquip(true);
                    }
                    
                    onEquipBtnClick?.Invoke(_selectSlot.GetInventorySlot().Id);
                    UpdateInventory();
                }
            });
            
            _useBtn = transform.Find("UseBtn").GetComponent<ClickableImage>();
            _useBtn.onLeftClick.AddListener(() =>
            {
                if (_selectSlot is not null)
                {
                    onUseBtnClick?.Invoke(_selectSlot.GetInventorySlot().Id);
                }
                UpdateInventory();
            });
            
            _dropBtn = transform.Find("DropBtn").GetComponent<ClickableImage>();
            _dropBtn.onLeftClick.AddListener(() =>
            {
                if (_selectSlot is not null)
                {
                    if (_selectSlot == _equipSlot)
                    {
                        _equipSlot.SetEquip(false);
                        _equipSlot = null;
                    }
                    
                    onDropBtnClick?.Invoke(_selectSlot.GetInventorySlot().Id);
                }
                UpdateInventory();
            });
        }

        public void Start()
        {
            _slotContainer = gameObject.transform.Find("Slots");
            for (var i = 0; i < 12; i++)
            {
                var slot = Instantiate(slotPrefab, _slotContainer);
                slot.name = $"Slot_{i}";
                slot.OnSlotClick.AddListener(OnSlotClick);
                _itemSlots.Add(slot);
            }
            
            UpdateInventory();
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

        public void SetInventory(Inventory inventory)
        {
            _inventory = inventory;
        }

        public void UpdateInventory()
        {
            if (_itemSlots.Count <= 0) return;
            
            var playerInventory = _inventory.GetItems();
            for (var i = 0; i < 12; i++)
            {
                _itemSlots[i].SetSlot(playerInventory[i]);
            }

            var useBtn = transform.Find("UseBtn");
            var equipBtn = transform.Find("EquipBtn");
            var dropBtn = transform.Find("DropBtn");

            var itemNameText = transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var itemDescriptionText = transform.Find("Description").GetComponent<TextMeshProUGUI>();
            
            itemNameText.text = _selectSlot?.GetInventorySlot()?.Item is not null ? _selectSlot.GetInventorySlot().Item.name : string.Empty;
            itemDescriptionText.text = _selectSlot?.GetInventorySlot()?.Item is not null ? _selectSlot.GetInventorySlot().Item.description : string.Empty;
            
            if (_selectSlot?.GetInventorySlot()?.Item is null)
            {
                useBtn.gameObject.SetActive(false);
                equipBtn.gameObject.SetActive(false);
                dropBtn.gameObject.SetActive(false);
            }
            else
            {
                var item = _selectSlot.GetInventorySlot().Item;
                switch (item.type)
                {
                    case ItemType.Resource:
                        useBtn.gameObject.SetActive(false);
                        equipBtn.gameObject.SetActive(false);
                        break;
                    case ItemType.Equipment:
                        useBtn.gameObject.SetActive(false);
                        equipBtn.gameObject.SetActive(true);
                        _equipBtnLabel.text = _selectSlot == _equipSlot ? "Unequip" : "Equip";;
                        break;
                    case ItemType.Consumable:
                        useBtn.gameObject.SetActive(true);
                        equipBtn.gameObject.SetActive(false);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                dropBtn.gameObject.SetActive(true);
            }
        }

        public void ResetSelect()
        {
            foreach (var slot in _itemSlots)
            {
                slot.SetSelect(false);
            }
            _selectSlot = null;
            UpdateInventory();
        }
        
        private void OnSlotClick(Guid slotId)
        {
            ResetSelect();
            _selectSlot = _itemSlots.FirstOrDefault(x => x.GetInventorySlot().Id == slotId);
            _selectSlot?.SetSelect(true);
            UpdateInventory();
        }
    }
}