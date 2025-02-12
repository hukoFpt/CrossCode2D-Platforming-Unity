using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public string currentElement = "Neutral";
    public float currentHP = 100f;
    public float maxHP = 100f;

    private ElementUIManager uiManager;

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
    }

    void Update()
    {
        HandleElementChange();
        uiManager.UpdateElementSprite();
    }

    public void ChangeElement(string newElement)
    {
        currentElement = newElement;
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
}