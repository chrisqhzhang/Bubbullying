using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

[System.Serializable]
public class BubbleData
{
    [NonSerialized] public BigInteger Id;
    public List<int> Ids; 
    public string Description;
    public int Size;
    [NonSerialized] public GameObject Screenshot;
    
    public void ConstructBubbleData(BigInteger id, string description, GameObject screenshot)
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