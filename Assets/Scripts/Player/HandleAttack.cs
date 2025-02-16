using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrossCode2D.Player
{
    public class HandleAttack : MonoBehaviour
    {
        // Initialising variables components
        private Rigidbody2D rb;
        private Animator animator;
        private SpriteRenderer spriteRenderer;

        // References to other scripts
        private HandleMovement playerMovement;
        private AttackEffectController attackEffectController;
        private Player player;

        // Attack Variables
        public enum CombatStyle { Melee, Throw }
        public CombatStyle currentStyle = CombatStyle.Melee;

        public enum Element { Neutral, Heat, Cold, Shock, Wave }
        public Element currentElement = Element.Neutral;

        private float lastAttackTime;
        private int attackCounter;
        private bool isCooldown;
        private float attackSpeed;

        // Slide-related variables
        public float slideForce = 10f;
        public float slideDuration = 0.2f;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            playerMovement = GetComponent<HandleMovement>();
            attackEffectController = GetComponentInChildren<AttackEffectController>();
            player = GetComponent<Player>();
        }

        void Update()
        {
            HandlePlayerAttack();
            ChangeAttackStyle();
            ChangeElement();
        }

        public void HandlePlayerAttack()
        {
            if (currentStyle == CombatStyle.Melee)
            {
                if (Input.GetKeyDown(KeyCode.K) && !isCooldown)
                {
                    float timeSinceLastAttack = Time.time - lastAttackTime;

                    // Get the attack speed based on the current element
                    attackSpeed = GetAttackSpeed(currentElement);

                    // Reset the attack counter if the time between attacks is greater than 0.6 seconds
                    if (timeSinceLastAttack > 0.6f)
                    {
                        attackCounter = 0;
                    }

                    // Increment the attack counter and update the last attack time
                    attackCounter++;
                    lastAttackTime = Time.time;

                    // Disable movement during attack
                    playerMovement.SetPlayerSpeed(0f);
                    StartCoroutine(EnableMovementAfterDelay(0.3f)); // Re-enable movement after 0.3 seconds

                    // Handle attack based on the attack counter
                    switch (attackCounter)
                    {
                        case 1:
                            animator.SetTrigger("Attack_1");
                            attackEffectController.Attack("1", !spriteRenderer.flipX, currentElement.ToString());
                            StartCoroutine(AttackCooldown(1.0f / attackSpeed)); // Regular cooldown
                            break;
                        case 2:
                            animator.SetTrigger("Attack_2");
                            attackEffectController.Attack("2", !spriteRenderer.flipX, currentElement.ToString());
                            StartCoroutine(AttackCooldown(1.0f / attackSpeed)); // Regular cooldown
                            break;
                        case 3:
                            animator.SetTrigger("Attack_1");
                            attackEffectController.Attack("1", !spriteRenderer.flipX, currentElement.ToString());
                            StartCoroutine(AttackCooldown(1.0f / attackSpeed)); // Regular cooldown
                            break;
                        case 4:
                            animator.SetTrigger("Attack_3");
                            attackEffectController.Attack("1", !spriteRenderer.flipX, currentElement.ToString());
                            StartCoroutine(SlideCharacter());
                            StartCoroutine(AttackCooldown(3.0f / attackSpeed)); // Longer cooldown for the fourth attack
                            attackCounter = 0; // Reset the counter after the last attack move
                            break;
                    }
                }
            }
        }

        void ChangeAttackStyle()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                currentStyle = (CombatStyle)(((int)currentStyle + 1) % System.Enum.GetValues(typeof(CombatStyle)).Length);
            }
        }

        void ChangeElement()
        {
            Dictionary<KeyCode, Element> elementKeyMap = new Dictionary<KeyCode, Element>
        {
            { KeyCode.BackQuote, Element.Neutral }, // `~` key
            { KeyCode.Alpha1, Element.Heat },       // `1` key
            { KeyCode.Alpha2, Element.Cold },       // `2` key
            { KeyCode.Alpha3, Element.Shock },      // `3` key
            { KeyCode.Alpha4, Element.Wave }        // `4` key
        };

            foreach (var entry in elementKeyMap)
            {
                if (Input.GetKeyDown(entry.Key))
                {
                    currentElement = currentElement == entry.Value ? Element.Neutral : entry.Value;
                    break;
                }
            }
        }

        float GetAttackSpeed(Element element)
        {
            // Example logic to determine attack speed based on element
            switch (element)
            {
                case Element.Heat:
                case Element.Cold:
                    return 3.0f;
                case Element.Shock:
                case Element.Wave:
                    return 5.0f;
                default:
                    return 4.0f;
            }
        }

        IEnumerator EnableMovementAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            playerMovement.SetPlayerSpeed(5.0f); // Reset to default speed
        }

        IEnumerator AttackCooldown(float duration)
        {
            isCooldown = true;
            yield return new WaitForSeconds(duration);
            isCooldown = false;
        }

        private IEnumerator SlideCharacter()
        {
            Debug.Log("Sliding character");
            // Apply force to slide the character based on the facing direction
            Vector2 slideDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;
            rb.AddForce(slideDirection * slideForce, ForceMode2D.Impulse);

            yield return new WaitForSeconds(slideDuration);

            // Stop the slide by setting the velocity to zero
            rb.linearVelocity = Vector2.zero;
        }

        public void TakeDamage(float damage)
        {
            player.TakeDamage(damage);
        }
    }
}