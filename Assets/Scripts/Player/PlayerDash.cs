using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public float dashDistance = 1.3f;
    public float dashDuration = 0.1f;
    public float dashCooldown = 0.5f;
    public float dashDelay = 0.2f;
    private bool isDashing = false;
    private bool isCooldown = false;
    private bool canDash = true;
    private float dashTimer = 0f;
    private float cooldownTimer = 0f;
    private float delayTimer = 0f;
    private Vector2 dashTarget;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.J) && !isDashing && animator.GetFloat("Speed")>0)
        {
            if (isCooldown)
            {
                dashDistance = 0.5f;
                cooldownTimer = dashCooldown;
            }
            else
            {
                dashDistance = 1.3f;
                isCooldown = true;
                cooldownTimer = dashCooldown;
            }

            isDashing = true;
            dashTimer = dashDuration;
            canDash = false;
            delayTimer = dashDelay;
            animator.SetBool("IsDashing", true);

            if (spriteRenderer.flipX)
            {
                dashTarget = rb.position + Vector2.left * dashDistance;
                animator.SetBool("IsDashingLeft", true);
                animator.SetBool("IsDashingRight", false);
            }
            else
            {
                dashTarget = rb.position + Vector2.right * dashDistance;
                animator.SetBool("IsDashingLeft", false);
                animator.SetBool("IsDashingRight", true);
            }
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0 || Vector2.Distance(rb.position, dashTarget) < 0.1f)
            {
                isDashing = false;
                animator.SetBool("IsDashing", false);
                animator.SetBool("IsDashingLeft", false);
                animator.SetBool("IsDashingRight", false);
            }
        }

        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                isCooldown = false;
            }
        }

        if (!canDash)
        {
            delayTimer -= Time.deltaTime;
            if (delayTimer <= 0)
            {
                canDash = true;
            }
        }
    }

    public void FixedHandleDash()
    {
        if (isDashing)
        {
            Vector2 newPosition = Vector2.MoveTowards(rb.position, dashTarget, dashDistance / dashDuration * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);
        }
    }
}