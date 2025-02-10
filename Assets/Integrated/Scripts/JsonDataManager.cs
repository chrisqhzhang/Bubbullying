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
        
        // TextAsset bubbleDataTextAsset = Resources.Load<TextAsset>("Data/BubbleDataFile");
        // Debug.Log(bubbleDataTextAsset);
        // TextAsset mergeBubbleDataTextAsset = Resources.Load<TextAsset>("Data/MergeBubbles");
        // Debug.Log(mergeBubbleDataTextAsset);
        //
        // MindBubbleManager.Instance.PossibleMergeBubbles =
        //     JsonUtility.FromJson<MergeBubbles>(mergeBubbleDataTextAsset.text).mergeBubbles;
        // bubbleAppData = JsonUtility.FromJson<BubbleAppData>(bubbleDataTextAsset.text);
        
        string socialDataFile = "BubbleDataFile.json";
        string mergeDataFile = "MergeBubbles.json";
        
        //var socialFilePath = GetFilePath(socialDataFile);
        var socialFilePath = Application.dataPath + "/StreamingAssets/BubbleDataFile.json";
        var socialDataText = File.ReadAllText(socialFilePath);
        
        //var mergeFilePath = GetFilePath(mergeDataFile);
        var mergeFilePath = Application.dataPath + "/StreamingAssets/MergeBubbles.json";
        var mergeDataText = File.ReadAllText(mergeFilePath);
        
        Debug.Log($"Trying to read file {socialFilePath} & {mergeFilePath}");
        
        MindBubbleManager.Instance.PossibleMergeBubbles =
            JsonUtility.FromJson<MergeBubbles>(mergeDataText).mergeBubbles;
        
        bubbleAppData = JsonUtility.FromJson<BubbleAppData>(socialDataText);
        
        ParsePostAndComment();
        ParseMergeRecipes();
        
    }
    
    private string GetFilePath(string fileName)
    {
        string dataFolder = Application.streamingAssetsPath;
        return Path.Combine(dataFolder, fileName);
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
