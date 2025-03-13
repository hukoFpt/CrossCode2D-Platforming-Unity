using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Leaderboard : MonoBehaviour
{
    public Text leaderboardText; // Reference to the Text element to display the leaderboard

    private void Start()
    {
        StartCoroutine(FetchLeaderboardData());
    }
    
    [System.Serializable]
    public class LeaderboardEntry
    {
        public string createdAt;
        public string name;
        public float time;
        public string id;
    }

    private IEnumerator FetchLeaderboardData()
    {
        string apiUrl = "https://67c8f39e0acf98d070882af5.mockapi.io/time";
        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error fetching leaderboard data: " + www.error);
            }
            else
            {
                List<LeaderboardEntry> leaderboardEntries = ParseLeaderboardData(www.downloadHandler.text);
                DisplayLeaderboard(leaderboardEntries);
            }
        }
    }

    private List<LeaderboardEntry> ParseLeaderboardData(string jsonData)
    {
        LeaderboardEntry[] entries = JsonHelper.FromJson<LeaderboardEntry>(jsonData);
        List<LeaderboardEntry> sortedEntries = new List<LeaderboardEntry>(entries);
        sortedEntries.Sort((x, y) => x.time.CompareTo(y.time));
        return sortedEntries.GetRange(0, Mathf.Min(10, sortedEntries.Count)); 
    }

    private void DisplayLeaderboard(List<LeaderboardEntry> leaderboardEntries)
    {
        for (int i = 0; i < leaderboardEntries.Count; i++)
        {
            LeaderboardEntry entry = leaderboardEntries[i];
            leaderboardText.text += $"{i + 1}. {entry.name}: {FormatTime(entry.time)}\n";
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        return string.Format("{0:00}m{1:00}s", minutes, seconds);
    }
}