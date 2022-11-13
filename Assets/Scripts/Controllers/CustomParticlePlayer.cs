using DG.Tweening;
using Extensions;
using UnityEngine;

namespace DefaultNamespace
{
    public class CustomParticlePlayer : MonoBehaviour
    {
        [Space] 
        [SerializeField] private ParticleSystem _personalExplosion;
        [SerializeField] private ParticleSystem _personalSmokeFlow;
        [SerializeField] private float _startVolume = 1.0f;
        [SerializeField] private AudioSource _explosionSource;
        [SerializeField] private Ease _explosionEase = Ease.InCubic;
        public void Play1()
        {
            _explosionSource = GetComponent<AudioSource>();
            if (_personalExplosion != null)
            {
                _personalExplosion.transform.parent = null;
                _personalExplosion.Play(true);
            }

            if (_personalSmokeFlow != null)
            {
                _personalSmokeFlow.Play(true);
            }

            if (_explosionSource != null)
            {
                _explosionSource.Play();
                _explosionSource.DoVolume(_startVolume, 0.0f, 2).SetEase(_explosionEase);
            }
        }
        
    }
}