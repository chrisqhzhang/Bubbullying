using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class MindBubbleManager : Singleton<MindBubbleManager>
{
    private Queue<MindBubble> mindBubbles = new Queue<MindBubble>();
    private Queue<MindBubble> newBubbles = new Queue<MindBubble>();
    private HashSet<int> capturedIds = new HashSet<int>();
    
    public List<HashSet<int>> mergeRecipes = new List<HashSet<int>>();

    public List<BubbleData> PossibleMergeBubbles;
    
    [SerializeField] private GameObject mainPage;
    [SerializeField] private GameObject bubblePage;
    
    public Transform startTransform;
     
    private AudioSource audioSource;
    [SerializeField] private AudioClip bubbleMergeSound;
    [SerializeField] private AudioClip bubbleCollideSound;
    
    private float displayOffsetX = 2.2f;
    private float displayOffsetY = 4f;
    
    public readonly object lockObject = new object();

    public bool isMerge;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public bool IsCaptured(int Id)
    {
        return capturedIds.Contains(Id);
    }
    
    public BubbleData GetBubbleData(int Id)
    {
        foreach (var bubble in PossibleMergeBubbles)
        {
            if (bubble.Id == Id) return bubble;
        }
        return null;
    }
    
    public void CaptureData(BubbleData bubbleData)
    {
        if (IsCaptured(bubbleData.Id)) return;
        
        GameObject newBubbleObject = Instantiate(Resources.Load("Bubble1"), bubblePage.transform) as GameObject;
        MindBubble newBubble = newBubbleObject.GetComponent<MindBubble>();

        newBubbleObject.transform.position =  new Vector2(startTransform.position.x + displayOffsetX * (GetBubbleCount() % 10),
            startTransform.position.y - displayOffsetY * (GetBubbleCount() / 10));
        
        newBubble.ConstructMindBubble(bubbleData);
        
        mindBubbles.Enqueue(newBubble);
        newBubbles.Enqueue(newBubble);
        capturedIds.Add(newBubble.GetId());
        
        bubblePage.SetActive(false);
    }

    public void RecordMergedNewBubble(MindBubble newBubble)
    {
        mindBubbles.Enqueue(newBubble);
        // newBubbles.Enqueue(newBubble);
        // capturedIds.Add(newBubble.GetId());
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

    public bool CanMerge(BubbleData b1, BubbleData b2)
    {
        if (b1 == null || b2 == null) return false;
        
        int mergeSize = b1.Size + b2.Size;

        return (mergeSize - 1) <= mergeRecipes.Count
               && mergeRecipes[mergeSize - 2].Contains(toBinaryId(b1.Id) + toBinaryId(b2.Id));
    }

    private int toBinaryId(int Id)
    {
        return 1 << (Id - 1);
    }
}