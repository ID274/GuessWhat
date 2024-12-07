using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour, INode
{
    protected void Awake()
    {
        SetName();
        Debug.Log($"Node {name} created");
    }
    public virtual void SetName()
    {
        gameObject.name = "Node";
    }
}
