using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidAnimationController : MonoBehaviour
{
    #region Referances
    private Animator _anim;
    #endregion

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void PlayIdle()
    {
        _anim.SetBool("isMoving", false);
    }

    public void PlayRunning()
    {
        _anim.SetBool("isMoving", true);
    }

    public void PlayAttack()
    {
        _anim.Play("Attack");
    }

    public void PlayAttackB()
    {
        _anim.Play("Attack_B");
    }

    public void PlayHit()
    {
        _anim.Play("Hit");
    }

    public void PlayDodge()
    {
        _anim.Play("Dodge");
    }

    public void PlayJump(bool inAir)
    {
        if (!inAir) 
        {
            _anim.Play("Jump");
            return;
        }

        _anim.Play("Jump_Air");
    }

    public void PlayLanding()
    {
        _anim.Play("Landing");
    }

    public void PlayDie()
    {

    }
}
