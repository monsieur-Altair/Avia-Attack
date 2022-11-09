using UnityEngine;

namespace DefaultNamespace
{
    public class Aerocarrier : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Material _newMat;
        [SerializeField] private int _index = 11;

        public void ChangeMat()
        {
            Material[] materials = _meshRenderer.sharedMaterials;
            materials[_index] = _newMat;
            _meshRenderer.sharedMaterials = materials;
        }
    }
}