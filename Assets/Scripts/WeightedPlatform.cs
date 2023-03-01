using System;
using Unity.VisualScripting;
using UnityEngine;

public class WeightedPlatform : MonoBehaviour
{
    private bool shouldPlatformRotate;
    private Transform player;
    private PlayerMovement _playerMovement;
    
    private float _platformLength;
    private float _heightCheckDistance = 0.5f;
    private BoxCollider2D _platformBoxCollider;

    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private float rotationMaxDegrees = 30;
    // [SerializeField] private BoxCollider2D _playerLeavePlatformCollider;
    private void Awake()
    {
        _platformBoxCollider = GetComponent<BoxCollider2D>();
        _platformLength = _platformBoxCollider.bounds.size.x;

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            shouldPlatformRotate = true;
            //get player transform
            player = col.gameObject.GetComponent<Transform>();
            _playerMovement = col.gameObject.GetComponent<PlayerMovement>();

        }
    }

    private void Update()
    {
        if (shouldPlatformRotate)
        {
            RaycastHit2D raycastHit2D = Physics2D.BoxCast(_platformBoxCollider.bounds.center, _platformBoxCollider.bounds.size, 0, Vector2.up,
                _heightCheckDistance, playerLayerMask);
            if (raycastHit2D.collider == null)
            { 
                player = null;
                shouldPlatformRotate = false;
                _playerMovement = null;
                return;
            }
            
            Vector3 playerRelativePosition = transform.InverseTransformPoint(player.position);
            int rotationDirection = playerRelativePosition.x < 0 ? 1 : -1;
            float rotationSpeedMultiplier = Mathf.Abs(Mathf.Clamp((playerRelativePosition.x * 2 / _platformLength) * rotationDirection, -1, 1));

            bool canPlatformRotate = CanPlatformRotate(rotationDirection);
            if( canPlatformRotate)

            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    Quaternion.AngleAxis(rotationMaxDegrees * rotationDirection, Vector3.forward),
                    Time.deltaTime * 20 * rotationSpeedMultiplier);

                if (_playerMovement && _playerMovement.distanceToGround > 0)
                {
                    Debug.Log(_playerMovement.distanceToGround);
                    player.GetComponent<Rigidbody2D>().MovePosition(new Vector2(player.transform.position.x,
                        player.transform.position.y - _playerMovement.distanceToGround));
                }

            }
            
        } else if (transform.rotation.eulerAngles.z != 0)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                Quaternion.Euler(0, 0, 0),
                Time.deltaTime  * 50);

        } 
        
    }


    public bool CanPlatformRotate(int rotationDirection)
    {
        bool canRotate = false;
        if ((transform.rotation.eulerAngles.z < rotationMaxDegrees)
            || (transform.rotation.eulerAngles.z > 360 - rotationMaxDegrees))
        {
            canRotate = true;
        } else if (transform.rotation.eulerAngles.z >= rotationMaxDegrees && rotationDirection == 1)
        {
            canRotate = true;
        } else if (transform.rotation.eulerAngles.z <= 360 - rotationMaxDegrees && rotationDirection == -1)
        {
            canRotate = true;
        }
        

        return canRotate;
    }
}
