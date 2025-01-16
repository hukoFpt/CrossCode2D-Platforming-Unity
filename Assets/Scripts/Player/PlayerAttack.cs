using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isCooldown = false;
    private int attackCounter = 0;
    private float lastAttackTime = 0f;

    [Range(0.1f, 2.0f)]
    public float attackSpeed = 0.3f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.K) && !isCooldown)
        {
            float timeSinceLastAttack = Time.time - lastAttackTime;

            if (timeSinceLastAttack > 1.0f)
            {
                attackCounter = 0; // Reset the counter if the time between attacks is greater than 1 second
            }

            attackCounter++;
            lastAttackTime = Time.time;

            switch (attackCounter)
            {
                case 1:
                    animator.SetTrigger("Attack_1");
                    break;
                case 2:
                    animator.SetTrigger("Attack_2");
                    break;
                case 3:
                    animator.SetTrigger("Attack_1");
                    break;
                case 4:
                    animator.SetTrigger("Attack_3");
                    attackCounter = 0; // Reset the counter after the last attack move
                    break;
            }

            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        isCooldown = true;
        float cooldownDuration = 1.0f / attackSpeed; // Adjust cooldown based on attack speed
        yield return new WaitForSeconds(cooldownDuration);
        isCooldown = false;
    }
}