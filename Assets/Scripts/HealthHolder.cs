using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHolder : MonoBehaviour
{
    public GameObject[] hearts;
    private Player _player;
    private int _currentPlayerHealth;
    private int _maxPlayerHealth;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _maxPlayerHealth = _player.GetHealth();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealth()
    {
        _currentPlayerHealth = _player.GetHealth();
        for (int i = 0; i < hearts.Length; i++)
        {
            Debug.Log(hearts.Length);
            if (i < _currentPlayerHealth)
            {
                Debug.Log("This is i: " + i + "This is health: " + _currentPlayerHealth);
                hearts[i].SetActive(true);
            }
            else if(_currentPlayerHealth == 0) 
            {
                hearts[0].SetActive(false);
            } 
            else
            {
                hearts[i].SetActive(false);
            }
        }
    }
}
