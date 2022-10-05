using UnityEditor;
using UnityEngine;

public class TimelineObject : MonoBehaviour
{
    public void Subscribe()
    {
        SceneView.duringSceneGui += SceneView_DuringSceneGui;
        Debug.Log("subs");
    }

    protected virtual void SceneView_DuringSceneGui(SceneView obj)
    {
    }

    public void Unsubscribe()
    {
        SceneView.duringSceneGui -= SceneView_DuringSceneGui;
        Debug.Log("unsubs");
    }
}