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
        public float HitPoints;
        
		public List<SpawnResourceObjectData> ResourceList = new();
    }

    [Serializable]
    public class SpawnResourceObjectData
    {
        public PickUpModel Resource;

        public float Chance;
    }
}