using System;
using JetBrains.Annotations;
using Pool_And_Particles;
using UnityEngine;

namespace DefaultNamespace
{
    public class PoolSpawner : MonoBehaviour
    {
        [SerializeField] private ParticleController[] _particleControllers;
        [SerializeField] private AnimatedCannon _animatedCannon;
        [SerializeField] private CannonsController _cannonsController;
        [SerializeField] private Aerocarrier _aerocarrier;
        [SerializeField] private AudioController _audioController;
        [SerializeField] private VCameraController _vCameraController;
        
        private Pool _pool;
        
        
        private void Awake()
        {
            Application.targetFrameRate = 60;
            _pool = new Pool(transform);

            foreach (ParticleController particleController in _particleControllers)
            {
                particleController.OnAwake(_pool);
            }

            _animatedCannon.OnAwake(_pool);
            _cannonsController.OnAwake(_pool);
        }

        [UsedImplicitly]
        public void OnSceneSwitched()
        {
            _vCameraController.OnSceneSwitched();
            _audioController.OnSceneSwitched();
            _cannonsController.OnSceneSwitched();
            _aerocarrier.OnSceneSwitched();
        }
    }
}