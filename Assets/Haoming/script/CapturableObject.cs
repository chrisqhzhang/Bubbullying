using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CapturableObject : MonoBehaviour
{
    private BubbleData bubbleData;
    
    protected void Awake()
    {
        bubbleData = new BubbleData();
        bubbleData.ConstructBubbleData(1, 
            transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text, 
            gameObject);
    }

    protected void OnMouseUp()
    {
        bubbleData.HandleClick(this);
    }

    public int GetId()
    {
        return bubbleData.Id;
    }
    
    public BubbleData GetBubbleData()
    {
        return bubbleData;
    }
    
}
