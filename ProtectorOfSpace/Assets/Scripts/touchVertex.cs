using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchVertex
{
    public int touchId;
    public Vector2 position;
    public int triangleIndex; 
    public List<touchVertex> touchVectors;

    public touchVertex(int newTouchID, Vector2 pos, int index)
    {
        touchId = newTouchID;
        position = pos;
        triangleIndex = index;
        touchVectors = new List<touchVertex>();
    }
}
