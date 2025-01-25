using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public class MindBubbleManager : Singleton<MindBubbleManager>
{
    private Queue<MindBubble> mindBubbles = new Queue<MindBubble>();
    private HashSet<int> capturedIds = new HashSet<int>();
    
    [SerializeField] private GameObject mainPage;
    [SerializeField] private GameObject bubblePage;
    
    public Transform startTransform;
     
    public bool IsCaptured(int Id)
    {
        return capturedIds.Contains(Id);
    }
    
    public void CaptureData(BubbleData bubbleData)
    {
        if (IsCaptured(bubbleData.Id)) return;
        
        GameObject newBubbleObject = Instantiate(Resources.Load("SmallBubble"), bubblePage.transform) as GameObject;
        MindBubble newBubble = newBubbleObject.GetComponent<MindBubble>();
        
        newBubble.ConstructMindBubble(bubbleData);
        
        mindBubbles.Enqueue(newBubble);
        capturedIds.Add(newBubble.GetId());
        
        bubblePage.SetActive(false);
    }

    public void ToggleBubblePageOn()
    {
        bubblePage.SetActive(true);
        mainPage.SetActive(false);
    }
    
    public void ToggleBubblePageOff()
    {
        bubblePage.SetActive(false);
        mainPage.SetActive(true);
    }

    public int GetBubbleCount()
    {
        return capturedIds.Count;
    }
}