using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator animator;
    private GameClock gameClock;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        gameClock = FindFirstObjectByType<GameClock>(); // Find the GameClock instance in the scene
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TriggerCompleteAnimation();
            StopClockAndUploadTime();

            CrossCode2D.Player.Player player = collision.GetComponent<CrossCode2D.Player.Player>();
            player.DisableControlsAndComplete();
        }
    }

    private void TriggerCompleteAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Complete");
        }
    }

    private void StopClockAndUploadTime()
    {
        if (gameClock != null)
        {
            gameClock.StopClock();
            gameClock.UploadPlayTime("https://67c8f39e0acf98d070882af5.mockapi.io/time"); // Replace with your actual API URL
        }
        else
        {
            Debug.LogError("GameClock instance not found.");
        }
    }
}