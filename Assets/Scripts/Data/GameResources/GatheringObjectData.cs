using System;
using System.Collections.Generic;
using Items;
using UnityEngine;

namespace Data.GameResources
{
    [Serializable]
    [CreateAssetMenu(fileName = "GatheringObjectData", menuName = "Scriptable Objects/GatheringObjectData")]
    public class GatheringObjectData : ScriptableObject
    {
        public readonly Guid Id = Guid.NewGuid();
        
        public float hitPoints;
        
		public List<SpawnResourceObjectData> resourceList = new();
    }

    [Serializable]
    public class SpawnResourceObjectData
    {
        public PickUpModel resource;

        public float chance;
    }
}