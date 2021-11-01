using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class NodeGrid : MonoBehaviour
{
    [SerializeField] private int width = 0;
    [SerializeField] private int height = 0;
    [SerializeField] private int xOffset = 0;
    [SerializeField] private int yOffset = 0;
    
    [SerializeField] private List<Node> nodeList = new List<Node>();
    public List<Node> Nodes => nodeList;
    
    void OnEnable()
    {
        for(var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var node = new Node { x = x + xOffset, y = y + yOffset };
                nodeList.Add(node);
            }
        }
    }

    private void OnDisable() => nodeList.Clear();
    
    public Node GetNode(Vector2Int position)
    {
        return nodeList.FirstOrDefault(node => position.x == node.x && position.y == node.y);
    }


    
}
