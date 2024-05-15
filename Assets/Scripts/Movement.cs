using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    private PlayerInput input = null;

    private Rigidbody _rb;

    [SerializeField]
    private float _force;
    private int _attackCounter;

    [SerializeField]
    private float _speed;
    private float _velocity;

    private bool _attacking;
    private bool _isGrounded;
    private int _jumpCount = 2;
    private bool _dashCD;
    private bool _dashing;
    private TrailRenderer _dashTrail;

    private Vector2 moveVector;
    private Vector3 movementDirection3d;

    private HumanoidAnimationController _playerAnim;
    private TrailEnabler _trail;
    private EnemyKnockback _knock;

    void Start()
    {
        _dashTrail = GetComponent<TrailRenderer>();
        _knock = FindObjectOfType<EnemyKnockback>();
        _trail = FindObjectOfType<TrailEnabler>();
        _playerAnim = GetComponent<HumanoidAnimationController>();
        _rb = GetComponent<Rigidbody>();
        _isGrounded = true;
    }

    private void Awake()
    {
        input = new PlayerInput();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCanceled;
        input.Player.Jump.performed += OnJumpPerformed;
        input.Player.Attack.performed += OnAttackPerformed;
        input.Player.Dash.performed += OnDashPerformed;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCanceled;
        input.Player.Jump.performed -= OnJumpPerformed;
        input.Player.Attack.performed -= OnAttackPerformed;
        input.Player.Dash.performed -= OnDashPerformed;
    }

    private void FixedUpdate()
    {
        if (!_attacking)
        {
            _rb.velocity = new Vector3(moveVector.x, _velocity, moveVector.y) * _speed;
        }
        if(_rb.velocity.x != 0 || _rb.velocity.z != 0)
        {
            _playerAnim.PlayRunning();
            RotatePlayer();
        }
        else
        {
            _playerAnim.PlayIdle();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isGrounded && !_dashing)
        {
            _velocity += Physics.gravity.y * Time.deltaTime;
        }
    }

    private void RotatePlayer()
    {
        Quaternion toRotation = Quaternion.LookRotation(movementDirection3d, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 640 * Time.deltaTime);
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
        movementDirection3d = new Vector3(moveVector.x, 0, moveVector.y);
    }

    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }

    private void OnJumpPerformed(InputAction.CallbackContext value)
    {
        if (_isGrounded || _jumpCount > 0)
        {
            if (_isGrounded)
            {
                _playerAnim.PlayJump(false);
            } else
            {
                _playerAnim.PlayJump(true);
            }
            _velocity = _force;
            _jumpCount--;
            _isGrounded = false;
        }
    }

    private void OnAttackPerformed(InputAction.CallbackContext value)
    {
        if (!_attacking && _isGrounded)
        {
            _knock.UseSword();
            _trail.EnableTrail();
            _attacking = true;
            _attackCounter++;
            if(_attackCounter == 1)
            {
                _playerAnim.PlayAttack();
            } else
            {
                _playerAnim.PlayAttackB();
                _attackCounter = 0;
            }
            
            StartCoroutine(Attack());
        }
    }

    private void OnDashPerformed(InputAction.CallbackContext value)
    {
        if (!_dashCD)
        {
            _playerAnim.PlayDodge();
            StartCoroutine(Dash());
        }
    }


    private IEnumerator Dash()
    {
        _dashTrail.emitting = true;
        _dashCD = true;
        _dashing = true;
        _speed += 10;
        _velocity = 0;
        yield return new WaitForSeconds(0.3f);
        _dashTrail.emitting = false;
        _dashing = false;
        _speed -= 10;
        yield return new WaitForSeconds(1f);
        _dashCD = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //if(_jumpCount != 2) { _playerAnim.PlayLanding(); }
            _jumpCount = 2;
            _velocity = 0;
            _isGrounded = true;
        } else
        {
            _isGrounded = false;
        }
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.3f);
        _knock.UseSword();
        _trail.EnableTrail();
        _attacking = false;
    }
}
