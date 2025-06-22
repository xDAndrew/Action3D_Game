using System;
using System.Collections.Generic;
using System.Linq;
using Core.InventoryService;
using UI;
using UI.InventoryUI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StorageHUD : MonoBehaviour
{
    public UnityEvent onClickCloseBtn;

    private Inventory _playerInventory;
    private Inventory _storageInventory;

    private ItemSlot _itemSlotPrefab;
    private ItemSlot _selectSlot;

    private ClickableImage _leftBtn;
    private ClickableImage _rightBtn;
    
    private readonly List<ItemSlot> _playerInventorySlots = new();
    private readonly List<ItemSlot> _storageInventorySlots = new();

    private const string InventorySlotUIPrefabName = "InventorySlotUI";

    private void Awake()
    {
        var closeBtn = transform.Find("CloseBtn").GetComponent<ClickableImage>();
        closeBtn.onLeftClick.AddListener(() =>
        {
            ResetSelect();
            onClickCloseBtn?.Invoke();
        });
        
        _leftBtn = transform.Find("LeftBtn").GetComponent<ClickableImage>();
        _leftBtn.onLeftClick.AddListener(OnLeftBtnClick);
        
        _rightBtn = transform.Find("RightBtn").GetComponent<ClickableImage>();
        _rightBtn.onLeftClick.AddListener(OnRightBtnClick);
    }

    private void Start()
    {
        var playerSlotsContainer = transform.Find("PlayerInventory");
        var storageSlotsContainer = transform.Find("StorageInventory");
        
        Addressables.LoadAssetAsync<GameObject>(InventorySlotUIPrefabName).Completed += handle =>
        {
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError("Не удалось загрузить префаб InventorySlotUI");
                return;
            }

            var prefab = handle.Result;

            var index = 0;
            foreach (var slot in _playerInventory.GetItems())
            {
                var instance = Instantiate(prefab, playerSlotsContainer);
                var newSlot = instance.GetComponent<ItemSlot>();

                if (newSlot is null)
                {
                    Debug.LogError("На префабе InventorySlotUI нет компонента ItemSlot");
                    continue;
                }
                
                newSlot.OnSlotClick.AddListener(OnSlotClick);
                newSlot.name = $"PlayerInventorySlot_{index}";
                newSlot.SetSlot(slot);
                _playerInventorySlots.Add(newSlot);
                index++;
            }

            index = 0;
            foreach (var slot in _storageInventory.GetItems())
            {
                var instance = Instantiate(prefab, storageSlotsContainer);
                var newSlot = instance.GetComponent<ItemSlot>();

                if (newSlot is null)
                {
                    Debug.LogError("На префабе InventorySlotUI нет компонента ItemSlot");
                    continue;
                }

                newSlot.OnSlotClick.AddListener(OnSlotClick);
                newSlot.name = $"StorageInventorySlot_{index}";
                newSlot.SetSlot(slot);
                _storageInventorySlots.Add(newSlot);
                index++;
            }
        };
    }

    public void SetInventory(Inventory playerInventory, Inventory storageInventory)
    {
        _playerInventory = playerInventory;
        _storageInventory = storageInventory;
    }

    public void UpdateHUD()
    {
        var index = 0;
        foreach (var slot in _playerInventory.GetItems())
        {
            _playerInventorySlots[index].SetSlot(slot);
            index++;
        }

        index = 0;
        foreach (var slot in _storageInventory.GetItems())
        {
            _storageInventorySlots[index].SetSlot(slot);
            index++;
        }
    }

    private void ResetSelect()
    {
        foreach (var slot in _playerInventorySlots)
        {
            slot.SetSelect(false);
        }
        foreach (var slot in _storageInventorySlots)
        {
            slot.SetSelect(false);
        }
        _selectSlot = null;
        UpdateHUD();
    }

    private void OnSlotClick(Guid slotId)
    {
        ResetSelect();
        _selectSlot = _playerInventorySlots.FirstOrDefault(x => x.GetInventorySlot().Id == slotId) ??
                      _storageInventorySlots.FirstOrDefault(x => x.GetInventorySlot().Id == slotId);
        
        _selectSlot?.SetSelect(true);
        UpdateHUD();
    }

    private void OnLeftBtnClick()
    {
        var slot = _storageInventorySlots.FirstOrDefault(x => x.GetInventorySlot().Id == _selectSlot.GetInventorySlot().Id);
        if (slot is null) return;
        
        var canRemove = _storageInventory.TryRemoveItem(slot.GetInventorySlot().Id, 1);
        var canAdd = _playerInventory.TryAddItem(slot.GetInventorySlot().Item);
        if (!canAdd || !canRemove) return;
        
        _playerInventory.AddItem(slot.GetInventorySlot().Item);
        _storageInventory.RemoveItem(slot.GetInventorySlot().Id, 1);
        
        UpdateHUD();
    }

    private void OnRightBtnClick()
    {
        var slot = _playerInventorySlots.FirstOrDefault(x => x.GetInventorySlot().Id == _selectSlot.GetInventorySlot().Id);
        if (slot is null) return;
        
        var canRemove = _playerInventory.TryRemoveItem(slot.GetInventorySlot().Id, 1);
        var canAdd = _storageInventory.TryAddItem(slot.GetInventorySlot().Item);
        if (!canAdd || !canRemove) return;
        
        _storageInventory.AddItem(slot.GetInventorySlot().Item);
        _playerInventory.RemoveItem(slot.GetInventorySlot().Id, 1);
        
        UpdateHUD();
    }
}