using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    //animator je povezan za Ty kao sto je i ova skripta
    private Animator animator;
    private const string DALIHODA = "DaLiHoda";

    [SerializeField] private Player player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
    }

    private void Update()
    {
        animator.SetBool(DALIHODA, player.DaLiHoda());

    }
}
