using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Test : MonoBehaviour
{
    private Pathfinding _ai = null;
    private NodeGrid _grid = null;
    private List<Node> _path = null;

    private void OnEnable()
    {
        _grid = GetComponent<NodeGrid>();
        _ai = new Pathfinding(_grid.Nodes);
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        Debug.Log("Click");
        // test path
        var start = _grid.GetNode(new Vector2Int(0, 0));
        var end = _grid.GetNode(new Vector2Int(5, 5));
        _path = _ai.FindPath(start, end);
        Debug.Log(_path.Count);

    }

    private void OnDrawGizmos()
    {
        if (_path == null) return;
        DrawGrid();
    }
    
    private void DrawGrid()
    {
        var z = transform.position.z;
        foreach (var n in _path)
        {
            DrawCell(new Vector3(n.x, n.y, z));
        }
    }

    private void DrawCell(Vector3 position)
    {
        var box = new Vector3(.75f, .75f, .25f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(position, box );

    }
}
