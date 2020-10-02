using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TangibleMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject monster;
    void Start()
    {
        Physics.IgnoreCollision(monster.GetComponent<Collider>(), GetComponent<Collider>());
    }


}
