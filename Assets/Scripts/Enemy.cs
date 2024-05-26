using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHitable
{
    #region EnemyStats
    [Header("EnemyName")]
    [SerializeField]
    private string _enemyName;

    [Header("MaxHealth")]
    [SerializeField]
    private int _maxHealth;

    [Header("CurrentHealth")]
    [SerializeField]
    private int _currentHealth;

    [Header("DamagePower")]
    [SerializeField]
    private int _damage;

    public int GetDamage()
    {
        return _damage;
    }

    [Header("MovementSpeed")]
    [SerializeField]
    private int _speed;

    public int GetSpeed()
    {
        return _speed;
    }

    #endregion

    #region Referances
    private Rigidbody _rb;
    private HumanoidAnimationController _anim;
    #endregion

    #region DmgCD
    private bool _hit;

    public bool Hit
    {
        get { return _hit; }
        set { _hit = value; }
    }
    #endregion


    void Start()
    {
        _currentHealth = _maxHealth;
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<HumanoidAnimationController>();
    }

    public void Execute(Transform executionSource)
    {
        StartCoroutine(ApplyKnockbackOverTime(executionSource, 0.5f, 70));
    }

    private IEnumerator ApplyKnockbackOverTime(Transform executionSource, float duration, float force)
    {
        Vector3 direction = (transform.position - executionSource.transform.position).normalized;
        _rb.velocity = Vector3.zero;
        _rb.AddForce(direction * force, ForceMode.Impulse);

        float elapsed = 0;
        StartHit();
        while (elapsed < duration)
        {
            _rb.AddForce(direction * (force * 0.1f * Time.deltaTime / duration), ForceMode.Impulse);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private void StartHit()
    {
        if (!_hit)
        {
            AkSoundEngine.PostEvent("Play_sword", gameObject);
            _anim.PlayHit();
            StartCoroutine(GotHit());
        }
    }

    private IEnumerator GotHit()
    {
        _hit = true;
        yield return new WaitForSeconds(1f);
        _hit = false;
    }

    public void Damage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
