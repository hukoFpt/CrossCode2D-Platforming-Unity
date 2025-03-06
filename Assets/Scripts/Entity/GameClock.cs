using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class GameClock : MonoBehaviour
{
    public Text clockText; // Optional: UI Text to display the clock
    private float playTime;
    private bool isRunning;

    private void Start()
    {
        playTime = 0f;
        isRunning = true;
        StartCoroutine(UpdateClock());
    }

    private IEnumerator UpdateClock()
    {
        while (isRunning)
        {
            playTime += Time.deltaTime;
            if (clockText != null)
            {
                clockText.text = FormatTime(playTime);
            }
            yield return null;
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        int milliseconds = Mathf.FloorToInt((time - minutes * 60 - seconds) * 1000);
        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    public void StopClock()
    {
        isRunning = false;
    }

    public void UploadPlayTime(string apiUrl)
    {
        Debug.Log("Uploading play time...");
        StartCoroutine(UploadPlayTimeCoroutine(apiUrl));
    }

    private IEnumerator UploadPlayTimeCoroutine(string apiUrl)
    {
        WWWForm form = new WWWForm();
        form.AddField("time", playTime.ToString());
        form.AddField("name", "Huko");

        using (UnityWebRequest www = UnityWebRequest.Post(apiUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error uploading play time: " + www.error);
            }
            else
            {
                Debug.Log("Play time uploaded successfully.");
            }
        }
    }
}