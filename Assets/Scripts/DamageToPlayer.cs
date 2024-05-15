using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageToPlayer : MonoBehaviour
{
    public Enemy _enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {   
            other.GetComponent<Player>().ApplyDamage(_enemy.GetDamage());
        }
    }
}
