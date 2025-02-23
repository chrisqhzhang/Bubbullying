using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine.UI;

public class BubbleAppManager : Singleton<BubbleAppManager>
{
    public Transform postContent;
    public Transform detailViewPort;
    public Transform detailContent;
    public Transform commentsParent;
    public GameObject postPrefab;
    [SerializeField] private float pageHeightOffset;
    [SerializeField] private float detailPageHeightOffset;
    [SerializeField] private float verticalOffset;
    [SerializeField] private float horizontalOffset;

    [SerializeField] private GameObject detailPage;
    [SerializeField] private GameObject mainPage;
    [SerializeField] private GameObject commentPrefab;
    [SerializeField] private float commentVerticalOffset;

    [SerializeField] private Scrollbar scrollbar;
    
    private float postVerticalHeight;
    private int postCount;

    public Dictionary<BigInteger, int> commentCounts = new Dictionary<BigInteger, int>();
    private Queue<GameObject> commentsInDetail = new Queue<GameObject>();
    
    private const double epson = 0.001;
    
    private void Start()
    {
    }

    public void InitializeBubbleApp()
    {
        LoadPosts();
        detailPage.SetActive(false);
        mainPage.SetActive(true);
        postPrefab.SetActive(false);
        foreach (var entry in commentCounts)
        {
            Debug.Log($"Post globalId: {entry.Key}, Comment count: {entry.Value}");
        }
    }

    private void LoadPosts()
    {
        postCount = 0;
        
        foreach (PostData post in JsonDataManager.Instance.GetPosts())
        {
            //Debug.Log($"post:{post.content}");
            GameObject postObj = Instantiate(postPrefab, postContent);
            
            postObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = post.title;
            postObj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = post.content;
            postObj.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = FormatTime(post.time);
            postObj.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = commentCounts[post.globalId] + "";
            postObj.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = post.poster;

            postObj.GetComponent<PostObject>().ConstructPostData(post);

            postObj.SetActive(true);
            
            postCount++;
        }

    }
    
    
    private string FormatTime(string timestamp)
    {
        long unixTime = long.Parse(timestamp);
        System.DateTime dateTime = System.DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }
    
    public void ShowDetail(PostData postData)
    {
        Transform postObjectTransform = detailContent.GetChild(0);
        postObjectTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = postData.poster;
        postObjectTransform.GetChild(1).GetComponent<TextMeshProUGUI>().text = postData.title;
        postObjectTransform.GetChild(2).GetComponent<TextMeshProUGUI>().text = postData.content;
        
        detailViewPort.GetChild(1).GetComponent<TextMeshProUGUI>().text = commentCounts[postData.globalId] + "";
        detailViewPort.GetChild(2).GetComponent<TextMeshProUGUI>().text = FormatTime(postData.time);
        
        ShowComments(postData);
        UnityEngine.Debug.Log($"Showing details for post: {postData.title} (globalId: {postData.globalId})");

        postObjectTransform.GetComponent<PostObject>().ConstructPostData(postData);
        
        mainPage.SetActive(false);
        detailPage.SetActive(true);
        scrollbar.value = 1;
    }

    public void CleanComments()
    {
        foreach (var comment in commentsInDetail)
        {
            Destroy(comment);
        }
        commentsInDetail.Clear();
    }
    
    private void ShowComments(PostData postData)
    {
        int count = 0;
          
        UnityEngine.Debug.Log($"Checking comments for post with globalId: {postData.globalId}");

        commentPrefab.SetActive(true);
        
        foreach (CommentData comment in JsonDataManager.Instance.bubbleAppData.comments)
        {
            UnityEngine.Debug.Log($"Comment globalId: {comment.globalId}");
            if (comment.parentPostId != postData.globalId)
            {
                UnityEngine.Debug.Log("Comment does not match, skipping.");
                UnityEngine.Debug.Log($"Post globalId: {postData.globalId}, Comment globalId: {comment.globalId}");
                continue;
            }
            UnityEngine.Debug.Log("Found matching comment!");

            GameObject commentObj = Instantiate(commentPrefab, commentsParent);
            
            commentObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = comment.poster;
            commentObj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = FormatTime(comment.time);
            commentObj.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = comment.content;

            commentObj.GetComponent<CommentObject>().ConstructCommentData(comment);
            
            commentObj.SetActive(true);
            commentsInDetail.Enqueue(commentObj);
            
            count++;
        }
        
        commentPrefab.SetActive(false);
        // VerticalLayoutController.Instance.SetVerticalLayout();
    }

    public void HideTimeAndCommentCounts()
    {
        if (Mathf.Abs(scrollbar.value - 1f) > epson)
        {
            detailViewPort.GetChild(1).gameObject.SetActive(false);
            detailViewPort.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            detailViewPort.GetChild(1).gameObject.SetActive(true);
            detailViewPort.GetChild(2).gameObject.SetActive(true);
        }
    }
}
