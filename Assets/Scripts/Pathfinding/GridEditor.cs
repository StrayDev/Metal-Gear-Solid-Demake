using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteAlways]
public class GridEditor : MonoBehaviour
{
    [SerializeField] private NodeGrid grid = default;

    private void OnEnable()
    {
        grid = GetComponent<NodeGrid>();
        SceneView.beforeSceneGui += OnScene;
    }

    private void OnDisable()
    {
        grid = null;
        SceneView.beforeSceneGui -= OnScene;
    }
    
    [SerializeField] private bool showGrid = false;
    [SerializeField] private bool canEdit = false;

    private Plane _plane = new Plane(Vector3.forward, Vector3.zero);  
    private readonly Vector3 _cellBox = new Vector3(1, 1, .01f);
    private readonly Color _red = new Color(1,0,0, .3f);
    private readonly Color _black = new Color(0,0,0, .3f);
    
    private void OnScene(SceneView scene)
    {
        if (!canEdit) return;
        
        var e = Event.current;
        if (e.type != EventType.MouseDown || e.button != 0) return;
        
        var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        _plane.Raycast(ray, out var enter);
        
        var worldPoint = ray.GetPoint(enter);
        var cell = new Vector2Int((int)Mathf.Round(worldPoint.x), (int)Mathf.Round(worldPoint.y));
        
        var node = grid.GetNode(cell);
        if (node == null) return;
       
        node.IsBlocked = !node.IsBlocked;
        
        Event.current.Use();
    }

    private void OnDrawGizmos()
    {
        if (!showGrid) return;
        DrawGrid();
    }
    
    private void DrawGrid()
    {
        var z = transform.position.z;
        foreach (var n in grid.Nodes)
        {
            DrawCell(new Vector3(n.x, n.y, z), n.IsBlocked);
        }
    }

    private void DrawCell(Vector3 position, bool blocked)
    {
        
        Gizmos.color = blocked ? _red: _black;
        Gizmos.DrawCube(position, _cellBox);
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(position + Vector3.back * 0.001f, _cellBox);
    }
}
