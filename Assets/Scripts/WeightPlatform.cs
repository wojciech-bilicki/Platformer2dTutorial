using System;
using UnityEngine;

public class WeightPlatform : MonoBehaviour
{
    private bool shouldPlatformRotate;
    private PlayerMovement _playerMovement;

    
    private float _platformLength;
    private Transform player;
    private BoxCollider2D _platformBoxCollider;
    private float _heightCheckDistance = 0.5f;

    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float rotationMaxDegrees = 30f;


    private void Awake()
    {
        _platformBoxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            shouldPlatformRotate = true;
            player = col.gameObject.GetComponent<Transform>();
            _playerMovement = col.gameObject.GetComponent<PlayerMovement>();
        }
    }

    private void Update()
    {
        if (shouldPlatformRotate)
        {
            RaycastHit2D raycastHit2D = Physics2D.BoxCast(
                _platformBoxCollider.bounds.center,
                _platformBoxCollider.bounds.size,
                0,
                Vector2.up,
                _heightCheckDistance,
                playerLayerMask);

            if (raycastHit2D.collider == null)
            {
                player = null;
                shouldPlatformRotate = false;
                return;
            }

            Vector3 playerRelativePosition = transform.InverseTransformPoint(player.position);
            float rotationSpeedMultiplier = CalculateRotationMultiplier(playerRelativePosition);
            int rotationDirection = playerRelativePosition.x < 0 ? 1 : -1;
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.AngleAxis(rotationMaxDegrees * rotationDirection,  Vector3.forward),
                Time.deltaTime * rotationSpeed * rotationSpeedMultiplier);
            
            if (_playerMovement && _playerMovement.distanceToGround > 0)
            {
                Debug.Log(_playerMovement.distanceToGround);
                player.transform.position = new Vector3(player.transform.position.x,
                    player.transform.position.y - _playerMovement.distanceToGround, player.transform.position.z);
            }
           

        } else if (transform.rotation.eulerAngles.z != 0)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0),
                Time.deltaTime * rotationSpeed);
        }
    }

    private float CalculateRotationMultiplier(Vector3 playerRelativePosition)
    {
        
        int rotationDirection = playerRelativePosition.x < 0 ? 1 : -1;
        float rotationSpeedMultiplier =
            Mathf.Abs(Mathf.Clamp((playerRelativePosition.x * 2 / _platformLength) * rotationDirection, -1, 1));
        return rotationSpeedMultiplier;
    }
}
