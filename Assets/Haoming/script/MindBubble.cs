using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MindBubble : MonoBehaviour
{
    private Vector2 mousePosition;
    private float offsetX, offsetY;
    private static bool mouseReleased;
    
    private BubbleData bubbleData;
    
    public void ConstructMindBubble(BubbleData data)
    {
        bubbleData = data;
        GameObject newBubbleObject = Instantiate(data.Screenshot, this.transform);
        
        newBubbleObject.transform.localPosition = Vector3.zero + 0.2f * Vector3.up;
        newBubbleObject.transform.localRotation = Quaternion.identity;
        newBubbleObject.transform.localScale = this.transform.lossyScale * 0.1f;
        
        transform.GetChild(0).GetComponent<TextMeshPro>().text = data.Description;
    }

    public int GetId()
    {
        return bubbleData.Id;
    }
    
    private void OnMouseDown()
    {
        mouseReleased = false;
        offsetX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
        offsetX = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
    }

    private void OnMouseDrag()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(mousePosition.x - offsetX, mousePosition.y - offsetY);
    }

    private void OnMouseUp()
    {
        mouseReleased = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!mouseReleased) return;
    }
}
