using Pool_And_Particles;
using UnityEngine;

namespace DefaultNamespace
{
    public class AnimatedCannon : MonoBehaviour
    {
        [SerializeField] private Transform _point0;
        [SerializeField] private Transform _point1;
        [SerializeField] private Transform _staticParent;

        [Space, SerializeField] private PFXParams _explosionPS;
        
        private Pool _pool;
        private int _index;

        public void OnFired()
        {
            PooledParticleSystem pfx = _explosionPS.Get(_index == 0 ? _point0 : _point1, _pool);
            pfx.Transform.SetParent(_staticParent);
            _index++;
        }

        public void OnAwake(Pool pool)
        {
            _pool = pool;
            _index = 0;
        }
    }
}