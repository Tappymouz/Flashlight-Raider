using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Action OnDestroyCallback;

    private void Start()
    {
        Invoke("DestroyProjectile", 3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyMovement>())
        {

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

