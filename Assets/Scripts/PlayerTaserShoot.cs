using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTaserShoot : MonoBehaviour
{
    public float attackRange = 1f;
    public LayerMask enemyLayers;
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

        RaycastHit2D hitInfo = Physics2D.Raycast(attackPoint.position, attackPoint.up, attackRange, enemyLayers);
        if (hitInfo)
        {
            Debug.Log("Hit " + hitInfo.collider.name);
            EnemyMovement enemy = hitInfo.collider.GetComponent<EnemyMovement>();
            if (enemy != null)
            {
                enemy.SetDeadFlag();
            }
        }

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
