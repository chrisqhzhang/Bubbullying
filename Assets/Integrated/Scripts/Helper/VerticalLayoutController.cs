using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class VerticalLayoutController : Singleton<VerticalLayoutController>
{
    public void SetVerticalLayout()
    {
        float position = 0, commentHeight;
        for (int i = 0; i < transform.childCount; i++)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetChild(i).GetComponent<RectTransform>());
            commentHeight = LayoutUtility.GetPreferredHeight(transform.GetChild(i).GetComponent<RectTransform>());
            position += commentHeight;
            
            Transform commentTransform = transform.GetChild(i).transform;
            commentTransform.position = new Vector2(commentTransform.position.x, position);
        }
    }
}