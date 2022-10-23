using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class mover : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private void Update()
        {
            transform.position += Vector3.forward * _speed * Time.deltaTime;
        }
    }
}