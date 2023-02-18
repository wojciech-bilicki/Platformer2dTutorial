using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    [SerializeField]
    [Range(1, 10)] private float smoothFactor;

    [SerializeField] private bool shouldUseSmoothing = true;
    
    private void FixedUpdate()
    {
     Follow(); 
    }

    void Follow()
    {
        Vector3 targetPosition = target.position + offset;

        transform.position = shouldUseSmoothing
            ? Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.fixedDeltaTime)
            : new Vector3(targetPosition.x, transform.position.y, transform.position.z);
    }
}
