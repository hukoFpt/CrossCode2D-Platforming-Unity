using UnityEngine;
using System.Collections;

namespace CrossCode2D.Enemies
{
    public class JungleShockcatController : MonoBehaviour
    {
        public static JungleShockcatController Instance { get; private set; }
        private JungleShockcat jungleShockcat;

        private Rigidbody2D rb;
        private Animator animator;
        private SpriteRenderer spriteRenderer;

        public float moveSpeed = 2f;
        private Vector2 leftBoundary;
        private Vector2 rightBoundary;
        private bool movingRight = true;
        private bool isStopped = false;
        private bool isAttacking = false;

        private Transform playerTransform;
        public float attackDistance = 4f;
        public float dashAttackDistance = 1f; // Distance to check for damage during DashAttack

        private Collider2D playerCollider;
        private Collider2D shockcatCollider;

        private bool hasDealtDamage = false; // Flag to track if damage has been dealt during the current attack
        public float attackCooldown = 5f; // Cooldown duration for the attack
        private bool isOnCooldown = false; // Flag to track if the attack is on cooldown

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
            SetBoundaries();
            StartCoroutine(RandomStopCoroutine());

            // Find the player using the tag
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
                playerCollider = player.GetComponent<Collider2D>();
            }

            shockcatCollider = GetComponent<Collider2D>();
        }

        void Update()
        {
            CheckPlayerDistance();
            HandlePatrol();
        }

        void FixedUpdate()
        {
            HandleFixedMove();
        }

        private void InitializeComponents()
        {
            jungleShockcat = GetComponent<JungleShockcat>();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void SetBoundaries()
        {
            leftBoundary = new Vector2(transform.position.x - 5f, transform.position.y);
            rightBoundary = new Vector2(transform.position.x + 5f, transform.position.y);
        }

        private void HandlePatrol()
        {
            if (isStopped || isAttacking) return;

            if (movingRight)
            {
                if (transform.position.x >= rightBoundary.x)
                {
                    movingRight = false;
                }
            }
            else
            {
                if (transform.position.x <= leftBoundary.x)
                {
                    movingRight = true;
                }
            }
        }

        private void HandleFixedMove()
        {
            if (!isStopped && !isAttacking)
            {
                float direction = movingRight ? 1 : -1;
                rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

                // Set the isWalking parameter in the animator
                animator.SetBool("isWalking", rb.linearVelocity.x != 0);

                // Flip the sprite based on movement direction
                if (rb.linearVelocity.x != 0)
                {
                    spriteRenderer.flipX = rb.linearVelocity.x < 0;
                }
            }
            else if (!isAttacking)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

                // Set the isWalking parameter in the animator
                animator.SetBool("isWalking", false);
            }
        }

        private void CheckPlayerDistance()
        {
            if (playerTransform != null && Vector2.Distance(transform.position, playerTransform.position) <= attackDistance)
            {
                if (!isAttacking && !isOnCooldown)
                {
                    StartCoroutine(PreAttack());
                }
            }
        }

        private IEnumerator PreAttack()
        {
            isAttacking = true;
            animator.SetTrigger("PreAttack");

            // Flip the sprite to face the player
            if (playerTransform.position.x > transform.position.x)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }

            yield return new WaitForSeconds(2f); // Pre-attack duration

            StartCoroutine(DashAttack());
        }

        private IEnumerator DashAttack()
        {
            animator.SetBool("isDashAttack", true);
            float originalSpeed = moveSpeed;
            moveSpeed = 25f;
            float direction = playerTransform.position.x > transform.position.x ? 1 : -1;

            // Temporarily disable collision with the player
            Physics2D.IgnoreCollision(shockcatCollider, playerCollider, true);

            rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

            float dashTime = 0.3f; // Duration of the dash attack
            float elapsedTime = 0f;

            hasDealtDamage = false; // Reset the damage flag at the start of the attack

            while (elapsedTime < dashTime)
            {
                elapsedTime += Time.deltaTime;

                // Check if the player is within the dash attack distance and if damage has not been dealt yet
                if (!hasDealtDamage && Vector2.Distance(transform.position, playerTransform.position) <= dashAttackDistance)
                {
                    playerTransform.GetComponent<CrossCode2D.Player.Player>().TakeDamage(jungleShockcat.stats.attack);
                    hasDealtDamage = true; // Set the flag to true after dealing damage
                }

                yield return null;
            }

            // Re-enable collision with the player
            Physics2D.IgnoreCollision(shockcatCollider, playerCollider, false);

            moveSpeed = originalSpeed;
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isDashAttack", false);
            isAttacking = false;

            // Start the cooldown
            StartCoroutine(AttackCooldown());
        }

        private IEnumerator AttackCooldown()
        {
            isOnCooldown = true;
            yield return new WaitForSeconds(attackCooldown);
            isOnCooldown = false;
        }

        private IEnumerator RandomStopCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(2f, 5f)); // Random stop interval
                isStopped = true;
                yield return new WaitForSeconds(Random.Range(1f, 3f)); // Random stop duration
                isStopped = false;
            }
        }

        public void SetMoveSpeed(float speed)
        {
            moveSpeed = speed;
        }

        // Draw a sphere with a radius of 4f to visualize the attack distance
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDistance);

            // Draw a blue line to visualize 1f distance in the x-axis
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z));

            // Draw a blue line to visualize 0.5f thickness in the y-axis
            Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z),
                            new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z));
        }
    }
}