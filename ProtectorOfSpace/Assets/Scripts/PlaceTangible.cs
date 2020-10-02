using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

using UnityEngine;

public class PlaceTangible : MonoBehaviour
{
    public GameObject tangibleModel;

    public LayerMask groundLayer;


    public Camera cam;


    GameManager GameManager;

    void Start()
    {
        GameManager = GameManager.instance;
    }

    void Update()
    {
        //transform.position = Vector3.Lerp(transform.position, myposition, 0.75f * Time.deltaTime);
        if (Input.touchCount > 0)
        {

            Touch touch = Input.GetTouch(0);

            if (GameManager.placedTangible)
            {
                GameManager.placedTangible = false;
                return;
            }
            if (touch.phase == TouchPhase.Began && !GameManager.placingTangible)
            {
                GameManager.placingTangible = true;
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 30f));
                Vector3 groundPos = DetectGroundHeight(touchPosition);
                GameManager.objectToPlace = (GameObject)Instantiate(tangibleModel, groundPos, Quaternion.identity);

                //transform.LookAt(new Vector3(touchPosition.x, myposition.y, touchPosition.z));
            }
            if (touch.phase == TouchPhase.Moved)
            {
                if (GameManager.objectToPlace != null)
                {
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 30f));
                    Vector3 groundPos = DetectGroundHeight(touchPosition);

                    GameManager.objectToPlace.transform.position = Vector3.MoveTowards(GameManager.objectToPlace.transform.position, groundPos, 100f);
                }
            }


        }
    }


    public Vector3 DetectGroundHeight(Vector3 position)
    {
        position.y = 100f; //Make sure this value is higher that your tallest bit of ground
        RaycastHit hit = new RaycastHit();


        Ray ray = cam.ScreenPointToRay(Input.GetTouch(0).position);



        if (Physics.Raycast(ray, out hit, 1000, groundLayer))
        {
            Vector3 hitPosition = hit.point;
            return hitPosition;
        }

        //If no hit then you have attempted to measure the height somewhere off the mesh
        Debug.Log("REACH" + position);
        return new Vector3(0f, 0f, 0f);
    }
}
