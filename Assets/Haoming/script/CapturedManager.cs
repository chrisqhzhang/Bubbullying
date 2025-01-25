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
    
    private CapturableObject _currentCapturedObject;
    
    public delegate void ButtonEventHandler(CapturableObject capturedObject = null);
    
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
        
        if (EventSystem.current.IsPointerOverGameObject()) return; // ignore UI
        
        if (Physics2D.Raycast(
                mainCamera.ScreenToWorldPoint(Input.mousePosition), 
                Vector2.zero, 
                Mathf.Infinity, 
                capturableLayer)) return;
        
        OnClickOnBlank?.Invoke(null);
    }

    public void HandleClickOnCapture()
    {
        if (_currentCapturedObject == null) return;
        if (MindBubbleManager.Instance.IsCaptured(_currentCapturedObject.GetId())) return;

        isCaptureRunning = true;
        OnClickOnCaptureButton?.Invoke(null);
    }
 
    void ShowCapturedButton(CapturableObject capturedObject)
    {
        buttonImage.sprite = defaultImage;
        
        // capturedButton.transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition) + capturedButtonOffset;
        
        Vector3 loc = capturedObject.gameObject.transform.GetChild(0).position;
        Vector3 scale = capturedObject.gameObject.transform.GetChild(0).lossyScale;

        capturedButton.transform.position = loc + scale / 2;
        
        capturedButton.SetActive(true);
    }
    
    void ShowCapturedDoneButton(CapturableObject capturedObject)
    {
        buttonImage.sprite = clickedImage;
        
        // capturedButton.transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition) + capturedButtonOffset;

        Vector3 loc = capturedObject.gameObject.transform.GetChild(0).position;
        Vector3 scale = capturedObject.gameObject.transform.GetChild(0).lossyScale;

        capturedButton.transform.position = loc +  scale / 2;
        
        capturedButton.SetActive(true);
    }
    
    void HideCapturedButton(CapturableObject capturedObject = null)
    {
        capturedButton.SetActive(false);
    }

    void UpdateCurrentClickedObject(CapturableObject capturedObject)
    {
        _currentCapturedObject = capturedObject;
        if (capturedObject)
        {
            capturedObjectRenderer = capturedObject.gameObject.transform.GetChild(0).GetComponent<Renderer>();
            capturedOriginalMaterial = capturedObjectRenderer.material;
        }
    }
       
    private void TriggerCapturedSound(CapturableObject capturedObject = null)
    {
        audioSource.Play();
    }
    
    private void HandleButtonAfterCapture(CapturableObject capturedObject = null)
    {
        // buttonImage.sprite = clickedImage;
        capturedButton.SetActive(false);
        buttonImage.sprite = defaultImage;
    }
    
    void HandleCaptureData(CapturableObject capturedObject = null)
    {
        MindBubbleManager.Instance.CaptureData(_currentCapturedObject.GetBubbleData());
    }

    public void TriggerFlashAndFly(CapturableObject capturedObject = null)
    {
        GameObject newcapturedObject = Instantiate(_currentCapturedObject.gameObject,
                                        mainCamera.ScreenToWorldPoint(Input.mousePosition), 
                                                Quaternion.identity);
        
        LeanTween.scale(newcapturedObject, new Vector3(scaleReduction, scaleReduction, scaleReduction), flyDuration)
            .setEase(LeanTweenType.easeInQuad);
        LeanTween.move(newcapturedObject, targetCorner.position, flyDuration)
            .setEase(LeanTweenType.easeInOutExpo);
     
        StartCoroutine(FlashCoroutine());
        
        LeanTween.delayedCall(flyDuration, () =>
        {
            Destroy(newcapturedObject);
            isCaptureRunning = false;
            UpdateCurrentClickedObject(null);
        });
    }
    
    private IEnumerator FlashCoroutine()
    {
        GameObject capturedObjectChildren = _currentCapturedObject.transform.GetChild(0).GetChild(0).gameObject;
        capturedObjectChildren.SetActive(false);
        capturedObjectRenderer.material = flashMaterial;
        yield return new WaitForSeconds(flyDuration);
        capturedObjectChildren.SetActive(true);
        capturedObjectRenderer.material = capturedOriginalMaterial;
    }
    
    public bool IsCaptureRunning()
    {
        return isCaptureRunning;
    }

}
