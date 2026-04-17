using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public EndGameMenu endGameMenu;

    public bool gameEnded { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        AudioListener.pause = false;
    }

    public void WinGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        DestroyAllEnemies();
        endGameMenu.ShowVictory();
    }

    public void LoseGame(GameObject player)
    {
        if (gameEnded) return;
        gameEnded = true;

        Destroy(player);
        endGameMenu.ShowDefeat();
    }

    void DestroyAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        UnityEngine.SceneManagement.SceneManager.LoadScene("SnowHunt");
    }
}