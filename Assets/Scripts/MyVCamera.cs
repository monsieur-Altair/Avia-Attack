using Cinemachine;
using UnityEngine;

namespace DefaultNamespace
{
    public class MyVCamera : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _camera;
        
        public void DisableTranspose()
        {
            Debug.LogError("disabling");
            _camera.Follow = null;
        }
    }
}