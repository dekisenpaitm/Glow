using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

internal enum MovementType
{
    TransformBased,
    PhysicsBased
}
public class Movement : MonoBehaviour
{
    #region BasicMovement
    [Header("MovementSpeed")]
    [SerializeField]
    private float _speed;

    [Header("CurrentVelocity")]
    [SerializeField]
    public float _velocity;

    [SerializeField]
    private ForceMode _selectedForceMode;

    [SerializeField]
    private MovementType _movementType;

    private Vector2 _moveVector;
    private Vector3 _movementDirection3d;
    #endregion

    #region ComponentReferances
    private PlayerInput input = null;
    private HumanoidAnimationController _playerAnim;
    private Rigidbody _rb;
    private TrailRenderer _dashTrail;
    private TrailEnabler _trail;
    private EnemyKnockback _knock;
    #endregion

    #region Attack
    private int _attackCounter;
    private bool _attacking;
    #endregion

    #region Jump
    [Header("JumpForce")]
    [SerializeField]
    private float _force;

    public bool _isGrounded;
    private int _jumpCount = 2;
    #endregion

    #region Dash
    private bool _dashCD;
    private bool _dashing;
    #endregion


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
            _rb.velocity = new Vector3(_moveVector.x, _velocity, _moveVector.y) * _speed;
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
        if (transform.position.y <= 0.1)
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }

        if (_jumpCount == 2 && !_isGrounded)
        {
            _velocity = 0;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);            
        }

        if (!_isGrounded && !_dashing)
        {
            _velocity += Physics.gravity.y * Time.deltaTime;
        }
    }

    private void RotatePlayer()
    {
        Quaternion toRotation = Quaternion.LookRotation(_movementDirection3d, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 640 * Time.deltaTime);
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        if (_movementType == MovementType.TransformBased)
        {
            _moveVector = value.ReadValue<Vector2>();
            _movementDirection3d = new Vector3(_moveVector.x, 0, _moveVector.y);
        }
        else if (_movementType == MovementType.PhysicsBased)
        {
            _rb.AddForce(_movementDirection3d, ForceMode.Force);
        }
    }

    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        _moveVector = Vector2.zero;
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
