using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy took " + damage + " damage!");

        if (health <= 0)
        {
            Die();
        }
    }

    public void ReactToHit()
    {
        // Example reaction, e.g., knockback or flashing effect
        Debug.Log("Enemy reacts to hit!");
    }

    private void Die()
    {
        Debug.Log("Enemy died!");
        // Add death animation, sound, or effect here
        Destroy(gameObject);
    }
}
