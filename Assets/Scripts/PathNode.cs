using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PathNode
{
    private GridPosition gridPosition;
    private int gCost;
    private int hCost;
    private int fCost;
    private PathNode prevPathNode;
    private bool isWalkable = true;

    public PathNode(GridPosition gridPosition) { this.gridPosition = gridPosition; }

    public override string ToString() { return gridPosition.ToString(); }

    public int GetGCost() { return gCost; }
    public int GetHCost() { return hCost; }
    public int GetFCost() { return fCost; }
    public GridPosition GetGridPosition() { return gridPosition; }
    public PathNode GetPrevPathNode() { return prevPathNode; }
    public bool IsWalkable() { return isWalkable; }

    public void SetGCost(int gCost) { this.gCost = gCost; }
    public void SetHCost(int hCost) { this.hCost = hCost; }
    public void CalculateFCost() { fCost = gCost + hCost; }
    public void setPrevPathNode(PathNode pathNode) { prevPathNode = pathNode; }
    public void setIsWalkable(bool isWalkable) { this.isWalkable = isWalkable; }

    public void ResetPrevPathNode() { prevPathNode = null; }
}
