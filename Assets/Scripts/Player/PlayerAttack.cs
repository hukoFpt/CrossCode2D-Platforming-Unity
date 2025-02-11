using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement; // Reference to the PlayerMovement script
    private AttackEffectController attackNeutralController; // Reference to the AttackNeutralController
    private Player player; // Reference to the Player script

    private bool isCooldown = false;
    private int attackCounter = 0;
    private float lastAttackTime = 0f;

    public float slideDistance = 1.2f;
    public float slideDuration = 0.4f;

    public float attackSpeed;
    private float GetAttackSpeed(string element)
    {
        switch (element)
        {
            case "Neutral":
                return 4f;
            case "Heat":
            case "Cold":
                return 3f;
            case "Shock":
            case "Wave":
                return 5f;
            default:
                return 4f;
        }
    }
    public float fourthAttackCooldown => 3 / attackSpeed; // Cooldown duration for the fourth attack

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>(); // Get the PlayerMovement component
        attackNeutralController = GetComponentInChildren<AttackEffectController>();
        player = GetComponent<Player>();
    }

    void Update()
    {
        HandleAttack();
    }

    public void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.K) && !isCooldown)
        {
            float timeSinceLastAttack = Time.time - lastAttackTime;
            string currentElement = player.currentElement;

            attackSpeed = GetAttackSpeed(currentElement);

            if (timeSinceLastAttack > 0.6f)
            {
                attackCounter = 0; // Reset the counter if the time between attacks is greater than 1 second
            }

            attackCounter++;
            lastAttackTime = Time.time;

            // Disable movement during attack
            playerMovement.DisableMovement();
            StartCoroutine(EnableMovementAfterDelay(1f)); // Re-enable movement after 1 second

            switch (attackCounter)
            {
                case 1:
                    animator.SetTrigger("Attack_1");
                    attackNeutralController.Attack("1", !spriteRenderer.flipX, currentElement); // Trigger the attack effect                                                                               // Trigger the attack effect
                    StartCoroutine(AttackCooldown(1.0f / attackSpeed)); // Regular cooldown
                    break;
                case 2:
                    animator.SetTrigger("Attack_2");
                    attackNeutralController.Attack("2", !spriteRenderer.flipX, currentElement); // Trigger the attack effect
                    StartCoroutine(AttackCooldown(1.0f / attackSpeed)); // Regular cooldown
                    break;
                case 3:
                    animator.SetTrigger("Attack_1");
                    attackNeutralController.Attack("1", !spriteRenderer.flipX, currentElement); // Trigger the attack effect
                    StartCoroutine(AttackCooldown(1.0f / attackSpeed)); // Regular cooldown
                    break;
                case 4:
                    animator.SetTrigger("Attack_3");
                    attackNeutralController.Attack("1", !spriteRenderer.flipX, currentElement); // Trigger the attack effect
                    StartCoroutine(SlideCharacter());
                    StartCoroutine(AttackCooldown(fourthAttackCooldown)); // Longer cooldown for the fourth attack
                    attackCounter = 0; // Reset the counter after the last attack move
                    break;
            }
        }
    }

    private IEnumerator SlideCharacter()
    {
        // Slide the character based on the facing direction
        Vector3 slideDirection = spriteRenderer.flipX ? Vector3.left : Vector3.right;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + slideDirection * slideDistance;
        float elapsedTime = 0f;

        while (elapsedTime < slideDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / slideDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
    }

    private IEnumerator AttackCooldown(float cooldownDuration)
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownDuration);
        isCooldown = false;
    }

    private IEnumerator EnableMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerMovement.EnableMovement();
    }
}