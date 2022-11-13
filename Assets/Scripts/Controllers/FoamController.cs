using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class FoamController : MonoBehaviour
    {
        [SerializeField] private ParticleSystemRenderer _particle;
        [SerializeField] private Transform _targetTf;

        [SerializeField] private float[] _heights;
        [SerializeField] private float[] _minSizes;

        private void Update()
        {
            _particle.minParticleSize = GetSize(_targetTf.position.y);
        }

        private float GetSize(float height)
        {
            int maxIndex = _heights.Length - 1;
            for (int i = 0; i < _heights.Length; i++)
            {
                if (Mathf.Abs(height - _heights[i]) < float.Epsilon || height > _heights[i])
                {
                    maxIndex = i;
                    break;
                }
            }

            int minIndex = Mathf.Clamp(maxIndex - 1, 0, _heights.Length - 1);
            maxIndex = Mathf.Clamp(maxIndex, 0, _heights.Length - 1);

            if (minIndex == maxIndex)
                return _minSizes[minIndex];

            float t = Mathf.InverseLerp(_heights[minIndex], _heights[maxIndex], height);
            var res = Mathf.Lerp(_minSizes[minIndex], _minSizes[maxIndex], t);
            return res;
        }
    }
}