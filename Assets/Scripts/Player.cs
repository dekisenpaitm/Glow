using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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

    private void Start()
    {
        currentHealth = maxHealth;
        GameManager.instance.Health = maxHealth;
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
