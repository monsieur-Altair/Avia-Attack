using Cinemachine;
using UnityEngine;

public class aaa : MonoBehaviour
{
    public CinemachineImpulseSource Source;
    public float Base;
    public float Coeff;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("a");
            Source.GenerateImpulse(Random.insideUnitSphere*Base*Coeff);
        }
    }
}