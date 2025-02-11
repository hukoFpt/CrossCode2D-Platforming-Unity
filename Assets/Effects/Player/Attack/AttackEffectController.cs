using UnityEngine;

public class AttackEffectController : MonoBehaviour
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

        animator.Play("Attack"+Element, 0, 0f);

        spriteRenderer.enabled = false;
    }
}