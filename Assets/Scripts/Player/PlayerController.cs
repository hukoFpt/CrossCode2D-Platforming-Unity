using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerDash playerDash;
    private PlayerJump playerJump;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerDash = GetComponent<PlayerDash>();
        playerJump = GetComponent<PlayerJump>();
    }

    void Update()
    {
        playerMovement.HandleMovement();
        playerDash.HandleDash();
        playerJump.HandleJump();
    }

    void FixedUpdate()
    {
        playerMovement.FixedHandleMovement();
        playerDash.FixedHandleDash();
    }
}