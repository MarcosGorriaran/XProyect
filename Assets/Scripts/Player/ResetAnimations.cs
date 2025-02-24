using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimations : MonoBehaviour
{
    public Animator animator;
    public void OnShootAnimationComplete()
    {
        animator.SetBool("isShooting", false);
        Debug.Log("Animaci�n de disparo completada.");
    }

    public void OnRechargeAnimationComplete()
    {
        animator.SetBool("isRecharging", false);
        Debug.Log("Animaci�n de recarga completada.");
    }

}
