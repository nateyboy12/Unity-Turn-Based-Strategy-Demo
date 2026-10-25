using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBarrel : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform barrelDestroyedPrefab;
    [SerializeField] private GameObject barrelPrefab;

    private GridPosition gridPosition;
    private Action onInteractComplete;
    private float timer;
    private bool isActive;
    private int coinCount = 1;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        Pathfinding.Instance.SetWalkableGridPosition(gridPosition, false);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            isActive = false;
            Pathfinding.Instance.SetWalkableGridPosition(gridPosition, true);
            LevelGrid.Instance.ClearInteractableAtGridPosition(gridPosition);

            Destroy(gameObject);
            onInteractComplete();
        }
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public void Interact(Action onInteractComplete)
    {
        this.onInteractComplete = onInteractComplete;
        isActive = true;
        timer = 0.5f;

        barrelPrefab.SetActive(false);
        Transform barrelDestroyedTransform = Instantiate(barrelDestroyedPrefab, barrelPrefab.transform.position, barrelPrefab.transform.rotation);

        CurrencySystem.Instance.AddCoins(coinCount);

        ApplyExplosionToChildren(barrelDestroyedTransform, 150f, transform.position, 10f);
    }

    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidBody))
            {
                childRigidBody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }
            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
