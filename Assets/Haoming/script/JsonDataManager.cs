using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using System.Numerics;


public class JsonDataManager : Singleton<JsonDataManager>
{
    public BubbleAppData bubbleAppData;
    
    void OnEnable()
    {
        LoadFromJson();
    }

    public void LoadFromJson()
    {
        string bubbleDataPath = Application.dataPath + "/Assets_Anthea/Resources/BubbleDataFile.json";
        string mergeBubbleDataPath = Application.dataPath + "/Assets_Anthea/Resources/MergeBubbles.json";

        if (!File.Exists(bubbleDataPath))
        {
            Debug.LogError("JSON file not found at " + bubbleDataPath);
            return;
        }
        
        MindBubbleManager.Instance.PossibleMergeBubbles = JsonUtility.FromJson<MergeBubbles>(File.ReadAllText(mergeBubbleDataPath)).mergeBubbles;
        bubbleAppData = JsonUtility.FromJson<BubbleAppData>(File.ReadAllText(bubbleDataPath));
        
        ParsePostAndComment();
        ParseMergeRecipes();
    }

    public List<PostData> GetPosts()
    {
        return bubbleAppData.posts;
    }

    private void ParsePostAndComment()
    {
        foreach (var post in bubbleAppData.posts)
        {
            post.Id = BubbleHelper.IdToBinaryId(post.globalId);
            BubbleAppManager.Instance.commentCounts.Add(post.globalId, 0);
        }
        
        foreach (var comment in bubbleAppData.comments)
        {
            comment.Id = BubbleHelper.IdToBinaryId(comment.globalId);
            BubbleAppManager.Instance.commentCounts[comment.parentPostId] += 1;
        }
    }
    

    private void ParseMergeRecipes()
    {
        MindBubbleManager.Instance.mergeRecipes = new List<HashSet<BigInteger>>(5);
        for (int i = 0; i < 5; i++)
        {
            MindBubbleManager.Instance.mergeRecipes.Add(new HashSet<BigInteger>());
        }
        
        foreach (var bubbleData in MindBubbleManager.Instance.PossibleMergeBubbles)
        {
            BigInteger finalId  = 0;
            
            foreach (var id in bubbleData.Ids)
            {
                finalId += BubbleHelper.IdToBinaryId(id);
            }

            bubbleData.Id = finalId;
            MindBubbleManager.Instance.mergeRecipes[bubbleData.Size-2].Add(finalId);
        }
    }
}
