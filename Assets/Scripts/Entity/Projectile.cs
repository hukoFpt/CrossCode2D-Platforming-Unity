using CrossCode2D.Enemies;
using UnityEngine;


namespace CrossCode2D.Player
{
    public class Projectile : MonoBehaviour
    {
        private Animator animator;
        private Rigidbody2D rigidbody;
        private SpriteRenderer spriteRenderer;
        private Collider2D attackCollider;
        private Vector2 direction;

        public float speed = 10f;
        public float damage = 10f;
        public float lifetime = 5f;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            attackCollider = GetComponent<Collider2D>();
            attackCollider.isTrigger = true;
        }

        private void Update()
        {
            // Move the projectile in the set direction
            transform.position += (Vector3)direction * speed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }

        public void Launch(Vector2 launchDirection)
        {
            // Ensure the projectile only flies in the horizontal direction
            launchDirection.y = 0;
            direction = launchDirection.normalized;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                IEnemy enemy = collision.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(10f);
                    Debug.Log($"Player attacked {collision.name} and dealt {10f} damage.");
                }
                Destroy(gameObject);
            }

            if (collision.CompareTag("Ground"))
            {
                Destroy(gameObject);
            }
        }
    }
}