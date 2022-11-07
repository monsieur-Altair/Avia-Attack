using System;
using System.Collections.Generic;
using Pool_And_Particles;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class SceneInfo
    {
        public float MinRadCoefficient = 1.0f;
        public float MaxRadCoefficient = 1.0f;
        public float   FreqCoefficient = 1.0f;
        public float  SpeedCoefficient = 1.0f;
        public float  FlightCoefficient = 1.0f;
        public bool UseExplosionPFX = false;
    }
    
    public class CannonsController : MonoBehaviour
    {
        [SerializeField, NonReorderable] private List<SimpleCannon> _cannons;
        [Space,SerializeField, NonReorderable] private List<CannonBullet> _bullets;
        [Space,SerializeField] private List<int> _disableLevels;
        [Space,SerializeField] private List<Transform> _targets;
        [Space,SerializeField] private List<SceneInfo> _sceneInfos;
        [Space, SerializeField] private float _fireFrequency;
        [SerializeField] private float _speed;
        [SerializeField] private float _minRadius = 7.5f;
        [SerializeField] private float _maxRadius = 10.5f;
        [Space, SerializeField] private Transform _customTarget;
        
        private Pool _pool;
        private bool _isActive = true;
        private int _sceneIndex = -2;

        public void OnAwake(Pool pool)
        {
            _pool = pool;

            foreach (CannonBullet cannonBullet in _bullets)
            {
                _pool.Prepare(cannonBullet, 300);
            }
            
            foreach (SimpleCannon cannon in _cannons)
            {
                cannon.OnAwake(_pool, _fireFrequency, _speed, _bullets, _minRadius, _maxRadius);
            }   
            
            OnSceneSwitched();
        }

        public void OnTargetSwitched()
        {
            foreach (SimpleCannon cannon in _cannons)
            {
                cannon.SetTarget(_customTarget);
            }
        }

        public void OnSceneSwitched()
        {
            _sceneIndex++;
            Debug.Log("switched "+_sceneIndex);
            
            foreach (CannonBullet cannonBullet in _bullets)
            {
                _pool.FreeAll(cannonBullet);
            }

            if (_disableLevels.Exists(i => i == _sceneIndex))
            {
                if (_isActive)
                {
                    Disable();
                }
                return;
            }

            if (_sceneIndex < 0)
            {
                return;
            }
            
            Transform target = _targets[_sceneIndex];
            SimpleCannon.NewBulletAmount = GetBulletAmount(target);
            SceneInfo sceneInfo = _sceneInfos[_sceneIndex];
            foreach (SimpleCannon cannon in _cannons)
            {
                cannon.OnSceneSwitched(target, sceneInfo.MinRadCoefficient, sceneInfo.MaxRadCoefficient, 
                    sceneInfo.FreqCoefficient, sceneInfo.SpeedCoefficient, sceneInfo.FlightCoefficient, 
                    sceneInfo.UseExplosionPFX);
            }

            if (_isActive == false)
            {
                Enable();
            }
        }

        private int GetBulletAmount(Transform target)
        {
            float distance = Vector3.Distance(target.position, transform.position);
            float t = distance / _speed;
            return Mathf.RoundToInt(_fireFrequency * t);
        }

        private void Disable()
        {
            _isActive = false;
            foreach (SimpleCannon cannon in _cannons)
            {
                cannon.Disable();
            }
        }
        
        private void Enable()
        {
            _isActive = true;
            foreach (SimpleCannon cannon in _cannons)
            {
                cannon.Enable();
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