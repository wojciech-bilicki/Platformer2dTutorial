using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

internal enum MovementAxis
{
    X,
    Y
}

public class AxisMovement : MonoBehaviour
{
    [SerializeField] private float movementOffset = 0.75f;
    [SerializeField] private float movementSpeed = 0.1f;
    [Tooltip("Set value to 0 to disable movement pausing")]
    [SerializeField] private float movementPauseInSeconds = 1f;

    [SerializeField] private MovementAxis movementAxis = MovementAxis.Y;
    
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    
    // Indicates in what direction the trap is currently moving
    private Vector3 _movementDirection;
    private bool _wait;

    private void Awake()
    {
        _startPosition = transform.position;
        _endPosition = _startPosition + (movementAxis == MovementAxis.Y ? Vector3.up : Vector3.right) * movementOffset;
    }

    private void Update()
    {
        if (_wait)
        {
            return;
        }

        if (movementAxis == MovementAxis.X)
        {
            MoveLeftRight();
        }
        else
        {
            MoveUpDown();    
        }
        
    }

    private void MoveLeftRight()
    {
        if (_endPosition.x - transform.position.x > 0.1f && _movementDirection == Vector3.right) // should the trap keep moving right
        {
            //Move right
            transform.position += Vector3.right * (Time.deltaTime * movementSpeed);
        } else if (transform.position.x - _startPosition.x > 0.1f && _movementDirection == Vector3.left)// should the trap keep moving left
        {
            transform.position += Vector3.left * (Time.deltaTime * movementSpeed); // Move left
        }
        else
        {
            _movementDirection = _movementDirection == Vector3.right ? Vector3.left : Vector3.right; //switch the direction at the left or right end point
            if (movementPauseInSeconds > 0f)
            {
                _wait = true;
           
                StartCoroutine(Wait());
            }
           
        }
    }

    private void MoveUpDown()
    {
       
        if (_endPosition.y - transform.position.y > 0.1f && _movementDirection == Vector3.up) // should the trap keep moving up
        {
            //Move up
            transform.position += Vector3.up * (Time.deltaTime * movementSpeed);
        } else if (transform.position.y - _startPosition.y > 0.1f && _movementDirection == Vector3.down)// should the trap keep moving down
        {
            transform.position += Vector3.down * (Time.deltaTime * movementSpeed);
        }
        else
        {
            _movementDirection = _movementDirection == Vector3.down ? Vector3.up : Vector3.down; //switch the direction at the top or bottom
            if (movementPauseInSeconds > 0f)
            {
                _wait = true;
           
                StartCoroutine(Wait());
            }
           
        }
        
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(movementPauseInSeconds);
        _wait = false;
    }

}
