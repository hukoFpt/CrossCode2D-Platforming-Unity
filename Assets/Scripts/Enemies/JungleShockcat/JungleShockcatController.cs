using System.Collections;
using UnityEngine;

public class JungleShockcatController : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float changeDirectionTime = 2f;
    public float maxDistanceFromSpawn = 3f; // Maximum distance from the initial spawn position
    public float minStopTime = 1f; // Minimum time to stop
    public float maxStopTime = 3f; // Maximum time to stop
    public float maxHP = 100f;
    public float currentHP;
    public float knockbackForce = 5f; // Force applied for knockback
    public GameObject healthBarPrefab;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 walkDirection;
    private float changeDirectionTimer;
    private SpriteRenderer spriteRenderer;
    private Vector2 spawnPosition;
    private bool isStopped;
    private Color originalColor;
    private HealthBar healthBar;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.bodyType = RigidbodyType2D.Dynamic;

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        changeDirectionTimer = changeDirectionTime;
        spawnPosition = transform.position; // Store the initial spawn position
        currentHP = maxHP; // Initialize health
        SetRandomDirection();

        GameObject healthBarInstance = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
        healthBar = healthBarInstance.GetComponent<HealthBar>();
        healthBar.Initialize(transform);
    }

    void Update()
    {
        if (isStopped) return;

        changeDirectionTimer -= Time.deltaTime;
        if (changeDirectionTimer <= 0 || IsBeyondMaxDistance())
        {
            SetRandomDirection();
            changeDirectionTimer = changeDirectionTime;
        }

        animator.SetBool("isWalking", walkDirection != Vector2.zero);
        healthBar.UpdateHealth(currentHP, maxHP);
    }

    void FixedUpdate()
    {
        if (isStopped)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(walkDirection.x * walkSpeed, rb.linearVelocity.y);
        }
        FlipSprite();
    }

    void SetRandomDirection()
    {
        if (IsBeyondMaxDistance())
        {
            // Move towards the spawn position if beyond max distance
            walkDirection = (transform.position.x > spawnPosition.x) ? Vector2.left : Vector2.right;
        }
        else
        {
            // Randomly choose to stop or move left/right
            float randomValue = Random.Range(0f, 1f);
            if (randomValue < 0.3f) // 30% chance to stop
            {
                StartCoroutine(StopForRandomTime());
            }
            else
            {
                walkDirection = Random.Range(0, 2) == 0 ? Vector2.left : Vector2.right;
            }
        }
    }

    IEnumerator StopForRandomTime()
    {
        isStopped = true;
        walkDirection = Vector2.zero;
        float stopTime = Random.Range(minStopTime, maxStopTime);
        yield return new WaitForSeconds(stopTime);
        isStopped = false;
        SetRandomDirection();
    }

    void FlipSprite()
    {
        // Flip the sprite based on the direction
        if (walkDirection == Vector2.left)
        {
            spriteRenderer.flipX = true;
        }
        else if (walkDirection == Vector2.right)
        {
            spriteRenderer.flipX = false;
        }
    }

    bool IsBeyondMaxDistance()
    {
        // Check if the enemy is beyond the maximum distance from the spawn position
        return Vector2.Distance(spawnPosition, transform.position) > maxDistanceFromSpawn;
    }

    public void TakeDamage(float damage, Vector2 attackDirection)
    {
        StartCoroutine(FlashWhite());
        Debug.Log("JungleShockcat took " + damage + " damage.");
        currentHP -= damage;
        ApplyKnockback(attackDirection);
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashWhite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.gray; // Change color to white
            yield return new WaitForSeconds(0.05f); // Wait for 0.2 seconds
            spriteRenderer.color = originalColor; // Revert to original color
        }
    }

    private void ApplyKnockback(Vector2 attackDirection)
    {
        // Apply knockback force in the opposite direction of the attack
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Reset horizontal velocity
        rb.AddForce(-attackDirection.normalized * knockbackForce, ForceMode2D.Impulse);
    }

    private void Die()
    {
        // Handle death logic, e.g., play animation, disable object, etc.
        Debug.Log("JungleShockcat has died.");
        healthBar.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}