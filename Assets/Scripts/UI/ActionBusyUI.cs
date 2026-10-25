using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionBusyUI : MonoBehaviour
{
    private void Start()
    {
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
        DisableActionBusyUI();
    }

    private void UnitActionSystem_OnBusyChanged(object sender, bool isBusy)
    {
        if (isBusy)
        {
            EnableActionBusyUI();
        }
        else
        {
            DisableActionBusyUI();
        }
    }

    private void EnableActionBusyUI()
    {
        gameObject.SetActive(true);
    }

    private void DisableActionBusyUI()
    {
        gameObject.SetActive(false);

    }
}
