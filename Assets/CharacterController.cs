using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed = 5f;
    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get horizontal movement input
        float move = Input.GetAxis("Horizontal");

        // Set the velocity of the Rigidbody2D
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        // Check if the character is moving
        if (move != 0)
        {
            animator.SetBool("IsRunning", true);

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
        else
        {
            animator.SetBool("IsRunning", false);
        }

        // Check if specific keys are pressed
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("A key was pressed down");
        }
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("A key is being held down");
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("A key was released");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("D key was pressed down");
        }
        if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("D key is being held down");
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            Debug.Log("D key was released");
        }
    }
}