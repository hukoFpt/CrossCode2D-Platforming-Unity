using UnityEngine;
using UnityEngine.UI;
using CrossCode2D.Player;

namespace CrossCode2D.UI
{
    public class StyleUIManager : MonoBehaviour
    {
        public Image MeleeStyleActive;
        public Image ThrowStyleActive;
        public Image MeleeStyle;
        public Image ThrowStyle;

        private CrossCode2D.Player.HandleAttack player; 

        void Start()
        {
            // Find the player object and get the HandleAttack component
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<CrossCode2D.Player.HandleAttack>();
        }

        void Update()
        {
            UpdatePlayerStyle();
        }

        public void UpdatePlayerStyle()
        {
            switch (player.currentStyle)
            {
                case HandleAttack.CombatStyle.Melee:
                    MeleeStyleActive.enabled = true;
                    ThrowStyleActive.enabled = false;
                    SetImageOpacity(MeleeStyle, 1f); // Fully opaque
                    SetImageOpacity(ThrowStyle, 0.5f); // Semi-transparent
                    break;
                case HandleAttack.CombatStyle.Throw:
                    MeleeStyleActive.enabled = false;
                    ThrowStyleActive.enabled = true;
                    SetImageOpacity(MeleeStyle, 0.5f); // Semi-transparent
                    SetImageOpacity(ThrowStyle, 1f); // Fully opaque
                    break;
            }
        }

        private void SetImageOpacity(Image image, float opacity)
        {
            Color color = image.color;
            color.a = opacity;
            image.color = color;
        }
    }
}