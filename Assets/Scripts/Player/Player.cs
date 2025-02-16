using UnityEngine;
using System.Collections.Generic;

namespace CrossCode2D.Player
{

    public class Player : MonoBehaviour
    {
        public float currentHP = 1200f;
        public float maxHP = 1200f;
        void Start()
        {
        }

        void Update()
        {
        }

        public void TakeDamage(float damage)
        {
            currentHP -= damage;
            if (currentHP <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            Debug.Log("Player died!");
        }
    }
}