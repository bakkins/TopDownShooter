using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public int health = 3;
    private Transform player;
    public event Action OnDeath;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            transform.position += (Vector3)dir * speed * Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}
