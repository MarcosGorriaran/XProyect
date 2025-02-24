using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimations : MonoBehaviour
{
    public Animator animator;
    public void OnShootAnimationComplete()
    {
        animator.SetBool("isShooting", false);
        Debug.Log("Animación de disparo completada.");
    }

    public void OnRechargeAnimationComplete()
    {
        animator.SetBool("isRecharging", false);
        Debug.Log("Animación de recarga completada.");
    }

}
