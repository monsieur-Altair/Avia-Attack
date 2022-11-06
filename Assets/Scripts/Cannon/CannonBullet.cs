using Pool_And_Particles;
using UnityEngine;

namespace DefaultNamespace
{
    public class CannonBullet : PooledBehaviour
    {
        public const float FLIGHT_COEFFICIENT = 1.1f;

        private float _speed;
        
        private Vector3 _direction;
        private Vector3 _launchPoint;
        private Transform _transform;
        private float _elapsedTime;

        public float FlightTime { get; private set; }

        public void Awake()
        {
            _transform = transform;
        }
        
        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            
            if (_elapsedTime > FlightTime)
            {
                _pool.TryFree(this);
                return;
            }

            SetPosition();
        }

        public void SetElapsedTime(float time)
        {
            _elapsedTime = time;
        }
        
        public void SetPosition()
        {
            _transform.position = _launchPoint + _speed * _elapsedTime * _direction;
        }

        public void SetBaseInfo(float speed, Vector3 direction, Vector3 spawnPoint, float flightTime)
        {
            _speed = speed;
            _direction = direction;
            _launchPoint = spawnPoint;
            
            _transform.forward = direction;
            _transform.position = _launchPoint;

            _elapsedTime = 0.0f;
            FlightTime = flightTime * FLIGHT_COEFFICIENT;
        }

    }
}