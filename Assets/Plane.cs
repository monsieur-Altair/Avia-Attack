using PathCreation;
using UnityEditor;
using UnityEngine;

public class Plane : TimelineObject
{
    [SerializeField] private float _t;
    [SerializeField] private PathCreator _pathCreator;
 
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