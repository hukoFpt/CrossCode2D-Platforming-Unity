using System.Collections;
using UnityEngine;

namespace CrossCode2D.Enemies
{
    public class JunglePlantController : MonoBehaviour
    {
        private JunglePlant junglePlant;
        private Rigidbody2D rb;
        private Animator animator;
        private SpriteRenderer spriteRenderer;

        void Start()
        {
            InitializeComponents();
        }

        void Update()
        {
        }

        void FixedUpdate()
        {
        }

        private void InitializeComponents()
        {
            junglePlant = GetComponent<JunglePlant>();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        
    }
}