using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    
    [SerializeField] private Image imageA;
    [SerializeField] private Image imageB;

    private GameObject notif;
    private GameObject popupAnim;

    private void PushNotification()
    {
        imageA.gameObject.SetActive(true);
        imageB.gameObject.SetActive(true);
        
        WaitForSeconds wait = new WaitForSeconds(3f);
        imageB.gameObject.SetActive(false);
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        WaitForSeconds wait = new WaitForSeconds(1.5f);
        PushNotification();
    }
}
