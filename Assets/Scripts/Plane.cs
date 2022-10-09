using System;
using PathCreation;
using UnityEditor;
using UnityEngine;

public class Plane : TimelineObject
{
    [SerializeField] protected int _currentSceneIndex;
    [SerializeField] protected float _t;
    [SerializeField, NonReorderable, Space] protected PathCreator[] _pathCreators;

    // [SerializeField, NonReorderable] private MotionInfo[] _motionInfos;

   // private MotionInfo Info => _motionInfos[_currentSceneIndex];

   private void LateUpdate()
   {
       UpdateTransform();
   }

   protected virtual void UpdateTransform()
    {
        transform.position = _pathCreators[_currentSceneIndex].path.GetPointAtTime(_t);
        transform.rotation = _pathCreators[_currentSceneIndex].path.GetRotation(_t);
    }

    protected override void SceneView_DuringSceneGui(SceneView obj)
    {
        base.SceneView_DuringSceneGui(obj);
        
        UpdateTransform();
    }
}