using System;
using Pool_And_Particles;
using UnityEngine;

namespace DefaultNamespace
{
    public class PoolSpawner : MonoBehaviour
    {
        [SerializeField] private ParticleController[] _particleControllers;
        [SerializeField] private AnimatedCannon _animatedCannon;
        [SerializeField] private CannonsController _cannonsController;
        
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

        public void OnSceneSwitched()
        {
            _cannonsController.OnSceneSwitched();
        }
    }
}