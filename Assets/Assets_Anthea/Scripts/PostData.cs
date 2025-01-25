using System.Collections.Generic;

[System.Serializable]

public class PostData 
{
    public string title;
    public string poster;
    public string content;    
    public string imagePath;
}
public class PostDataList
{
    public List<PostData> posts; 
}
