using UnityEngine;
using UnityEngine.UI;

public class StyleUIManager : MonoBehaviour
{
    public Image MeleeStyleActive;
    public Image ThrowStyleActive;
    public Image MeleeStyle;
    public Image ThrowStyle;

    private Player Player => FindFirstObjectByType<Player>();

    public void UpdatePlayerStyle()
    {
        switch(Player.currentStyle)
        {
            case "Melee":
                MeleeStyleActive.enabled = true;
                ThrowStyleActive.enabled = false;
                SetImageOpacity(MeleeStyle, 1f); // Fully opaque
                SetImageOpacity(ThrowStyle, 0.5f); // Semi-transparent
                break;
            case "Throw":
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