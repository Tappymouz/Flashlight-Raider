using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 1f;
    public int attackDamage = 10;
    public LayerMask enemyLayers;
    public Transform attackPoint;
    public float attackRate = 1f;

    private float nextAttackTime = 0f;
    private Animator animator;
    private bool isAttacking = false; // Flag to prevent spamming

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void OnMelee(InputValue inputValue)
    {
        if (inputValue.isPressed && !isAttacking && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    private void Attack()
    {
        isAttacking = true; // Set the flag to true
        animator.SetBool("Melee", true);
        Debug.Log("Player Attacks!");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Assuming the enemy has a script with a TakeDamage method
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);

            // Trigger any enemy reaction, e.g., knockback
            enemy.GetComponent<Enemy>().ReactToHit();

            // Optional: Play a hit effect
            PlayHitEffect(enemy.transform.position);

            // Optional: Play a hit sound
            PlayHitSound();
        }

        StartCoroutine(ResetAttack());
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.8f); // Adjust to your animation length

        animator.SetBool("Melee", false);
        isAttacking = false; // Reset the flag
    }

    

    private void PlayHitEffect(Vector2 position)
    {
        // Example of instantiating a hit effect
        // Instantiate(hitEffectPrefab, position, Quaternion.identity);
    }

    private void PlayHitSound()
    {
        // Example of playing a hit sound
        // AudioSource.PlayClipAtPoint(hitSoundClip, transform.position);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public bool IsAttacking()
{
    return isAttacking;
}

}
