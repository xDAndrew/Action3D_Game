using System;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "StorageModel", menuName = "Scriptable Objects/StorageModel")]
    public class StorageModel : ScriptableObject
    {
        [Header("Information")]
        public readonly string Id = Guid.NewGuid().ToString();
        public string displayName;

        [Header("Inventory")]
        public int slotsCount;
    }
}