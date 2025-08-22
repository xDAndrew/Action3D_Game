using System.Collections;
using System.Collections.Generic;
using Player.Interfaces;
using UnityEngine;

namespace Environment
{
    public class Cactus : MonoBehaviour
    {
        public int damage;
        public float damageRate;

        private Coroutine _coroutine;
        private readonly List<IDamageable> _damageableObjectList = new();

        private IEnumerator DealDamage()
        {
            while (true)
            {
                foreach (var damageableObject in _damageableObjectList)
                {
                    damageableObject.TakeDamage(damage);
                }
            
                yield return new WaitForSeconds(damageRate);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.TryGetComponent<IDamageable>(out var damageableObject))
            {
                return;
            }
            
            _damageableObjectList.Add(damageableObject);
            
            if (_damageableObjectList.Count > 0 && _coroutine == null)
            {
                _coroutine = StartCoroutine(DealDamage());
            }
        }
        
        private void OnCollisionExit(Collision other)
        {
            if (!other.gameObject.TryGetComponent<IDamageable>(out var damageableObject))
            {
                return;
            }
            
            _damageableObjectList.Remove(damageableObject);

            if (_damageableObjectList.Count > 0 || _coroutine == null)
            {
                return;
            }
            
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
}
