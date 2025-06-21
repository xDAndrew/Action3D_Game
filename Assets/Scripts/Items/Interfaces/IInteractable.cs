using UnityEngine;

namespace Items.Interfaces
{
    public interface IInteractable
    {
        string GetInteractionPromt();
        
        void OnInteract(GameObject interactingObject);
    }
}