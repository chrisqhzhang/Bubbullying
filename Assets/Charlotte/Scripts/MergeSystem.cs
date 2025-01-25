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
        CreateRecipes();
    }

    private void Update()
    {
        CheckMergeRecipes();
    }

    private void CreateRecipes()
    {
        mergeRecipes[0] = new List<long> {21};
        mergeRecipes[1] = new List<long> {31};
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