using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public TMP_Text popupText;
    
    [SerializeField] private GameObject notification;
    [SerializeField] private GameObject redDot;
    
    public GameObject[] images;
    public string[] texts;
    
    public TMP_Text dialogueText;
    
    private Button buttonNext;
    
    private Animator popupNotifAnimator;
    private Animator popupRedDotAnimator;
    
    private Animator crossfadeAnimator;
    
    private Queue<string> popupQueue;
    private Coroutine queueChecker;
    private bool isActive;
    
    [SerializeField] private CanvasGroup canvasGroupImg;
    [SerializeField] private CanvasGroup canvasGroupTxt;
    [SerializeField] private CanvasGroup canvasGroupBtn;
    
    private bool _fadeIn = false;
    private bool _fadeOut = false;
    public float fadeTime;
    public float loadSceneTime;

    /*private void ShowPopupNotif(string text)
    {
        isActive = true;
        notification.SetActive(true);
        popupText.text = text;
        popupNotifAnimator.Play("Popup_Notif");
    }
    
    private void ShowPopupRedDot()
    {
        isActive = true;
        redDot.SetActive(true);
        popupRedDotAnimator.Play("Popup_RedDot");
    }*/
    
    public void FadeIn()
    {
        _fadeIn = true;
    }

    public void FadeOut()
    {
        _fadeOut = true;
    }

    public void LoadScene()
    {
        
    }

    public void Crossfade()
    {
        StartCoroutine(CrossfadeDelay());
    }

    IEnumerator CrossfadeDelay()
    {
        yield return new WaitForSeconds(2f);
        // crossfadeAnimator.Play("Crossfade_Start");
    }

    
    public void FadeInImg()
    {
        if (_fadeIn)
        {
            if (canvasGroupImg.alpha < 1)
            {
                canvasGroupImg.alpha += Time.deltaTime * fadeTime;
                
                if (canvasGroupImg.alpha >= 1)
                {
                    _fadeIn = false;
                }
            }
        }
    }
    
    public void FadeOutImg()
    {
        if (_fadeOut)
        {
            if (canvasGroupImg.alpha >= 0)
            {
                canvasGroupImg.alpha -= Time.deltaTime * fadeTime;
                    
                if (canvasGroupImg.alpha == 0)
                {
                    _fadeOut = false;
                }
            }
        }
    }
    
    public void FadeInTxt()
    {
        if (_fadeIn)
        {
            if (canvasGroupTxt.alpha < 1)
            {
                canvasGroupTxt.alpha += Time.deltaTime * fadeTime + 0.5f;
                
                if (canvasGroupTxt.alpha >= 1)
                {
                    _fadeIn = false;
                }
            }
        }
    }
    
    public void FadeOutTxt()
    {
        if (_fadeOut)
        {
            if (canvasGroupTxt.alpha >= 0)
            {
                canvasGroupTxt.alpha -= Time.deltaTime * fadeTime;
                    
                if (canvasGroupTxt.alpha == 0)
                {
                    _fadeOut = false;
                }
            }
        }
    }
    
    public void FadeInBtn()
    {
        if (_fadeIn)
        {
            if (canvasGroupBtn.alpha < 1)
            {
                canvasGroupBtn.alpha += Time.deltaTime * fadeTime + 0.5f;
                
                if (canvasGroupBtn.alpha >= 1)
                {
                    _fadeIn = false;
                }
            }
        }
    }
    
    public void FadeOutBtn()
    {
        if (_fadeOut)
        {
            if (canvasGroupBtn.alpha >= 0)
            {
                canvasGroupBtn.alpha -= Time.deltaTime * fadeTime;
                    
                if (canvasGroupBtn.alpha == 0)
                {
                    _fadeOut = false;
                }
            }
        }
    }

    public void ChangeText()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            dialogueText.text = texts[i];
        }
    }
    
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
        // FadeInImg();
        // FadeInTxt();
        // Invoke("FadeInBtn", 2f);
        //
        // FadeOutImg();
        // FadeOutTxt();
        // FadeOutBtn();
    }
}
