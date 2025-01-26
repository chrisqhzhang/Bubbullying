using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void LaunchBubble()
    {
        SceneManager.LoadScene("BubbleApp");
    }

    public void LaunchAnima()
    {
        SceneManager.LoadScene("Animasphere");
    }

    public void BackToRoom()
    {
        SceneManager.LoadScene("In-Game");
    }
}
