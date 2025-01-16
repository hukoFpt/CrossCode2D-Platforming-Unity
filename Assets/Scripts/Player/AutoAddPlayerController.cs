using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerDash))]
[RequireComponent(typeof(PlayerJump))]
[RequireComponent(typeof(PlayerAttack))]
public class AutoAddComponents : MonoBehaviour
{
    void Awake()
    {
        // Ensure all required components are added
        if (GetComponent<Rigidbody2D>() == null)
            gameObject.AddComponent<Rigidbody2D>();

        if (GetComponent<Animator>() == null)
            gameObject.AddComponent<Animator>();

        if (GetComponent<SpriteRenderer>() == null)
            gameObject.AddComponent<SpriteRenderer>();

        if (GetComponent<PlayerController>() == null)
            gameObject.AddComponent<PlayerController>();

        if (GetComponent<PlayerMovement>() == null)
            gameObject.AddComponent<PlayerMovement>();

        if (GetComponent<PlayerDash>() == null)
            gameObject.AddComponent<PlayerDash>();

        if (GetComponent<PlayerJump>() == null)
            gameObject.AddComponent<PlayerJump>();

        if (GetComponent<PlayerAttack>() == null)
            gameObject.AddComponent<PlayerAttack>();
    }
}