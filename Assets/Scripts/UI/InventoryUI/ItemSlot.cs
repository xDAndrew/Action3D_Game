using System;
using Core.InventoryService;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.InventoryUI
{
    public class ItemSlot : MonoBehaviour, IPointerClickHandler
    {
        private InventorySlot _inventorySlot;
        
        private bool _isSelected;

        public readonly UnityEvent<Guid> OnSlotClick = new();
        
        public void SetSlot(InventorySlot slot)
        {
            _inventorySlot = slot;
            UpdateSlot();
        }

        public void SetSelect(bool isSelected)
        {
            _isSelected = isSelected;
            UpdateSlot();
        }

        public InventorySlot GetInventorySlot() => _inventorySlot;
        
        private void UpdateSlot()
        {
            var icon = transform.Find("Icon");
            var countText = transform.Find("Amount");
            var selectedBorder = transform.Find("SelectBorder");
            var equipBorder = transform.Find("EquipBorder");
            
            if (_inventorySlot.Item is null)
            {
                icon.gameObject.SetActive(false);
                icon.GetComponent<Image>().sprite = null;
                countText.gameObject.SetActive(false);
            }
            else
            {
                icon.gameObject.SetActive(true);
                icon.GetComponent<Image>().sprite = _inventorySlot.Item.icon;
                
                countText.gameObject.SetActive(_inventorySlot.Item.stackable);
                var text = countText.GetComponent<TextMeshProUGUI>();
                text.text = _inventorySlot.Amount.ToString();
            }
            
            selectedBorder.gameObject.SetActive(_isSelected);
            equipBorder.gameObject.SetActive(_inventorySlot.IsEquip);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnSlotClick?.Invoke(_inventorySlot.Id);
            }
        }
    }
}
