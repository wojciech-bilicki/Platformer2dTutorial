using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float distanceToGround = 0;
    
    [SerializeField] private float speed = 5;
    [SerializeField] private LayerMask groundLayerMask;

    private Rigidbody2D _rigidbody2D;
    private PolygonCollider2D _polygonCollider2D;
    private Vector2 _velocity;

    private float _heightCheckDisctance = .015f;
    private InputManager _inputManager;
    private AnimationController _animationController;
    private Transform _spriteTransform;
    

    //TODO: this could be represented as enum 
    #region PlayerStates
    public bool _isGrounded;
    private bool _isJumping;
    #endregion

    #region JumpingProperties

    [SerializeField] public float fallingDownMultiplier =2.0f;
    [SerializeField] private float maxJumpHeight = 2.5f;
    [SerializeField] private float maxJumpTime = 1.0f;

    private float jumpForce =>  ( maxJumpHeight) / (maxJumpTime / 2f);
    private float gravity => (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2);

    public bool disableGravity = false;
    #endregion
    
    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _inputManager = GetComponent<InputManager>();
        _animationController = GetComponent<AnimationController>();
        _spriteTransform = GetComponentInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalMovement();
        _isGrounded = IsGrounded();
        if (_isGrounded)
        {
            TryApplyJump();
        }

        if (!disableGravity)
        {
              ApplyGravity();
        }
      
        _animationController.SetIsWalking(_isGrounded && _velocity.x != 0);
        _animationController.SetIsJumping(_isJumping);

        _spriteTransform.localScale = new Vector3(_velocity.x > 0 ? 1 : -1, 1, 1);
    }

    private void ApplyGravity()
    {
        bool isFalling = _velocity.y < 0.0f;
        float gravityMultiplier = isFalling  ? fallingDownMultiplier : 1;
        float jumpForceMultiplier = _inputManager.IsJumpingPressed ? 2f : 1f;
        _velocity.y += gravity * Time.deltaTime * gravityMultiplier / jumpForceMultiplier;
        _velocity.y = Mathf.Max(_velocity.y, gravity / 2f);
    }


    private void HorizontalMovement()
    {
        float horizontalVelocity =
            Mathf.MoveTowards(_velocity.x, _inputManager.InputVelocity.x * speed, speed * Time.deltaTime);
        _velocity.x = horizontalVelocity;
    }
    
    private void TryApplyJump()
    {
        _velocity.y = Mathf.Max(_velocity.y, 0);
        if (_inputManager.InputVelocity.y > 0)
        {
            _velocity.y = jumpForce;
        }

        _isJumping = _velocity.y > 0.0f;
    }

    private void FixedUpdate()
    {
        Vector2 position = _rigidbody2D.position;
        position += _velocity * Time.fixedDeltaTime;
        _rigidbody2D.MovePosition(position);
        if (_velocity.y > 0)
        {
            _inputManager.ResetJump();
        }
    }

    bool IsGrounded()
    {
      
        Vector3 raycastOrigin = _polygonCollider2D.bounds.center;
        
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(raycastOrigin, _polygonCollider2D.bounds.size, 0, Vector2.down,
            _heightCheckDisctance, groundLayerMask);

        Color rayColor;

        if (raycastHit2D.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }

        distanceToGround = raycastHit2D.distance;
        
        Debug.DrawRay(raycastOrigin, Vector3.down *(_heightCheckDisctance), rayColor, groundLayerMask);
        return raycastHit2D.collider != null;
    }


    public void OnPlayerDeath()
    {
        _inputManager.enabled = false;
        _polygonCollider2D.enabled = false;
        _velocity.y = jumpForce * 2;
        _rigidbody2D.constraints = RigidbodyConstraints2D.None;
        _rigidbody2D.AddTorque(20, ForceMode2D.Impulse);
       
    }
    
}
