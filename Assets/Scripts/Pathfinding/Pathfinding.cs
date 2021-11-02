using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding
{
    private Node _start;
    private Node _end;
    private List<Node> _list;

    public Pathfinding(List<Node> list)
    {
        _list = list;
    }

    public List<Node> FindPath(Node start, Node end)
    {
        // set all to infinity
        foreach (var node in _list)
        {
            node.f = int.MaxValue;
            node.g = int.MaxValue;
            node.h = int.MaxValue;
            node.last = null;
        }
        
        // set start to f0 g0 h0
        start.f = 0;
        start.g = 0;
        start.h = 0;

        // create & add start to the open set
        var open = new List<Node> { start };
        var closed = new List<Node>();
        var current = start;
        
        // add all obstacles to the closed list
        closed.AddRange(_list.Where(n => n.IsBlocked));
        
        // ERROR check if start or end are inside an obsticle
        if (start.IsBlocked || end.IsBlocked) return null;
        
        // loop until you reach the end
        while (current != end)
        {
            // remove current from the open list 
            open.Remove(current);
            closed.Add(current);
            
            // get neighbours
            var neighbours = GetNeighbours(current);
            if (neighbours == null) return null;
            
            // set neighbour values
            foreach (var n in neighbours)
            {
                // skip if they are in the closed list 
                if (closed.Exists(node => node.x == n.x && node.y == n.y)
                    || open.Exists(node=> node.x == n.x && node.y == n.y))
                {
                    continue;
                }

                // set the distance
                n.h = Vector2.Distance(new Vector2(n.x, n.y), new Vector2(end.x, end.y)); 
                // set the travel cost 
                n.g = 1;
                // set total cost 
                n.f = n.h + n.g;
                // set the last node
                n.last = current;
                // ad to the open list
                open.Add(n);
            }
            
            // get lowest f score
            var lowest = float.MaxValue;
            foreach (var n in open)
            {
                if (n.f < lowest)
                {
                    lowest = n.f;
                    current = n;
                }
            }

            if (open.Count < 1) return null;
        }
        
        // backtrack along the path to make the list
        var path = new List<Node> { end };
        while (current.last != null)
        {
            path.Add(current);
            current = current.last;
        }
            
        // reverse the list and return the path
        path.Reverse();
        return path;
    }

    private List<Node> GetNeighbours(Node current)
    {
        var list = new List<Node>();
        var x = current.x;
        var y = current.y;
        
        var up    = _list.FirstOrDefault(node => x == node.x && (y + 1) == node.y);
        var down  = _list.FirstOrDefault(node => x == node.x && (y - 1) == node.y);
        var left  = _list.FirstOrDefault(node => (x - 1) == node.x && y == node.y);
        var right = _list.FirstOrDefault(node => (x + 1) == node.x && y == node.y);

        AddValid(up, list);
        AddValid(down, list);
        AddValid(left, list);
        AddValid(right, list);

        
        // return list or null if empty
        return list.Count < 1 ? null : list;
    }

    private void AddValid(Node n, List<Node> list)
    {
        if (n == null) return;
        list.Add(n);
    }
}
