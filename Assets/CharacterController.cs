using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement parameters
    public float moveSpeed = 5f;

    // Dash parameters
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

    // Jump parameters
    public float jumpForce = 400f; // Adjusted for AddForce

    // General parameters
    private bool isGrounded = false;

    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Debug.Log("Rigidbody2D mass: " + rb.mass);
        Debug.Log("Rigidbody2D gravity scale: " + rb.gravityScale);
    }

    void Update()
    {
        // Handle input
        movement.x = Input.GetAxisRaw("Horizontal");

        // Set animation parameters
        if (movement != Vector2.zero && !isDashing)
        {
            animator.SetFloat("Speed", Mathf.Abs(movement.x));

            // Flip the character based on the direction
            if (movement.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (movement.x > 0)
            {
                spriteRenderer.flipX = false;
            }
        }
        else if (!isDashing)
        {
            animator.SetFloat("Speed", 0);
        }

        // Handle dash input
        if (Input.GetKeyDown(KeyCode.J) && !isDashing && movement.x != 0 && canDash)
        {
            if (isCooldown)
            {
                dashDistance = 0.5f;
                cooldownTimer = dashCooldown; // Reset cooldown to 1 second
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

            // Set dash direction based on character's facing direction
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

        // Update dash timer
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

        // Update cooldown timer
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                isCooldown = false;
            }
        }

        // Update delay timer
        if (!canDash)
        {
            delayTimer -= Time.deltaTime;
            if (delayTimer <= 0)
            {
                canDash = true;
            }
        }

        // Handle jump input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                animator.SetBool("IsJumping", true);
                rb.AddForce(Vector2.up * jumpForce);
                isGrounded = false;
            }
        }
        if (isGrounded)
        {
            animator.SetFloat("yVelocity", 0f);
        }
        else
        {
            animator.SetFloat("yVelocity", rb.linearVelocity.y);
        }
    }

    void FixedUpdate()
    {
        // Move the player
        if (isDashing)
        {
            Vector2 newPosition = Vector2.MoveTowards(rb.position, dashTarget, dashDistance / dashDuration * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);
        }
        else
        {
            rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("IsJumping", false);
        }
    }
}