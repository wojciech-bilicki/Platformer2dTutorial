using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    private const String BOTTOM_COLLIDER_TAG = "BottomCollider";
    private const String TRAP_COLLIDER_TAG = "Trap";

    private SpriteRenderer _spriteRenderer;
    private Animator _playerAnimator;
    private Sprite _deadPlayerSprite;
    private PlayerMovement _playerMovement;
    private FollowTarget _followTarget;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _playerAnimator = GetComponentInChildren<Animator>();
        _deadPlayerSprite = Resources.Load<Sprite>("Red_Player_Dead");
        _followTarget = Camera.main.GetComponent<FollowTarget>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(BOTTOM_COLLIDER_TAG) || col.CompareTag(TRAP_COLLIDER_TAG))
        {
            Die();
        }
    }

    private void Die()
    {
        _followTarget.enabled = false;
        _playerAnimator.enabled = false;
        _spriteRenderer.sprite = _deadPlayerSprite;
        _playerMovement.OnPlayerDeath();
    }
}
