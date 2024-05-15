using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    private CoinCounter _coinCounter;
    private Player _player;
    public bool heart;
    public bool coin;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _coinCounter = FindObjectOfType<CoinCounter>(includeInactive:true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (coin)
            {
                _coinCounter.coinCounter += 1;
                Destroy(gameObject);
            }

            if (heart)
            {
                _player.HealthUp();
                Destroy(gameObject);
            }
        }
    }

}
