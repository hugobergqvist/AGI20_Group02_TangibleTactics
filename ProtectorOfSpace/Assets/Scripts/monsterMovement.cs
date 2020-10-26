using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Monster))]
public class monsterMovement : MonoBehaviour
{
    public Vector3 target;
    private Monster monster;

    void Start()
    {
        monster = GetComponent<Monster>();
    }

    void Update()
    {
        target = new Vector3(0, 0, 0);
        float step = monster.speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }



}
