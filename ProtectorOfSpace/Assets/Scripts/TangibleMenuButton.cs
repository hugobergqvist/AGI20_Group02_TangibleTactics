using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // this namespace makes the magic, tho

public class TangibleMenuButton : MonoBehaviour
{


    GameManager GameManager;

    void Start()
    {
        GameManager = GameManager.instance;
    }

    public GameObject button;
    public GameObject targetModel;
    [SerializeField] UnityEvent anEvent; // puts an easy to setup event in the inspector and anEvent references it so you can .Invoke() it

    // This captures a click as long as you have a collider, even if it's set to just be a trigger, and nothing blocking it.
    // However, it will still capture even if a Gui button is on top of it, so make sure you aren't checking this while also checking Gui
    // Only other colliders block and it's not as manageable as Canvas Groups.

    private void OnMouseDown()
    {

        PlaceCurrentTangible();
        return;

        //anEvent.Invoke(); // Triggers the events you have setup in the inspector
    }

    // This is the first method the event is setup to do, the second audio part needed no script to just do a one shot effect, thanks to the event system.
    // You just set up the Event in the inspector for easy peasy, but the UnityEvent could also be coded the same way if needed.
    public void EventClick() // methods have to be public void to be used by UnityEvents, they can't really return anything either, as far as I know... At least I don't know how an event will capture the return...
    {
        return;
        Debug.Log("Which also triggered this method as a UnityEvent!");
    }



    public void PlaceCurrentTangible()
    {
        if (GameManager.placingTangible)
        {
            GameObject currentGO;
            if (button.name == "OptionWall")
            {
                currentGO = GameManager.objectToPlace.transform.GetChild(0).gameObject;
                currentGO.SetActive(true);
                currentGO.GetComponent<Turret>().enabled = true;

                GameManager.objectToPlace.transform.GetChild(1).gameObject.SetActive(false);
                GameManager.objectToPlace.transform.GetChild(2).gameObject.SetActive(false);
            }
            else if (button.name == "OptionDecoy")
            {
                currentGO = GameManager.objectToPlace.transform.GetChild(2).gameObject;
                currentGO.SetActive(true);
                currentGO.GetComponent<Turret>().enabled = true;
                GameManager.objectToPlace.transform.GetChild(1).gameObject.SetActive(false);
                GameManager.objectToPlace.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                currentGO = GameManager.objectToPlace.transform.GetChild(1).gameObject;
                currentGO.GetComponent<Turret>().enabled = true;
            }

            GameManager.objectToPlace.transform.GetChild(3).gameObject.SetActive(false);
            GameManager.objectToPlace = null;
            GameManager.placingTangible = false;
            GameManager.placedTangible = true;

        }
    }
}