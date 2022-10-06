using System;
using PathCreation;
using UnityEditor;
using UnityEngine;

public class CameraTargetPoint : TimelineObject
{
    [SerializeField] private float _t;
    [SerializeField, NonReorderable, Space] private PathCreator _pathCreator;

    // [SerializeField, NonReorderable] private MotionInfo[] _motionInfos;

    // private MotionInfo Info => _motionInfos[_currentSceneIndex];

    private void Update()
    {
        UpdateTransform();
    }

    private void UpdateTransform()
    {
        transform.position = _pathCreator.path.GetPointAtTime(_t);
        transform.rotation = _pathCreator.path.GetRotation(_t);
    }

    protected override void SceneView_DuringSceneGui(SceneView obj)
    {
        base.SceneView_DuringSceneGui(obj);
        
        UpdateTransform();
    }
}