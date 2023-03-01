using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlatformCollision : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private Vector3 _previousPosition;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.TryGetComponent(out PlayerMovement movement))
        {
            _playerMovement = movement;
            _previousPosition = transform.position;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        // player has left the platform
        if (col.gameObject.TryGetComponent(out PlayerMovement movement))
        {
            _playerMovement.AdjustMovementWhenOnMovingPlatform(Vector3.zero);
            _playerMovement = null;
        }
    }

    private void Update()
    {
        if (_playerMovement)
        {
            Vector3 deltaPosition = transform.position - _previousPosition;
            _previousPosition = transform.position;
            _playerMovement.AdjustMovementWhenOnMovingPlatform(deltaPosition);
        }
    }
}
