using UnityEngine;
using UnityEngine.InputSystem.Android;

namespace CrossCode2D.Player
{
    public class PlayerController : MonoBehaviour
    {
        private int movementLayerIndex;
        private int combatLayerIndex;

        private Animator animator;
        private HandleMovement handleMovement;
        private HandleAttack handleAttack;

        void Start()
        {
            animator = GetComponent<Animator>();
            handleMovement = GetComponent<HandleMovement>();
            handleAttack = GetComponent<HandleAttack>();

            movementLayerIndex = animator.GetLayerIndex("Movement");
            combatLayerIndex = animator.GetLayerIndex("Combat");

            animator.SetLayerWeight(movementLayerIndex, 1);
            animator.SetLayerWeight(combatLayerIndex, 1);
        }

        void Update()
        {
        }

        void FixedUpdate()
        {
        }
    }
}