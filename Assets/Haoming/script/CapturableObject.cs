using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class CapturableObject : MonoBehaviour
{
    private BubbleData bubbleData;
    
    protected void Awake()
    {
        bubbleData = new BubbleData();
        bubbleData.ConstructBubbleData(Random.Range(1, 10),  //TODO
            transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text, 
            gameObject);
    }

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
