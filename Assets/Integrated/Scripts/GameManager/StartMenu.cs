using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject bubbleApp;
    [SerializeField] private GameObject animaApp;

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
        //SceneManager.LoadScene("BubbleApp");
        bubbleApp.SetActive(true);
    }

    public void CloseBubble()
    {
        //SceneManager.LoadScene("In-Game");

        //GameObject bubbleApp = GameObject.Find("BubbleApp");
        //if (bubbleApp != null)
        //{
        //    bubbleApp.SetActive(false);
        //}
        bubbleApp.SetActive(false);
    }
}
