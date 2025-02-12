using System.Collections;
using UnityEngine;

public class AttackEffectController : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Collider2D attackCollider;
    public float attackDamage = 10f; // Damage dealt by the attack
   
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackCollider = GetComponent<Collider2D>();
        if (attackCollider == null)
        {
            attackCollider = gameObject.AddComponent<BoxCollider2D>();
        }
        attackCollider.isTrigger = true;
        attackCollider.enabled = false; // Disable the collider initially
    }

    private void Update()
    {
        // Check if the attack animation is playing
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("AttackNeutral") || stateInfo.IsName("AttackHeat") || stateInfo.IsName("AttackCold") || stateInfo.IsName("AttackShock") || stateInfo.IsName("AttackWave"))
        {
            spriteRenderer.enabled = true; // Show the sprite when the attack animation is playing
        }
        else
        {
            spriteRenderer.enabled = false; // Hide the sprite when the attack animation is not playing
        }
    }

    public void Attack(string move, bool characterFacingRight, string Element)
    {
        if (characterFacingRight)
        {
            spriteRenderer.flipY = false;
            attackCollider.offset = new Vector2(0f, 0.2f); // Set collider offset to the right
        }
        else
        {
            spriteRenderer.flipY = true;
            attackCollider.offset = new Vector2(0f, -0.2f); // Set collider offset to the left
        }

        if (move == "1")
        {
            spriteRenderer.flipX = true; // Normal attack
        }
        else if (move == "2")
        {
            spriteRenderer.flipX = false; // Flipped attack
        }

        animator.Play("Attack" + Element, 0, 0f);

        spriteRenderer.enabled = true;
        StartCoroutine(EnableColliderForOneFrame());
    }

    private IEnumerator EnableColliderForOneFrame()
    {
        attackCollider.enabled = true;
        for (int i = 0; i < 5; i++)
        {
            yield return null; // Wait for one frame
        }
        attackCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            JungleShockcat enemy = collision.GetComponent<JungleShockcat>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
                Debug.Log($"Player attacked {enemy.name} and dealt {attackDamage} damage.");
            }
        }
    }
}