using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int movementLayerIndex;
    private int combatLayerIndex;

    private Animator animator;
    private PlayerMovement playerMovement;
    private PlayerDash playerDash;
    private PlayerJump playerJump;
    private PlayerAttack playerAttack;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerDash = GetComponent<PlayerDash>();
        playerJump = GetComponent<PlayerJump>();
        playerAttack = GetComponent<PlayerAttack>();

        movementLayerIndex = animator.GetLayerIndex("Movement");
        combatLayerIndex = animator.GetLayerIndex("Combat");

        animator.SetLayerWeight(movementLayerIndex, 1);
        animator.SetLayerWeight(combatLayerIndex, 1);
    }

    void Update()
    {
        playerMovement.HandleMovement();
        playerDash.HandleDash();
        playerJump.HandleJump();
        playerAttack.HandleAttack();
    }

    void FixedUpdate()
    {
        playerMovement.FixedHandleMovement();
        playerDash.FixedHandleDash();
    }
}