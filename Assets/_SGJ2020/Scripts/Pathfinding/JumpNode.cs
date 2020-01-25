using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class JumpNode : MonoBehaviour
{
    public List<JumpNode> connectedNodes;

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void BuildPath() {
        foreach(JumpNode node in connectedNodes) {
            if(connectedNodes == null) continue;

            node.AddPointIfNotExists(this);
        }
    }

    public void AddPoint(JumpNode node)
    {
        connectedNodes.Add(node);
        node.AddPointIfNotExists(this);
    }

    public void AddPointIfNotExists(JumpNode node)
    {
        if (!connectedNodes.Contains(node))
            connectedNodes.Add(node);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.2f);
        if (connectedNodes == null) return;
        foreach (JumpNode node in connectedNodes)
        {
            if(node == null) continue;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }
}
