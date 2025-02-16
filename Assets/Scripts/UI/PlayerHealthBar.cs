using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CrossCode2D.Player; // Correct namespace

namespace CrossCode2D.UI
{
    public class PlayerHealthBar : MonoBehaviour
    {
        public Image HPFill1;
        public Image HPFill2;
        public Image HPFill3;
        public TMP_Text HPText;

        private CrossCode2D.Player.Player player; // Fully qualified name to avoid ambiguity

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<CrossCode2D.Player.Player>();
        }

        private void Update()
        {
            UpdateHealth(player.currentHP, player.maxHP);
        }

        public void UpdateHealth(float currentHealth, float maxHealth)
        {
            HPFill1.fillAmount = currentHealth / maxHealth;
            HPFill2.fillAmount = currentHealth / maxHealth;
            HPFill3.fillAmount = currentHealth / maxHealth;

            HPText.text = $"{currentHealth}";
        }
    }
}