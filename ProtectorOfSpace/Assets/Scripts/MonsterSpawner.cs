﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private Vector3 Min;
    private Vector3 Max;
    private float _xAxis;
    private float _yAxis;
    private float _zAxis; //If you need this, use it
    private Vector3 _randomPosition;
    public bool _canInstantiate;
    public static MonsterSpawner instance;
    private List<GameObject> unityGameObjects = new List<GameObject>();
    public GameObject Monster;


    private AudioSource audioSource;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one Monster Spawner in scene!");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SetRanges();
    }
    void Update()
    {
        _xAxis = UnityEngine.Random.Range(Min.x, Max.x) * (Random.Range(0, 2) * 2 - 1);
        _yAxis = 5f;
        _zAxis = UnityEngine.Random.Range(Min.z, Max.z) * (Random.Range(0, 2) * 2 - 1);
        _randomPosition = new Vector3(_xAxis, _yAxis, _zAxis);

        if (unityGameObjects.Count < 20)
        {
            InstantiateRandomObjects();
        }
    }
    //Here put the ranges where your object will appear, or put it in the inspector.
    private void SetRanges()
    {
        Min = new Vector3(15, 0, 15); //Random value.
        Max = new Vector3(45, 0, 45); //Another ramdon value, just for the example.
    }
    private void InstantiateRandomObjects()
    {
        if (_canInstantiate)
        {
            GameObject g = Instantiate(Monster, _randomPosition, Quaternion.identity);
            g.transform.LookAt(new Vector3(0f, 0f, 0f));
            unityGameObjects.Add(g);
        }

    }

    public void DestroyMonster(GameObject monsterToDestroy)
    {
        for (int i = 0; i < unityGameObjects.Count; i++)
        {
            // Check if list at index of i is equal to destroyedObject / list is containing destroyedObject
            if (unityGameObjects[i] == monsterToDestroy)
            {
                audioSource.Play();
                unityGameObjects.Remove(unityGameObjects[i]);
                Destroy(monsterToDestroy);
                return;
            }
        }



    }
}