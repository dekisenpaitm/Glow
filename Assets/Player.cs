using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private string playerName;

    [SerializeField]
    private int maxHealth;

    [SerializeField]
    private int currentHealth;

    [SerializeField]
    private int damage;

    private bool _hit;

    private HealthHolder _healthHolder;

    private void Start()
    {
        currentHealth = maxHealth;
        _healthHolder = FindObjectOfType<HealthHolder>();
    }

    public void HealthUp()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth++;
            _healthHolder.UpdateHealth();
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
            _healthHolder.UpdateHealth();
            StartCoroutine(GotDamage());

            if (currentHealth <= 0)
            {
                //Destroy(this);
            }
        }
    }

    public IEnumerator GotDamage()
    {
        _hit = true;
        yield return new WaitForSeconds(0.3f);
        _hit = false;
    }
}
