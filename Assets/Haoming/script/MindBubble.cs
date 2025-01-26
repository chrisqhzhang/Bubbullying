using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class MindBubble : MonoBehaviour
{
    private Vector2 mousePosition;
    private float offsetX, offsetY;
    private static bool mouseReleased;
    
    private float bubbleRadius;  
    private Vector2 screenBounds;
    private Vector2 positionTemp;
    
    [SerializeField] private float repulsionForce = 0.1f;
    
    [SerializeField] private float pulseAmplitude = 0.1f; 
    [SerializeField] private float pulseSpeed = 2f; 
    private float enmergingDuration = 2f;
    
    private Vector3 originalScale;
    
    private BubbleData bubbleData;
    
    private bool isEnmerging;
    

    private void Awake()
    {
        Camera mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        bubbleRadius = collider.bounds.extents.x;
        
        originalScale = transform.localScale;

        pulseAmplitude += Random.Range(-0.02f, 0.02f);
        pulseSpeed += Random.Range(-0.3f, 0.3f);
    }

    void Update()
    {
        EnsureBoundary();
        Breathing();
    }

    private void EnsureBoundary()
    {
        positionTemp.x = Mathf.Clamp(transform.position.x, -screenBounds.x + bubbleRadius, screenBounds.x - bubbleRadius);
        positionTemp.y = Mathf.Clamp(transform.position.y, -screenBounds.y + bubbleRadius, screenBounds.y - bubbleRadius);
        transform.position = positionTemp;
    }

    private void Breathing()
    {
        if (isEnmerging) return;
        float scale = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseAmplitude;
        transform.localScale = originalScale * scale;
    }
    
    public void Enmerge()
    {
        isEnmerging = true;
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, originalScale, enmergingDuration).setEase(LeanTweenType.easeOutBack);
        LeanTween.delayedCall(enmergingDuration, () =>
        {
            isEnmerging = false;
            transform.localScale = originalScale;
        });
    }
    
    public void ConstructMindBubble(BubbleData data)
    {
        bubbleData = data;
        // GameObject newBubbleObject = Instantiate(data.Screenshot, this.transform);
        //
        // newBubbleObject.transform.localPosition = Vector3.zero + 0.2f * Vector3.up;
        // newBubbleObject.transform.localRotation = Quaternion.identity;
        // newBubbleObject.transform.localScale = this.transform.lossyScale * 0.1f;
        
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
        Vector2 pos = new Vector2(mousePosition.x - offsetX, mousePosition.y - offsetY);
        transform.position = pos;
    }

    private void OnMouseUp()
    {
        mouseReleased = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!mouseReleased) return;

        BubbleData bubbleDataOther = collision.GetComponent<MindBubble>().bubbleData;
        
        if (!MindBubbleManager.Instance.CanMerge(bubbleData, bubbleDataOther)) return;

        MergeBubbles(bubbleData.Size + bubbleDataOther.Size, 
                        bubbleData.Id + bubbleDataOther.Id);
        
        Destroy(this.gameObject);
        Destroy(collision.gameObject);
    }

    private void MergeBubbles(int size, int Id)
    {
        GameObject newBubble = null;
        switch (size)
        {
            case 2:
                newBubble = Instantiate(Resources.Load("Bubble2"), transform.parent) as GameObject;
                return;
            case 3:
                newBubble = Instantiate(Resources.Load("Bubble3"), transform.parent) as GameObject;
                return;
            case 4:
                newBubble = Instantiate(Resources.Load("Bubble4"), transform.parent) as GameObject;
                return;
        }

        MindBubble mindBubble = newBubble.GetComponent<MindBubble>();
        mindBubble.ConstructMindBubble(MindBubbleManager.Instance.GetBubbleData(Id));
        MindBubbleManager.Instance.RecordMergedNewBubble(newBubble.GetComponent<MindBubble>());
    }
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector2 direction = (transform.position - other.transform.position).normalized;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction * repulsionForce, ForceMode2D.Impulse);
        
        MindBubbleManager.Instance.TriggerBubbleCollideSound();
    }

}
