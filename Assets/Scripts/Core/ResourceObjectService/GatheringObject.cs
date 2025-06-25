using System;
using Data.GameResources;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.ResourceObjectService
{
    [Serializable]
    public class GatheringObject : MonoBehaviour, IGatherable
    {
        public GatheringObjectData resource;
        
        private float _hitPoints;
        
        private void Awake()
        {
            _hitPoints = resource.HitPoints;
        }

        public void TakeGathering(float damage)
        {
            _hitPoints -= damage;
            if (_hitPoints <= 0)
            {
                BrokeObject();
            }
        }

        public void BrokeObject()
        {
            gameObject.SetActive(false);
            
            var index = 0;
            foreach (var pickUpModel in resource.ResourceList)
            {
                if (!RollChance(pickUpModel.Chance)) continue;
                
                var spawnPoint = transform.position.y + index * 5;
                Instantiate(pickUpModel.Resource.prefab, new Vector3(transform.position.x, spawnPoint, transform.position.z), Quaternion.identity);
                index++;
            }
            
            Destroy(gameObject);
        }
        
        private static bool RollChance(float chance)
        {
            return chance switch
            {
                <= 0f => false,
                >= 1f => true,
                _ => Random.value < chance
            };
        }
    }
}