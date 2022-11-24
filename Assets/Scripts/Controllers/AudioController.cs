using System.Collections;
using DG.Tweening;
using Extensions;
using JetBrains.Annotations;
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
        [SerializeField] private AudioSource _nuclearSound;
        
        private float _elapsedTime;
        private int _explosionIndex;
        private int _index = -1;

        private void Awake()
        {
            Extensionss.Wait(2.0f).OnComplete(StartAmbient);
            _sabatonSource.DoVolume(0.0f, _sabatonEndVolume, _sabatonDur).SetEase(_soundEase);
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
        }

        public void OnSceneSwitched()
        {
            _index++;

            if (_index == 19)
            {
                _sabatonSource.DoVolume(_sabatonSource.volume, 0.0f, 3f).SetEase(Ease.Linear);
            }
        }

        public void OnFired()
        {
            _cannonSound[_explosionIndex].Play();
            _cannonSound[_explosionIndex].DoVolume(1f, 0f, 2f).SetEase(_cannonEase);
            _explosionIndex++;
        }

        public void OnNuclear()
        {
            _nuclearSound.Play();
        }
        
        [UsedImplicitly]
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
            float dur = 100.0f;
            while (_elapsedTime<dur)
            {
                float delay = Random.Range(_soundIntervalRange.x, _soundIntervalRange.y);
                yield return new WaitForSeconds(delay);
                _ambientSource.clip = _ambientSounds[Random.Range(0, _ambientSounds.Length - 1)];
                _ambientSource.Play();
            }
        }
    }
}