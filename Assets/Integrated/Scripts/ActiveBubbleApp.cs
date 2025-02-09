using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActiveBubbleApp : MonoBehaviour
{
    void OnSceneLoaded(Scene scene)
    {
        if (scene.name == "In-Game")
        {
            this.gameObject.SetActive(false);
        }
        if (scene.name == "BubbleApp")
        {
            this.gameObject.SetActive(true);
        }
    }
}
