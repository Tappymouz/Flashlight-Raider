using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private int maxAmmo = 10;

    [SerializeField]
    private float reloadTime = 2f;

    [SerializeField]
    private TextMeshProUGUI ammoDisplay;

    private int currentAmmo;
    private bool isReloading = false;

    private void Start()
    {
        currentAmmo = maxAmmo; // Initialize with a full magazine
        UpdateAmmoDisplay();
    }

    private void FireBullet()
    {
        if (currentAmmo > 0 && !isReloading)
        {
            GameObject bullet = Instantiate(projectilePrefab, offset.position, transform.rotation);
            Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();

            rigidbody.velocity = projectileSpeed * offset.up;

            ProjectileManager.Instance.RegisterProjectile(bullet.transform);
            bullet.GetComponent<Projectile>().OnDestroyCallback = () => ProjectileManager.Instance.UnregisterProjectile(bullet.transform);

            currentAmmo--;
            UpdateAmmoDisplay();
            AudioManager.Instance.PlaySFX("Gun");
        }
        else if (currentAmmo <= 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    private void OnAttack(InputValue inputValue)
    {
        if (inputValue.isPressed && !isReloading)
        {
            FireBullet();
        }
    }

    private void OnReload(InputValue inputValue)
    {
        if (inputValue.isPressed && !isReloading && currentAmmo != maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Reload complete");
        UpdateAmmoDisplay();
    }

    private void UpdateAmmoDisplay()
    {
        if (ammoDisplay != null)
        {
            ammoDisplay.text = $"Ammo: {currentAmmo}/{maxAmmo}";
        }
    }
}


