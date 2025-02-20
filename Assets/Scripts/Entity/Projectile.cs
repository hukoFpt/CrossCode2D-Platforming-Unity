using CrossCode2D.Enemies;
using UnityEngine;
using System;
using System.Collections;

namespace CrossCode2D.Player
{
    public class Projectile : MonoBehaviour
    {
        private Animator animator;
        private new Rigidbody2D rigidbody;
        private SpriteRenderer spriteRenderer;
        private Collider2D attackCollider;
        private Vector2 direction;

        public float speed = 10f;
        public float damage = 10f;
        public float lifetime = 5f;
        public Guid projectileId; // Unique identifier for each projectile
        private string currentElement;
        private bool isCharged;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            attackCollider = GetComponent<Collider2D>();
            attackCollider.isTrigger = true;
            projectileId = Guid.NewGuid(); // Assign a unique ID to each projectile
        }

        private void Update()
        {
            // Move the projectile in the set direction
            transform.position += (Vector3)direction * speed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }

        public void Launch(Vector2 launchDirection, string element, bool charged)
        {
            // Ensure the projectile only flies in the horizontal direction
            launchDirection.y = 0;
            direction = launchDirection.normalized;

            currentElement = element;
            isCharged = charged;

            // Set the projectile's animation based on the current element and charge state
            string animationName = isCharged ? $"{currentElement}BallCharged" : $"{currentElement}Ball";
            animator.Play(animationName);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                IEnemy enemy = collision.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Debug.Log($"Player attacked {collision.name} and dealt {damage} damage.");
                }
                speed = 0;
                StartCoroutine(PlayCollisionAnimationAndDestroy());
            }

            if (collision.CompareTag("Ground"))
            {
                speed = 0;
                StartCoroutine(PlayCollisionAnimationAndDestroy());
            }
        }

        private IEnumerator PlayCollisionAnimationAndDestroy()
        {
            // Play the collision animation based on the current element and charge state
            string collisionAnimationName = isCharged ? $"{currentElement}BallChargedCollide" : $"{currentElement}BallCollide";
            animator.Play(collisionAnimationName);

            // Wait for the animation to finish
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            // Destroy the projectile
            Destroy(gameObject);
        }
    }
}