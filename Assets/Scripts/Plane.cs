using UnityEditor;
using UnityEngine;

public class Plane : TimelineObject
{
    [SerializeField] private int _currentSceneIndex;
    [SerializeField] private MotionInfo[] _motionInfos;

    private MotionInfo Info => _motionInfos[_currentSceneIndex];
    
    private void UpdateTransform()
    {
        transform.position = Info.PathCreator.path.GetPointAtTime(Info.T);
        transform.rotation = Info.PathCreator.path.GetRotation(Info.T);
    }

    protected override void SceneView_DuringSceneGui(SceneView obj)
    {
        base.SceneView_DuringSceneGui(obj);
        
        UpdateTransform();
    }
}