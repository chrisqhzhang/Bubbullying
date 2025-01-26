using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BubbleData
{
    public int Id;
    public string Description;
    public int Size;
    [NonSerialized] public GameObject Screenshot;
    
    public void ConstructBubbleData(int id, string description, GameObject screenshot)
    {
        this.Screenshot = null; // TODO screenshot
        this.Id = id;
        this.Description = description;
        this.Size = 1;
    }
    
    public void ConstructBubbleDataFromOther(BubbleData data)
    {
        this.Screenshot = data.Screenshot; // TODO
        this.Id = data.Id;
        this.Description = data.Description;
        this.Size = 1;
    }
    
}