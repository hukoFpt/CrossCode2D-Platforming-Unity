using UnityEngine;

public class AttackNeutralController : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        // Check if the attack animation is playing
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Attack"))
        {
            spriteRenderer.enabled = true; // Show the sprite when the attack animation is playing
        }
        else
        {
            spriteRenderer.enabled = false; // Hide the sprite when the attack animation is not playing
        }
    }

    public void Attack(string move, bool characterFacingRight)
    {
        if (characterFacingRight)
        {
            spriteRenderer.flipY = false;
        }
        else
        {
            spriteRenderer.flipY = true;
        }

        if (move == "1")
        {
            spriteRenderer.flipX = true; // Normal attack
        }
        else if (move == "2")
        {
            spriteRenderer.flipX = false; // Flipped attack
        }

        animator.Play("Attack", 0, 0f);

        spriteRenderer.enabled = false;
    }
}