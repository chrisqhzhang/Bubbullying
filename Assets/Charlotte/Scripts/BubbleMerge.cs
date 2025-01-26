using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleMerge : MonoBehaviour
{
    //public Button mindButton;
    //private Queue<MindBubble> mindBubbles;

    private Vector2 mousePosition;
    private float offsetX, offsetY;
    private static bool mouseReleased;

    //public MergeSystem ms;

    //private void Start()
    //{
    //    Button btn = mindButton.GetComponent<Button>();
    //    btn.onClick.AddListener(GetBubbleData);
    //}

    private void Update()
    {
        DissembleBubble();
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

    public void OnTriggerStay2D(Collider2D collision)
    {
        string thisGameObjectName;
        string collisionGameObjectName;

        thisGameObjectName = gameObject.name.Substring(0, name.IndexOf("_"));
        collisionGameObjectName = collision.gameObject.name.Substring(0, name.IndexOf("_"));

        if (/*ms.isMergable &&*/ mouseReleased && thisGameObjectName == "SmallBubble" && thisGameObjectName == collisionGameObjectName)
        {
            Instantiate(Resources.Load("MidBubble_Object"), transform.position, Quaternion.identity);
            mouseReleased = false;
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
        else if (/*ms.isMergable &&*/ mouseReleased && thisGameObjectName == "MidBubble")
        {
            Instantiate(Resources.Load("BigBubble_Object"), transform.position, Quaternion.identity);
            mouseReleased = false;
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }

    private void DissembleBubble()
    {
        if (Input.GetMouseButtonDown(1))
        {
            InstantiateBubbles(2);
        }
    }

    private void InstantiateBubbles(int bubbleNum)
    {
        string thisGameObjectName;

        thisGameObjectName = gameObject.name.Substring(0, name.IndexOf("_"));

        for (int i = 0; i < bubbleNum; i++)
        {
            if (thisGameObjectName == "BigBubble")
            {
                Instantiate(Resources.Load("MidBubble_Object"), new Vector2(transform.position.x + i, transform.position.y), Quaternion.identity);
                Destroy(gameObject);
            }
            else if (thisGameObjectName == "MidBubble")
            {
                Instantiate(Resources.Load("SmallBubble_Object"), new Vector2(transform.position.x + i, transform.position.y), Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    //public void GetBubbleData()
    //{
    //    mindBubbles = MindBubbleManager.Instance.GetCapturedData();
    //    foreach (var mindBubble in mindBubbles)
    //    {
    //        Debug.Log($"{mindBubble.Id},{mindBubble.Content},{mindBubble.size}");
    //    }
    //}
}

