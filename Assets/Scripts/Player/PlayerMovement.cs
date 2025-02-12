using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool canMove = true; // Flag to control movement

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (canMove)
        {
            HandleMovement();
        }
        else
        {
            animator.SetFloat("Speed", 0); // Ensure the speed is set to 0 when movement is disabled
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            FixedHandleMovement();
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Stop horizontal movement when movement is disabled
        }
    }

    public void HandleMovement()
    {
        movement.x = Input.GetAxisRaw("Horizontal");

        if (movement != Vector2.zero)
        {
            animator.SetFloat("Speed", Mathf.Abs(movement.x));

            if (movement.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (movement.x > 0)
            {
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }

    public void FixedHandleMovement()
    {
        rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
    }

    public void EnableMovement()
    {
        moveSpeed = 5f;
    }

    public void DisableMovement()
    {
        moveSpeed = 1.5f;
    }
}