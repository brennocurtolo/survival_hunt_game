using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("SnowHunt");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}