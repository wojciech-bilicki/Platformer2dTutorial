using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxFactor;
    private Transform cameraTransform;
    private Vector3 previousCameraPosition;
    private float spriteWidthInUnits;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.drawMode = SpriteDrawMode.Tiled;
        spriteRenderer.size = new Vector2(spriteRenderer.size.x * 3, spriteRenderer.size.y);
        float textureWidth = spriteRenderer.sprite.texture.width;
        float pixelsPerUnit = spriteRenderer.sprite.pixelsPerUnit;


        spriteWidthInUnits = textureWidth / pixelsPerUnit;
    }


    private void LateUpdate()
    {
        Vector3 currentCameraPosition = cameraTransform.position;
        Vector3 cameraMovementThisFrame = currentCameraPosition - previousCameraPosition;

         transform.position -= new Vector3(cameraMovementThisFrame.x * parallaxFactor.x,
             cameraMovementThisFrame.y * parallaxFactor.y, 0);

        previousCameraPosition = currentCameraPosition;
        if (cameraTransform.position.x - transform.position.x >= spriteWidthInUnits)
        {
             float parallaxTextureOffset = (cameraTransform.position.x - transform.position.x) % spriteWidthInUnits;
            transform.position = new Vector3(cameraTransform.position.x, transform.position.y, transform.position.z);
        }
    }
}
