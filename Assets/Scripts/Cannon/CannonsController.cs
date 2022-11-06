using System.Collections.Generic;
using Pool_And_Particles;
using UnityEngine;

namespace DefaultNamespace
{
    public class CannonsController : MonoBehaviour
    {
        [SerializeField, NonReorderable] private List<SimpleCannon> _cannons;
        [SerializeField, NonReorderable] private List<CannonBullet> _bullets;
        [SerializeField] private Transform _target;

        [Space, SerializeField] private float _fireFrequency;
        [SerializeField] private float _speed;
        [SerializeField] private float _radius;
        private Pool _pool;

        public void OnAwake(Pool pool)
        {
            _pool = pool;

            foreach (CannonBullet cannonBullet in _bullets)
            {
                _pool.Prepare(cannonBullet, 300);
            }
            
            foreach (SimpleCannon cannon in _cannons)
            {
                cannon.OnAwake(_pool, _fireFrequency, _speed, _bullets, _radius);
            }   
            
            OnSceneSwitched();
        }

        public void OnSceneSwitched()
        {
            Debug.Log("switched");
            
            foreach (CannonBullet cannonBullet in _bullets)
            {
                _pool.FreeAll(cannonBullet);
            }
            
            foreach (SimpleCannon cannon in _cannons)
            {
                cannon.OnSceneSwitched(_target);
            }
        }
        
        // private void OnValidate()
        // {
        //     foreach (SimpleCannon cannon in _cannons)
        //     {
        //         cannon.Validate(_fireFrequency, _speed);
        //     }   
        // }
    }
}