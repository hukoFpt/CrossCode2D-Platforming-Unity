using UnityEngine;
using UnityEngine.UI;

public class ElementUIManager: MonoBehaviour
{
    public Sprite neutralIcon;
    public Sprite heatIcon;
    public Sprite coldIcon;
    public Sprite shockIcon;
    public Sprite waveIcon;

    private Player Player => FindFirstObjectByType<Player>();

    void Start()
    {
        UpdateElementSprite();
    }

    public void UpdateElementSprite()
    {
        switch (Player.currentElement)
        {
            case "Neutral":
                GetComponent<Image>().sprite = neutralIcon;
                break;
            case "Heat":
                GetComponent<Image>().sprite = heatIcon;
                break;
            case "Cold":
                GetComponent<Image>().sprite = coldIcon;
                break;
            case "Shock":
                GetComponent<Image>().sprite = shockIcon;
                break;
            case "Wave":
                GetComponent<Image>().sprite = waveIcon;
                break;
        }
    }
}
