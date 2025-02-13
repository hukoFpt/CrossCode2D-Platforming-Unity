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
    public float attackRange = 2f; // Range to detect the player for attack
    public float preAttackDuration = 0.8f; // Duration of the PreAttack animation
    public float teleportDistance = 4f; // Distance to teleport during attack
    public float attackCooldown = 3f; // Cooldown duration for the attack

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 walkDirection;
    private float changeDirectionTimer;
    private SpriteRenderer spriteRenderer;
    private Vector2 spawnPosition;
    private bool isStopped;
    private Color originalColor;
    private HealthBar healthBar;
    private Transform playerTransform;
    private bool isOnCooldown;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

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

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Find the player by tag
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

        CheckForPlayerAndAttack();
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
        Debug.Log("Applying knockback force. Direction: " + attackDirection + ", Force: " + knockbackForce);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Reset horizontal velocity

        // Apply knockback force only along the x-axis
        Vector2 knockback = new Vector2(-attackDirection.x, 0).normalized * knockbackForce;
        rb.AddForce(knockback, ForceMode2D.Impulse);
    }

    private void Die()
    {
        // Handle death logic, e.g., play animation, disable object, etc.
        Debug.Log("JungleShockcat has died.");
        healthBar.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void CheckForPlayerAndAttack()
    {
        if (playerTransform != null && Vector2.Distance(transform.position, playerTransform.position) <= attackRange && isOnCooldown == false)
        {
            StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
        isStopped = true;
        isOnCooldown = true;

        // Face the player before performing the attack
        if (playerTransform != null)
        {
            if (playerTransform.position.x < transform.position.x)
            {
                walkDirection = Vector2.left;
                spriteRenderer.flipX = true;
            }
            else
            {
                walkDirection = Vector2.right;
                spriteRenderer.flipX = false;
            }
        }

        animator.SetTrigger("PreAttack");
        yield return new WaitForSeconds(preAttackDuration);
        animator.SetBool("isAttacking", true);

        // Calculate the new position after teleporting
        Vector2 originalPosition = transform.position;
        Vector2 newPosition = originalPosition + (spriteRenderer.flipX ? Vector2.left : Vector2.right) * teleportDistance;

        // Check if the player is between the original and new positions
        if (playerTransform != null)
        {
            float playerPositionX = playerTransform.position.x;
            float playerPositionY = playerTransform.position.y;
            if (((originalPosition.x < playerPositionX && playerPositionX < newPosition.x) ||
                (newPosition.x < playerPositionX && playerPositionX < originalPosition.x)) &&
                Mathf.Abs(playerPositionY - transform.position.y) <= 1f)
            {
                Debug.Log("Player hit by JungleShockcat attack.");
                Player player = playerTransform.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(10f);
                }
            }
        }

        // Teleport to the new position
        transform.position = newPosition;
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("isAttacking", false);
        isStopped = false;

        yield return new WaitForSeconds(attackCooldown);
        isOnCooldown = false;
    }
}