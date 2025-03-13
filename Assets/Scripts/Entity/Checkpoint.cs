using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    private Animator animator;
    private GameClock gameClock;
    public GameObject playerInputCanvas; // Reference to the PlayerInputCanvas
    private InputField playerNameInput; // Reference to the InputField
    private Button submitButton; // Reference to the SubmitButton

    private void Awake()
    {
        animator = GetComponent<Animator>();
        gameClock = FindFirstObjectByType<GameClock>(); // Find the GameClock instance in the scene

        // Find the PlayerInputCanvas and its components
        playerInputCanvas = GameObject.Find("InputSystem");
        playerNameInput = playerInputCanvas.transform.Find("InputField").GetComponent<InputField>();
        submitButton = playerInputCanvas.transform.Find("SubmitButton").GetComponent<Button>();

        // Add listener to the SubmitButton
        submitButton.onClick.AddListener(OnSubmit);

        // Ensure the canvas is initially disabled
        playerInputCanvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TriggerCompleteAnimation();
            StopClock();

            CrossCode2D.Player.Player player = collision.GetComponent<CrossCode2D.Player.Player>();
            player.DisableControlsAndComplete();

            // Show the PlayerInputCanvas
            playerInputCanvas.SetActive(true);
        }
    }

    private void TriggerCompleteAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Complete");
        }
    }

    private void StopClock()
    {
        if (gameClock != null)
        {
            gameClock.StopClock();
        }
        else
        {
            Debug.LogError("GameClock instance not found.");
        }
    }

    private void OnSubmit()
    {
        string playerName = playerNameInput.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            gameClock.UploadPlayTime("https://67c8f39e0acf98d070882af5.mockapi.io/time", playerName); // Replace with your actual API URL
            playerInputCanvas.SetActive(false);

            SceneManager.LoadScene("Leaderboard");
        }
    }
}