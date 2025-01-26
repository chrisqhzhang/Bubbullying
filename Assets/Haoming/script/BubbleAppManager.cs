using System;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Vector3 = System.Numerics.Vector3;
using System.Collections.Generic;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;


public class BubbleAppManager : Singleton<BubbleAppManager>
{
    public Transform postContent;
    public Transform detailContent;
    public GameObject postPrefab;
    [SerializeField] private float pageHeightOffset;
    [SerializeField] private float detailPageHeightOffset;
    [SerializeField] private float verticalOffset;
    [SerializeField] private float horizontalOffset;
    
    [SerializeField] private GameObject detailPage;
    [SerializeField] private GameObject mainPage;
    [SerializeField] private Transform commentContent;
    [SerializeField] private GameObject commentPrefab;
    [SerializeField] private float commentVerticalOffset;
    
    private float postVerticalHeight;
    private int postCount;

    public Dictionary<BigInteger, int> commentCounts = new Dictionary<BigInteger, int>();
    
    private Queue<GameObject> commentsInDetail = new Queue<GameObject>();

    
    // TODO, only return past posts

    private void Start()
    {
        LoadPosts();
        detailPage.SetActive(false);
        postVerticalHeight = postPrefab.transform.GetChild(0).GetComponent<RectTransform>().rect.height;
        foreach (var entry in commentCounts)
        {
            UnityEngine.Debug.Log($"Post globalId: {entry.Key}, Comment count: {entry.Value}");
        }
    }

    public void LoadPosts()
    {
        postCount = 0;
        
        foreach (PostData post in JsonDataManager.Instance.GetPosts())
        {
            GameObject postObj = Instantiate(postPrefab, postContent);
            
            postObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = post.title;
            postObj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = post.content;
            postObj.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = FormatTime(post.time);
            postObj.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = commentCounts[post.globalId] + "";
            postObj.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = post.poster;

            postObj.GetComponent<PostObject>().ConstructPostData(post);
            
            Vector2 positionTemp = postPrefab.transform.position;
            postObj.transform.position = new Vector2( positionTemp.x + horizontalOffset * (postCount % 3), 
                                                        positionTemp.y - verticalOffset * ( postCount / 3 ) );
            postObj.SetActive(true);
            
            postCount++;
        }

        SetContentHeight();
    }

    private void SetContentHeight()
    {
        Rect rectTemp = postContent.GetComponent<RectTransform>().rect;
        rectTemp.height = postCount * (postVerticalHeight + verticalOffset) + pageHeightOffset;
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
        postObjectTransform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = postData.poster;
        postObjectTransform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = FormatTime(postData.time);
        postObjectTransform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text = postData.content;
        postObjectTransform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = postData.title;
        postObjectTransform.GetChild(0).GetChild(5).GetComponent<TextMeshProUGUI>().text = commentCounts[postData.globalId] + "";
        
        ShowComments(postData);
        UnityEngine.Debug.Log($"Showing details for post: {postData.title} (globalId: {postData.globalId})");

        postObjectTransform.GetComponent<PostObject>().ConstructPostData(postData);
        
        mainPage.SetActive(false);
        detailPage.SetActive(true);
    }

    public void CleanComments()
    {
        foreach (var comment in commentsInDetail)
        {
            Destroy(comment);
        }
        commentsInDetail.Clear();
        commentPrefab.SetActive(false);
    }
    
    private void ShowComments(PostData postData)
    {
        int count = 0;
            
        RectTransform rectTransform = detailContent.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, commentCounts[postData.globalId] * ( commentPrefab.GetComponent<RectTransform>().rect.height) + detailPageHeightOffset);

        UnityEngine.Debug.Log($"Checking comments for post with globalId: {postData.globalId}");

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

            GameObject commentObj = Instantiate(commentPrefab, detailContent);
            
            commentObj.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = comment.content;
            commentObj.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = FormatTime(comment.time);
            commentObj.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text = comment.poster;

            commentObj.GetComponent<CommentObject>().ConstructCommentData(comment);
            
            Vector2 positionTemp = commentObj.transform.position;
            commentObj.transform.position = new Vector2( positionTemp.x, 
                positionTemp.y - commentVerticalOffset * count);
            
            commentObj.SetActive(true);
            commentsInDetail.Enqueue(commentObj);
            
            count++;
        }
    }
    
    
}
