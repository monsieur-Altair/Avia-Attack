using System.Collections.Generic;
using Extensions;
using Pool_And_Particles;
using UnityEngine;

namespace DefaultNamespace
{
    public class SimpleCannon : MonoBehaviour
    {
        [SerializeField, NonReorderable] private List<Transform> _spawnPoints;

        private List<CannonBullet> _bullets;
        private Transform _target;
        private bool _isActive = true;
        private Pool _pool;
        private float _elapsedTime;
        private float _fireFrequency;
        private float _bulletSpeed;
        private float _radius;
        private const int NewBulletAmount = 50;

        public void OnAwake(Pool pool, float frequency, float bulletSpeed, List<CannonBullet> cannonBullets, float radius)
        {
            _radius = radius;
            _pool = pool;
            _bulletSpeed = bulletSpeed;
            _fireFrequency = frequency;
            _bullets = cannonBullets;
        }

        public void Validate(float frequency, float bulletSpeed)
        {
            _bulletSpeed = bulletSpeed;
            _fireFrequency = frequency;
        }

        public void OnSceneSwitched(Transform target)
        {
            _target = target;
            
            for (int i = 0; i < NewBulletAmount; i++)
            {
                CannonBullet bullet = Fire();
                bullet.SetElapsedTime(Random.Range(0, bullet.FlightTime / CannonBullet.FLIGHT_COEFFICIENT));
                bullet.SetPosition();
            }
        }

        private void Update()
        {
            if (_isActive == false)
                return;

            _elapsedTime += Time.deltaTime;
            
            if (_elapsedTime > 1f / _fireFrequency)
            {
                Fire();
                _elapsedTime = 0.0f;
            }
        }

        private CannonBullet Fire()
        {
            Transform randomSpawnPoint = _spawnPoints.GetRandom();
            Vector3 spawnPointPos = randomSpawnPoint.position;
            Vector3 distance = GetDistance(spawnPointPos);
            CannonBullet bullet = _pool.Get(_bullets.GetRandom());
            bullet.SetBaseInfo(_bulletSpeed, distance.normalized,  spawnPointPos, distance.magnitude / _bulletSpeed);
            return bullet;
        }

        private Vector3 GetDistance(Vector3 startPos)
        {
            Vector3 zAxis = (_target.position - startPos).normalized;
            Vector3 negativeXAxis = _target.right;
            Vector3 yAxis = Vector3.Cross(zAxis, negativeXAxis);

            float xOffset = Random.Range(-_radius, _radius);
            float yOffset = Random.Range(-_radius, _radius);

            Vector3 targetPoint = _target.position + xOffset * negativeXAxis + yOffset * yAxis;
            return targetPoint - startPos;
        }
    }
}