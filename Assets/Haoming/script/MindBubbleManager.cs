using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindBubbleManager : MonoBehaviour
{
    public static MindBubbleManager Instance;

    private Queue<MindBubble> mindBubbles = new Queue<MindBubble>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void CaptureData(string id, string content)
    {
        // Store the captured data
        mindBubbles.Enqueue(new MindBubble(id, content));
        Debug.Log($"Captured: {content} (ID: {id})");

        // Additional logic: Save data, display it in a bubble, etc.
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
