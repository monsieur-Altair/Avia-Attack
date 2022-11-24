using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class MatCannonSwitcher : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;
        [SerializeField] private Material _dissolveMaterial;
        
        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void SwitchToDissolve()
        {
            _meshRenderer.sharedMaterial = _dissolveMaterial;
            
        }
        
    }
}