using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHitable
{
    #region PlayerStats
    [SerializeField]
    private string playerName;

    [SerializeField]
    private int maxHealth;

    [SerializeField]
    private int currentHealth;

    [SerializeField]
    private int damage;
    #endregion

    #region DamageCheck
    private bool _hit;
    #endregion

    #region Referances
    private HumanoidAnimationController _anim;
    private Rigidbody _rb;
    #endregion

    private void Start()
    {
        currentHealth = maxHealth;
        GameManager.instance.Health = maxHealth;
        _anim = GetComponent<HumanoidAnimationController>();
        _rb = GetComponent<Rigidbody>();
    }

    public void HealthUp()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth++;
        }
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public int GetDamageValue()
    {
        return damage;
    }

    public void ApplyDamage(int damage)
    {
        Damage(damage);
    }

    private void Damage(int damage)
    {
        if (!_hit && currentHealth > 0)
        {
            Debug.Log("I got hit with " + damage);
            currentHealth -= damage;
            GameManager.instance.DecreaseHealth(damage);
            if (currentHealth <= 0)
            {
                //Destroy(this);
            }
        }
    }

    public void Execute(Transform executionSource)
    {
        if (!_hit)
        {
            StartCoroutine(ApplyKnockbackOverTime(executionSource, 0.5f, 70));
        }
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
        _anim.PlayHit();
        StartCoroutine(GotDamage());
    }

    public IEnumerator GotDamage()
    {
        _hit = true;
        yield return new WaitForSeconds(0.3f);
        _hit = false;
    }
}
