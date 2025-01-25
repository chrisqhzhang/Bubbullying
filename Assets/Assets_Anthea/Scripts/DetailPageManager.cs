using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DetailPageManager : MonoBehaviour
{
    public Text titleText;             
    public Text contentText;           
    public Image postImage;              
    public Transform commentsContent;     // ���۹�����ͼ�� Content ���󣬻�δʵװ
    public GameObject commentPrefab;      // �������۵� Prefab����δʵװ

    public GameObject mainPageCanvas;
    public GameObject detailPageCanvas;

    //�ڶ���������Ϣ��List<string> comments��δʵװ
    public void UpdateDetailPage(PostData postData)
    {
        // ���ñ��������
        titleText.text = postData.title;
        contentText.text = postData.content;

        // ����ͼƬ
        if (!string.IsNullOrEmpty(postData.imagePath))
        {
            Sprite sprite = Resources.Load<Sprite>(postData.imagePath);
            postImage.sprite = sprite;
        }

        // ��վ�����
       // foreach (Transform child in commentsContent)
        //{
            //Destroy(child.gameObject);
        //}

        // ��̬��������
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
