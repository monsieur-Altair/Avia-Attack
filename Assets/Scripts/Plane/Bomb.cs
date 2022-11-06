using PathCreation;
using UnityEditor;
using UnityEngine;

public class Bomb : TimelineObject
{
    [SerializeField] private int _currentIndex;
    [SerializeField] private float _t;
    
    [SerializeField] private PathCreator[] _pathCreators;
    [SerializeField] private bool _isScriptActive;
    
    private void LateUpdate()
    {
        UpdateTransform();
    }

    protected virtual void UpdateTransform()
    {
        if(_isScriptActive==false)
            return;
        
        transform.position = _pathCreators[_currentIndex].path.GetPointAtTime(_t);
        transform.rotation = _pathCreators[_currentIndex].path.GetRotation(_t) * Quaternion.Euler(90, 0, 0);
    }

    protected override void SceneView_DuringSceneGui(SceneView obj)
    {
        base.SceneView_DuringSceneGui(obj);
        
        UpdateTransform();
    }
}
