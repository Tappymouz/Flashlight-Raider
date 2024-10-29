using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private float projectileSpeed;

    [SerializeField]
    private Transform offset;

    [SerializeField]
    private float timeBetweenShots;

    private float lastFireTime;


    private void FireBullet()
    {
        GameObject bullet = Instantiate(projectilePrefab, offset.position, transform.rotation);
        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();

        rigidbody.velocity = projectileSpeed * offset.up;

        ProjectileManager.Instance.RegisterProjectile(bullet.transform);
        bullet.GetComponent<Projectile>().OnDestroyCallback = () => ProjectileManager.Instance.UnregisterProjectile(bullet.transform);
    }

    private void OnAttack(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            float timeSinceLastFire = Time.time - lastFireTime;

            if (timeSinceLastFire >= timeBetweenShots)
            {
                FireBullet();
                lastFireTime = Time.time;
            }
        }
    }

}

