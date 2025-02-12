using System.Collections;
using UnityEngine;

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
        rb.bodyType = RigidbodyType2D.Kinematic;

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

    public void TakeDamage(float damage)
    {
        StartCoroutine(FlashWhite());
        Debug.Log("JungleShockcat took " + damage + " damage.");
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
           
    }
    private IEnumerator FlashWhite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.gray; // Change color to white
            yield return new WaitForSeconds(0.05f); // Wait for 0.2 seconds
            spriteRenderer.color = originalColor; // Revert to original color
        }
    }

    private void Die()
    {
        // Handle death logic, e.g., play animation, disable object, etc.
        Debug.Log("JungleShockcat has died.");
        gameObject.SetActive(false);
    }
}