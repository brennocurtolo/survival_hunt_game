using UnityEngine;
using TMPro;

public class EndGameMenu : MonoBehaviour
{
    public TextMeshProUGUI titleText;

    public GameObject panel;
    public GameObject restartButton;
    public GameObject playAgainButton;
    public GameObject exitButton;

    void Start()
    {
        restartButton.SetActive(false);
        playAgainButton.SetActive(false);
        exitButton.SetActive(false);
        panel.SetActive(false);
    }
    
    public void ShowVictory()
    {
        titleText.text = "VICTORY";
        
        panel.SetActive(true);
        playAgainButton.SetActive(true);
        exitButton.SetActive(true);

        Time.timeScale = 0f;

        AudioListener.pause = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ShowDefeat()
    {
        titleText.text = "DEFEAT";

        panel.SetActive(true);
        restartButton.SetActive(true);
        exitButton.SetActive(true);

        Time.timeScale = 0f;

        AudioListener.pause = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}