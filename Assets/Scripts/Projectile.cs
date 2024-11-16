using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Action OnDestroyCallback;
    public LayerMask wallLayers; // Add a LayerMask for walls

    private void Start()
    {
        // Destroy the projectile after 2 seconds if it hasn't collided
        Invoke(nameof(DestroyProjectile), 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the projectile hit an enemy
        EnemyMovement enemy = collision.GetComponent<EnemyMovement>();
        if (enemy != null)
        {
            enemy.SetDeadFlag();
            DestroyProjectile(); // Destroy the projectile after hitting an enemy
            return;
        }

        // Check if the projectile hit a wall
        if (((1 << collision.gameObject.layer) & wallLayers) != 0)
        {
            Debug.Log("Projectile hit a wall and will be destroyed.");
            DestroyProjectile(); // Destroy the projectile after hitting a wall
            return;
        }
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        OnDestroyCallback?.Invoke();
    }
}
