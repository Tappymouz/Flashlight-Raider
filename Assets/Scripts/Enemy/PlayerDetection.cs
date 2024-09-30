using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [HideInInspector]
    public bool playerDetect { get; private set; }
    public Vector2 directionPlayer { get; private set; }

    [SerializeField]
    private float playerDistance;
    private Transform player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerMove>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerDetection();
    }

    private void CheckPlayerDetection()
    {
        Vector2 enemyToPlayerVector = player.position - transform.position;
        directionPlayer = enemyToPlayerVector.normalized;

        float distanceToPlayer = enemyToPlayerVector.magnitude;

        if (distanceToPlayer <= playerDistance)
        {
            playerDetect = true;
        }
        else
        {
            playerDetect = false;
        }
    }
}
