using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Numerics;

public class JsonDataManager : Singleton<JsonDataManager>
{
    public BubbleAppData bubbleAppData;
    
    void OnEnable()
    {
        LoadFromJson();
    }

  public async Task LoadFromJson()
    {
        string socialDataFile = "BubbleDataFile.json";
        string mergeDataFile = "MergeBubbles.json";

        string socialFilePath = Path.Combine(Application.streamingAssetsPath, socialDataFile);
        string mergeFilePath = Path.Combine(Application.streamingAssetsPath, mergeDataFile);

        Debug.Log($"Trying to read file {socialFilePath} & {mergeFilePath}");

        try
        {
            Task<string> socialDataTask = File.ReadAllTextAsync(socialFilePath);
            Task<string> mergeDataTask = File.ReadAllTextAsync(mergeFilePath);

            string[] results = await Task.WhenAll(socialDataTask, mergeDataTask);
            string socialDataText = results[0];
            string mergeDataText = results[1];

            MindBubbleManager.Instance.PossibleMergeBubbles =
                JsonUtility.FromJson<MergeBubbles>(mergeDataText).mergeBubbles;

            bubbleAppData = JsonUtility.FromJson<BubbleAppData>(socialDataText);

            ParsePostAndComment();
            ParseMergeRecipes();

            Debug.Log("JsonDataMgr LoadFromJson loaded");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Fail: {ex.Message}");
        }
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
