using System;
using PathCreation;
using UnityEditor;
using UnityEngine;

public class Plane : TimelineObject
{
    [SerializeField] protected int _currentSceneIndex;
    [SerializeField] protected float _t;
    [SerializeField] private Transform _planeModel;
    [SerializeField, NonReorderable, Space] protected PathCreator[] _pathCreators;

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

    public void OnFarSceneStarted()
    {
        _planeModel.rotation = Quaternion.Euler(-60, 0, 180);
    }
    
    public void OnFarSceneEnded()
    {
        _planeModel.rotation = Quaternion.Euler(-90, 0, 180);
    }

}