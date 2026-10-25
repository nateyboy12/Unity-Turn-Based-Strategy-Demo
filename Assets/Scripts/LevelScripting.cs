using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScripting : MonoBehaviour
{
    [SerializeField] private GameObject allHiderSections;

    //Using lists allows me to reveal objects within a section depending on unit distance to the object
    [SerializeField] private List<GameObject> hiderSection1;
    [SerializeField] private List<GameObject> hiderSection2;
    [SerializeField] private List<GameObject> hiderSection3;
    [SerializeField] private List<GameObject> hiderSection4;
    [SerializeField] private List<GameObject> enemyList1;
    [SerializeField] private List<GameObject> enemyList2;
    [SerializeField] private List<GameObject> enemyList3;
    [SerializeField] private List<GameObject> enemyList4;
    [SerializeField] private Door door1;
    [SerializeField] private Door door2;

    private bool hasShownFirstHider = false;
    private bool hasShownThirdHider = false;

    private void Start()
    {
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;
        allHiderSections.SetActive(true);
        door1.OnDoorOpened += (object sender, EventArgs e) =>
        {
            SetActiveGameObjectList(hiderSection2, false);
            SetActiveGameObjectList(enemyList2, true);
        };
        door2.OnDoorOpened += (object sender, EventArgs e) =>
        {
            SetActiveGameObjectList(hiderSection4, false);
            SetActiveGameObjectList(enemyList4, true);
        };
    }

    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, LevelGrid.OnAnyUnitMovedGridPositionEventArgs e)
    {
        if (e.unit.IsEnemy())
        {
            //Enemy shouldn't open up a new zone
            return;
        }

        if (e.toGridPosition.z >= 5 && !hasShownFirstHider)
        {
            hasShownFirstHider = true;
            SetActiveGameObjectList(hiderSection1, false);
            SetActiveGameObjectList(enemyList1, true);
        }

        if (e.toGridPosition.z >= 11 && e.toGridPosition.x >= 9 && !hasShownThirdHider)
        {
            hasShownThirdHider = true;
            SetActiveGameObjectList(hiderSection3, false);
            SetActiveGameObjectList(enemyList3, true);
        }
    }

    private void SetActiveGameObjectList(List<GameObject> gameObjectList, bool isActive)
    {
        foreach (GameObject gameObject in gameObjectList)
        {
            gameObject.SetActive(isActive);
        }
    }

}
