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
        public Text HPText;
        public Text LevelText;
        public Text AttackText;
        public Text DefenseText;

        private CrossCode2D.Player.Player player;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<CrossCode2D.Player.Player>();
        }

        private void Update()
        {
            UpdateHealth(player.stats.currentHP, player.stats.maxHP);
            UpdateStats(player.stats.level, player.stats.attack, player.stats.defense);
        }

        public void UpdateHealth(float currentHealth, float maxHealth)
        {
            HPFill1.fillAmount = currentHealth / maxHealth;
            HPFill2.fillAmount = currentHealth / maxHealth;
            HPFill3.fillAmount = currentHealth / maxHealth;

            HPText.text = $"{currentHealth}";
        }

        public void UpdateStats(int level, float attack, float defense)
        {
            LevelText.text = player.stats.level.ToString();
            AttackText.text = player.stats.attack.ToString();
            DefenseText.text = player.stats.defense.ToString();
        }
    }
}