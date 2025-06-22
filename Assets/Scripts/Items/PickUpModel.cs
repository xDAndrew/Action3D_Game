using System;
using System.Collections.Generic;
using Items.Types;
using Player.Models;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "PickUpModel", menuName = "Scriptable Objects/PickUpModel")]
    public class PickUpModel : ScriptableObject
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
