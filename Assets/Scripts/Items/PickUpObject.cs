using Items.Interfaces;
using Player.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Items
{
    public class PickUpObject : MonoBehaviour, IInteractable
    {
        [FormerlySerializedAs("item")] public PickUpModel pickUp;
        
        public string GetInteractionPromt()
        {
            return $"Press <b>[E]</b> take {pickUp.displayName}";
        }
        
        public void OnInteract(GameObject interactingObject)
        {
            var inventory = interactingObject.GetComponent<IInventoryController>();
            var canPickUp = inventory.TryPickUpItem(pickUp);
            
            if (canPickUp)
            {
                Destroy(gameObject);
            }
        }
    }
}
