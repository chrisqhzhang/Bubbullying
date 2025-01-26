using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MindBubble : MonoBehaviour
{
    private Vector2 mousePosition;
    private float offsetX, offsetY;
    
    private static bool isMouseReleased;
    private static bool isMouseHold;
    
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

    [SerializeField] private Collider2D avoidCollisionCollider;
    [SerializeField] private Collider2D dragToMiddleCollider; 
    
    private void Awake()
    {
        Camera mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        bubbleRadius = collider.bounds.extents.x;
        
        originalScale = transform.localScale;

        pulseAmplitude += Random.Range(-0.02f, 0.02f);
        pulseSpeed += Random.Range(-0.3f, 0.3f);

        avoidCollisionCollider = GetComponent<Collider2D>();
        dragToMiddleCollider = transform.GetChild(0).GetComponent<Collider2D>();
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
            if (this != null)
            {
                transform.localScale = originalScale;
            }
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
        
        transform.GetChild(1).GetComponent<TextMeshPro>().text = data.Description;
    }

    public BigInteger GetId()
    {
        return bubbleData.Id;
    }
    
    private void OnMouseDown()
    {
        isMouseReleased = false;
        isMouseHold = true;
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
        isMouseReleased = true;
        isMouseHold = false;
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isMouseReleased) return;
        
        if (dragToMiddleCollider.bounds.Intersects(other.bounds) &&
            other.CompareTag("MergeCollider"))
        {
            if (MindBubbleManager.Instance.isMerge) return; 
        
            BubbleData bubbleDataOther = other.transform.parent?.GetComponent<MindBubble>().bubbleData;

            if (!MindBubbleManager.Instance.CanMerge(bubbleData, bubbleDataOther)) return;

            MindBubbleManager.Instance.isMerge = true; 
        
            MergeBubbles(bubbleData.Size + bubbleDataOther.Size,
                BubbleHelper.GetMergeId(bubbleData.Id, bubbleDataOther.Id));
            
            MindBubbleManager.Instance.isMerge = false;
            
            Destroy(other.transform.parent.gameObject);
            Destroy(this.gameObject);
            
            return;
        }
    }
    
    
    private void MergeBubbles(int size, BigInteger Id)
    {
        GameObject newBubble = null;
        switch (size)
        {
            case 2:
                newBubble = Instantiate(Resources.Load("Bubble2"), transform.parent) as GameObject;
                break;
            case 3:
                newBubble = Instantiate(Resources.Load("Bubble3"), transform.parent) as GameObject;
                break;
            case 4:
                newBubble = Instantiate(Resources.Load("Bubble4"), transform.parent) as GameObject;
                break;
        }

        newBubble.transform.position = transform.position;
        
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
