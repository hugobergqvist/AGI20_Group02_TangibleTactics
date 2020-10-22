using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public float timeBetweenWaves = 20f;
    private float countDown = 2f;

    public Text waveCountdownText;

    public Text enemyCountText;

    private int waveIndex = 0;

    private int monsterCount = 0;

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

    //Here put the ranges where your object will appear, or put it in the inspector.
    private void SetRanges()
    {
        Min = new Vector3(15, 0, 15); //Random value.
        Max = new Vector3(45, 0, 45); //Another ramdon value, just for the example.
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SetRanges();
    }

    IEnumerator SpawnWave()
    {
        waveIndex++;
        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }
    void Update()
    {
        if (countDown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countDown = timeBetweenWaves;
        }
        countDown -= Time.deltaTime;

        countDown = Mathf.Clamp(countDown, 0f, Mathf.Infinity);
        waveCountdownText.text = string.Format("{0:00.00}", countDown);
        enemyCountText.text = monsterCount.ToString();
    }

    private void SpawnEnemy()
    {
        _xAxis = UnityEngine.Random.Range(Min.x, Max.x) * (Random.Range(0, 2) * 2 - 1);
        _yAxis = 5f;
        _zAxis = UnityEngine.Random.Range(Min.z, Max.z) * (Random.Range(0, 2) * 2 - 1);
        _randomPosition = new Vector3(_xAxis, _yAxis, _zAxis);
        monsterCount++;
        GameObject g = Instantiate(Monster, _randomPosition, Quaternion.identity);
        g.transform.LookAt(new Vector3(0f, 0f, 0f));
    }

    public void DestroyMonster(GameObject monsterToDestroy)
    {
        monsterCount--;
        Destroy(monsterToDestroy);
        return;

    }



}

