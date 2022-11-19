using UnityEngine;

public static class MenuItem
{
#if UNITY_EDITOR

    [UnityEditor.MenuItem("Max Tools/Subscribe Objects")]
    private static void SubscribeObjects()
    {
        TimelineObject[] obj = Object.FindObjectsOfType<TimelineObject>();
        foreach (TimelineObject timelineObject in obj)
        {
            timelineObject.Subscribe();
        }
    }
    
    [UnityEditor.MenuItem("Max Tools/Unsubscribe Objects")]
    private static void UnsubscribeObjects()
    {
        TimelineObject[] obj = Object.FindObjectsOfType<TimelineObject>();
        foreach (TimelineObject timelineObject in obj)
        {
            timelineObject.Unsubscribe();
        }
    }
#endif
}