using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentObject : CapturableObject
{
    private CommentData commentData;
    
    public void ConstructCommentData(CommentData comment)
    {
        commentData = comment;
        bubbleData = new BubbleData();
        bubbleData.ConstructBubbleData(comment.globalId, comment.summary, gameObject);
    }
}
