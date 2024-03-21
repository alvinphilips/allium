using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Scripts.Patterns;
using Game.Scripts.Game;
using TMPro;

public class HUDManager : Singleton<HUDManager>
{
    ResourceManager resourceManager;

    public TMP_Text MoneyText;

    private void Start()
    {
        resourceManager = ResourceManager.Instance; 
    }

    private void Update()
    {
        MoneyText.text = resourceManager.GetMoney().ToString();
    }
}
