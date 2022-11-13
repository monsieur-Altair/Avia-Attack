using System;
using System.Collections.Generic;
using DG.Tweening;
using Extensions;
using Pool_And_Particles;
using UnityEngine;

namespace DefaultNamespace
{
    public class ParticleController : MonoBehaviour
    {
        [SerializeField] private PooledParticleSystem _baseExplosion;
        [SerializeField] private Transform _leftBotNearPoint;
        [SerializeField] private Transform _rightTopFarPoint;
        [SerializeField] private Transform _deathZoneCentrePoint;
        [SerializeField] private float _deathZoneRadius;
        [SerializeField] private float _particleSpawnInterval;
        [SerializeField] private int _startExplosionAmount;
        [SerializeField] private int _startExplosionPooledAmount;
        [SerializeField] private Transform _staticParent;
        [SerializeField] private float _rad = 10f;
        [Space, SerializeField] private ParticleSystem _waterSplashPS;
        [Space, SerializeField] private List<ParticleSystem> _lastExplosionsPS;
        
        private AudioSource[] _audioSource;
        
        private float _localBot;
        private float _localTop;
        private float _localFar;
        private float _localLeft;
        private float _localNear;
        private float _localRight;
        
        private float _elapsedTime;
        private Pool _pool;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;

            Vector3 lbp = _leftBotNearPoint.position;
            Vector3 rtf = _rightTopFarPoint.position;
            
            Gizmos.DrawSphere(lbp, _rad);
            Gizmos.DrawSphere(rtf, _rad);
            
            Vector3 p1 = lbp;
            Vector3 p2 = lbp.With(z: rtf.z);
            Vector3 p3 = lbp.With(x: rtf.x, z: rtf.z);
            Vector3 p4 = lbp.With(x: rtf.x);
            Vector3 p5 = rtf;
            Vector3 p6 = rtf.With(z: lbp.z);
            Vector3 p7 = rtf.With(x: lbp.x, z: lbp.z);
            Vector3 p8 = rtf.With(x: lbp.x);
            
            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, p3);
            Gizmos.DrawLine(p3, p4);
            Gizmos.DrawLine(p4, p1);
            
            Gizmos.DrawLine(p5, p6);
            Gizmos.DrawLine(p6, p7);
            Gizmos.DrawLine(p7, p8);
            Gizmos.DrawLine(p8, p5);
            
            Gizmos.DrawLine(p1, p7);
            Gizmos.DrawLine(p2, p8);
            Gizmos.DrawLine(p3, p5);
            Gizmos.DrawLine(p4, p6);
        }
#endif

        public void OnAwake(Pool pool)
        {
            Vector3 localPosBNP = _leftBotNearPoint.localPosition;
            Vector3 localPosRFP = _rightTopFarPoint.localPosition;
            
            _localLeft   = localPosBNP.x;
            _localRight  = localPosRFP.x;
            _localTop    = localPosRFP.y;
            _localBot    = localPosBNP.y;
            _localFar    = localPosRFP.z;
            _localNear   = localPosBNP.z;

            _elapsedTime = 0.0f;

            _pool = pool;
            
            _pool.Prepare(_baseExplosion, _startExplosionPooledAmount);

            for (int i = 0; i < _startExplosionAmount; i++)
            {
                SpawnExplosion();
            }

            _audioSource = GetComponents<AudioSource>();
        }

        public void OnLastExploded()
        {
            if (_lastExplosionsPS != null)
            {
                float[] intervals = {0.0f, 0.3f, 1.2f};
                for (int i = 0; i < _lastExplosionsPS.Count; i++)
                {
                    ParticleSystem ps = _lastExplosionsPS[i];
                    int i1 = i;
                    Extensionss.Wait(intervals[i])
                        .OnComplete(() =>
                        {
                            _audioSource[i1].Play();
                            ps.Play(true);
                        });
                }
            }
        }
        
        public void OnWaterSplashed()
        {
            if (_waterSplashPS != null)
            {
                _waterSplashPS.transform.localPosition = Vector3.zero;
                _waterSplashPS.Play(true);
                _waterSplashPS.transform.SetParent(null);
            }
        }
        
        private void Update()
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _particleSpawnInterval)
            {
                SpawnExplosion();
                _elapsedTime = 0.0f;
            }
        }

        private void SpawnExplosion()
        {
            Vector3? pos = GetLocalPos();
            
            if (pos == null)
                return;

            PooledParticleSystem particle = _pool.Get(_baseExplosion, parent: transform, localPosition: pos);
            particle.Transform.parent = _staticParent;
        }

        private Vector3? GetLocalPos()
        {
            float localX = UnityEngine.Random.Range(_localLeft, _localRight);
            float localY = UnityEngine.Random.Range(_localBot, _localTop);
            float localZ = UnityEngine.Random.Range(_localNear, _localFar);

            Vector3 resultPos = new (localX, localY, localZ);
            Vector3 localDeathZoneCenter = transform.InverseTransformPoint(_deathZoneCentrePoint.position);

            if (Vector3.Distance(localDeathZoneCenter, resultPos) < _deathZoneRadius)
                return null;

            return resultPos;
        }
            
    }
}