using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    #region Attributes
    [Header("HitEffect")]
    public GameObject hitEffect;

    private int _damage;

    #endregion

    #region Referances
    public Enemy _enemy;
    private SphereCollider _hand;
    #endregion

    private void Start()
    {
        _hand = GetComponent<SphereCollider>();
        _damage = FindObjectOfType<Player>().GetDamageValue();
    }

    public void UseHand()
    {
        _hand.enabled = !_hand.enabled;
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 collisionPoint = other.ClosestPoint(transform.position);

        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Player>().ApplyDamage(_damage);
            GameObject hit = Instantiate(hitEffect);
            hit.transform.position = collisionPoint;
            IHitable hitable = other.transform.GetComponent<IHitable>();
            hitable.Execute(_enemy.transform);
        }
    }
}
