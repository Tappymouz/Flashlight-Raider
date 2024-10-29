using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager Instance { get; private set; }

    public List<Transform> projectiles = new List<Transform>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterProjectile(Transform projectile)
    {
        projectiles.Add(projectile);
    }

    public void UnregisterProjectile(Transform projectile)
    {
        projectiles.Remove(projectile);
    }

    public List<Transform> GetProjectiles()
    {
        return projectiles;
    }
}

