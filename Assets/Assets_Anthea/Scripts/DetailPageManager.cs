using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DetailPageManager : MonoBehaviour
{
    public Text titleText;             
    public Text contentText;           
    public Image postImage;              
    public Transform commentsContent;     // 评论滚动视图的 Content 对象，还未实装
    public GameObject commentPrefab;      // 单条评论的 Prefab，还未实装

    public GameObject mainPageCanvas;
    public GameObject detailPageCanvas;

    //第二个输入信息：List<string> comments，未实装
    public void UpdateDetailPage(PostData postData)
    {
        // 设置标题和内容
        titleText.text = postData.title;
        contentText.text = postData.content;

        // 加载图片
        if (!string.IsNullOrEmpty(postData.imagePath))
        {
            Sprite sprite = Resources.Load<Sprite>(postData.imagePath);
            postImage.sprite = sprite;
        }

        // 清空旧评论
       // foreach (Transform child in commentsContent)
        //{
            //Destroy(child.gameObject);
        //}

        // 动态生成评论
        //foreach (string comment in comments)
        //{
            GameObject commentObj = Instantiate(commentPrefab, commentsContent);
         ///   commentObj.GetComponent<Text>().text = comment;
        //}
    }

    public void OnBackButtonClicked()
    {
        detailPageCanvas.SetActive(false);
        mainPageCanvas.SetActive(true);
    }
}
