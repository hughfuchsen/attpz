using System.Collections.Generic;
using UnityEngine;

public class DecisionNode : MonoBehaviour
{
    public enum NodeType
    {
        Top,
        Bottom
    }

    public enum NodeColor
    {
        Red,
        Green,
        Blue
    }

    public NodeType nodeType;
    public NodeColor nodeColor;

    [Header("Transport Link")]
    public DecisionNode linkedNode;

    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        UpdateColour();
    }

    void UpdateColour()
    {
        if (sr == null) return;

        switch (nodeColor)
        {
            case NodeColor.Red:
                sr.color = Color.red;
                break;

            case NodeColor.Green:
                sr.color = Color.green;
                break;

            case NodeColor.Blue:
                sr.color = Color.blue;
                break;
        }
    }

    public void TryTransport(GameObject entity)
    {
        if (linkedNode == null) return;

        entity.transform.position =
            linkedNode.transform.position + new Vector3(-8, 28, 0);
    }
}