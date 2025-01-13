using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed = 5f;
    public float dashSpeed = 10f;
    public float dashDistance = 0.5f; // Distance to dash
    public float jumpForce = 0.3f;
    public Transform groundCheck; // Transform to check if the character is grounded
    public float groundCheckRadius = 0.2f; // Radius of the ground check
    public LayerMask groundLayer; // Layer to check for ground

    private Animator animator;
    private Rigidbody2D rb;
    private bool isDashing = false;
    private float dashTime = 0.3f; // Duration of the dash in seconds
    private float dashTimer = 0f;
    private Vector2 dashDirection;

    // Jump variables
    private bool isJumping = false;
    private float initialJumpDirection;
    private bool isGrounded;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        // Check if the character is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Get horizontal movement input
        float move = Input.GetAxis("Horizontal");

        // Handle dash input
        if (Input.GetKeyDown(KeyCode.J) && !isDashing && move != 0 && isGrounded)
        {
            StartDash(move);
        }

        // Handle movement and dashing
        if (isDashing)
        {
            HandleDash();
        }
        else
        {
            HandleMovement(move);
        }

        // Handle jump input and animation
        if (Input.GetKeyDown(KeyCode.K) && !isJumping)
        {
            StartJump(move);
        }

        if (isJumping)
        {
            HandleJump();
        }

        // Update animator parameters
        UpdateAnimator(move);
    }

    private void StartDash(float move)
    {
        isDashing = true;
        dashTimer = dashTime;

        // Determine the dash direction based on the current facing direction
        dashDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        animator.SetBool("IsDashingRight", dashDirection == Vector2.right);
        animator.SetBool("IsDashingLeft", dashDirection == Vector2.left);
    }

    private void HandleDash()
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

    private void HandleMovement(float move)
    {
        if (!isJumping)
        {
            rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
        }
        else
        {
            // Allow slight horizontal adjustment while jumping
            rb.linearVelocity = new Vector2(initialJumpDirection * speed + move * 0.5f, rb.linearVelocity.y);
        }
    }

    private void StartJump(float move)
    {
        isJumping = true;
        initialJumpDirection = move; // Store the initial jump direction
        animator.SetBool("IsStopping", false);
        animator.SetBool("IsJumping", true);

        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }

    private void HandleJump()
    {
        // Set the vertical velocity parameter for the jump animation
        if (rb.linearVelocity.y > 0)
        {
            animator.SetBool("IsJumpingUp", true);
            animator.SetBool("IsFalling", false);
        }
        else if (rb.linearVelocity.y < 0)
        {
            animator.SetBool("IsJumpingUp", false);
            animator.SetBool("IsFalling", true);
        }

        if (rb.linearVelocity.y == 0)
        {
            isJumping = false;
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsJumpingUp", false);
            animator.SetBool("IsFalling", false);
        }
    }

    private void UpdateAnimator(float move)
    {
        if (move != 0 && !isDashing && !isJumping)
        {
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsStopping", false);

            // Flip the character sprite based on the direction of movement
            transform.localScale = new Vector3(move < 0 ? -1 : 1, 1, 1);
        }
        else if (!isDashing && !isJumping)
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsStopping", true);
        }
    }

    void OnDrawGizmos()
    {
        // Draw a sphere in the Scene view to visualize the ground check
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}