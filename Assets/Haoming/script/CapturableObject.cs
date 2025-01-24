using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturableObject : MonoBehaviour
{
    private void OnMouseUp()
    {
        if (CapturedManager.Instance.IsCaptureRunning()) return;
        
        if (MindBubbleManager.Instance.IsCaptured(gameObject))
        {
            CapturedManager.Instance.OnClickOnCapturedObject?.Invoke(gameObject);
        }
        else
        {
            CapturedManager.Instance.OnClickOnNotCapturedObject?.Invoke(gameObject);
        }
    }
}
