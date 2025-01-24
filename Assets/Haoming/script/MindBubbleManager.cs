using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindBubbleManager : Singleton<MindBubbleManager>
{
    private Queue<MindBubble> mindBubbles = new Queue<MindBubble>();
    
    public bool IsCaptured(GameObject go)
    {
        return false;
        // TODO: return if a go is captured
    }
    
    public void CaptureData(GameObject go)
    {
        // mindBubbles.Enqueue(new MindBubble(id, content));

        // mark as captured
    }

    public Queue<MindBubble> GetCapturedData()
    {
        return mindBubbles;
    }
}

// Data structure for captured data
[System.Serializable]
public class MindBubble
{
    public string Id;
    public string Content;
    public int size;

    public MindBubble(string id, string content)
    {
        Id = id;
        Content = content;
        size = 1;
    }

    
}
