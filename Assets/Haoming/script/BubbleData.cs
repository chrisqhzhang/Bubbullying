using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleData
{
    public int Id;
    public string Description;
    public int Size;
    public GameObject Screenshot;
    
    public void ConstructBubbleData(int id, string description, GameObject screenshot)
    {
        this.Screenshot = screenshot;
        this.Id = id;
        this.Description = description;
        this.Size = 1;
    }
    
    public void ConstructBubbleDataFromOther(BubbleData data)
    {
        this.Screenshot = data.Screenshot;
        this.Id = data.Id;
        this.Description = data.Description;
        this.Size = 1;
    }

    public void HandleClick(CapturableObject clickedObject)
    {
        if (CapturedManager.Instance.IsCaptureRunning()) return;
        
        if (MindBubbleManager.Instance.IsCaptured(this.Id))
        {
            CapturedManager.Instance.OnClickOnCapturedObject?.Invoke(clickedObject);
        }
        else
        {
            CapturedManager.Instance.OnClickOnNotCapturedObject?.Invoke(clickedObject);
        }
    }

}

public class BubbleDataPost : BubbleData
{

}
