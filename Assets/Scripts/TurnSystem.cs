using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnChanged;

    private int turnNumber = 1;
    private bool isPlayerTurn = true;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("TurnSystem already exists: " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void NextTurn()
    {
        isPlayerTurn = !isPlayerTurn;
        if (isPlayerTurn)
        {
            turnNumber++;
        }

        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
