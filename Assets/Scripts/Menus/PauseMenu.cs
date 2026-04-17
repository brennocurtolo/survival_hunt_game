using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pauseMenuUI;

    private void Start()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
    }
    
    private void Update()
    {
        if (GameManager.Instance.gameEnded) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        AudioListener.pause = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        AudioListener.pause = false;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}