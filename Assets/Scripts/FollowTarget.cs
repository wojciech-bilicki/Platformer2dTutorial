using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [Tooltip("Target for camera to follow")]
    [SerializeField] private Transform target;

    [Tooltip("Offset of the camera from the player")]
    [SerializeField]
    private Vector3 offset;

    [SerializeField] [Range(1, 10)] private float smoothFactor;

    private void FixedUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        Vector3 cameraPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, cameraPosition, smoothFactor*Time.fixedDeltaTime);
    }
}
