using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Load the game scene (replace "NoahsPark" with the name of your game scene)
        SceneManager.LoadScene("NoahsPark");
    }

    public void OpenLeaderboard()
    {
        // Placeholder for the leaderboard functionality
        Debug.Log("Leaderboard opened");
    }

    public void ExitGame()
    {
        // Quit the game
        Debug.Log("Game is quitting");
        Application.Quit();
    }
}