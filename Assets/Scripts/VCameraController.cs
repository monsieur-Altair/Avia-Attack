using System;
using Cinemachine;
using DG.Tweening;
using Extensions;
using NaughtyAttributes;
using UnityEngine;


namespace DefaultNamespace
{
    public class VCameraController : MonoBehaviour
    {
        [SerializeField] private float _returningDelay;
        [SerializeField] private CinemachineVirtualCamera[] _cameras;
        [ShowNonSerializedField] private int _currentIndex;

        private Vector3 _cachedTransposerDamping;
        private Vector2 _cachedComposerDamping;

        private CinemachineTransposer _cinemachineTransposer;
        private CinemachineComposer _cinemachineComposer;

        private void Awake()
        {
            _currentIndex = -1;
        }

        public void OnSceneSwitched()
        {
            _currentIndex++;

            //Debug.LogError("switched"+_currentIndex);
            
            if(_currentIndex is 1 or 2)
                return;
            
            SetToZero();
            Extensionss.Wait(_returningDelay).OnComplete(ResetToCached);
        }

        private void ResetToCached()
        {
            if (_cinemachineTransposer != null)
            {
                _cinemachineTransposer.m_XDamping = _cachedTransposerDamping.x;
                _cinemachineTransposer.m_YDamping = _cachedTransposerDamping.y;
                _cinemachineTransposer.m_ZDamping = _cachedTransposerDamping.z;
            }

            if (_cinemachineComposer != null)
            {
                _cinemachineComposer.m_HorizontalDamping = _cachedComposerDamping.x;
                _cinemachineComposer.m_VerticalDamping = _cachedComposerDamping.y;
            }
        }

        private void SetToZero()
        {
            CinemachineVirtualCamera camera = _cameras[_currentIndex];
            
            _cinemachineTransposer = camera.GetCinemachineComponent<CinemachineTransposer>();

            if (_cinemachineTransposer != null)
            {
                _cachedTransposerDamping = new Vector3(
                    _cinemachineTransposer.m_XDamping,
                    _cinemachineTransposer.m_YDamping,
                    _cinemachineTransposer.m_ZDamping);
            
                _cinemachineTransposer.m_XDamping = 0.0f;
                _cinemachineTransposer.m_YDamping = 0.0f;
                _cinemachineTransposer.m_ZDamping = 0.0f;
            }
            
            _cinemachineComposer = camera.GetCinemachineComponent<CinemachineComposer>();

            if (_cinemachineComposer != null)
            {
                _cachedComposerDamping = new Vector2(
                    _cinemachineComposer.m_HorizontalDamping,
                    _cinemachineComposer.m_VerticalDamping);
            
                _cinemachineComposer.m_HorizontalDamping = 0.0f;
                _cinemachineComposer.m_VerticalDamping = 0.0f;
            }
        }
    }
}