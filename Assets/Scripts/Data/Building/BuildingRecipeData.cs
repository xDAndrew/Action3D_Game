using System.Collections.Generic;
using Data.Crafting;
using UnityEngine;

namespace Data.Building
{
    [CreateAssetMenu(fileName = "BuildingRecipeData", menuName = "Scriptable Objects/BuildingRecipeData")]
    public class BuildingRecipeData : ScriptableObject
    {
        public string DisplayName;
        public Sprite Icon;
        public GameObject spawnPrefab;
        public GameObject previewPrefab;
        public List<CraftingRecipe> resourceCost;
    }
}
