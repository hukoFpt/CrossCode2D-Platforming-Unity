using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed = 5f;
    public float dashSpeed = 10f;
    public float dashDistance = 0.5f; // Distance to dash
    private Animator animator;
    private Rigidbody2D rb;
    private bool isDashing = false;
    private float dashTime = 0.3f; // Duration of the dash in seconds
    private float dashTimer = 0f;
    private Vector2 dashDirection;

    // Jump variables
    private bool isJumping = false;
    private float jumpDuration = 3f;
    private float jumpFrameTime = 1.5f;
    private float jumpTimer = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get horizontal movement input
        float move = Input.GetAxis("Horizontal");

        // Check if the dash key (J) is pressed and the character is moving
        if (Input.GetKeyDown(KeyCode.J) && !isDashing && move != 0)
        {
            isDashing = true;
            dashTimer = dashTime;

            // Determine the dash direction based on the current facing direction
            if (transform.localScale.x > 0)
            {
                dashDirection = Vector2.right;
                animator.SetBool("IsDashingRight", true);
            }
            else
            {
                dashDirection = Vector2.left;
                animator.SetBool("IsDashingLeft", true);
            }
        }

        // Set the velocity of the Rigidbody2D
        if (isDashing)
        {
            rb.linearVelocity = dashDirection * dashSpeed;
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                isDashing = false;
                animator.SetBool("IsDashingRight", false);
                animator.SetBool("IsDashingLeft", false);
                rb.linearVelocity = Vector2.zero; // Stop the character after dashing
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
        }

        // Check if the character is moving
        if (move != 0 && !isDashing)
        {
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsStopping", false);

            // Flip the character sprite based on the direction of movement
            if (move < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1); // Face left
            }
            else if (move > 0)
            {
                transform.localScale = new Vector3(1, 1, 1); // Face right
            }
        }
        else if (!isDashing)
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsStopping", true);
        }

        // Handle jump input and animation
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            jumpTimer = 0f;
            animator.SetBool("IsJumping", true);
            animator.SetBool("JumpFrame1", true);
            animator.SetBool("JumpFrame2", false);
        }

        if (isJumping)
        {
            jumpTimer += Time.deltaTime;

            if (jumpTimer >= jumpFrameTime)
            {
                animator.SetBool("JumpFrame1", false);
                animator.SetBool("JumpFrame2", true);
            }

            if (jumpTimer >= jumpDuration || Input.GetKeyUp(KeyCode.Space))
            {
                isJumping = false;
                animator.SetBool("IsJumping", false);
                animator.SetBool("JumpFrame1", false);
                animator.SetBool("JumpFrame2", false);
            }
        }
    }
}