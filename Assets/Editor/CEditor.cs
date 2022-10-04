using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Plane))]
    public class CEditor : UnityEditor.Editor
    {
        private Plane _target;

        public Plane Target
        {
            get 
            {
                if (_target is null)
                {
                    _target = (Plane) target;
                }
                return _target;
            }
        }

        
        
        // private void OnSceneGUI()
        // {
        //     Target.UpdateTransform();
        // }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
        }
    }
}