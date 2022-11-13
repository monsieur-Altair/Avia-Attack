using System.Collections;
using DG.Tweening;
using Extensions;
using UnityEngine;

namespace DefaultNamespace
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource _ambientSource;
        [SerializeField] private AudioClip[] _ambientSounds;
        [SerializeField] private float _ambientMax = 0.8f;
        [Space, SerializeField] private AudioSource _waterSplashSound;
        [SerializeField] private AudioSource[] _cannonSound;
        [SerializeField] private Ease _cannonEase;

        [SerializeField] private AudioSource _sabatonSource;
        [SerializeField] private float _sabatonEndVolume= 0.7f;
        [SerializeField] private float _sabatonDur= 4f;
        [SerializeField] private Ease _soundEase;
        [SerializeField] private Vector2 _soundIntervalRange;
        
        private float _elapsedTime;
        private int _explosionIndex;

        private void Awake()
        {
            Extensionss.Wait(2.0f).OnComplete(StartAmbient);
            _sabatonSource.DoVolume(0.0f, _sabatonEndVolume, _sabatonDur).SetEase(_soundEase);
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
        }

        public void OnFired()
        {
            _cannonSound[_explosionIndex].Play();
            _cannonSound[_explosionIndex].DoVolume(1f, 0f, 2f).SetEase(_cannonEase);
            _explosionIndex++;
        }

        public void WaterSplashed()
        {
            _waterSplashSound.Play();
        }

        private void StartAmbient()
        {
            _elapsedTime = 0.0f;
            _ambientSource.volume = _ambientMax;
            StartCoroutine(PlayAmbient());
        }

        private IEnumerator PlayAmbient()
        {
            float dur = 88.0f;
            while (_elapsedTime<dur)
            {
                float delay = Random.Range(_soundIntervalRange.x, _soundIntervalRange.y);
                yield return new WaitForSeconds(delay);
                _ambientSource.clip = _ambientSounds[UnityEngine.Random.Range(0, _ambientSounds.Length - 1)];
                _ambientSource.Play();
            }
        }
    }
}