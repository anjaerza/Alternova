using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LeaderboardUI : MonoBehaviour
{
    public GameObject entryPrefab;
    public Transform entryParent;

    private Leaderboard leaderboard;

    void Start()
    {
        LoadLeaderboard();
        DisplayLeaderboard();
    }

    public void LoadLeaderboard()
    {
        string filePath = Application.persistentDataPath + "/leaderboard.json";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            leaderboard = JsonUtility.FromJson<Leaderboard>(json);
        }
        else
        {
            leaderboard = new Leaderboard();
        }
    }

    public void DisplayLeaderboard()
    {
        foreach (Transform child in entryParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var entry in leaderboard.entries)
        {
            GameObject entryObj = Instantiate(entryPrefab, entryParent);
            entryObj.GetComponent<LeaderboardEntryUI>().Setup(entry);
        }
    }

    public void AddNewEntry(string playerName, int score)
    {
        LeaderboardEntry newEntry = new LeaderboardEntry(playerName, score);
        leaderboard.AddEntry(newEntry);

        string json = JsonUtility.ToJson(leaderboard);
        File.WriteAllText(Application.persistentDataPath + "/leaderboard.json", json);

        DisplayLeaderboard();
    }
}
