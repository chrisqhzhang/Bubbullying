using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostLoader : MonoBehaviour
{
    
    public TextAsset jsonFile;
    public GameObject postPrefab;
    public Transform contentTransform;    // Scroll View �� Content �� Transform
    public GameObject detailPageCanvas;
    public DetailPageManager detailPageManager;
    public PostDataList postDataList; 

    void Start()
    {

        if (jsonFile == null)
        {
            Debug.LogError("JSON file is not assigned.");
            return;
        }
        detailPageManager = detailPageCanvas.GetComponent<DetailPageManager>();

        postDataList = JsonUtility.FromJson<PostDataList>(jsonFile.text);

        if (postDataList == null || postDataList.posts == null)
        {
            Debug.LogError("Failed to parse JSON file. Check the format.");
            return;
        }

        Debug.Log($"Successfully loaded {postDataList.posts.Count} posts.");

        foreach (PostData post in postDataList.posts)
        {
            CreatePost(post);
        }
    }

    private void CreatePost(PostData postData)
    {
        // ʵ��������
        GameObject postObj = Instantiate(postPrefab, contentTransform);

        // ���� Title �� Content ��ť
        Button titleButton = postObj.transform.Find("Title").GetComponent<Button>();
        Button contentButton = postObj.transform.Find("Content").GetComponent<Button>();

        // ������������
        TextMeshProUGUI titleText = titleButton.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI contentText = contentButton.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        titleText.text = postData.title;
        contentText.text = postData.content;

        // ��ӵ���¼�����������ݣ�

        titleButton.onClick.AddListener(() => OnPostClicked(postData));
        contentButton.onClick.AddListener(() => OnPostClicked(postData));
    }

    // �������ʱ����
    public void OnPostClicked(PostData postData)
    {
        // ģ��������ۣ��������滻Ϊ��ʵ���ݣ�
        //List<string> comments = new List<string> {
         //   "Great post!",
         //   "I love this content!",
           // "This is very helpful, thanks!"
        //};

        // ��������ҳ����������
        detailPageCanvas.SetActive(true);
        detailPageManager.UpdateDetailPage(postData);
    }
}
