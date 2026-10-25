using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    [SerializeField] private Transform grenadeProjectilePrefab;
    [SerializeField] private LayerMask obstaclesLayerMask;

    private int maxThrowDistance = 7;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
    }

    public override string GetActionName()
    {
        return "Grenade";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
        {
            for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (unitGridPosition == testGridPosition) //starting location
                {
                    continue;
                }
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Math.Abs(x) + Math.Abs(z);
                if (testDistance > maxThrowDistance)
                {
                    //Out of range
                    continue;
                }

                GrenadeProjectile grenadeProjectile = grenadeProjectilePrefab.GetComponent<GrenadeProjectile>();
                if (grenadeProjectile.IsThrowPathBlocked(
                        unit.GetWorldPosition(),
                        testGridPosition,
                        obstaclesLayerMask))
                {
                    //Grenade arc is blocked by object
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Transform grenadeProjectileTransform = Instantiate(grenadeProjectilePrefab, unit.GetWorldPosition() + Vector3.up * UNIT_SHOULDER_HEIGHT, Quaternion.identity);
        GrenadeProjectile grenadeProjectile = grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
        grenadeProjectile.Setup(gridPosition, OnGrenadeBehaviorComplete);

        ActionStart(onActionComplete);
    }

    private void OnGrenadeBehaviorComplete()
    {
        ActionComplete();
    }
}
