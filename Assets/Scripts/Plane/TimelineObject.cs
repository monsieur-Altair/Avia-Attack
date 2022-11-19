#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class TimelineObject : MonoBehaviour
{
    
    public void Subscribe()
    {
#if UNITY_EDITOR
        SceneView.duringSceneGui += SceneView_DuringSceneGui;
        Debug.Log("subs");
#endif
    }

#if UNITY_EDITOR

    protected virtual void SceneView_DuringSceneGui(SceneView obj)
    {
    }
#endif
    
    public void Unsubscribe()
    {
#if UNITY_EDITOR

        SceneView.duringSceneGui -= SceneView_DuringSceneGui;
        Debug.Log("unsubs");
#endif

    }
    
}