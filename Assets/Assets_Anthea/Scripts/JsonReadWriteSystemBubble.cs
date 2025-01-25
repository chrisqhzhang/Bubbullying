using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JsonReadWriteSystemBubble : MonoBehaviour
{
    public TMP_InputField titleInputField;
    public TMP_InputField postContentInputField;
    public TMP_InputField posterInputField;
    public TMP_InputField timeInputField;

    private BubbleData bubbleData;

    void Start()
    {
        LoadFromJson();
    }

    public void LoadFromJson()
    {
        // 读取 JSON 文件
        string jsonPath = Application.dataPath + "/Assets_Anthea/Resources/BubbleDataFile.json";
        if (!File.Exists(jsonPath))
        {
            Debug.LogError("JSON file not found at " + jsonPath);
            return;
        }

        string json = File.ReadAllText(jsonPath);
        bubbleData = JsonUtility.FromJson<BubbleData>(json);

        // 测试显示第一个帖子的内容
        if (bubbleData.posts.Count > 0)
        {
            DisplayPost(bubbleData.posts[0]);
        }
    }

    private void DisplayPost(PostData postData)
    {
        // 显示帖子内容
        titleInputField.text = postData.title;
        postContentInputField.text = postData.content;
        posterInputField.text = postData.poster;
        timeInputField.text = FormatTime(postData.time);
    }

    private string FormatTime(string timestamp)
    {
        // 将时间戳转换为可读格式
        long unixTime = long.Parse(timestamp);
        System.DateTime dateTime = System.DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }
}