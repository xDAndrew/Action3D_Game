using Core.InventoryService;
using Items.Interfaces;
using UnityEngine;

namespace Items
{
    public class StorageObject : MonoBehaviour, IInteractable
    {
        public StorageModel storageObject;

        private Inventory _storage;

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
            throw new System.NotImplementedException();
        }
    }
}