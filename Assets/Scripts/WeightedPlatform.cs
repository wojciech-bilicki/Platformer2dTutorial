using System;
using Unity.VisualScripting;
using UnityEngine;

public class WeightedPlatform : MonoBehaviour
{
    private bool shouldPlatformRotate;
    private Transform player;
    
    private float _platformLength;

    [SerializeField] private float rotationMaxDegrees = 30;
    // [SerializeField] private BoxCollider2D _playerLeavePlatformCollider;
    private void Awake()
    {
        _platformLength = GetComponent<BoxCollider2D>().bounds.size.x;
        Debug.Log(_platformLength);
       
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            shouldPlatformRotate = true;
            //get player transform
            player = col.gameObject.GetComponent<Transform>();
            col.gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            shouldPlatformRotate = false;
            player = null;
            col.gameObject.GetComponent<Transform>().rotation = Quaternion.Euler(0,0,0);
            col.gameObject.GetComponent<Rigidbody2D>().freezeRotation = true;
        }
    }

    private void Update()
    {
        // Debug.Log(GetDistanceBetweenPlayerAndTransform());
        if (shouldPlatformRotate)
        {   
            Vector3 playerRelativePosition = transform.InverseTransformPoint(player.position);
            int rotationDirection = playerRelativePosition.x < 0 ? 1 : -1;
            float rotationSpeedMultiplier = Mathf.Abs(Mathf.Clamp((playerRelativePosition.x * 2 / _platformLength) * rotationDirection, -1, 1));
            
            if (transform.rotation.eulerAngles.z< rotationMaxDegrees || transform.rotation.eulerAngles.z  > 360 - rotationMaxDegrees)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    Quaternion.AngleAxis(rotationMaxDegrees * rotationDirection, Vector3.forward),
                    Time.deltaTime  * 50 * rotationSpeedMultiplier);
             }
        } else if (transform.rotation.eulerAngles.z != 0)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                Quaternion.Euler(0, 0, 0),
                Time.deltaTime  * 50);
        } 
        
    }


    public float GetDistanceBetweenPlayerAndTransform()
    {
        return transform.position.y - player.position.y;
    }
}
