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
        private ChargeEffectController chargeEffectController;
        private Player player;

        // Attack Variables
        public enum CombatStyle { Melee, Throw }
        public CombatStyle currentStyle = CombatStyle.Melee;

        public enum Element { Neutral, Heat, Cold, Shock, Wave }
        public Element currentElement = Element.Neutral;

        // Melee-related variables
        private float lastAttackTime;
        private int attackCounter;
        private bool isCooldown;
        private float attackSpeed;

        // Throw-related variables
        private float chargeTime = 2f; // Time required to charge the throw
        private bool isThrowCooldown = false;
        private bool throwCycle = false; // To alternate between Throw_1 and Throw_2
        private bool isThrowing = false;
        private float keyDownTime;

        // Slide-related variables
        public float slideForce = 10f;
        public float slideDuration = 0.2f;

        // Projectile-related variables
        public GameObject projectilePrefab; // Assign the projectile prefab in the Inspector

        // SFX Variables
        public AudioSource neutralSweep1SFX;
        public AudioSource neutralSweep2SFX;
        public AudioSource neutralSweep3SFX;
        public AudioSource neutralSweep4SFX;
        public AudioSource heatSweep1SFX;
        public AudioSource heatSweep2SFX;
        public AudioSource heatSweep3SFX;
        public AudioSource heatSweep4SFX;
        public AudioSource coldSweep1SFX;
        public AudioSource coldSweep2SFX;
        public AudioSource coldSweep3SFX;
        public AudioSource coldSweep4SFX;
        public AudioSource shockSweep1SFX;
        public AudioSource shockSweep2SFX;
        public AudioSource shockSweep3SFX;
        public AudioSource shockSweep4SFX;
        public AudioSource waveSweep1SFX;
        public AudioSource waveSweep2SFX;
        public AudioSource waveSweep3SFX;
        public AudioSource waveSweep4SFX;

        // Throw SFX Variables
        public AudioSource neutralThrowSFX;
        public AudioSource heatThrowSFX;
        public AudioSource coldThrowSFX;
        public AudioSource shockThrowSFX;
        public AudioSource waveThrowSFX;
        public AudioSource neutralChargedThrowSFX;
        public AudioSource heatChargedThrowSFX;
        public AudioSource coldChargedThrowSFX;
        public AudioSource shockChargedThrowSFX;
        public AudioSource waveChargedThrowSFX;

        private Dictionary<string, AudioSource> sfxDictionary;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            playerMovement = GetComponent<HandleMovement>();
            attackEffectController = GetComponentInChildren<AttackEffectController>();
            chargeEffectController = GetComponentInChildren<ChargeEffectController>();
            player = GetComponent<Player>();

            // Initialize the SFX dictionary
            sfxDictionary = new Dictionary<string, AudioSource>
            {
                { "NeutralSweep-1", neutralSweep1SFX },
                { "NeutralSweep-2", neutralSweep2SFX },
                { "NeutralSweep-3", neutralSweep3SFX },
                { "NeutralSweep-4", neutralSweep4SFX },
                { "HeatSweep-1", heatSweep1SFX },
                { "HeatSweep-2", heatSweep2SFX },
                { "HeatSweep-3", heatSweep3SFX },
                { "HeatSweep-4", heatSweep4SFX },
                { "ColdSweep-1", coldSweep1SFX },
                { "ColdSweep-2", coldSweep2SFX },
                { "ColdSweep-3", coldSweep3SFX },
                { "ColdSweep-4", coldSweep4SFX },
                { "ShockSweep-1", shockSweep1SFX },
                { "ShockSweep-2", shockSweep2SFX },
                { "ShockSweep-3", shockSweep3SFX },
                { "ShockSweep-4", shockSweep4SFX },
                { "WaveSweep-1", waveSweep1SFX },
                { "WaveSweep-2", waveSweep2SFX },
                { "WaveSweep-3", waveSweep3SFX },
                { "WaveSweep-4", waveSweep4SFX },
                { "NeutralThrow", neutralThrowSFX },
                { "HeatThrow", heatThrowSFX },
                { "ColdThrow", coldThrowSFX },
                { "ShockThrow", shockThrowSFX },
                { "WaveThrow", waveThrowSFX },
                { "NeutralChargedThrow", neutralChargedThrowSFX },
                { "HeatChargedThrow", heatChargedThrowSFX },
                { "ColdChargedThrow", coldChargedThrowSFX },
                { "ShockChargedThrow", shockChargedThrowSFX },
                { "WaveChargedThrow", waveChargedThrowSFX }
            };
        }

        void Update()
        {
            HandlePlayerAttack();
            ChangeAttackStyle();
            ChangeElement();

            if (isThrowing)
            {
                chargeEffectController.UpdatePosition(!spriteRenderer.flipX);
            }
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

                    // Reset the attack counter if the time between attacks is greater than 0.6f
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
                            PlaySFX(currentElement.ToString(), 1);
                            StartCoroutine(AttackCooldown(1.0f / attackSpeed)); // Regular cooldown
                            break;
                        case 2:
                            animator.SetTrigger("Attack_2");
                            attackEffectController.Attack("2", !spriteRenderer.flipX, currentElement.ToString());
                            PlaySFX(currentElement.ToString(), 2);
                            StartCoroutine(AttackCooldown(1.0f / attackSpeed)); // Regular cooldown
                            break;
                        case 3:
                            animator.SetTrigger("Attack_1");
                            attackEffectController.Attack("1", !spriteRenderer.flipX, currentElement.ToString());
                            PlaySFX(currentElement.ToString(), 3);
                            StartCoroutine(AttackCooldown(1.0f / attackSpeed)); // Regular cooldown
                            break;
                        case 4:
                            animator.SetTrigger("Attack_3");
                            attackEffectController.Attack("1", !spriteRenderer.flipX, currentElement.ToString());
                            PlaySFX(currentElement.ToString(), 4);
                            StartCoroutine(SlideCharacter());
                            StartCoroutine(AttackCooldown(3.0f / attackSpeed)); // Longer cooldown for the fourth attack
                            attackCounter = 0; // Reset the counter after the last attack move
                            break;
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.K) && !isThrowCooldown)
                {
                    isThrowing = true;
                    keyDownTime = Time.time; // Record the time when the key was pressed down
                    StartCoroutine(HandleAim());
                }

                if (Input.GetKeyUp(KeyCode.K) && isThrowing)
                {
                    isThrowing = false;
                    float keyHoldDuration = Time.time - keyDownTime; // Calculate the duration the key was held down

                    if (keyHoldDuration >= chargeTime)
                    {
                        PerformChargedThrow();
                        animator.SetBool("Aim", false);
                        chargeEffectController.StopCharge();
                        playerMovement.SetPlayerSpeed(5.0f); 
                    }
                    else
                    {
                        PerformNormalThrow();
                        animator.SetBool("Aim", false);
                        chargeEffectController.StopCharge();
                        playerMovement.SetPlayerSpeed(5.0f);
                    }

                    StartCoroutine(ThrowCooldown());
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

        private IEnumerator HandleAim()
        {
            animator.SetBool("PreAim", true);
            yield return new WaitForSeconds(3f / 60f);
            animator.SetBool("PreAim", false);
            playerMovement.SetPlayerSpeed(2f);
            animator.SetBool("Aim", true);
            chargeEffectController.Charge(!spriteRenderer.flipX, currentElement.ToString());
            StartCoroutine(ChargeThrow());
        }

        private IEnumerator ChargeThrow()
        {
            yield return new WaitForSeconds(chargeTime);
        }

        private void PerformNormalThrow()
        {
            if (throwCycle)
            {
                animator.SetTrigger("Throw_1");
            }
            else
            {
                animator.SetTrigger("Throw_2");
            }
            throwCycle = !throwCycle;

            LaunchProjectile(currentElement.ToString(), false);
            PlayThrowSFX(currentElement.ToString(), false);
            StartCoroutine(ThrowCooldown());
        }

        private void PerformChargedThrow()
        {
            animator.SetTrigger("ChargedThrow");
            LaunchProjectile(currentElement.ToString(), true);
            PlayThrowSFX(currentElement.ToString(), true);
        }

        private void LaunchProjectile(string currentElement, bool isCharged)
        {
            // Instantiate the projectile prefab
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            // Get the direction to launch the projectile
            Vector2 launchDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;

            // Launch the projectile
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.Launch(launchDirection, currentElement, isCharged);
            }
        }

        private IEnumerator ThrowCooldown()
        {
            isThrowCooldown = true;
            yield return new WaitForSeconds(0.5f); // Adjust the cooldown duration as needed
            isThrowCooldown = false;
        }

        private void PlaySFX(string element, int attackNumber)
        {
            string sfxKey = $"{element}Sweep-{attackNumber}";
            if (sfxDictionary.ContainsKey(sfxKey))
            {
                sfxDictionary[sfxKey].Play();
            }
        }

        private void PlayThrowSFX(string element, bool isCharged)
        {
            string sfxKey = isCharged ? $"{element}ChargedThrow" : $"{element}Throw";
            if (sfxDictionary.ContainsKey(sfxKey))
            {
                sfxDictionary[sfxKey].Play();
            }
        }
    }
}