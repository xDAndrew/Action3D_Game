namespace Items.Interfaces
{
    public interface IInteractable
    {
        string GetInteractionPromt();
    
        ItemModel GetItem();
        
        void OnInteract();
    }
}