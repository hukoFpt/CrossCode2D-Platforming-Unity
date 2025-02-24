using UnityEngine;
using System.Collections;

namespace CrossCode2D.Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] public EnemyStats stats = new EnemyStats();
        private HealthBar healthBar;
        public GameObject HealthBarPrefab;
        public EnemyStats Stats => stats;
        public HealthBar HealthBar => healthBar;
        private SpriteRenderer spriteRenderer;
        private Rigidbody2D rigidBody;

        protected virtual void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            rigidBody = GetComponent<Rigidbody2D>();

            GameObject healthBarInstance = Instantiate(HealthBarPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            healthBar = healthBarInstance.GetComponent<HealthBar>();
            healthBar.Initialize(transform);
        }

        protected virtual void Update()
        {
            healthBar.UpdateHealth(stats.currentHP, stats.maxHP);
        }

        public virtual void TakeDamage(float attack, EnemyStats enemyStats, string element, Vector2 direction)
        {
            float statDmgMod;

            if (attack > enemyStats.defense)
            {
                statDmgMod = 1 + Mathf.Pow(1 - (enemyStats.defense / attack), 0.5f) * 0.2f;
            }
            else
            {
                statDmgMod = Mathf.Pow(attack / enemyStats.defense, 1.5f);
            }

            float modifiedDamage = attack * statDmgMod;

            switch (element)
            {
                case "Heat":
                    modifiedDamage *= 1 - enemyStats.heatResistance;
                    break;
                case "Cold":
                    modifiedDamage *= 1 - enemyStats.coldResistance;
                    break;
                case "Shock":
                    modifiedDamage *= 1 - enemyStats.shockResistance;
                    break;
                case "Wave":
                    modifiedDamage *= 1 - enemyStats.waveResistance;
                    break;
                case "Neutral":
                    break;
            }

            stats.currentHP -= Mathf.Round(modifiedDamage);
            StartCoroutine(FlashWhite());
            ApplyKnockback(direction, 5.0f);

            if (stats.currentHP <= 0)
            {
                Die();
            }
        }

        public virtual void Die()
        {
            Destroy(gameObject);
            if (healthBar != null)
            {
                healthBar.gameObject.SetActive(false);
            }
        }

        private IEnumerator FlashWhite()
        {
            spriteRenderer.color = Color.gray;
            yield return new WaitForSeconds(0.05f);
            spriteRenderer.color = Color.white;
        }

        public void ApplyKnockback(Vector2 direction, float force)
        {
            Debug.Log("Applying knockback");
            rigidBody.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }
}