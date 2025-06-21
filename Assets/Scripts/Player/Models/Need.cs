using System;
using UnityEngine.UI;

namespace Player.Models
{
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