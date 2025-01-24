using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CapturedManager : Singleton<CapturedManager>
{
    [SerializeField] private GameObject capturedButton;
    [SerializeField] private Sprite defaultImage;  // Default button image
    [SerializeField] private Sprite clickedImage;  // Image to show after clicking
    [SerializeField] private Vector3 capturedButtonOffset;
    [SerializeField] private Transform targetCorner; 
    
    [SerializeField] private float flyDuration; 
    [SerializeField] private float scaleReduction;
    
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashDuration = 0.5f;
    
    private int capturableLayer;
    
    private Material capturedOriginalMaterial;
    private Renderer capturedObjectRenderer;
    private Image buttonImage;
    private Camera mainCamera;
    
    private AudioSource audioSource;

    private bool isCaptureRunning;
    private GameObject currentCapturedObject;
    
    public delegate void ButtonEventHandler(GameObject go = null);
    
    public ButtonEventHandler OnClickOnCapturedObject;
    public ButtonEventHandler OnClickOnNotCapturedObject;
    public ButtonEventHandler OnClickOnBlank;
    public ButtonEventHandler OnClickOnCaptureButton;

    void Start()
    {
        SetUp();
        
        OnClickOnNotCapturedObject += ShowCapturedButton;
        OnClickOnNotCapturedObject += UpdateCurrentClickedObject;

        OnClickOnCapturedObject += ShowCapturedDoneButton;

        OnClickOnBlank += UpdateCurrentClickedObject;
        OnClickOnBlank += HideCapturedButton;

        OnClickOnCaptureButton += HandleCaptureData;
        OnClickOnCaptureButton += TriggerFlashAndFly;
        OnClickOnCaptureButton += TriggerCapturedSound;
        OnClickOnCaptureButton += HandleButtonAfterCapture;
    }

    private void OnDestroy()
    {
        OnClickOnNotCapturedObject -= ShowCapturedButton;
        OnClickOnNotCapturedObject -= UpdateCurrentClickedObject;

        OnClickOnCapturedObject -= ShowCapturedDoneButton;

        OnClickOnBlank -= UpdateCurrentClickedObject;
        OnClickOnBlank -= HideCapturedButton;

        OnClickOnCaptureButton -= HandleCaptureData;
        OnClickOnCaptureButton -= TriggerFlashAndFly;
        OnClickOnCaptureButton -= TriggerCapturedSound;
        OnClickOnCaptureButton -= HandleButtonAfterCapture;
    }
    
    void Update()
    {
        HandleClickOnBlank();
    }

    private void SetUp()
    {
        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();

        capturableLayer = LayerMask.GetMask("CapturableLayer");
        
        capturedButton.SetActive(false);
        
        buttonImage = capturedButton.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        buttonImage.sprite = defaultImage;
    }
    
    private void HandleClickOnBlank()
    {
        if (!Input.GetMouseButtonUp(0)) return;
        
        // if (EventSystem.current.IsPointerOverGameObject()) return; // ignore UI
        
        if (Physics2D.Raycast(
                mainCamera.ScreenToWorldPoint(Input.mousePosition), 
                Vector2.zero, 
                Mathf.Infinity, 
                capturableLayer)) return;
        
        OnClickOnBlank?.Invoke(null);
    }

    public void HandleClickOnCapture()
    {
        if (currentCapturedObject == null) return;
        if (MindBubbleManager.Instance.IsCaptured(currentCapturedObject)) return;

        isCaptureRunning = true;
        OnClickOnCaptureButton?.Invoke(null);
    }
 
    void ShowCapturedButton(GameObject go)
    {
        buttonImage.sprite = defaultImage;
        
        // capturedButton.transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition) + capturedButtonOffset;
        
        Vector3 loc = go.transform.GetChild(0).position;
        Vector3 scale = go.transform.GetChild(0).lossyScale;

        capturedButton.transform.position = loc + scale / 2;
        
        capturedButton.SetActive(true);
    }
    
    void ShowCapturedDoneButton(GameObject go)
    {
        buttonImage.sprite = clickedImage;
        
        // capturedButton.transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition) + capturedButtonOffset;

        Vector3 loc = go.transform.GetChild(0).position;
        Vector3 scale = go.transform.GetChild(0).lossyScale;

        capturedButton.transform.position = loc +  scale / 2;
        
        capturedButton.SetActive(true);
    }
    
    void HideCapturedButton(GameObject go = null)
    {
        capturedButton.SetActive(false);
    }

    void UpdateCurrentClickedObject(GameObject go)
    {
        currentCapturedObject = go;
        if (go)
        {
            capturedObjectRenderer = go.transform.GetChild(0).GetComponent<Renderer>();
            capturedOriginalMaterial = capturedObjectRenderer.material;
        }
    }
       
    private void TriggerCapturedSound(GameObject go = null)
    {
        audioSource.Play();
    }
    
    private void HandleButtonAfterCapture(GameObject go = null)
    {
        // buttonImage.sprite = clickedImage;
        capturedButton.SetActive(false);
        buttonImage.sprite = defaultImage;
    }
    
    void HandleCaptureData(GameObject go = null)
    {
        MindBubbleManager.Instance.CaptureData(currentCapturedObject);
    }

    public void TriggerFlashAndFly(GameObject go = null)
    {
        GameObject capturedObject = Instantiate(currentCapturedObject, mainCamera.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
        
        LeanTween.scale(capturedObject, new Vector3(scaleReduction, scaleReduction, scaleReduction), flyDuration)
            .setEase(LeanTweenType.easeInQuad);
        LeanTween.move(capturedObject, targetCorner.position, flyDuration)
            .setEase(LeanTweenType.easeInOutExpo);
     
        StartCoroutine(FlashCoroutine());
        
        LeanTween.delayedCall(flyDuration, () =>
        {
            Destroy(capturedObject);
            isCaptureRunning = false;
            UpdateCurrentClickedObject(null);
        });
    }
    
    private IEnumerator FlashCoroutine()
    {
        capturedObjectRenderer.material = flashMaterial;
        yield return new WaitForSeconds(flyDuration);
        capturedObjectRenderer.material = capturedOriginalMaterial;
    }
    
    public bool IsCaptureRunning()
    {
        return isCaptureRunning;
    }
    
    public string GetObjectId()
    {
        return "001";
    }
    
    public string GetContent()
    {
        return "test";
    }

}
