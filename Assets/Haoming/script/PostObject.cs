using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PostObject : CapturableObject
{
    private PostData postData;
    
    public void ConstructPostData(PostData post)
    {
        postData = post;
        bubbleData = new BubbleData();
        bubbleData.ConstructBubbleData(post.globalId, post.summary, gameObject);
    }

    public void ShowDetail()
    {
        BubbleAppManager.Instance.ShowDetail(postData);
    }
    
    protected void OnMouseUp()
    {
        HandleClick(this);
    }

}
