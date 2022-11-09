using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using Pool_And_Particles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class SimpleCannon : MonoBehaviour
    {
        [SerializeField, NonReorderable] private List<Transform> _spawnPoints;
        [Space, SerializeField] private PFXParams _explosionPS;
        [SerializeField] private Vector2 _piCoeff;
        
        private List<CannonBullet> _bullets;
        private Transform _target;
        private Pool _pool;
        private float _elapsedTime;
        private bool _useCustom;
        
        private float FireFrequency => _fireFrequencyCoefficient * _fireFrequencyConst;
        private float BulletSpeed => _bulletSpeedCoefficient * _bulletSpeedConst;
        private float MinRadius => _minRadiusConst * _minRadiusCoefficient;
        private float MaxRadius => _maxRadiusConst * _maxRadiusCoefficient;
        
        private float _fireFrequencyConst;
        private float _bulletSpeedConst;
        private float _minRadiusConst;
        private float _maxRadiusConst;
        
        private float _fireFrequencyCoefficient;
        private float _bulletSpeedCoefficient;
        private float _minRadiusCoefficient;
        private float _maxRadiusCoefficient;
        
        private Vector3 zAxis;
        private Vector3 negativeXAxis;
        private Vector3 yAxis;
        private Vector3 _startPos;
        
        public static int NewBulletAmount = 50;
        private bool _isActive = true;
        private float _flightTimeCoefficient;
        private bool _usePFX;
        private Transform _parent;

        public void OnAwake(Pool pool, float frequency, float bulletSpeed, List<CannonBullet> cannonBullets, 
            float minRadius, float maxRadius)
        {
            _pool = pool;
            _bullets = cannonBullets;

            _minRadiusConst = minRadius;
            _maxRadiusConst = maxRadius;
            _bulletSpeedConst = bulletSpeed;
            _fireFrequencyConst = frequency;
            
            _fireFrequencyCoefficient = 1.0f;
            _bulletSpeedCoefficient = 1.0f;
            _minRadiusCoefficient = 1.0f;
            _maxRadiusCoefficient = 1.0f;
            _usePFX = false;
            _parent = transform.parent.parent;
        }
        
        public void OnSceneSwitched(Transform target, float minRadCoeff, float maxRadCoeff, float freqCoeff, 
            float speedCoeff, float flightCoefficient, bool useExplosionPFX, int frameCount = 1)
        {
            _target = target;

            _minRadiusCoefficient = minRadCoeff;
            _maxRadiusCoefficient = maxRadCoeff;
            _fireFrequencyCoefficient = freqCoeff;
            _bulletSpeedCoefficient = speedCoeff;
            _flightTimeCoefficient = flightCoefficient;
            _usePFX = useExplosionPFX;
            
            StartCoroutine(WaitAndDo(frameCount));
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }
        
        public void Disable()
        {
            _isActive = false;
        }
        public void Enable()
        {
            _isActive = true;
        }

        private void Update()
        {
            if (_isActive == false)
                return;

            _elapsedTime += Time.deltaTime;
            
            if (_elapsedTime > 1f / FireFrequency)
            {
                var bullet = Fire();
                if (_usePFX)
                {
                    _explosionPS.Get(bullet.LaunchPoint, _pool);
                }
                _elapsedTime = 0.0f;
            }
        }

        private IEnumerator WaitAndDo(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return null;
            }
            
            for (int i = 0; i < NewBulletAmount; i++)
            {
                CannonBullet bullet = Fire();
                bullet.SetElapsedTime(Random.Range(0, bullet.FlightTime / CannonBullet.FLIGHT_COEFFICIENT));
                bullet.SetPosition();
            }
        }

        private CannonBullet Fire()
        {
            Transform randomSpawnPoint = _spawnPoints.GetRandom();
            Vector3 spawnPointPos = randomSpawnPoint.position;
            Vector3 distance = GetDistance(spawnPointPos);
            CannonBullet bullet = _pool.Get(_bullets.GetRandom());
            bullet.SetBaseInfo(BulletSpeed, distance.normalized,  randomSpawnPoint, 
                distance.magnitude / BulletSpeed * _flightTimeCoefficient);

            return bullet;
        }

        private Vector3 GetDistance(Vector3 startPos)
        {
            _startPos = startPos;
            zAxis = (_target.position - _parent.position).normalized;
            negativeXAxis = Vector3.Cross(zAxis, Vector3.up).normalized;
            yAxis = Vector3.Cross(negativeXAxis,zAxis).normalized;
            negativeXAxis = Vector3.Cross(zAxis, yAxis).normalized;

            Vector2 off = GetOffset();
            Vector3 targetPoint = _target.position + off.x * negativeXAxis + off.y * yAxis;
            return targetPoint - startPos;
        }

        private Vector2 GetOffset()
        {
            return PolarToCartesian(
                Random.Range(MinRadius, MaxRadius), 
                Random.Range(Mathf.PI * _piCoeff.x, Mathf.PI * _piCoeff.y));
        }

        private static Vector2 PolarToCartesian(float radius, float angle)
        {
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);
            return new Vector2(x, y);
        }

        private void OnDrawGizmos()
        {
            if(_target==null)
                return;
            
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_target.position, _startPos);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_target.position, _target.position + 7f * negativeXAxis);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_target.position, _target.position + 7f * yAxis);
        }
    }
}