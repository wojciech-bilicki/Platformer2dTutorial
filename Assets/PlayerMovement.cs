using System;
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
    private InputManager _inputManager;
    private AnimationController _animationController;
    private Transform _spriteTransform;
    
    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _inputManager = GetComponent<InputManager>();
        _animationController = GetComponent<AnimationController>();
        _spriteTransform = GetComponentInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    { 
        if (IsGrounded()) 
        { 
          _velocity = new Vector2(_inputManager.InputVelocity.x * speed,  _inputManager.InputVelocity.y *jumpForce);
        }
        else
        {
            _velocity = _rigidbody2D.velocity;
        }
        
        _animationController.SetIsWalking(_velocity.x != 0);
        _animationController.SetIsJumping(_velocity.y != 0);

        _spriteTransform.localScale = new Vector3(_velocity.x > 0 ? 1 : -1, 1, 1);
    }

    private void FixedUpdate()
    {
        _rigidbody2D.velocity = _velocity;
        if (_velocity.y > 0)
        {
            _inputManager.ResetJump();
        }
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
