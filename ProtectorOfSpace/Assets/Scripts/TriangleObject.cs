using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleObject
{
    public touchVertex corner;
    public touchVertex opposite;
    public touchVertex adjecent;
    public List<touchVertex> vertices;

    public Vector2 cathetusOne;
    public Vector2 cathetusTwo;
    public Vector2 hypotenuse;

    public Vector3 worldPosition;

    public GameObject currentGameOb;

    public float angle;
    public int type;
    public Vector2 trianglePosition;




    public TriangleObject(
        Vector2 pos,
        int triangleType,
        float globalAngle,
        touchVertex vertexCorner,
        touchVertex vertexOpposite,
        touchVertex vertexAdjecent,
        Vector3 worldPos,
        GameObject gameOb)
    {
        type = triangleType;
        trianglePosition = pos;
        angle = globalAngle;
        corner = vertexCorner;
        opposite = vertexOpposite;
        adjecent = vertexAdjecent;
        worldPosition = worldPos;
        currentGameOb = gameOb;


    }
}
