using UnityEngine;

public class BarrierController : MonoBehaviour
{
    public string jungleShockcatName = "JungleShockcat 1"; // Name of the JungleShockcat instances
    private GameObject[] jungleShockcats; // Array to hold references to the JungleShockcat instances

    private void Start()
    {
        // Find all JungleShockcat instances with the specified name
        jungleShockcats = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void Update()
    {
        // Check if all JungleShockcat instances with the specified name are gone
        bool allGone = true;
        foreach (GameObject shockcat in jungleShockcats)
        {
            if (shockcat != null && shockcat.name == jungleShockcatName)
            {
                allGone = false;
                break;
            }
        }

        // If all JungleShockcat instances with the specified name are gone, remove the barrier
        if (allGone)
        {
            Destroy(gameObject); // Remove the barrier
        }
    }
}