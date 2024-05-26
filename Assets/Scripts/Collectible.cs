using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectibleType
{
    heart,
    coin
}

public class Collectible : MonoBehaviour
{
    
    [Header("TypeOfCollectible")]
    [SerializeField]
    private CollectibleType _collectibleType;

    #region Referances
    private Player _player;
    #endregion

    void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_collectibleType == CollectibleType.coin)
            {
                GameManager.instance.IncreaseCollectedCoins(1);
                AkSoundEngine.PostEvent("Play_orbs", gameObject);
                Destroy(gameObject);
            }

            if (_collectibleType == CollectibleType.heart)
            {
                GameManager.instance.IncreaseHealth(1);
                _player.HealthUp();
                Destroy(gameObject);
            }
        }
    }

}
