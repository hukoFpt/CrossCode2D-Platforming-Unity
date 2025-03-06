using System;
using System.Collections;
using UnityEngine;

namespace CrossCode2D.Player
{
    public class HandleMovement : MonoBehaviour
    {
        // Initialize variables components
        private Rigidbody2D rb;
        private Animator animator;
        private Vector2 movement;
        private SpriteRenderer spriteRenderer;

        // Initialize variables
        public float moveSpeed = 5.0f;

        // Dash-related variables
        private bool isDashing = false;
        private bool isCooldown = false;

        // Jump-related variables
        private bool isGrounded = false;
        private float jumpForce = 800f;

        // Control-related variables
        private bool controlsEnabled = true;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (controlsEnabled)
            {
                HandleMove();
                HandleDash();
                HandleJump();
            }
        }

        // Fixed update is called once per physics update
        void FixedUpdate()
        {
            if (controlsEnabled)
            {
                HandleFixedMove();
            }
        }

        // Handle player movement
        public void HandleMove()
        {
            // Get the horizontal input
            movement.x = Input.GetAxisRaw("Horizontal");

            // Get the vertical speed input
            if (movement != Vector2.zero)
            {
                animator.SetFloat("Speed", Mathf.Abs(movement.x));
                spriteRenderer.flipX = movement.x < 0;
            }
            else
            {
                animator.SetFloat("Speed", 0);
            }
        }

        public void HandleFixedMove()
        {
            // Move the player
            rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
        }

        public void SetPlayerSpeed(float speed)
        {
            // Set the player speed
            moveSpeed = speed;
        }

        // Handle player dash
        public void HandleDash()
        {
            if (Input.GetKeyDown(KeyCode.L) && !isDashing && animator.GetFloat("Speed") > 0)
            {
                isDashing = true;
                if (spriteRenderer.flipX)
                {
                    animator.SetTrigger("DashLeft");
                }
                else
                {
                    animator.SetTrigger("DashRight");
                }

                if (!isCooldown)
                {
                    StartCoroutine(SetSpeedForDuration(8.0f, 0.2f));
                }
                else
                {
                    StartCoroutine(SetSpeedForDuration(4.5f, 0.2f));
                }

                isCooldown = true;
                StartCoroutine(DashCooldown(0.8f));
                StartCoroutine(DashDelay(0.4f));
            }
        }

        private IEnumerator SetSpeedForDuration(float speed, float duration)
        {
            SetPlayerSpeed(speed);
            yield return new WaitForSeconds(duration);
            SetPlayerSpeed(5f);
            animator.SetBool("IsDashing", false);
        }

        private IEnumerator DashDelay(float duration)
        {
            yield return new WaitForSeconds(duration);
            isDashing = false;
        }

        private IEnumerator DashCooldown(float duration)
        {
            yield return new WaitForSeconds(duration);
            isCooldown = false;
        }

        // Handle player jump
        public void HandleJump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                animator.SetBool("IsJumping", true);
                rb.AddForce(Vector2.up * jumpForce);
                isGrounded = false;
            }

            if (isGrounded)
            {
                animator.SetFloat("ylinearVelocity", 0f);
            }
            else
            {
                animator.SetFloat("ylinearVelocity", rb.linearVelocity.y);
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

        // Method to disable player controls and trigger the "Complete" animation
        public void DisableControlsAndComplete()
        {
            controlsEnabled = false;
            rb.linearVelocity = Vector2.zero; // Stop player movement
            animator.SetFloat("Speed", 0f);
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsDashing", false);
            animator.SetTrigger("Complete"); // Trigger the "Complete" animation
        }
    }
}