using UnityEngine;

namespace CrossCode2D.Player
{
    public class Player : MonoBehaviour
    {
        public PlayerStats stats = new PlayerStats();
        private HandleMovement handleMovement;

        void Start()
        {
            stats.InitializeStats();
            handleMovement = GetComponent<HandleMovement>(); // Get the HandleMovement component
        }

        void Update()
        {
        }

        public void TakeDamage(float attackerAttack)
        {
            float statDmgMod;

            if (attackerAttack > stats.defense)
            {
                statDmgMod = 1 + Mathf.Pow(1 - (stats.defense / attackerAttack), 0.5f) * 0.2f;
            }
            else
            {
                statDmgMod = Mathf.Pow(attackerAttack / stats.defense, 1.5f);
            }

            float modifiedDamage = attackerAttack * statDmgMod;
            stats.currentHP -= Mathf.Round(modifiedDamage);

            if (stats.currentHP <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            Debug.Log("Player died!");
        }

        // Method to disable player controls and trigger the "Complete" animation
        public void DisableControlsAndComplete()
        {
            if (handleMovement != null)
            {
                handleMovement.DisableControlsAndComplete();
            }
        }
    }
}