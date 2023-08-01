using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    private Player player;
    
    private Animator animator;
    
    private void Awake()
    {
        player = GetComponentInParent<Player>();
        animator = GetComponent<Animator>();
        animator.SetBool(IsWalking, player.IsWalking());
    }

    private void Update()
    {
        animator.SetBool(IsWalking, player.IsWalking());
    }
}
