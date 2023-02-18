using System;
using System.Collections;
using UnityEngine;



enum MovementAxis
{
    X,
    Y
}

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private float movementOffset = 0.5f;
    [SerializeField] private float movementPauseInSeconds = 1f;
    
    [Tooltip("Set movement speed to 0 to make the trap static")]
    [SerializeField] private float movementSpeed = 0.1f;

    [SerializeField] private MovementAxis movementAxis = MovementAxis.Y;
    
    [SerializeField]
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Vector3 _movementDirection;

    private bool _wait;
    void Start()
    {
        _startPosition = transform.position;
        _endPosition = _startPosition + ( movementAxis == MovementAxis.Y ? Vector3.up : Vector3.right) * movementOffset;
        _movementDirection = movementAxis == MovementAxis.Y ? Vector3.up : Vector3.right;
    }

    // Update is called once per frame
    
        void Update()
        {
            if (_wait) return;

            if (movementAxis == MovementAxis.X)
            {
                MoveLeftRight();
            } 
            else if (movementAxis == MovementAxis.Y)
            {
                MoveUpDown();
            }
        }



        private void MoveUpDown()
        {
              
            if (  _endPosition.y - transform.position.y > 0.1f && _movementDirection == Vector3.up)  // If the GameObject hasn't moved up to the offset distance yet
            {
                transform.position +=  Vector3.up * (Time.deltaTime * movementSpeed);  // Move up
            }
            else if (transform.position.y - _startPosition.y > 0.1f && _movementDirection == Vector3.down)  // If the GameObject is above its starting position
            {
               
                transform.position  +=  Vector3.down * (Time.deltaTime * movementSpeed);;  // Move down
            } else 
            { 
                StartCoroutine(Wait());
                _wait = true;
                _movementDirection = _movementDirection ==  Vector3.down ? Vector3.up : Vector3.down;
            }
        }

        private void MoveLeftRight()
        {
              
            if (  _endPosition.x- transform.position.x > 0.1f && _movementDirection == Vector3.right)  // If the GameObject hasn't moved up to the offset distance yet
            {
                transform.position += Vector3.right * (Time.deltaTime * movementSpeed); // Move right
            }
            else if (transform.position.x - _startPosition.x > 0.1f && _movementDirection == Vector3.left)  // If the GameObject is above its starting position
            {

                transform.position += Vector3.left * (Time.deltaTime * movementSpeed);  // Move left
            } else 
            { 
                StartCoroutine(Wait());
                _wait = true;
                _movementDirection = _movementDirection ==  Vector3.right ? Vector3.left : Vector3.right;
            }
        }


        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(movementPauseInSeconds);
            _wait = false;
        }


        private void OnTriggerEnter2D(Collider2D col)
        {
            var player = col.gameObject.GetComponent<PlayerManager>();
            if (player)
            {
                player.Die();
            }
        }
}
