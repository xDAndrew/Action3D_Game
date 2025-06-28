using System.Collections.Generic;
using Items;
using UnityEngine;

namespace Data.Crafting
{
    [CreateAssetMenu(fileName = "CraftRecipeData", menuName = "Scriptable Objects/CraftRecipeData")]
    public class CraftRecipeData : ScriptableObject
    {
        public PickUpModel craftItem;
        public List<CraftingRecipe> craftingRecipe;
    }

    [System.Serializable]
    public class CraftingRecipe
    {
        public PickUpModel resource;
        public int amount;
    }
}