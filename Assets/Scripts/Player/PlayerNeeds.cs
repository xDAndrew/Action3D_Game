using System;
using Player.Interfaces;
using Player.Models;
using Player.Types;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Player
{
    public class PlayerNeeds : MonoBehaviour, IDamageable, IAppliable
    {
        public Need health;
        public Need hunger;
        public Need water;
        public Need sleep;

        public float hungerHealthDecay;
        public float thirstHealthDecay;

        public UnityEvent onTakeDamage;
        
        private void Start()
        {
            // set start values
            health.currentValue = health.startValue;
            hunger.currentValue = hunger.startValue;
            water.currentValue = water.startValue;
            sleep.currentValue = sleep.startValue;
        }

        private void Update()
        {
            // reduce needs
            hunger.Subtract(hunger.decayRate * Time.deltaTime);
            water.Subtract(water.decayRate * Time.deltaTime);
            sleep.Subtract(sleep.decayRate * Time.deltaTime);
            
            //reduce health if we don't have gunger
            if (hunger.currentValue == 0.0f)
            {
                health.Subtract(hungerHealthDecay * Time.deltaTime);
            }
            
            if (water.currentValue == 0.0f)
            {
                health.Subtract(thirstHealthDecay * Time.deltaTime);
            }

            if (health.currentValue == 0.0f)
            {
                Die();
            }
            
            health.uiBar.fillAmount = health.GetPercentage();
            hunger.uiBar.fillAmount = hunger.GetPercentage();
            water.uiBar.fillAmount = water.GetPercentage();
            sleep.uiBar.fillAmount = sleep.GetPercentage();
        }

        public void Heal(float amount)
        {
            health.Add(amount);
        }
        
        public void Eat(float amount)
        {
            hunger.Add(amount);
        }
        
        public void Drink(float amount)
        {
            water.Add(amount);
        }
        
        public void Sleep(float amount)
        {
            sleep.Add(amount);
        }
        
        public void TakeDamage(int damageAmount)
        {
            health.Subtract(damageAmount);
            onTakeDamage?.Invoke();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Die()
        {
            Debug.Log("Player is dead");
        }

        public void Apply(ApplyEffect effect)
        {
            switch (effect.Type)
            {
                case EffectType.Food:
                    hunger.Add(effect.Amount);
                    break;
                case EffectType.Water:
                    water.Add(effect.Amount);
                    break;
                case EffectType.Sleep:
                    sleep.Add(effect.Amount);
                    break;
                case EffectType.Hp:
                    health.Add(effect.Amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    [Serializable]
    public class Need
    {
        public float currentValue;
        public float maxValue;
        public float startValue;
        public float regen;
        public float decayRate;
        public Image uiBar;

        public void Add(float amount)
        {
            currentValue = MathF.Min(currentValue + amount, maxValue);
        }

        public void Subtract(float amount)
        {
            currentValue = MathF.Max(currentValue - amount, 0.0f);
        }

        public float GetPercentage()
        {
            return currentValue / maxValue;
        }
    }
}