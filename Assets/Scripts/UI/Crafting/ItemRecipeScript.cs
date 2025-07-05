using Data.Crafting;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace UI.Crafting
{
    public class ItemRecipeScript : MonoBehaviour, IPointerClickHandler
    {
        public UnityEvent<ItemRecipeScript> OnClickEvent;
        public CraftRecipeData recipeData;
        
        private bool _isSelected;
        private bool _prefabLoaded;

        private const string RecipeCostPrefab = "RecipeCost";
        private GameObject _recipeCostPrefab;
        
        public void Awake()
        {
            Addressables.LoadAssetAsync<GameObject>(RecipeCostPrefab).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    _recipeCostPrefab = handle.Result;
                    _prefabLoaded = true;
                    SetRecipe(recipeData);
                }
                else
                {
                    Debug.LogError($"Error while handling prefab loading '{RecipeCostPrefab}'");
                }
            };
        }

        public void SetSelected(bool isSelected)
        {
            _isSelected = isSelected;
            UpdateUI();
        }

        public void SetRecipe(CraftRecipeData recipeData)
        {
            if (recipeData is null)
            {
                Debug.LogError("CraftRecipeData is null");
            }
            
            this.recipeData = recipeData;

            if (_prefabLoaded)
            {
                SpawnResources();
                UpdateUI();
            }
        }

        private void SpawnResources()
        {
            var recipeCostPanel = transform.Find("RecipePanel");
            if (recipeData?.craftingRecipe?.Count > 0)
            {
                var index = 0;
                foreach (var recipe in recipeData.craftingRecipe)
                {
                    var instantiate = Instantiate(_recipeCostPrefab, recipeCostPanel);
                    instantiate.name = $"RecipeResource_{index}";
                    var resource = instantiate.GetComponent<RecipeCostScript>();
                    resource.SetItem(recipe.resource, recipe.amount);
                    index++;
                }
            }
        }
        
        private void UpdateUI()
        {
            //Update border
            var border = GetComponent<Outline>();
            var borderSize = _isSelected ? 5.0f : 1.0f;
            border.effectDistance = new Vector2(borderSize, borderSize);

            if (recipeData is null) return;
            var itemIcon = transform.Find("ItemPanel").Find("Icon").GetComponent<Image>();
            var itemName = transform.Find("ItemPanel").Find("Name").GetComponent<TextMeshProUGUI>();
            itemIcon.sprite = recipeData.craftItem.icon;
            itemName.text = recipeData.craftItem.displayName;
        }

        private void OnDestroy()
        {
            if (_recipeCostPrefab is not null)
            {
                Addressables.Release(_recipeCostPrefab);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickEvent?.Invoke(this);
        }
    }
}
