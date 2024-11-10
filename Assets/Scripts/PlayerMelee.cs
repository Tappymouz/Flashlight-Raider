using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMelee : MonoBehaviour
{
    public enum MeleeType
    {
        Normal,
        Taser
    }

    [SerializeField]
    private MeleeType meleeType;

    public float attackRange = 1f;
    public LayerMask enemyLayers;
    public Transform attackPoint;
    public float attackRate = 1f;
    public float attackTime = 0f;

    private float nextAttackTime = 0f;
    private Animator animator;
    private bool isAttacking = false;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void OnAttack(InputValue inputValue)
    {
        if (inputValue.isPressed && !isAttacking && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    private void Attack()
    {
        isAttacking = true; 
        animator.SetBool("Melee", true);
        Debug.Log("Player Attacks!");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D hit in hitEnemies)
        {
            EnemyMovement enemy = hit.GetComponent<EnemyMovement>();
            if (enemy != null)
            {
                enemy.SetDeadFlag();
                Debug.Log("Hit " + hit.name + " with melee attack and set dead flag.");
            }
        }

        if (meleeType == MeleeType.Normal)
        {
            AudioManager.Instance.PlaySFX("Melee");
        }
        else if (meleeType == MeleeType.Taser)
        {
            AudioManager.Instance.PlaySFX("Taser");
        }

        StartCoroutine(ResetAttack());
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.8f); // Adjust to your animation length

        animator.SetBool("Melee", false);
        isAttacking = false; // Reset the flag
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
