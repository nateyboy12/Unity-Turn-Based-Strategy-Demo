using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    private const int PATHFINDING_DISTANCE_MULTIPLIER = 10;
    [SerializeField] private int maxMoveDistance = 4;

    private List<Vector3> positionList;
    private int currentPositionIndex;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);

        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            currentPositionIndex++;
            if (currentPositionIndex >= positionList.Count)
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }
        }
    }

    public override void TakeAction(GridPosition targetPosition, Action onActionComplete)
    {
        List<GridPosition> pathGridPositionList = Pathfinding.Instance.FindPath(unit.GetGridPosition(), targetPosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach (GridPosition gridPosition in pathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(gridPosition));
        }
        OnStartMoving?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)
                    || unitGridPosition == testGridPosition                                                                                          //Starting spot
                    || LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)                                                                 //Other unit is here
                    || !Pathfinding.Instance.IsWalkableGridPosition(testGridPosition)                                                                //Obstacle
                    || !Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition)                                                             //No path to location
                    || Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * PATHFINDING_DISTANCE_MULTIPLIER)   //Don't search thru walls

                {
                    continue;
                }
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCount = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCount * 10,
        };
    }
}
