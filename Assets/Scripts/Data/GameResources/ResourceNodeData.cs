using System;
using System.Collections.Generic;
using Items;
using UnityEngine;

namespace Data.GameResources
{
    [Serializable]
    [CreateAssetMenu(fileName = "GatheringObjectData", menuName = "Scriptable Objects/GatheringObjectData")]
    public class ResourceNodeData : ScriptableObject
    {
        public readonly Guid Id = Guid.NewGuid();
        
        public float hitPoints;
        
		public List<SpawnResourceNode> resourceList = new();
    }

    [Serializable]
    public class SpawnResourceNode
    {
        public PickUpModel resource;

        public float chance;
    }
}