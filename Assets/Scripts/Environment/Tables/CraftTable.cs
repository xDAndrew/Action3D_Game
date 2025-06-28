using Items.Interfaces;
using Player.Interfaces;
using UI.Crafting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Environment.Tables
{
    public class CraftTable : MonoBehaviour, IInteractable
    {
        private IInventoryController _inventoryController;
        private GameObject _hudInstance;
    
        public string GetInteractionPromt()
        {
            return "Press [E] to open craft menu";
        }

        public void OnInteract(GameObject interactingObject)
        {
            _inventoryController = interactingObject.GetComponent<IInventoryController>();
            _inventoryController.LockInventory(true);
            var playerInventory = _inventoryController.GetPlayerInventory();
        
            Addressables.InstantiateAsync("CraftHUD").Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    _hudInstance = handle.Result;
                    
                    var mainWindow = _hudInstance.transform.Find("CraftWindow").GetComponent<CraftWindowScript>();
                    mainWindow.SetData(playerInventory);
                    mainWindow.onCloseWindow.AddListener(OnClickCloseBtn);
                    
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
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
