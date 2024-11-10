using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyMovement : MonoBehaviour
{
    public enum EnemyType { Wandering, Hiding }
    public EnemyType enemyType;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float idleDuration;
    [SerializeField]
    private float moveDuration;
    [SerializeField]
    private float stuckDetectionRadius = 0.5f;
    [SerializeField]
    private float stuckDetectionTime = 1f;

    [Header("Death Settings")]
    [SerializeField] private Sprite fallenWanderingSprite;
    [SerializeField] private Sprite fallenHidingSprite;


    private Rigidbody2D rb;
    private PlayerDetection pdController;
    private Vector2 targetDirection;
    private float changeDirectionCD;
    private float stateTimer;
    private bool isIdle = false;
    private bool runningAway = false;
    private bool handledDeath = false;
    private Vector2 lastPosition;
    private float stuckTimer = 0f;
    private CircleCollider2D enemyCollider;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pdController = GetComponent<PlayerDetection>();
        enemyCollider = GetComponent<CircleCollider2D>();
        targetDirection = transform.up;
        stateTimer = moveDuration; // Start by moving for wandering type
        lastPosition = rb.position;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (enemyCollider == null)
        {
            Debug.LogError("Collider2D component is missing on the enemy object.", this);
        }

        
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            // Ensure HandleDeath is called only once per enemy instance
            if (!handledDeath) // Adding a new flag to track death handling
            {
                HandleDeath();
                handledDeath = true; // Set flag to prevent further calls
            }
        }
        else
        {
            UpdateTargetDirection();
            RotateTowardsTarget();
            Move();
            CheckIfStuck();
        }
    }

    private void UpdateTargetDirection()
    {
        runningAway = pdController.playerDetect;

        if (runningAway)
        {
            targetDirection = -pdController.directionPlayer;
        }
        else if (enemyType == EnemyType.Wandering)
        {
            if (isIdle)
            {
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0)
                {
                    isIdle = false;
                    stateTimer = moveDuration;
                }
            }
            else
            {
                WanderRandomly();

                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0)
                {
                    isIdle = true;
                    stateTimer = idleDuration;
                }
            }
        }
    }

    private void WanderRandomly()
    {
        if (!runningAway && enemyType == EnemyType.Wandering)
        {
            changeDirectionCD -= Time.deltaTime;
            if (changeDirectionCD <= 0)
            {
                float angleChange = Random.Range(-90f, 90f);
                Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
                targetDirection = rotation * targetDirection;

                changeDirectionCD = Random.Range(1f, 5f);
            }
        }
    }

    private void RotateTowardsTarget()
    {
        if (enemyType == EnemyType.Hiding && !runningAway)
        {
            // Hiding type does not rotate unless running away
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        rb.SetRotation(rotation);
    }

    private void Move()
    {
        if (isIdle && enemyType == EnemyType.Wandering && !runningAway)
        {
            rb.velocity = Vector2.zero; // idle for wandering type
        }
        else if (enemyType == EnemyType.Hiding && !runningAway)
        {
            rb.velocity = Vector2.zero; // idle for hiding type unless running away
        }
        else
        {
            rb.velocity = transform.up * speed; // Move for both types when running away
        }
    }

    private void CheckIfStuck()
    {
        if (enemyType == EnemyType.Hiding && !runningAway)
        {
            return; // Hiding enemies won't need stuck detection unless running away
        }

        if (Vector2.Distance(rb.position, lastPosition) < stuckDetectionRadius)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer >= stuckDetectionTime)
            {
                ForceChangeDirection();
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }
        lastPosition = rb.position;
    }

    private void ForceChangeDirection()
    {
        float randomAngle = Random.Range(150f, 210f); // Large turn angle to avoid getting stuck
        Quaternion rotation = Quaternion.AngleAxis(randomAngle, transform.forward);
        targetDirection = rotation * targetDirection;
    }

    private void HandleDeath()
    {
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false; // Disable collider
        }

        if (rb != null)
        {
            rb.velocity = Vector2.zero; // Stop any current movement
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Freeze Z-axis rotation
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = enemyType == EnemyType.Wandering ? fallenWanderingSprite : fallenHidingSprite;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddCurrency(1); 
        }

        StartCoroutine(DelayedDeath());
    }

    private IEnumerator DelayedDeath()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject); // Destroy after 1 second delay
    }

    public void SetDeadFlag()
    {
        isDead = true;
    }
}




