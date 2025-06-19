using System.Collections;
using System.Collections.Generic;
using Player;
using Player.Interfaces;
using UnityEngine;

namespace Environment
{
    public class Cactus : MonoBehaviour
    {
        public int damage;
        public float damageRate;

        private readonly List<IDamageable> _thingsToDamage = new();

        private void Start()
        {
            StartCoroutine(DealDamage());
        }
    
        private IEnumerator DealDamage()
        {
            while (true)
            {
                foreach (var thing in _thingsToDamage)
                {
                    thing.TakeDamage(damage);
                }
            
                yield return new WaitForSeconds(damageRate);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.GetComponent<IDamageable>() is not null)
            {
                _thingsToDamage.Add(other.gameObject.GetComponent<IDamageable>());
            } 
        }
        
        // Stop attack
        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.GetComponent<IDamageable>() is not null)
            {
                _thingsToDamage.Remove(other.gameObject.GetComponent<IDamageable>());
            } 
        }
    }
}
