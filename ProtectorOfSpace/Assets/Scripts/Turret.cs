﻿using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{

    private Transform target;

    private AudioSource audioSource;
    [Header("Attributes")]
    public float range = 15f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 10f;

    public GameObject bulletPrefab;
    public Transform firePoint;

    public Transform LineOfSightChecker;


    // Start is called before the first frame update
    void Start()
    {
        // Makes the script UpdateTarget run 2 times a second. 
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        audioSource = GetComponent<AudioSource>();
    }

    void UpdateTarget()
    {
        // store all enemies in a gameobject array
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        // Target lock on
        if (CheckIfFreeSight())
        {
            Vector3 dir = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }
    }


    bool CheckIfFreeSight()
    {

        Vector3 dir = (target.position - LineOfSightChecker.position).normalized;
        RaycastHit hit = new RaycastHit();
        Ray ray = new Ray(LineOfSightChecker.position, dir);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Enemy")
            {
                return true;
            }
            else
            {
                // Another object is in the way 
                return false;
            }

        }
        else
        {
            // No object is in line of sight. 
            return false;
        }

    }
    void Shoot()
    {

        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        audioSource.Play();
        if (bullet != null)
            bullet.Seek(target);

    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
