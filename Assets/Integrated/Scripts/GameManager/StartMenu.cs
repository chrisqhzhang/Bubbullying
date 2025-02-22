using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject bubbleApp;
    [SerializeField] private GameObject animaApp;
    [SerializeField] private GameObject animaAppBkg;
    [SerializeField] private GameObject homePage;

    // public BubbleAppManager bubbleAppManager;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(2);
        homePage.SetActive(true);
        bubbleApp.SetActive(false);
        animaApp.SetActive(false);
        animaAppBkg.SetActive(false);
    }

    public void LaunchBubble()
    {
        //SceneManager.LoadScene("BubbleApp");
        bubbleApp.SetActive(true);
        homePage.SetActive(false);
        animaApp.SetActive(false);
        animaAppBkg.SetActive(false);
    }

    public void CloseBubble()
    {
        //SceneManager.LoadScene(2);

        //GameObject bubbleApp = GameObject.Find("BubbleApp");
        //if (bubbleApp != null)
        //{
        //    bubbleApp.SetActive(false);
        //}
        bubbleApp.SetActive(false);
        homePage.SetActive(true);
    }
}
