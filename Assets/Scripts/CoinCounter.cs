using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    public int coinCounter = 0;
    public TextMeshProUGUI text;
    public TextMeshProUGUI winText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(coinCounter == 10)
        {
            winText.text = "You've won the game. Go touch grass...";
        }
        text.text = coinCounter.ToString();
    }
}
