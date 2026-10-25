using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    public event EventHandler<OnAnyUnitMovedGridPositionEventArgs> OnAnyUnitMovedGridPosition;
    public class OnAnyUnitMovedGridPositionEventArgs : EventArgs
    {
        public Unit unit;
        public GridPosition fromGridPosition;
        public GridPosition toGridPosition;
    }

    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellSize = 2f;

    private GridSystem<GridObject> gridSystem;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("LevelGrid already exists: " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        gridSystem = new GridSystem<GridObject>(width, height, cellSize,
                (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
        // gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    private void Start()
    {
        Pathfinding.Instance.Setup(width, height, cellSize);
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        gridSystem.GetGridObject(gridPosition).AddUnit(unit);
    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition) => gridSystem.GetGridObject(gridPosition).GetUnitList();

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        gridSystem.GetGridObject(gridPosition).RemoveUnit(unit);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);

        OnAnyUnitMovedGridPosition?.Invoke(this, new OnAnyUnitMovedGridPositionEventArgs
        {
            unit = unit,
            fromGridPosition = fromGridPosition,
            toGridPosition = toGridPosition
        });
    }

    public void SetInteractableAtGridPosition(GridPosition gridPosition, IInteractable interactable)
    {
        gridSystem.GetGridObject(gridPosition).SetInteractable(interactable);
    }

    public void ClearInteractableAtGridPosition(GridPosition gridPosition)
    {
        gridSystem.GetGridObject(gridPosition).ClearInteractable();
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition) => gridSystem.GetGridObject(gridPosition).HasAnyUnit();

    public Unit GetUnitAtGridPosition(GridPosition gridPosition) => gridSystem.GetGridObject(gridPosition).GetUnit();

    public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition) => gridSystem.GetGridObject(gridPosition).GetInteractable();

    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();
}
