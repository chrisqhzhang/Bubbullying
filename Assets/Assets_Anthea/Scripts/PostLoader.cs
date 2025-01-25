using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostLoader : MonoBehaviour
{
    
    public TextAsset jsonFile;
    public GameObject postPrefab;
    public Transform contentTransform;    // Scroll View 中 Content 的 Transform
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
        // 实例化帖子
        GameObject postObj = Instantiate(postPrefab, contentTransform);

        // 查找 Title 和 Content 按钮
        Button titleButton = postObj.transform.Find("Title").GetComponent<Button>();
        Button contentButton = postObj.transform.Find("Content").GetComponent<Button>();

        // 设置帖子数据
        TextMeshProUGUI titleText = titleButton.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI contentText = contentButton.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        titleText.text = postData.title;
        contentText.text = postData.content;

        // 添加点击事件（标题或内容）

        titleButton.onClick.AddListener(() => OnPostClicked(postData));
        contentButton.onClick.AddListener(() => OnPostClicked(postData));
    }

    // 点击帖子时触发
    public void OnPostClicked(PostData postData)
    {
        // 模拟加载评论（后续可替换为真实数据）
        //List<string> comments = new List<string> {
         //   "Great post!",
         //   "I love this content!",
           // "This is very helpful, thanks!"
        //};

        // 激活详情页并更新内容
        detailPageCanvas.SetActive(true);
        detailPageManager.UpdateDetailPage(postData);
    }
}
