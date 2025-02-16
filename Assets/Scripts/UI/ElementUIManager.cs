using UnityEngine;
using UnityEngine.UI;
using CrossCode2D.Player;

namespace CrossCode2D.UI
{
    public class ElementUIManager : MonoBehaviour
    {
        public Sprite neutralIcon;
        public Sprite heatIcon;
        public Sprite coldIcon;
        public Sprite shockIcon;
        public Sprite waveIcon;
        private CrossCode2D.Player.HandleAttack player;
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<CrossCode2D.Player.HandleAttack>();
        }

        void Update()
        {
            UpdateElementSprite();
        }

        public void UpdateElementSprite()
        {
            switch (player.currentElement)
            {
                case HandleAttack.Element.Neutral:
                    GetComponent<Image>().sprite = neutralIcon;
                    break;
                case HandleAttack.Element.Heat:
                    GetComponent<Image>().sprite = heatIcon;
                    break;
                case HandleAttack.Element.Cold:
                    GetComponent<Image>().sprite = coldIcon;
                    break;
                case HandleAttack.Element.Shock:
                    GetComponent<Image>().sprite = shockIcon;
                    break;
                case HandleAttack.Element.Wave:
                    GetComponent<Image>().sprite = waveIcon;
                    break;
            }
        }
    }
}