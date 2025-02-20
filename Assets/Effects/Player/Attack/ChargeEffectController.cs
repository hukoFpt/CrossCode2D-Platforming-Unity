using UnityEngine;

namespace CrossCode2D.Player
{
    public class ChargeEffectController : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private Animator animator;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            spriteRenderer.enabled = false;
        }

        public void Charge(bool characterFacingRight, string element)
        {
            spriteRenderer.enabled = true;
            UpdatePosition(characterFacingRight);

            switch (element)
            {
                case "Neutral":
                    animator.SetBool("Neutral", true);
                    break;
                case "Heat":
                    animator.SetBool("Heat", true);
                    break;
                case "Cold":
                    animator.SetBool("Cold", true);
                    break;
                case "Shock":
                    animator.SetBool("Shock", true);
                    break;
                case "Wave":
                    animator.SetBool("Wave", true);
                    break;
            }
        }

        public void StopCharge()
        {
            animator.SetBool("Neutral", false);
            animator.SetBool("Heat", false);
            animator.SetBool("Cold", false);
            animator.SetBool("Shock", false);
            animator.SetBool("Wave", false);

            spriteRenderer.enabled = false;
        }

        public void UpdatePosition(bool characterFacingRight)
        {
            if (characterFacingRight)
            {
                spriteRenderer.transform.localPosition = new Vector3(0.4f, 0.1f, -1f);
            }
            else
            {
                spriteRenderer.transform.localPosition = new Vector3(-0.4f, 0.1f, -1f);
            }
        }
    }
}