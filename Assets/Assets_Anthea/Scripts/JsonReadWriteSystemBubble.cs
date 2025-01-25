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
        // ��ȡ JSON �ļ�
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

        // ʵ����ÿ�����Ӳ���ʾ
        foreach (PostData post in bubbleAppData.posts)
        {
            GameObject postObj = Instantiate(postPrefab, postsContainer);

            // ��ȡ����Ԫ��
            Transform titleButtonTransform = postObj.transform.Find("TitleButton");
            TMP_InputField titleInputField = titleButtonTransform.Find("TitleInputField").GetComponent<TMP_InputField>();

            Transform ContentButtonTransform = postObj.transform.Find("ContentButton");
            TMP_InputField contentInputField = ContentButtonTransform.Find("ContentInputField").GetComponent<TMP_InputField>();

            TMP_InputField posterInputField = postObj.transform.Find("PosterInputField").GetComponent<TMP_InputField>();
            TMP_InputField timeStampInputField = postObj.transform.Find("TimeStampInputField").GetComponent<TMP_InputField>();

            // ����Ϊ���ɱ༭��interactable = false��
            titleInputField.interactable = false;
            contentInputField.interactable = false;
            posterInputField.interactable = false;
            timeStampInputField.interactable = false;


            // �������
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
        // ��ʱ���ת��Ϊ�ɶ���ʽ
        long unixTime = long.Parse(timestamp);
        System.DateTime dateTime = System.DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public void ShowDetailPage(PostData postData)
    {
        // ��������ҳ Canvas
        detailPageCanvas.SetActive(true);

        // ��ʾ��������ҳ��
        titleInputField.text = postData.title;
        postContentInputField.text = postData.content;
        posterInputField.text = postData.poster;
        timeInputField.text = FormatTime(postData.time);

        // Comments
        DisplayComments(postData);
    }

    public void DisplayComments(PostData postData)
    {
        // ��ȡ�����ӵ�������ص�����
        string comments = "";
        foreach (CommentData comment in bubbleAppData.comments)
        {
            if (comment.parentPostId == postData.globalId)
            {
                comments += comment.content + "\n";
            }
        }

        // ��������һ��������ʾ����
        TMP_InputField commentsInputField = detailPageCanvas.transform.Find("CommentsInputField").GetComponent<TMP_InputField>();
        commentsInputField.text = comments;
    }

}