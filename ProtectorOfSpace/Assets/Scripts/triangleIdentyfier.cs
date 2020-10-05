using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows;
using UnityEngine.UI;

public class triangleIdentyfier : MonoBehaviour
{
    public List<touchVertex> touches = new List<touchVertex>();
    public List<Vector2> allVectors = new List<Vector2>();
    public List<TriangleObject> allTriangles = new List<TriangleObject>();
    public float lenghtOfBaseCathetus = 500;
    // public GameObject T2;
    // public GameObject T3;

    public GameObject Turret;

    public GameObject Wall;

    public LayerMask groundLayer;
    public Camera cam;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Identyfier");

    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        //Clean up lists if no touches
        if (Input.touchCount == 0)
        {
            touches.Clear();
            allTriangles.Clear();
        }

        //Debug.Log("Triangles in allTriangles: " + allTriangles.Count);

        //Handle removal of touches
        i = 0;
        while (i < Input.touchCount)
        {
            Touch t = Input.GetTouch(i);
            if (t.phase == TouchPhase.Ended)
            {
                //Find the touch and if part of triangle
                touchVertex thisTouch = touches.Find(touchVertex => touchVertex.touchId == t.fingerId);
                TriangleObject thisTriangle = allTriangles.Find(TriangleObject => TriangleObject.type == thisTouch.triangleIndex);

                //If part of triangle, delete that triangle
                if (thisTriangle != null)
                {

                    //temp: send away lable
                    if (thisTriangle.type == 2)
                    {
                        Wall.gameObject.SetActive(false);
                        //T2.transform.eulerAngles = new Vector3(0, 0, 0);
                        //T2.transform.position = new Vector2(0, 2000);
                    }
                    else if (thisTriangle.type == 3)
                    {
                        Turret.gameObject.SetActive(false);
                        //T3.transform.eulerAngles = new Vector3(0, 0, 0);
                        //T3.transform.position = new Vector2(0, 2000);
                    }
                    allTriangles.RemoveAt(allTriangles.IndexOf(thisTriangle));

                    //Find the other vertices of the triangle and remove them from that triangle object
                    List<touchVertex> otherTrignaleTouches = touches.FindAll(touchVertex => touchVertex.triangleIndex == thisTouch.triangleIndex);
                    if (otherTrignaleTouches != null)
                    {
                        for (int v = 0; v < otherTrignaleTouches.Count; v++)
                        {
                            //TriangleIndex = 0 => No triangle
                            touches[v].triangleIndex = 0;
                        }
                    }
                }
                //Remove the touch 
                touches.RemoveAt(touches.IndexOf(thisTouch));

            }
            i++;
        }

        //Registrer all new touches
        i = 0;
        while (i < Input.touchCount)
        {
            Touch t = Input.GetTouch(i);
            if (t.phase == TouchPhase.Began)
            {
                touches.Add(new touchVertex(t.fingerId, t.position, 0));
            }
            i++;
        }

        //Create all possible vectors for vertices thats is not part of triangle
        List<touchVertex> allUnassigned = touches.FindAll(touchVertex => touchVertex.triangleIndex == 0);
        if (allUnassigned.Count > 2 && allTriangles.Count < 3)
        {
            CreateVectors();
        }


        //Create possible triangles
        if (allTriangles.Count < 3)
        {
            CreateTriangles();
        }



        // If touches are moved
        i = 0;
        while (i < Input.touchCount)
        {
            Touch t = Input.GetTouch(i);
            touchVertex thisTouch = touches.Find(touchVertex => touchVertex.touchId == t.fingerId);
            if (t.phase == TouchPhase.Moved)
            {
                thisTouch.position = t.position;
                UpdateTrianglePositions();
                UpdateTriangleAngle();

            }
            i++;
            /*
                TriangleObject one = allTriangles.Find(TriangleObject => TriangleObject.type == 1);
                if(one != null){
                    //Debug.Log(one.trianglePosition);
                    triangleLableOne.transform.position = one.trianglePosition;
                }
                */
        }
    }

    //Update the triangle objects position
    void UpdateTrianglePositions()
    {
        foreach (var triangle in allTriangles)
        {
            List<touchVertex> all = touches.FindAll(touchVertex => touchVertex.triangleIndex == triangle.type);
            if (all.Count == 3)
            {
                triangle.trianglePosition = (all[0].position + all[1].position + all[2].position) / 3;
                triangle.worldPosition = Convert2DPosition(triangle.trianglePosition);
                if (triangle.type == 2)
                {
                    Wall.transform.position = triangle.worldPosition;
                    //T2.transform.position = triangle.worldPosition;
                }
                else if (triangle.type == 3)
                {
                    Turret.transform.position = triangle.worldPosition;
                    //T3.transform.position = triangle.worldPosition;
                }
                //whatType.transform.position = triangle.trianglePosition;
            }
        }
    }

    //Create vectors between all vertices that is not part of a triangle
    void CreateVectors()
    {
        for (int i = 0; i < touches.Count && touches.Count > 1; i++)
        {
            if (touches[i].triangleIndex == 0)
            {
                for (int u = 0; u < touches.Count; u++)
                {
                    if (i != u && possibleVector(touches[i].position, touches[u].position) && touches[u].triangleIndex == 0)
                    {
                        touches[i].touchVectors.Add(touches[u]);
                        //touches.Add(touches[i].position - touches[u].position);
                        Debug.Log("Checking Vecotrs");
                    }
                }
            }
        }
    }

    //Create triangles
    void CreateTriangles()
    {
        List<touchVertex> allUnassigned = touches.FindAll(touchVertex => touchVertex.triangleIndex == 0);
        if (allUnassigned.Count > 2)
        {
            for (int i = 0; i < touches.Count; i++)
            {
                if (touches[i].triangleIndex == 0 && touches[i].touchVectors.Count > 1)
                {
                    CheckAngle(touches[i]);
                    Debug.Log("Checking Angles");
                }

            }
        }
    }

    //Check if the vector is a possible vector (e.g. not to long cathetus)
    bool possibleVector(Vector2 first, Vector2 second)
    {
        Vector2 temp = first - second;
        float lenghtOfVector = temp.magnitude;
        if (lenghtOfVector < lenghtOfBaseCathetus)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Check the angles between possible vectors on a touchVertex
    void CheckAngle(touchVertex vertex)
    {

        //Debug.Log("Checking Angles");
        Vector2 cathetusOne;
        Vector2 cathetusTwo;
        if (vertex.triangleIndex == 0)
        {
            for (int i = 0; i < vertex.touchVectors.Count && Input.touchCount > 2; i++)
            {
                cathetusOne = vertex.position - vertex.touchVectors[i].position;
                for (int u = 0; u < vertex.touchVectors.Count && u != i; u++)
                {
                    cathetusTwo = vertex.position - vertex.touchVectors[u].position;
                    float angle = Vector2.Angle(cathetusOne, cathetusTwo);
                    Debug.Log("Angle" + angle);
                    if (angle < 110 && angle > 78)
                    {
                        Vector2 triPos = GetTrianglePosition(vertex.position, vertex.touchVectors[i].position, vertex.touchVectors[u].position);
                        //Debug.Log("Angle:" + angle);
                        if (cathetusOne.magnitude > cathetusTwo.magnitude)
                        {
                            int type = GetTriangleType(vertex, vertex.touchVectors[i], vertex.touchVectors[u]);
                            //Debug.Log(type);
                            vertex.triangleIndex = type;
                            vertex.touchVectors[i].triangleIndex = type;
                            vertex.touchVectors[u].triangleIndex = type;
                            float globalAngle = GetTriangleAngle(cathetusOne);
                            CreateTriangleObject(
                                triPos,
                                type,
                                globalAngle,
                                vertex,
                                vertex.touchVectors[i],
                                vertex.touchVectors[u]
                                );

                            //CreateTriangleObject(vertex, vertex.touchVectors[i], vertex.touchVectors[u]);
                        }
                        else
                        {
                            int type = GetTriangleType(vertex, vertex.touchVectors[u], vertex.touchVectors[i]);
                            //Debug.Log(type);
                            vertex.triangleIndex = type;
                            vertex.touchVectors[i].triangleIndex = type;
                            vertex.touchVectors[u].triangleIndex = type;
                            float globalAngle = GetTriangleAngle(cathetusTwo);
                            CreateTriangleObject(
                                triPos,
                                type,
                                globalAngle,
                                vertex,
                                vertex.touchVectors[i],
                                vertex.touchVectors[u]);
                            //CreateTriangleObject(vertex, vertex.touchVectors[u], vertex.touchVectors[i]);
                        }
                    }
                    else
                    {
                        //Debug.Log("Incorrect Angle:" + angle);
                        // Debug.Log(angle);
                    }
                    UpdateTrianglePositions();
                    UpdateTriangleAngle();
                }
            }
        }

    }

    void CreateTriangleObject(
        Vector2 trianglePosition,
        int type,
        float globalAngle,
        touchVertex vertexCorner,
        touchVertex vertexOpposite,
        touchVertex vertexAdjecent)
    {

        if (allTriangles.Find(TriangleObject => TriangleObject.type == type) == null)
        {
            Debug.Log("Triangle Created, type: " + type);
            allTriangles.Add(new TriangleObject(
                trianglePosition,
                type,
                globalAngle,
                vertexCorner,
                vertexOpposite,
                vertexAdjecent,
                Convert2DPosition(trianglePosition),
                GetGameObject(type)));



        }
        else
        {
            //Debug.Log("triangle exist");
        }

    }

    float GetTriangleAngle(Vector2 cathetusTwo)
    {
        Vector2 vectorUp = new Vector2(-0.5f, 0.5f);
        float angle = Vector2.SignedAngle(cathetusTwo, vectorUp);
        angle = angle * -1;
        if (angle < 0)
        {
            angle = 360 + angle;
        }
        //Debug.Log("TriAngle: " + angle);
        return angle;
    }

    void UpdateTriangleAngle()
    {
        foreach (var triangle in allTriangles)
        {
            List<touchVertex> all = touches.FindAll(touchVertex => touchVertex.triangleIndex == triangle.type);
            Vector2 cathetusTwo = triangle.opposite.position - triangle.adjecent.position;
            float angle = GetTriangleAngle(cathetusTwo);
            if (all.Count == 3)
            {
                triangle.trianglePosition = (all[0].position + all[1].position + all[2].position) / 3;

                triangle.angle = angle;
                if (triangle.type == 2)
                {
                    Wall.transform.eulerAngles = new Vector3(0, -angle, 0);
                    //T2.transform.eulerAngles = new Vector3(0, 0, angle);
                }
                else if (triangle.type == 3)
                {
                    Turret.transform.eulerAngles = new Vector3(0, -angle, 0);
                    // T3.transform.eulerAngles = new Vector3(0, 0, angle);
                }

            }
        }
    }

    Vector2 GetTrianglePosition(Vector2 corner, Vector2 adjacent, Vector2 opposite)
    {
        return (corner + adjacent + opposite) / 3;
    }
    /*
        void CreateTriangleObject(touchVertex corner, touchVertex adjacent, touchVertex opposite){

            int triangleID = corner.touchId;
            List<touchVertex> triangleVertices = new List<touchVertex>();
            triangleVertices.Add(corner);
            triangleVertices.Add(adjacent);
            triangleVertices.Add(opposite);
            int type = GetTriangleType(corner, adjacent, opposite);
            corner.partOfTriangle = true;
            adjacent.partOfTriangle = true;
            opposite.partOfTriangle = true;

            allTriangles.Add(new TriangleObject(triangleVertices, triangleID, type));
        }
    */
    int GetTriangleType(touchVertex corner, touchVertex adjacent, touchVertex opposite)
    {
        float ratio;

        ratio = (opposite.position - corner.position).magnitude / (adjacent.position - corner.position).magnitude;
        Debug.Log("Ratio: " + ratio);
        if (ratio < 0.70)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }

    Vector3 Convert2DPosition(Vector2 screenPos)
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 40f));
        Vector3 groundPos = DetectGroundHeight(touchPosition, screenPos);
        return groundPos;
    }

    public Vector3 DetectGroundHeight(Vector3 position, Vector2 centerPoint)
    {
        position.y = 100f; //Make sure this value is higher that your tallest bit of ground
        RaycastHit hit = new RaycastHit();


        Ray ray = cam.ScreenPointToRay(centerPoint);



        if (Physics.Raycast(ray, out hit, 1000, groundLayer))
        {
            Vector3 hitPosition = hit.point;
            return hitPosition;
        }

        //If no hit then you have attempted to measure the height somewhere off the mesh
        Debug.Log("REACH" + position);
        return new Vector3(0f, 0f, 0f);
    }


    /*
        Text CreateTriangleLable(){
            Text t = Instantiate(triangleLableOne) as Text;
            t.name = "triangleLable";
            //text.transform.position = getTouchPosition(t.position);
            //t.transform.parent = canvas.transform;
            return t;
        }
        */

    GameObject GetGameObject(int type)
    {
        if (type == 2)
        {
            Wall.gameObject.SetActive(true);
            return Wall;
        }
        else
        {
            Turret.gameObject.SetActive(true);
            return Turret;
        }
    }

}
