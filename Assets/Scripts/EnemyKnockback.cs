using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    private CapsuleCollider _sword;
    public GameObject hitEffect;
    private int _damage;

    private void Start()
    {
        _sword = GetComponent<CapsuleCollider>();
        _damage = FindObjectOfType<Player>().GetDamageValue();
    }

    public void UseSword()
    {
        _sword.enabled = !_sword.enabled;
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 collisionPoint = other.ClosestPoint(transform.position);

        if (other.gameObject.CompareTag("Enemy"))
        {
            
            other.GetComponent<Enemy>().Damage(_damage);
            GameObject hit = Instantiate(hitEffect);
            hit.transform.position = collisionPoint;
            IHitable hitable = other.transform.GetComponent<IHitable>();
            Transform _player = FindObjectOfType<Movement>().gameObject.transform;
            hitable.Execute(_player);
        }
    }
}
