using System.Collections;
using UnityEngine;

namespace CrossCode2D.Enemies
{
    public class JungleShockcatController : MonoBehaviour
    {
        public static JungleShockcatController Instance { get; private set; }

        private JungleShockcat jungleShockcat;
        public bool isAttacking = false;
        public float walkSpeed = 2f;
        public float knockbackForce = 5f;

        private Rigidbody2D rb;
        private Animator animator;
        private Vector2 walkDirection;
        private float changeDirectionTimer;
        private SpriteRenderer spriteRenderer;
        private Vector2 spawnPosition;
        private bool isStopped;
        private Color originalColor;
        private Transform playerTransform;
        private bool isOnCooldown;
        private bool isKnockback;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            InitializeComponents();
            SetRandomDirection();
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        void Update()
        {
            HandleDirectionChange();
            animator.SetBool("isWalking", walkDirection != Vector2.zero);
            CheckForPlayerAndAttack();

            if (isAttacking)
            {
                TargetPlayer();
            }
        }

        void FixedUpdate()
        {
            Move();
            FlipSprite();
        }

        private void InitializeComponents()
        {
            float changeDirectionTime = 2f;

            jungleShockcat = GetComponent<JungleShockcat>();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            changeDirectionTimer = changeDirectionTime;
            spawnPosition = transform.position;
        }

        private void HandleDirectionChange()
        {
            float changeDirectionTime = 2f;

            changeDirectionTimer -= Time.deltaTime;
            if (changeDirectionTimer <= 0 || IsBeyondMaxDistance())
            {
                SetRandomDirection();
                changeDirectionTimer = changeDirectionTime;
            }
        }

        private void Move()
        {
            if (!isKnockback)
            {
                if (isStopped)
                {
                    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                }
                else
                {
                    rb.linearVelocity = new Vector2(walkDirection.x * walkSpeed, rb.linearVelocity.y);
                }
            }
        }

        private void SetRandomDirection()
        {
            if (IsBeyondMaxDistance() && !isAttacking)
            {
                walkDirection = (transform.position.x > spawnPosition.x) ? Vector2.left : Vector2.right;
            }
            else
            {
                float randomValue = Random.Range(0f, 1f);
                if (randomValue < 0.3f)
                {
                    StartCoroutine(StopForRandomTime());
                }
                else
                {
                    walkDirection = Random.Range(0, 2) == 0 ? Vector2.left : Vector2.right;
                }
            }
        }

        private IEnumerator StopForRandomTime()
        {
            float minStopTime = 1f;
            float maxStopTime = 3f;

            isStopped = true;
            walkDirection = Vector2.zero;
            float stopTime = Random.Range(minStopTime, maxStopTime);
            yield return new WaitForSeconds(stopTime);
            isStopped = false;
            SetRandomDirection();
        }

        private void FlipSprite()
        {
            spriteRenderer.flipX = walkDirection == Vector2.left;
        }

        private bool IsBeyondMaxDistance()
        {
            float maxDistanceFromSpawn = 3f;
            return Vector2.Distance(spawnPosition, transform.position) > maxDistanceFromSpawn;
        }

        private void CheckForPlayerAndAttack()
        {
            float attackRange = 2f;

            if (playerTransform != null && Vector2.Distance(transform.position, playerTransform.position) <= attackRange && !isOnCooldown)
            {
                StartCoroutine(PerformAttack());
            }
        }

        private IEnumerator PerformAttack()
        {
            float preAttackDuration = 0.8f;
            float attackCooldown = 3f;

            isStopped = true;
            isOnCooldown = true;

            FacePlayer();

            animator.SetTrigger("PreAttack");
            yield return new WaitForSeconds(preAttackDuration);
            animator.SetBool("isAttacking", true);

            Vector2 newPosition = CalculateNewPosition();
            DealDamageToPlayerIfInRange(newPosition);

            transform.position = newPosition;
            yield return new WaitForSeconds(0.1f);
            animator.SetBool("isAttacking", false);
            isStopped = false;

            yield return new WaitForSeconds(attackCooldown);
            isOnCooldown = false;
        }

        private void FacePlayer()
        {
            if (playerTransform != null)
            {
                walkDirection = playerTransform.position.x < transform.position.x ? Vector2.left : Vector2.right;
                spriteRenderer.flipX = walkDirection == Vector2.left;
            }
        }

        private Vector2 CalculateNewPosition()
        {
            float teleportDistance = 4f;
            return transform.position + new Vector3((spriteRenderer.flipX ? Vector2.left : Vector2.right).x * teleportDistance, 0, 0);
        }

        private void DealDamageToPlayerIfInRange(Vector2 newPosition)
        {
            if (playerTransform != null)
            {
                float playerPositionX = playerTransform.position.x;
                float playerPositionY = playerTransform.position.y;
                if (IsPlayerBetweenPositions(transform.position, newPosition, playerPositionX) && Mathf.Abs(playerPositionY - transform.position.y) <= 1f)
                {
                    Player.Player player = playerTransform.GetComponent<Player.Player>();
                    player?.TakeDamage(jungleShockcat.stats.attack);
                }
            }
        }

        private bool IsPlayerBetweenPositions(Vector2 originalPosition, Vector2 newPosition, float playerPositionX)
        {
            return (originalPosition.x < playerPositionX && playerPositionX < newPosition.x) ||
                   (newPosition.x < playerPositionX && playerPositionX < originalPosition.x);
        }

        // Function to always target the player when isAttacking is true
        private void TargetPlayer()
        {
            if (playerTransform != null)
            {
                Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
                rb.linearVelocity = new Vector2(directionToPlayer.x * walkSpeed, rb.linearVelocity.y);
            }
        }

        // Apply knockback to the enemy
        public void ApplyKnockback(Vector2 direction, float force)
        {
            StartCoroutine(ApplyKnockbackCoroutine(direction, force));
        }

        private IEnumerator ApplyKnockbackCoroutine(Vector2 direction, float force)
        {
            isKnockback = true;
            jungleShockcat.ApplyKnockback(direction, force); // Call the ApplyKnockback method from the Enemy class
            yield return new WaitForSeconds(0.2f); // Adjust the duration as needed
            isKnockback = false;
        }

        // Test method to apply knockback with a fixed direction
        public void TestApplyKnockback()
        {
            Vector2 testDirection = Vector2.right; // Change this to test different directions
            float testForce = 10f; // Change this to test different force values
            ApplyKnockback(testDirection, testForce);
        }
    }
}