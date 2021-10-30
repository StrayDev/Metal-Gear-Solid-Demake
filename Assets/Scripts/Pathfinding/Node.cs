using System;

[Serializable]
public class Node
{
    public bool IsBlocked;
    public int x;
    public int y;

    public float h; // heuristic (guess at the distance)
    public float g; // current shortest distance 
    public float f; // f = g + h

    public Node last = null;

}
