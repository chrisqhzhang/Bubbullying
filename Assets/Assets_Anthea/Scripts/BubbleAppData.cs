using System.Collections.Generic;

[System.Serializable]
public class BubbleAppData 
{
    public List<PostData> posts;
    public List<CommentData> comments;
}

[System.Serializable]
public class MergeBubbles
{
    public List<BubbleData> mergeBubbles;
}


[System.Serializable]
public class PostData
{
    public int globalId;
    public string contentId;
    public string title;
    public string content;
    public string summary;
    public string type; // "Post"
    public string time;
    public string poster; 
}

[System.Serializable]
public class CommentData
{
    public int globalId;
    public string contentId;
    public string title;
    public string content;
    public string summary;
    public string type; // "Comment"
    public string time;
    public string poster;
    public int parentPostId;
}

