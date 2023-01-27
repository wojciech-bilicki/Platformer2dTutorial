using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
   private Animator _animator;
   private static readonly int IsWalking = Animator.StringToHash("IsWalking");
   private static readonly int IsJumping = Animator.StringToHash("IsJumping");


   private void Awake()
   {
      _animator = GetComponentInChildren<Animator>();
   }

   public void SetIsWalking(bool isWalking) => _animator.SetBool(IsWalking, isWalking);

   public void SetIsJumping(bool isJumping) => _animator.SetBool(IsJumping, isJumping);
}
