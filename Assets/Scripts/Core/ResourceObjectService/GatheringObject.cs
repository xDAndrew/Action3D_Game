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
        
        public GameObject hitParticle;
        
        [Header("Audio")]
        public AudioClip toolClip;
        public AudioClip brokenClip;
        
        private float _hitPoints;
        private AudioSource _audioSource;
        
        private void Awake()
        {
            _hitPoints = resource.hitPoints;
            _audioSource = Camera.main?.GetComponent<AudioSource>();
        }

        public void TakeGathering(Guid toolId, float damage, Vector3 point, Vector3 normal)
        {
            if (resource.Id != toolId) return;

            if (toolClip is not null)
            {
                _audioSource?.PlayOneShot(toolClip);
            }

            if (hitParticle is not null)
            {
                Instantiate(hitParticle, point, Quaternion.LookRotation(normal, Vector3.up));
            }
            
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
            foreach (var pickUpModel in resource.resourceList)
            {
                if (!RollChance(pickUpModel.chance)) continue;
                
                var spawnPoint = transform.position.y + index * 0.4f;
                Instantiate(pickUpModel.resource.prefab, new Vector3(transform.position.x, spawnPoint, transform.position.z), Quaternion.identity);
                index++;
            }
            
            if (brokenClip is not null)
            {
                _audioSource?.PlayOneShot(brokenClip);
            }
            
            Invoke(nameof(ItemDestroy), 1);
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

        public void ItemDestroy()
        {
            Destroy(gameObject);
        }
    }
}