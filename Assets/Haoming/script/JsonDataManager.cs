using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;


public class JsonDataManager : Singleton<JsonDataManager>
{
    private BubbleAppData bubbleAppData;
    
    void OnEnable()
    {
        LoadFromJson();
    }

    public void LoadFromJson()
    {
        string bubbleDataPath = Application.dataPath + "/Assets_Anthea/Resources/BubbleDataFile.json";
        string mergeBubbleDataPath = Application.dataPath + "/Assets_Anthea/Resources/MergeBubbleFile.json";

        if (!File.Exists(bubbleDataPath))
        {
            Debug.LogError("JSON file not found at " + bubbleDataPath);
            return;
        }
        
        MindBubbleManager.Instance.PossibleMergeBubbles = JsonUtility.FromJson<MergeBubbles>(File.ReadAllText(mergeBubbleDataPath)).mergeBubbles;
        bubbleAppData = JsonUtility.FromJson<BubbleAppData>(File.ReadAllText(bubbleDataPath));
        
        CalculateCommentsCount();
        CalculateMergeRecipes();
    }

    public List<PostData> GetPosts()
    {
        return bubbleAppData.posts;
    }

    private void CalculateCommentsCount()
    {
        foreach (var post in bubbleAppData.posts)
        {
            int count = 0;
            foreach (var comment in post.comments)
            {
                count++;
            }
            post.commentCount = count;
        }
        
    }
    
    private void CalculateMergeRecipes()
    {
        MindBubbleManager.Instance.mergeRecipes = new List<HashSet<int>>(5);
        for (int i = 0; i < 5; i++)
        {
            MindBubbleManager.Instance.mergeRecipes.Add(new HashSet<int>());
        }
        
        foreach (var bubbleData in MindBubbleManager.Instance.PossibleMergeBubbles)
        {
            MindBubbleManager.Instance.mergeRecipes[bubbleData.Size-2].Add(Convert.ToInt32(bubbleData.Id.ToString(), 2));
        }
    }
}
