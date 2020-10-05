using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float bulletSpeed = 70f;
    public GameObject impactEffect;

    GameManager GameManager;
    MonsterSpawner MonsterSpawner;


    void Start()
    {

        MonsterSpawner = MonsterSpawner.instance;

    }



    // Update is called once per frame
    void Update()
    {

        if (Mathf.Abs(transform.position.x) > 60 || Mathf.Abs(transform.position.z) > 60)
        {
            Destroy(gameObject);
        }

        Vector3 dir = transform.forward;
        float distanceThisFrame = bulletSpeed * Time.deltaTime;
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            HitTarget(collision.gameObject);

        }
    }




    void HitTarget(GameObject targetHit)
    {
        GameObject effectInst = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectInst, 2f);
        MonsterSpawner.GetComponent<MonsterSpawner>().DestroyMonster(targetHit);
        Destroy(gameObject);

    }
}
