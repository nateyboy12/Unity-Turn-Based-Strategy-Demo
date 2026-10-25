using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class CurrencySystemUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI turnNumberText;

    private void Start()
    {
        CurrencySystem.Instance.OnCoinCountChanged += CurrencySystem_OnCoinCountChanged;
        UpdateTurnText();
    }

    private void UpdateTurnText()
    {
        turnNumberText.text = "COINS: " + CurrencySystem.Instance.GetCoinCount();
    }

    private void CurrencySystem_OnCoinCountChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
    }
}
