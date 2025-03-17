using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Leaderboard : MonoBehaviour
{
    public Text leaderboardNameText;
    public Text leaderboardTimeText; 
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
        leaderboardNameText.text = ""; 
        leaderboardTimeText.text = ""; 

        for (int i = 0; i < leaderboardEntries.Count; i++)
        {
            LeaderboardEntry entry = leaderboardEntries[i];
            string colorTag = "";
            string positionSuffix = GetPositionSuffix(i + 1);

            switch (i)
            {
                case 0:
                    colorTag = "<color=#FFD700>";
                    break;
                case 1:
                    colorTag = "<color=#C0C0C0>";
                    break;
                case 2:
                    colorTag = "<color=#CD7F32>";
                    break;
                default:
                    colorTag = "<color=#FFFFFF>";
                    break;
            }

            leaderboardNameText.text += $"{colorTag}{i + 1}{positionSuffix}. {entry.name}</color>\n";
            leaderboardTimeText.text += $"{colorTag}{FormatTime(entry.time)}</color>\n";
        }
    }

    private string GetPositionSuffix(int position)
    {
        if (position % 10 == 1 && position != 11) return "st";
        if (position % 10 == 2 && position != 12) return "nd";
        if (position % 10 == 3 && position != 13) return "rd";
        return "th";
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        return string.Format("{0:00}m{1:00}s", minutes, seconds);
    }
}