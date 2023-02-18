using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{

    private const String BOTTOM_COLLIDER_TAG = "BottomCollider";
    
    private SpriteRenderer _spriteRenderer;
    private Animator _playerAnimator;
    private Sprite _deadPlayerSprite;
    private PlayerMovement _playerMovement;
    private FollowPlayer _followPlayer;
    private void Awake()
    {
        
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _playerAnimator = GetComponentInChildren<Animator>();
        _deadPlayerSprite = Resources.Load<Sprite>("Player_Red_dead");
        _playerMovement = GetComponent<PlayerMovement>();
        _followPlayer = Camera.main.GetComponent<FollowPlayer>();
    }



    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(BOTTOM_COLLIDER_TAG))
        {
            Die();
        }
    }

    public void Die()
    {
        _followPlayer.enabled = false;
        _playerAnimator.enabled = false;
        _spriteRenderer.sprite = _deadPlayerSprite;
        _playerMovement.OnPlayerDeath();
    }
}
