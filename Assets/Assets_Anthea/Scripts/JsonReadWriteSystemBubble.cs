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
        // ��ȡ JSON �ļ�
        string jsonPath = Application.dataPath + "/Assets_Anthea/Resources/BubbleDataFile.json";
        if (!File.Exists(jsonPath))
        {
            Debug.LogError("JSON file not found at " + jsonPath);
            return;
        }

        string json = File.ReadAllText(jsonPath);
        bubbleData = JsonUtility.FromJson<BubbleData>(json);

        // ������ʾ��һ�����ӵ�����
        if (bubbleData.posts.Count > 0)
        {
            DisplayPost(bubbleData.posts[0]);
        }
    }

    private void DisplayPost(PostData postData)
    {
        // ��ʾ��������
        titleInputField.text = postData.title;
        postContentInputField.text = postData.content;
        posterInputField.text = postData.poster;
        timeInputField.text = FormatTime(postData.time);
    }

    private string FormatTime(string timestamp)
    {
        // ��ʱ���ת��Ϊ�ɶ���ʽ
        long unixTime = long.Parse(timestamp);
        System.DateTime dateTime = System.DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }
}