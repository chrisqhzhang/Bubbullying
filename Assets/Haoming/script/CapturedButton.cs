using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapturedButton : MonoBehaviour
{
    public Sprite defaultImage;  // Default button image
    public Sprite clickedImage;  // Image to show after clicking
    private Image buttonImage;   // Reference to the Button's Image component
    private bool isClicked = false;  // To track click state

    void Start()
    {
        // Get the Image component of the Button
        buttonImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        
        // Set the default image at the start
        if (defaultImage != null)
        {
            buttonImage.sprite = defaultImage;
        }
    }

    public string GetObjectId()
    {
        return "001";
    }
    
    public string GetContent()
    {
        return "test";
    }

    public void Capture()
    {
        if (isClicked) return;
        
        // Call the Capture Manager to handle the data
        MindBubbleManager.Instance.CaptureData(GetObjectId(), GetContent());
        
        isClicked = true;
        buttonImage.sprite = clickedImage;
    }
}
