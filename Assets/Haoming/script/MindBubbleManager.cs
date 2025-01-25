using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public class MindBubbleManager : Singleton<MindBubbleManager>
{
    private Queue<MindBubble> mindBubbles = new Queue<MindBubble>();
    private Queue<MindBubble> newBubbles = new Queue<MindBubble>();
    private HashSet<int> capturedIds = new HashSet<int>();
    
    [SerializeField] private GameObject mainPage;
    [SerializeField] private GameObject bubblePage;
    
    public Transform startTransform;
     
    private AudioSource audioSource;
    [SerializeField] private AudioClip bubbleMergeSound;
    [SerializeField] private AudioClip bubbleCollideSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

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
        newBubbles.Enqueue(newBubble);
        capturedIds.Add(newBubble.GetId());
        
        bubblePage.SetActive(false);
    }

    public void ToggleBubblePageOn()
    {
        bubblePage.SetActive(true);
        mainPage.SetActive(false);
        EnmergeNewBubbles();
    }
    
    public void ToggleBubblePageOff()
    {
        bubblePage.SetActive(false);
        mainPage.SetActive(true);
        audioSource.Stop();
    }

    public int GetBubbleCount()
    {
        return capturedIds.Count;
    }

    private void EnmergeNewBubbles()
    {
        foreach (var bubble in newBubbles)
        {
            bubble.Enmerge();
        }
        
        newBubbles.Clear();
    }

    public void TriggerBubbleMergeSound()
    {
        audioSource.volume = 1f;
        audioSource.PlayOneShot(bubbleMergeSound);
    }
    
    public void TriggerBubbleCollideSound()
    {
        audioSource.volume = 0.5f;
        audioSource.PlayOneShot(bubbleCollideSound);
    }
}