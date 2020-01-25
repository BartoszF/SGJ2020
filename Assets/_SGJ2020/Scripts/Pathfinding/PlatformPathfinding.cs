using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlatformPathfinding : MonoBehaviour
{
    public static List<JumpNode> nodes = new List<JumpNode>();
    void Start()
    {
        BuildPaths();
    }

    void Update()
    {

    }

    public void BuildPaths()
    {
        var nodes = GameObject.FindObjectsOfType(typeof(JumpNode));
        foreach (JumpNode node in nodes)
        {
            PlatformPathfinding.nodes.Add(node);
            node.BuildPath();
            
        }
    }

}
