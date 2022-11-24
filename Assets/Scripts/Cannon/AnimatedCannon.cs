using JetBrains.Annotations;
using Pool_And_Particles;
using UnityEngine;

namespace DefaultNamespace
{
    public class AnimatedCannon : MonoBehaviour
    {
        [SerializeField] private Transform _point0;
        [SerializeField] private Transform _point1;
        [SerializeField] private Transform _staticParent;
        [SerializeField] private AudioController _audioController;
        [SerializeField] private ShakeController _shakeController;
        [Space, SerializeField] private PFXParams _explosionPS;
        
        private Pool _pool;
        private int _index;

        [UsedImplicitly]
        public void OnFired()
        {
            PooledParticleSystem pfx = _explosionPS.Get(_index == 0 ? _point0 : _point1, _pool);
            pfx.Transform.SetParent(_staticParent);
            _index++;
            _audioController.OnFired();
            _shakeController.OnFired();
        }

        public void OnAwake(Pool pool)
        {
            _pool = pool;
            _index = 0;
        }
    }
}