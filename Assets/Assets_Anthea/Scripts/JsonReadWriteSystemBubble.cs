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

    public GameObject postPrefab;
    public Transform postsContainer;

    public GameObject detailPageCanvas;

    private BubbleAppData bubbleAppData;



    void Start()
    {
        LoadFromJson();
        LoadPosts();
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
        bubbleAppData = JsonUtility.FromJson<BubbleAppData>(json);

    }

    public void LoadPosts()
    {
        if (bubbleAppData == null || bubbleAppData.posts.Count == 0)
        {
            Debug.LogWarning("No posts available to load.");
            return;
        }

        // 实例化每个帖子并显示
        foreach (PostData post in bubbleAppData.posts)
        {
            GameObject postObj = Instantiate(postPrefab, postsContainer);

            // 获取帖子元素
            Transform titleButtonTransform = postObj.transform.Find("TitleButton");
            TMP_InputField titleInputField = titleButtonTransform.Find("TitleInputField").GetComponent<TMP_InputField>();

            Transform ContentButtonTransform = postObj.transform.Find("ContentButton");
            TMP_InputField contentInputField = ContentButtonTransform.Find("ContentInputField").GetComponent<TMP_InputField>();

            TMP_InputField posterInputField = postObj.transform.Find("PosterInputField").GetComponent<TMP_InputField>();
            TMP_InputField timeStampInputField = postObj.transform.Find("TimeStampInputField").GetComponent<TMP_InputField>();

            // 设置为不可编辑（interactable = false）
            titleInputField.interactable = false;
            contentInputField.interactable = false;
            posterInputField.interactable = false;
            timeStampInputField.interactable = false;


            // 填充内容
            titleInputField.text = post.title;
            contentInputField.text = post.content;
            posterInputField.text = post.poster;
            timeStampInputField.text = FormatTime(post.time);

            //OnClick
            Button titleButton = titleButtonTransform.GetComponent<Button>();
            titleButton.onClick.AddListener(() => ShowDetailPage(post));
        }
    }

    private string FormatTime(string timestamp)
    {
        // 将时间戳转换为可读格式
        long unixTime = long.Parse(timestamp);
        System.DateTime dateTime = System.DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public void ShowDetailPage(PostData postData)
    {
        // 激活详情页 Canvas
        detailPageCanvas.SetActive(true);

        // 显示帖子详情页面
        titleInputField.text = postData.title;
        postContentInputField.text = postData.content;
        posterInputField.text = postData.poster;
        timeInputField.text = FormatTime(postData.time);

        // Comments
        DisplayComments(postData);
    }

    public void DisplayComments(PostData postData)
    {
        // 获取与帖子的评论相关的内容
        string comments = "";
        foreach (CommentData comment in bubbleAppData.comments)
        {
            if (comment.parentPostId == postData.globalId)
            {
                comments += comment.content + "\n";
            }
        }

        // 假设你有一个评论显示区域
        TMP_InputField commentsInputField = detailPageCanvas.transform.Find("CommentsInputField").GetComponent<TMP_InputField>();
        commentsInputField.text = comments;
    }

}