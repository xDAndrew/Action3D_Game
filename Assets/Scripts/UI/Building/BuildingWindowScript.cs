using System.Collections.Generic;
using Data.Building;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Building
{
    public class BuildingWindowScript : MonoBehaviour
    {
        public UnityEvent onCloseWindow;
        public List<BuildingRecipeData> recipeList = new();
        
        /*private Inventory _playerInventory;
        private ItemRecipeScript _selectedRecipe;

        private const string ItemRecipe = "ItemRecipe";
        private GameObject _itemRecipePrefab;

        private void Awake()
        {
            var closeBtn = transform.Find("CloseBtn").GetComponent<ClickableImage>();
            closeBtn.onLeftClick.AddListener(OnCloseWindow);
            
            var craftBtn = transform.Find("CraftBtn").GetComponent<ClickableImage>();
            craftBtn.onLeftClick.AddListener(OnCraftBtnClick);
            
            Addressables.LoadAssetAsync<GameObject>(ItemRecipe).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    _itemRecipePrefab = handle.Result;
                    var panel = transform.Find("RecipePanel");

                    var index = 0;
                    if (recipeList.Count > 0)
                    {
                        foreach (var recipe in recipeList)
                        {
                            var instance = Instantiate(_itemRecipePrefab, panel);
                            instance.name = $"ItemRecipe_{index}";
                            instance.GetComponent<ItemRecipeScript>().SetRecipe(recipe);
                            instance.GetComponent<ItemRecipeScript>().OnClickEvent.AddListener(OnSelectRecipe);
                            index++;
                        }
                    }
                    
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    Debug.LogError($"Error while handling prefab loading '{ItemRecipe}'");
                }
            };
        }

        public void SetData(Inventory playerInventory)
        {
            _playerInventory = playerInventory;
            UpdateUI();
        }
        
        public void UpdateUI()
        {
        }

        private void OnSelectRecipe(ItemRecipeScript recipeScript)
        {
            _selectedRecipe?.SetSelected(false);
            _selectedRecipe = recipeScript;
            _selectedRecipe.SetSelected(true);
            UpdateUI();
        }
        
        private void OnCraftBtnClick()
        {
            if (_selectedRecipe?.recipeData is null || _playerInventory is null || _selectedRecipe.recipeData.craftingRecipe.Count <= 0)
            {
                Debug.LogError("OnCraftBtnClick() called without selected recipe or player inventory");
                return;
            }

            if (!_playerInventory.TryAddItem(_selectedRecipe.recipeData.craftItem))
            {
                return;
            };

            var isItemsEnough = _selectedRecipe.recipeData.craftingRecipe
                .Aggregate(true, (current, recipe) => current & _playerInventory.IsItemExist(recipe.resource.id, recipe.amount));

            if (!isItemsEnough)
            {
                return;
            }

            foreach (var recipe in _selectedRecipe.recipeData.craftingRecipe)
            {
                _playerInventory.RemoveItem(Guid.Parse(recipe.resource.id), recipe.amount);
            }
            _playerInventory.AddItem(_selectedRecipe.recipeData.craftItem);
        }
        
        private void OnCloseWindow()
        {
            onCloseWindow?.Invoke();
        }

        public void OnDestroy()
        {
            if (_itemRecipePrefab is not null)
            {
                Addressables.Release(_itemRecipePrefab);
            }
        }*/
    }
}
