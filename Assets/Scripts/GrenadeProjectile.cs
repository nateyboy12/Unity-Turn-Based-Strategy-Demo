using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyGrenadeExploded;

    [SerializeField] private Transform grenadeExplodeVfxPrefab;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve arcYAnimationCurve;

    private Vector3 targetPosition;
    private Action onGrenadeBehaviorComplete;
    private float totalDistance;
    private Vector3 positionXZ;

    private float moveSpeed = 15f;
    private float reachedTargetDistance = 0.2f;
    private float damageRadius = 4f;
    private Vector3 explosionOffset = Vector3.up * 0.25f;

    private void Update()
    {
        Vector3 moveDir = (targetPosition - positionXZ).normalized;

        positionXZ += moveDir * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1 - (distance / totalDistance);


        float arcHeight = GetArcHeight(distanceNormalized, totalDistance);

        transform.position = new Vector3(positionXZ.x, arcHeight, positionXZ.z);

        if (Vector3.Distance(positionXZ, targetPosition) < reachedTargetDistance)
        {
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(30);
                }

                if (collider.TryGetComponent<DestructibleCrate>(out DestructibleCrate destructibleCrate))
                {
                    destructibleCrate.Damage();
                }
            }

            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);

            trailRenderer.transform.parent = null;

            Instantiate(grenadeExplodeVfxPrefab, targetPosition + explosionOffset, Quaternion.identity);

            Destroy(gameObject);

            onGrenadeBehaviorComplete();
        }
    }

    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviorComplete)
    {
        this.onGrenadeBehaviorComplete = onGrenadeBehaviorComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);
    }

    public float GetArcHeight(float distanceNormalized, float totalDistance)
    {
        float maxHeight = totalDistance / 4f;
        float baseHeight = Mathf.Lerp(GrenadeAction.UNIT_SHOULDER_HEIGHT, 0f, distanceNormalized);
        float arcHeight = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        return baseHeight + arcHeight;
    }

    public Vector3 GetArcWorldPosition(Vector3 startXZ, Vector3 endXZ, float distanceNormalized)
    {
        float totalDistance = Vector3.Distance(startXZ, endXZ);
        Vector3 positionXZ = Vector3.Lerp(startXZ, endXZ, distanceNormalized);
        float positionY = GetArcHeight(distanceNormalized, totalDistance);
        return new Vector3(positionXZ.x, positionY, positionXZ.z);
    }

    public bool IsThrowPathBlocked(Vector3 startWorldPosition, GridPosition toGridPosition, LayerMask layerMask)
    {
        Vector3 startXZ = startWorldPosition;
        startXZ.y = 0f;

        Vector3 endXZ = LevelGrid.Instance.GetWorldPosition(toGridPosition);
        endXZ.y = 0f;

        float totalDistance = Vector3.Distance(startXZ, endXZ);
        if (totalDistance <= 0.01f)
        {
            return false;
        }

        int sampleCount = Mathf.Max(8, Mathf.RoundToInt(totalDistance * 2f));
        Vector3 previousPoint = GetArcWorldPosition(startXZ, endXZ, 0f);

        for (int i = 1; i <= sampleCount; i++)
        {
            float t = i / (float)sampleCount;
            Vector3 currentPoint = GetArcWorldPosition(startXZ, endXZ, t);

            if (Physics.Linecast(previousPoint, currentPoint, out RaycastHit hit, layerMask))
            {
                GridPosition hitGridPosition = LevelGrid.Instance.GetGridPosition(hit.point);
                if (hitGridPosition != toGridPosition)
                {
                    return true;
                }
            }

            previousPoint = currentPoint;
        }

        return false;
    }
}
