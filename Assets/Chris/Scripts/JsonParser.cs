using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonParser : MonoBehaviour
{
    private string path;
    private string jsonString;
    
    // Start is called before the first frame update
    void Start()
    {
        path = Application.dataPath + "/Chris/Data/BubbleData.json";
        
        jsonString = File.ReadAllText(path);
        
        SocialAppData Social = JsonUtility.FromJson<SocialAppData>(jsonString);
        
        ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class SocialAppData
{
    public long GlobalId;
    public string ContentId;
    public string Title;
    public string Content;
    public string Summary;
    public string Type;
    public long CreatedAt;
    public string Poster;
}