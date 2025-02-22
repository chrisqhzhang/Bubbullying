using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Numerics;

public class GameManager : Singleton<GameManager>
{
    void Start()
    {
        LoadAndInitialize();
    }
    
    public async Task LoadAndInitialize()
    {
        await JsonDataManager.Instance.LoadFromJson(); 
        BubbleAppManager.Instance.InitializeBubbleApp(); 
    }
}