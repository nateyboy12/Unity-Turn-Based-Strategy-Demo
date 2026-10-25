using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool inverted;

    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (inverted)
        {
            Vector3 dirAwayFromCamera = (transform.position - cameraTransform.position).normalized;
            transform.LookAt(transform.position + dirAwayFromCamera);

        }
        else
        {
            transform.LookAt(cameraTransform);
        }
    }
}
