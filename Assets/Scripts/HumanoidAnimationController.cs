using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidAnimationController : MonoBehaviour
{

    private Animator _anim;

    private bool _animationFinished;
    private AnimatorStateInfo _animState;
    private float _nTime;

    public bool AnimationName(string value)
    {
        return _animState.IsName(value);
    }

    public bool AnimationGoing()
    {
        _animState = _anim.GetCurrentAnimatorStateInfo(0);
        _nTime = _animState.normalizedTime;

        if (_nTime > 1.0f) return _animationFinished = true;

        return _animationFinished = false;
    }

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
