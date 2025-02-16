using UnityEngine;

namespace CrossCode2D.Enemies
{
    public class JungleShockcat : MonoBehaviour
    {
        public float maxHP = 100f;
        public float currentHP;

        private Rigidbody2D rb;
        private SpriteRenderer spriteRenderer;
        private Color originalColor;

        void Start()
        {
            // Initialize health
            currentHP = maxHP;

            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
            }
            rb.bodyType = RigidbodyType2D.Dynamic;

            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                originalColor = spriteRenderer.color;
            }
        }

        void Update()
        {
            // Placeholder for any update logic, if needed
        }
    }
}