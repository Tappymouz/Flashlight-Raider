using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTaserShoot : MonoBehaviour
{
    public float attackRange = 1f;
    public LayerMask enemyLayers;
    public LayerMask wallLayers;
    public Transform attackPoint;
    public float attackRate = 1f;
    public float attackTime = 0f;

    private float nextAttackTime = 0f;
    private Animator animator;
    private bool isAttacking = false;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void OnAttack(InputValue inputValue)
    {
        if (inputValue.isPressed && !isAttacking && Time.time >= nextAttackTime)
        {
            ShootTaser();
            nextAttackTime = Time.time + attackTime / attackRate;
        }
    }

    private void ShootTaser()
    {
        isAttacking = true; // Set the flag to true
        animator.SetBool("TaserShoot", true);
        Debug.Log("Taser Gun Fired!");

        // Perform raycast that checks for walls and enemies
        int combinedLayerMask = enemyLayers | wallLayers;
        RaycastHit2D hitInfo = Physics2D.Raycast(attackPoint.position, attackPoint.up, attackRange, combinedLayerMask);

        if (hitInfo)
        {
            Debug.Log("Hit " + hitInfo.collider.name);

            // Check if the hit object is an enemy
            if (((1 << hitInfo.collider.gameObject.layer) & enemyLayers) != 0)
            {
                EnemyMovement enemy = hitInfo.collider.GetComponent<EnemyMovement>();
                if (enemy != null)
                {
                    enemy.SetDeadFlag();
                    Debug.Log("Enemy hit by taser.");
                }
            }
            else if (((1 << hitInfo.collider.gameObject.layer) & wallLayers) != 0)
            {
                Debug.Log("Taser hit a wall. Stopping raycast.");
            }
        }
        else
        {
            Debug.Log("No hit detected.");
        }

        // Debug visualization of the raycast
        Debug.DrawLine(attackPoint.position, attackPoint.position + attackPoint.up * attackRange, Color.cyan, 0.2f);

        StartCoroutine(ResetAttack());
        AudioManager.Instance.PlaySFX("Taser");
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.8f); // Adjust based on animation length

        animator.SetBool("TaserShoot", false);
        isAttacking = false; // Reset the flag
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(attackPoint.position, attackPoint.position + attackPoint.up * attackRange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
