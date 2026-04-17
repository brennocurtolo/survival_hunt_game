using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    private int score = 0;

    void Start()
    {
        UpdateUI();
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = "SCORE: " + score;
    }
}