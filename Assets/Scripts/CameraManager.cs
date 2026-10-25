using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;

    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;

        HideActionCamera();
    }

    private void showActionCamera()
    {
        actionCameraGameObject.SetActive(true);
    }

    private void HideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();

                Vector3 characterCameraHeight = Vector3.up * 1.7f;

                Vector3 shoorDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;
                float shoulderOffsetAmount = 0.5f;
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shoorDir * shoulderOffsetAmount;

                Vector3 actionCameraPosition =
                    shooterUnit.GetWorldPosition() + //Location
                    characterCameraHeight + //Height
                    shoulderOffset - //To the side of shoulder
                    shoorDir; // Backwards so we can see the shooter

                actionCameraGameObject.transform.position = actionCameraPosition;
                actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + characterCameraHeight);

                showActionCamera();
                break;
        }
    }

    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;
        }
    }
}
