using System;
using Core.AudioSourcePool;
using Data.GameResources;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Environment.ResourceNode
{
    [Serializable]
    public class ResourceNode : MonoBehaviour
    {
        public ResourceNodeData resource;
        public GameObject hitParticle;
        
        [Header("Audio")]
        public AudioClip toolClip;
        public AudioClip brokenClip;
        
        private float _hitPoints;
        
        [Inject]
        private readonly AudioManager _audioManager;
        
        private void Awake()
        {
            _hitPoints = resource.hitPoints;
        }

        public void TakeDamage(Guid toolId, float damage, Vector3 point, Vector3 normal)
        {
            if (resource.Id != toolId) return;

            if (toolClip is not null)
            {
                _audioManager.PlayOneShotSound(toolClip, gameObject.transform);
            }

            if (hitParticle is not null)
            {
                Instantiate(hitParticle, point, Quaternion.LookRotation(normal, Vector3.up));
            }
            
            _hitPoints -= damage;
            if (_hitPoints <= 0)
            {
                DestroyResourceNode();
            }
        }

        public void DestroyResourceNode()
        {
            gameObject.SetActive(false);
            
            var index = 0;
            foreach (var pickUpModel in resource.resourceList)
            {
                if (Random.value >= pickUpModel.chance) continue;
                var spawnPoint = transform.position.y + index * 0.4f;
                Instantiate(pickUpModel.resource.prefab, new Vector3(transform.position.x, spawnPoint, transform.position.z), Quaternion.identity);
                index++;
            }
            
            if (brokenClip is not null)
            {
                _audioManager.PlayOneShotSound(brokenClip, gameObject.transform);
            }
            
            Destroy(gameObject);
        }
    }
}