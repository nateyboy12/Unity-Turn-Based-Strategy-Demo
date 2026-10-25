using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CurrencySystem : MonoBehaviour
{
    public static CurrencySystem Instance { get; private set; }

    public event EventHandler OnCoinCountChanged;

    private int coinCount = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("CurrencySystem already exists: " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddCoins(int coinCount)
    {
        this.coinCount += coinCount;
        OnCoinCountChanged?.Invoke(this, EventArgs.Empty);
    }

    public void LoseCoins(int coinCount)
    {
        this.coinCount -= coinCount;
        OnCoinCountChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetCoinCount()
    {
        return coinCount;
    }

}
