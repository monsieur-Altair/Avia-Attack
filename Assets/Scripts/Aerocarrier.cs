using System;
using DG.Tweening;
using Extensions;
using JetBrains.Annotations;
using UnityEngine;

namespace DefaultNamespace
{
    public class Aerocarrier : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Material _newMat;
        [SerializeField] private int _index = 11;
        [SerializeField] private AudioSource _machineGunSound;
        private int _index1 = -1;
        private void Awake()
        {
            _machineGunSound.loop = true;
            _machineGunSound.DoVolume(0, 0.7f, 2).SetEase(Ease.InExpo);
            
            Sequence sequence = DOTween.Sequence();
            sequence.AppendCallback(() =>
            {
                _machineGunSound.time = 0.0f;
                _machineGunSound.Play();
            });
            sequence.AppendInterval(30f);
            sequence.AppendCallback(() =>
            {
                _machineGunSound.time = 0.0f;
                _machineGunSound.Play();
            });
            sequence.AppendInterval(30f);
            sequence.AppendCallback(() =>
            {
                _machineGunSound.time = 0.0f;
                _machineGunSound.Play();
            });
            
        }

        public void ChangeMat()
        {
            Material[] materials = _meshRenderer.sharedMaterials;
            materials[_index] = _newMat;
            _meshRenderer.sharedMaterials = materials;
        }

        public void OnSceneSwitched()
        {
            _index1++;

            if (_index1 == 5)
            {
                _machineGunSound.enabled = false;
            }
            
            if (_index1 == 6)
            {
                _machineGunSound.enabled = true;
            }

        }
    }
}