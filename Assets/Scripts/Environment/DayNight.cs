using UnityEngine;

namespace Environment
{
    public class DayNight : MonoBehaviour
    {
        //11.59, 12.00
        [Range(0.0f,1.0f)]
        public float time;
    
        public float fullDayLength;

        public float startTime = 0.5f;
    
        public float timeRate;

        public Vector3 noon;
    
        [Header("Sun Settings")]
        public Light sun;
        public Gradient sunColor;
        public AnimationCurve sunIntensity;
    
        [Header("Moon Settings")]
        public Light moon;
        public Gradient moonColor;
        public AnimationCurve moonIntensity;
    
        [Header("Lighting Settings")]
        public AnimationCurve lightingIntensity;
        public AnimationCurve reflectionsIntensityMultiplayer;

        private void Start()
        {
            timeRate = 1.0f/fullDayLength;
            time = startTime;
        }

        private void Update()
        {
            time += timeRate * Time.deltaTime;
            if (time >= 1.0f)
            {
                time = 0.0f;
            }

            sun.transform.eulerAngles = noon * ((time - 0.25f) * 4.0f);
            moon.transform.eulerAngles = noon * ((time - 0.75f) * 4.0f);
        
            sun.intensity = sunIntensity.Evaluate(time);
            moon.intensity = moonIntensity.Evaluate(time);
        
            sun.color = sunColor.Evaluate(time);
            moon.color = moonColor.Evaluate(time);

            if (sun.intensity == 0)
            {
                if (sun.gameObject.activeInHierarchy)
                {
                    sun.gameObject.SetActive(false);
                }
            }
            else
            {
                if (!sun.gameObject.activeInHierarchy)
                {
                    sun.gameObject.SetActive(true);
                }
            }
        
            if (moon.intensity == 0)
            {
                if (moon.gameObject.activeInHierarchy)
                {
                    moon.gameObject.SetActive(false);
                }
            }
            else
            {
                if (!moon.gameObject.activeInHierarchy)
                {
                    moon.gameObject.SetActive(true);
                }
            }
        
            RenderSettings.ambientIntensity = reflectionsIntensityMultiplayer.Evaluate(time);
            RenderSettings.reflectionIntensity = reflectionsIntensityMultiplayer.Evaluate(time);
        }
    }
}
