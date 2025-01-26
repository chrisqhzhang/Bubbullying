using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class CapturableObject : MonoBehaviour
{
    protected BubbleData bubbleData;

    protected void OnMouseUp()
    {
        HandleClick(this);
    }

    public int GetId()
    {
        return bubbleData.Id;
    }
    
    public BubbleData GetBubbleData()
    {
        return bubbleData;
    }
    
    public void HandleClick(CapturableObject clickedObject)
    {
        if (CapturedManager.Instance.IsCaptureRunning()) return;
        
        if (MindBubbleManager.Instance.IsCaptured(bubbleData.Id))
        {
            CapturedManager.Instance.OnClickOnCapturedObject?.Invoke(clickedObject);
        }
        else
        {
            CapturedManager.Instance.OnClickOnNotCapturedObject?.Invoke(clickedObject);
        }
    }
    
}
