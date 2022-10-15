using PathCreation;
using UnityEditor;
using UnityEngine;

public class Bomb : TimelineObject
{
    [SerializeField] private float _t;
    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private bool _isScriptActive;
    
    private void LateUpdate()
    {
        UpdateTransform();
    }

    protected virtual void UpdateTransform()
    {
        if(_isScriptActive==false)
            return;
        
        transform.position = _pathCreator.path.GetPointAtTime(_t);
        transform.rotation =  _pathCreator.path.GetRotation(_t)*Quaternion.Euler(90, 0, 0);
    }

    protected override void SceneView_DuringSceneGui(SceneView obj)
    {
        base.SceneView_DuringSceneGui(obj);
        
        UpdateTransform();
    }
}
