using System;
using System.Collections.Generic;
using Items.Types;
using Player.Models;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "ItemModel", menuName = "Scriptable Objects/ItemModel")]
    public class ItemModel : ScriptableObject
    {
        [Header("Information")] 
        public string id = Guid.NewGuid().ToString();
        public string displayName;
        public string description;
        public ItemType type;
        
        public Sprite icon;
        public GameObject prefab;
        
        [Header("Stacking")]
        public bool stackable;
        public int maxStackAmount;

        [Header("Effect")]
        public List<ApplyEffect> applyEffects;
        
        [Header("Equipment")]
        public GameObject equipmentPrefab;
    }
}
