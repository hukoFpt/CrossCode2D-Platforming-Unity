using UnityEngine;
using CrossCode2D.UI;

namespace CrossCode2D.Enemies
{
    public class JungleShockcat : MonoBehaviour, IEnemy
    {
        public EnemyStats stats = new EnemyStats();
        public HealthBar healthBar;
        public GameObject HealthBarPrefab;
        public EnemyStats Stats => stats;
        public HealthBar HealthBar => healthBar;

        void Start()
        {
            stats.InitializeStats();

            if (HealthBarPrefab != null)
            {
                GameObject healthBarInstance = Instantiate(HealthBarPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                healthBar = healthBarInstance.GetComponent<HealthBar>();
                if (healthBar != null)
                {
                    healthBar.Initialize(transform);
                }
                else
                {
                    Debug.LogError("HealthBar component not found on HealthBarPrefab.");
                }
            }
            else
            {
                Debug.LogError("HealthBarPrefab is not assigned.");
            }
        }

        void Update()
        {
            if (healthBar != null)
            {
                healthBar.UpdateHealth(stats.currentHP, stats.maxHP);
            }
        }

        public void TakeDamage(float attack)
        {
            float statDmgMod;

            if (attack > stats.defense)
            {
                statDmgMod = 1 + Mathf.Pow(1 - (stats.defense / attack), 0.5f) * 0.2f;
            }
            else
            {
                statDmgMod = Mathf.Pow(attack / stats.defense, 1.5f);
            }

            float modifiedDamage = attack * statDmgMod;
            stats.currentHP -= Mathf.Round(modifiedDamage);

            if (stats.currentHP <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            Destroy(gameObject);
            if (healthBar != null)
            {
                healthBar.gameObject.SetActive(false);
            }
        }
    }
}