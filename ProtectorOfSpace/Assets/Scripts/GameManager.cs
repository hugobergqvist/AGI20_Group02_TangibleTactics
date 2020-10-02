using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool placingTangible = false;

    public bool placedTangible = false;
    public GameObject objectToPlace;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one game manager in scene!");
            return;
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
