using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Action OnDestroyCallback;

    private void Start()
    {
        Invoke("DestroyProjectile", 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyMovement enemy = collision.GetComponent<EnemyMovement>();
        if (enemy != null)
        {
            enemy.SetDeadFlag();
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

