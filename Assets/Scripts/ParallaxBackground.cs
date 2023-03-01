using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxFactor;
    private Transform cameraTransform;
    private Vector3 previousCameraPosition;

    private float spriteWidthInUnits;

    private const int REPEAT_BACKGROUND_TIMES = 3;
    void Start()
    {
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.drawMode = SpriteDrawMode.Tiled;
        spriteRenderer.size = new Vector2(spriteRenderer.size.x * REPEAT_BACKGROUND_TIMES, spriteRenderer.size.y);

        float textureWidth = spriteRenderer.sprite.texture.width;
        float pixelPerUnit = spriteRenderer.sprite.pixelsPerUnit;

        spriteWidthInUnits = textureWidth / pixelPerUnit;

    }

    
    private void LateUpdate()
    {
        Vector3 currentCameraPosition = cameraTransform.position;
        Vector3 cameraMovementThisFrame = currentCameraPosition - previousCameraPosition;
        transform.position -= new Vector3(cameraMovementThisFrame.x * parallaxFactor.x, cameraMovementThisFrame.y * parallaxFactor.y, 0);

        previousCameraPosition = currentCameraPosition;

        if ( Mathf.Abs(cameraTransform.position.x - transform.position.x) >= spriteWidthInUnits)
        {
            transform.position = new Vector3(cameraTransform.position.x, transform.position.y, transform.position.z);
        }
    }
}
