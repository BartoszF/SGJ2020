using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if (UNITY_EDITOR) 

[CustomEditor(typeof(PlatformPathfinding))]
public class PlatformPathfindingEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        PlatformPathfinding pathfinding = (PlatformPathfinding)target;
        if (GUILayout.Button("Bidirectionaly connect nodes"))
        {
            pathfinding.BuildPaths();
        }
        
    }
}

#endif

