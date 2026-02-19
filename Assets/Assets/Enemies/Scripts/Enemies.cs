using System;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public float speed = 2f;
    public float minX = 4.14f;
    public float maxX = -4.34f;

    private bool movingRight = true;

    public int health = 1;

    private EnemiesSpawner spawner;

    void Start()
    {
        spawner = FindObjectOfType<EnemiesSpawner>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float movement = speed * Time.deltaTime;
        Vector3 direction = movingRight ? Vector3.right : Vector3.left;

        transform.Translate(direction * movement);
        
        print(movingRight);

        if (transform.position.x > minX)
            movingRight = false;
        else if (transform.position.x < maxX)
            movingRight = true;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        spawner.EnemyDied();
        Destroy(gameObject);
    }
}

