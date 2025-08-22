using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.AudioSourcePool
{
    public class AudioManager : MonoBehaviour
    {
        private readonly Queue<AudioSource> _audioSourcePool = new();
        private int _audioSourceCount;
        
        public void PlayOneShotSound(AudioClip clip, Transform soundPosition)
        {
            var src = GetAudioSource();
            src.transform.SetPositionAndRotation(soundPosition.position, soundPosition.rotation);
            src.volume = 1f;
            src.pitch = 1f;
            src.loop = false;
            src.clip = clip;
            src.Play();
            
            StartCoroutine(ReturnAudioSource(src));
        }

        private AudioSource GetAudioSource()
        {
            if (_audioSourcePool.Count <= 0)
            {
                var audioSourceObject = new GameObject($"PooledAudioSource_{_audioSourceCount++}");
                audioSourceObject.transform.SetParent(transform);
                
                var source = audioSourceObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                return source;
            }
            
            var audioSource = _audioSourcePool.Dequeue();
            audioSource.gameObject.SetActive(true);
            return audioSource;
        }

        private IEnumerator ReturnAudioSource(AudioSource audioSource)
        {
            yield return new WaitUntil(() => !audioSource.isPlaying);
            audioSource.Stop();
            audioSource.clip = null;
            audioSource.loop = false;
            audioSource.gameObject.SetActive(false);
            _audioSourcePool.Enqueue(audioSource);
        }
    }
}