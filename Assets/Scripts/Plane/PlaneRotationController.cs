using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlaneRotationController : MonoBehaviour
    {
        [SerializeField, NonReorderable] private Plane[] _planes;
        [SerializeField, NonReorderable] private Plane[] _noRotPlanes;
        
        private void Awake()
        {
            Quaternion rot = Quaternion.Euler(-90, 0, 180f);
            
            foreach (Plane plane in _planes)
            {
                plane.SetPlaneRot(rot);
                plane.OnAwake();
            }

            foreach (Plane plane in _noRotPlanes)
            {
                plane.OnAwake();
            }
            
            StartCoroutine(WaitAndDo());
        }

        private IEnumerator WaitAndDo()
        {
            yield return new WaitForSeconds(63.5f+2f);
            SetNewRotation();
        }

        private void SetNewRotation()
        {
            Debug.LogError("set new rot");
            Quaternion rot = Quaternion.Euler(-76.8f, 0, 180f);

            foreach (Plane plane in _planes)
            {
                plane.SetPlaneRot(rot);
            }
        }
    }
}