using Core.InventoryService;
using Items.Interfaces;
using Player.Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Items
{
    public class StorageObject : MonoBehaviour, IInteractable
    {
        public StorageModel storageObject;

        private GameObject _hudInstance;
        private Inventory _storage;
        
        private IInventoryController _inventoryController;
        
        private const string StorageHUDPrefabName = "BoxSwapHUD";

        public void Awake()
        {
            _storage = new Inventory(storageObject.slotsCount);
        }

        public string GetInteractionPromt()
        {
            return $"Press <b>[E]</b> to open {storageObject.displayName}";
        }

        public void OnInteract(GameObject interactingObject)
        {
            _inventoryController = interactingObject.GetComponent<IInventoryController>();
            _inventoryController.LockInventory(true);
            var playerInventory = _inventoryController.GetPlayerInventory();
            
            Addressables.InstantiateAsync(StorageHUDPrefabName).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    _hudInstance = handle.Result;
                    
                    var mainWindow = _hudInstance.transform.Find("MainWindow").GetComponent<StorageHUD>();
                    mainWindow.SetInventory(playerInventory, _storage);
                    mainWindow.onClickCloseBtn.AddListener(OnClickCloseBtn);
                    
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    
                    Debug.Log("Префаб успешно загружен и инстанцирован!");
                }
                else
                {
                    Debug.LogError($"Ошибка при загрузке префаба {StorageHUDPrefabName}");
                }
            };
        }

        private void OnClickCloseBtn()
        {
            if (_hudInstance is not null)
            {
                Addressables.ReleaseInstance(_hudInstance);
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            _inventoryController.LockInventory(false);
        }
    }
}