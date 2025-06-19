using System;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class ItemSlot : MonoBehaviour, IItemSlot, IPointerClickHandler
    {
        private ItemModel _itemModel;
        private int _amount;
        private bool _isSelected;
        
        private GameObject _isSelectedGameObject;

        public readonly string Id = Guid.NewGuid().ToString();

        public readonly UnityEvent<string> OnSlotClick = new();

        private void Start()
        {
            _isSelectedGameObject = transform.Find("Select").gameObject;
            UpdateSlot();
        }

        public void AddItem(ItemModel itemModel, int amount)
        {
            if (itemModel is not null)
            {
                if (_itemModel is null)
                {
                    _itemModel = itemModel;
                    _amount = Mathf.Clamp(amount, 1, itemModel.stackable ? itemModel.maxStackAmount : 1);
                }
                else
                {
                    if (_itemModel.id == itemModel.id)
                    {
                        _amount = Mathf.Clamp(_amount + amount, 1, _itemModel.stackable ? _itemModel.maxStackAmount : 1);
                    }
                    else
                    {
                        _itemModel = itemModel;
                        _amount = Mathf.Clamp(amount, 1, _itemModel.stackable ? _itemModel.maxStackAmount : 1);
                    }
                }
            }

            UpdateSlot();
        }

        public ItemModel GetItem() => _itemModel;
        
        public void SetAmount(int amount)
        {
            _amount = Mathf.Clamp(_amount + amount, 0, _itemModel.stackable ? _itemModel.maxStackAmount : 1);
            if (_amount == 0)
            {
                _itemModel = null;
                _amount = 0;
            }
            
            UpdateSlot();
        }

        public int GetAmount() => _amount;
        
        public void SetSelect(bool isSelected)
        {
            _isSelected = isSelected;
            UpdateSlot();
        }

        private void UpdateSlot()
        {
            var icon = transform.Find("Icon");
            var countText = transform.Find("Amount");
            
            if (_itemModel is null)
            {
                icon.gameObject.SetActive(false);
                icon.GetComponent<Image>().sprite = null;
                
                countText.gameObject.SetActive(false);
            }
            else
            {
                icon.gameObject.SetActive(true);
                icon.GetComponent<Image>().sprite = _itemModel.icon;
                
                countText.gameObject.SetActive(_itemModel.stackable);
                var text = countText.GetComponent<TextMeshProUGUI>();
                text.text = _amount.ToString();
            }
            
            _isSelectedGameObject?.SetActive(_isSelected);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnSlotClick?.Invoke(Id);
            }
        }
    }
}
