using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpForce = 5;
    [SerializeField] private LayerMask groundLayerMask;

    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _boxCollider2D;
    private Vector2 _velocity;

    private float extraHeightCheck = .1f;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
      _velocity = new Vector2(Input.GetAxis("Horizontal") * speed, _rigidbody2D.velocity.y);

      if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
      { 
          _velocity += new Vector2(0, jumpForce);
      }

      _rigidbody2D.velocity = _velocity;
    }

    bool IsGrounded()
    {
        float raycastDistance = _boxCollider2D.bounds.extents.y + extraHeightCheck;
        Vector3 raycastOrigin = _boxCollider2D.bounds.center;
        
        RaycastHit2D raycastHit2D = Physics2D.Raycast(raycastOrigin, Vector2.down,
            raycastDistance, groundLayerMask);

        Color rayColor;

        if (raycastHit2D.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        
        Debug.DrawRay(raycastOrigin, Vector3.down *(raycastDistance), rayColor, groundLayerMask);
        return raycastHit2D.collider != null;
    }
}
