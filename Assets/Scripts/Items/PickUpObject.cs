using Items.Interfaces;
using Player.Interfaces;
using UnityEngine;

namespace Items
{
    public class PickUpObject : MonoBehaviour, IInteractable
    {
        public ItemModel item;
        
        public string GetInteractionPromt()
        {
            return $"Press <b>[E]</b> take {item.displayName}";
        }
        
        public void OnInteract(GameObject interactingObject)
        {
            var inventory = interactingObject.GetComponent<IInventoryController>();
            var canPickUp = inventory.TryPickUpItem(item);
            
            if (canPickUp)
            {
                Destroy(gameObject);
            }
        }
    }
}
