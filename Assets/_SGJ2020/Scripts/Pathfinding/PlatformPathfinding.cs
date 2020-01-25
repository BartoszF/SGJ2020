using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlatformPathfinding : MonoBehaviour
{
    public List<JumpNode> nodes = new List<JumpNode>();
    void Start()
    {
        BuildPaths();
    }

    public void BuildPaths()
    {
        var newNodes = FindObjectsOfType(typeof(JumpNode));
        foreach (JumpNode node in newNodes)
        {
            nodes.Add(node);
            node.BuildPath();
        }
    }
}
