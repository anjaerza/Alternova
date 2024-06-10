using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardEntryUI : MonoBehaviour
{
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI scoreText;

    public void Setup(LeaderboardEntry entry)
    {
        playerNameText.text = entry.playerName;
        scoreText.text = entry.score.ToString();
    }
}