using System.Collections;
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

        [SerializeField] private Material[] _dissolveMats;
        [SerializeField] private float _dissolveDur;
        [SerializeField] private float _cutoffValue = 0;
        [SerializeField] private ParticleSystem _nuclearExplosion;
        [SerializeField] private ShakeController _shakeController;
        [SerializeField] private Plane _dissolvedPlane;
        [SerializeField] private AudioController _audioController;

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

        [UsedImplicitly]
        public void OnNuclear()
        {
            ChangeToDissolve();
            _nuclearExplosion.Play(true);
            _shakeController.OnNuclear();
            _audioController.OnNuclear();
        }

        private void ChangeToDissolve()
        {
            Material[] materials = _meshRenderer.sharedMaterials;

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = _dissolveMats[i];
            }
            _meshRenderer.sharedMaterials = materials;

            if (_dissolvedPlane != null)
            {
                _dissolvedPlane.ChangeToDissolve();
            }
            
            StartCoroutine(Changing());
        }

        private IEnumerator Changing()
        {
            float elapsed = 0.0f;
            while (elapsed<20f)
            {
                foreach (Material mat in _dissolveMats)
                {
                    mat.SetFloat("_cutoff", _cutoffValue);
                }
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        [UsedImplicitly]
        public void ChangeMat()
        {
            Material[] materials = _meshRenderer.sharedMaterials;
            materials[_index] = _newMat;
            _meshRenderer.sharedMaterials = materials;
        }

        public void OnSceneSwitched()
        {
            _index1++;
            //Debug.Log(_index1);

            if (_index1 is 5 or 20)
            {
                Debug.Log("dis");
                _machineGunSound.enabled = false;
            }
            
            if (_index1 == 6)
            {
                Debug.Log("en");
                _machineGunSound.enabled = true;
            }

        }
    }
}