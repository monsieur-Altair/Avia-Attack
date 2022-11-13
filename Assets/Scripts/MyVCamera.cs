using System;
using Cinemachine;
using UnityEngine;

namespace DefaultNamespace
{
    public class MyVCamera : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField] private CinemachineVirtualCamera _camera;

        public static event Action TransposerDisabled = delegate {  }; 
        
        public void DisableTranspose()
        {
            Debug.LogError("disabling");
            _camera.Follow = null;
            TransposerDisabled();
        }
    }
}