using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DamageIndicator : MonoBehaviour
    {
        public Image image;
        public float flashSpeed;

        private Coroutine _fadeAway;

        public void Flash()
        {
            if (_fadeAway is not null)
            {
                StopCoroutine(_fadeAway);
            }
            
            image.enabled = true;
            image.color = Color.white;
            
            _fadeAway = StartCoroutine(FadeAway());
        }

        IEnumerator FadeAway()
        {
            var alphaImage = 1.0f;
            while (alphaImage > 0)
            {
                alphaImage -= 1.0f/flashSpeed * Time.deltaTime;
                image.color = new Color(1.0f, 1.0f, 1.0f, alphaImage);
                yield return null;
            }
            image.enabled = false;
        }
    }
}