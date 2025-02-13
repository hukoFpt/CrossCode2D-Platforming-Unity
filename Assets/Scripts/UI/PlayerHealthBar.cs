using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Image HPFill1;
    public Image HPFill2;
    public Image HPFill3;

    public TMP_Text HPText;
    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        HPFill1.fillAmount = currentHealth / maxHealth;
        HPFill2.fillAmount = currentHealth / maxHealth;
        HPFill3.fillAmount = currentHealth / maxHealth;

        HPText.text = $"{currentHealth}";
    }
}