using Pool_And_Particles;
using UnityEngine;

namespace DefaultNamespace
{
    public class CannonBullet : PooledBehaviour
    {
        public const float FLIGHT_COEFFICIENT = 1.1f;

        private float _speed;
        
        private Vector3 _direction;
        private Transform _transform;
        private float _elapsedTime;

        public Transform LaunchPoint { get; private set; }

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
            _transform.position = LaunchPoint.position + _speed * _elapsedTime * _direction;
        }

        public void SetBaseInfo(float speed, Vector3 direction, Transform spawnPoint, float flightTime)
        {
            _speed = speed;
            _direction = direction;
            LaunchPoint = spawnPoint;
            
            _transform.forward = direction;
            _transform.position = LaunchPoint.position;

            _elapsedTime = 0.0f;
            FlightTime = flightTime * FLIGHT_COEFFICIENT;
        }

    }
}