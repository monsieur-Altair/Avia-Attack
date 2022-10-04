using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Playables;

public class Plane : MonoBehaviour
{
    [SerializeField] private float _t;
    [SerializeField] private PathCreator _pathCreator;
 
    private void Update()
    {
        UpdateTransform();
    }

    public void UpdateTransform()
    {
        transform.position = _pathCreator.path.GetPointAtTime(_t);
        transform.rotation = _pathCreator.path.GetRotation(_t);
    }
}