using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemiesDamage : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Transform target;
    private Vector2 moveDirection;  // Corrected 'moveDirection'

    // Health values
    public float health = 3f;
    public float maxHealth = 3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()  // Fixed typo in 'Start'
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        health = maxHealth;
    }

    private void Update()
    {
        if (target != null)  // Ensure the target exists
        {
            Vector3 direction = (target.position - transform.position).normalized;  // Use UnityEngine.Vector3
            moveDirection = new Vector2(direction.x, direction.y);  // Convert to Vector2
        }
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            rb.velocity = moveDirection * moveSpeed;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);  // Fixed missing semicolon
        }
    }
}
