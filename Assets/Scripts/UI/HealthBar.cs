using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillImage;
    private Transform target;
    public Vector3 offset = new Vector3(0, -0.8f, 0); // Offset to position the health bar below the enemy

    public void Initialize(Transform target)
    {
        this.target = target;
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        fillImage.fillAmount = currentHealth / maxHealth;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset; // Position the health bar below the enemy
        }
    }
}