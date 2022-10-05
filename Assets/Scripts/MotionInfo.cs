using System;
using PathCreation;
using UnityEngine;

[Serializable]
public class MotionInfo
{
    [SerializeField] private float _t;
    [SerializeField] private PathCreator _pathCreator;

    public float T => _t;
    public PathCreator PathCreator => _pathCreator;
}