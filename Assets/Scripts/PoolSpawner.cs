using System;
using Pool_And_Particles;
using UnityEngine;

namespace DefaultNamespace
{
    public class PoolSpawner : MonoBehaviour
    {
        [SerializeField] private ParticleController[] _particleControllers;
        [SerializeField] private AnimatedCannon _animatedCannon;
        
        
        private Pool _pool;
        
        
        private void Awake()
        {
            _pool = new Pool();

            foreach (ParticleController particleController in _particleControllers)
            {
                particleController.OnAwake(_pool);
            }

            _animatedCannon.OnAwake(_pool);
        }
    }
}