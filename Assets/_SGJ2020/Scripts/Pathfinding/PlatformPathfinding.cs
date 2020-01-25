using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlatformPathfinding : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void BuildPaths()
    {
        var nodes = GameObject.FindObjectsOfType(typeof(JumpNode));
        foreach (JumpNode node in nodes)
        {
            node.BuildPath();
        }
    }

}
