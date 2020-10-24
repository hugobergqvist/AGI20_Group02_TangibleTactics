using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public float startSpeed = 2f;
    [HideInInspector]
    public float speed;

    public float startHealth = 100;
    private float health;

    public GameObject deathEffect;

    public int worth = 50;

    [Header("Unity Stuff")]
    public Image healthBar;

    private bool isDead = false;

    private Vector3 deathEffectOffset;

    MonsterSpawner MonsterSpawner;

    void Start()
    {
        speed = startSpeed;
        health = startHealth;
        MonsterSpawner = MonsterSpawner.instance;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        healthBar.fillAmount = health / startHealth;

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("DIE");
        isDead = true;
        deathEffectOffset = new Vector3(0f, 2f, 0f);
        GameObject effect = (GameObject)Instantiate(deathEffect, transform.position + deathEffectOffset, Quaternion.identity);
        Destroy(effect, 5f);
        MonsterSpawner.GetComponent<MonsterSpawner>().monsterCount--;
        Destroy(gameObject);
    }

}