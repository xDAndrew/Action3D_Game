using UnityEngine;

namespace Items
{
    public class ItemObject : MonoBehaviour, IInteractable
    {
        public ItemModel item;
        
        public string GetInteractionPromt()
        {
            return $"Pickup {item.displayName}";
        }

        public ItemModel GetItem()
        {
            return item;
        }
        
        public void OnInteract()
        {
            Destroy(gameObject);
        }
    }
}
