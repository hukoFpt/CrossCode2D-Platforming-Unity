using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public string currentElement = "Neutral";
    public string currentStyle = "Melee";

    public float currentHP = 1200f;
    public float maxHP = 1200f;

    private ElementUIManager uiManager;
    private PlayerHealthBar healthBar;
    private StyleUIManager style;

    private Dictionary<KeyCode, string> elementKeyMap;

    void Start()
    {
        // Initialize the element key map
        elementKeyMap = new Dictionary<KeyCode, string>
        {
            { KeyCode.BackQuote, "Neutral" }, // `~` key
            { KeyCode.Alpha1, "Heat" },       // `1` key
            { KeyCode.Alpha2, "Cold" },       // `2` key
            { KeyCode.Alpha3, "Shock" },      // `3` key
            { KeyCode.Alpha4, "Wave" }        // `4` key
        };

        uiManager = FindFirstObjectByType<ElementUIManager>();
        healthBar = FindFirstObjectByType<PlayerHealthBar>();
        style = FindFirstObjectByType<StyleUIManager>();
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
        healthBar.UpdateHealth(currentHP, maxHP);
    }

    private void Die()
    {
        // Handle player death logic here
        Debug.Log("Player has died.");
    }

    void Update()
    {
        healthBar.UpdateHealth( currentHP, maxHP);
        HandleElementChange();
        uiManager.UpdateElementSprite();
        HandleStyleChange();
        style.UpdatePlayerStyle();
    }

    public void ChangeElement(string newElement)
    {
        currentElement = newElement;
    }

    public void ChangeStyle(string  newStyle)
    {
        currentStyle = newStyle;
    }

    private void HandleElementChange()
    {
        if (elementKeyMap == null)
        {
            Debug.LogError("elementKeyMap is null in HandleElementChange.");
            return;
        }

        foreach (var entry in elementKeyMap)
        {
            if (Input.GetKeyDown(entry.Key))
            {
                ChangeElement(currentElement == entry.Value ? "Neutral" : entry.Value);
                break;
            }
        }
    }

    private void HandleStyleChange()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeStyle(currentStyle == "Melee" ? "Throw" : "Melee");
        }
    }
}