using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MergeSystem : MonoBehaviour
{
    public List<BubbleData> bubbleList;
    private List<List<long>> mergeRecipes = new List<List<long>>();
    public bool isMergable = false;

    private void Start()
    {
        bubbleList = new List<BubbleData>();

        GameObject bubbleObject1 = Instantiate(Resources.Load("SmallBubble")) as GameObject;
        GameObject bubbleObject2 = Instantiate(Resources.Load("SmallBubble")) as GameObject;
        GameObject bubbleObject3 = Instantiate(Resources.Load("SmallBubble")) as GameObject;
        GameObject bubbleObject4 = Instantiate(Resources.Load("SmallBubble")) as GameObject;

        AddBubble(10, "Bubble One", bubbleObject1);
        AddBubble(11, "Bubble Two", bubbleObject2);
        AddBubble(20, "Bubble Three", bubbleObject3);
        AddBubble(21, "Bubble Four", bubbleObject4);

        mergeRecipes[0] = new List<long> { 21 };
        mergeRecipes[1] = new List<long> { 31 };
    }

    private void Update()
    {
        CheckMergeRecipes();
    }

    private void AddBubble(int id, string description, GameObject screenshot)
    {
        BubbleData newBubble = new BubbleData();
        newBubble.ConstructBubbleData(id, description, screenshot);
        bubbleList.Add(newBubble);
    }

    public void CheckMergeRecipes()
    {
        long currentRecipe = 0;

        foreach (BubbleData bubble in bubbleList)
        {
            if(bubble != null)
            {
                currentRecipe += bubble.Id;
            }
            else
            {
                currentRecipe += 0;
            }
        }
        for (int i = 0; i < mergeRecipes.Count; i++)
        {
            if (mergeRecipes[i].Contains(currentRecipe))
            {
                isMergable = true;
                Debug.Log($"Bubbles are mergable");
            }
            else
            {
                isMergable = false;
                Debug.Log($"Bubbles aren't mergable");
            }
        }
    }
}