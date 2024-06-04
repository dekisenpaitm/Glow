using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wwise_Play_Trigger : MonoBehaviour
{
    // Start is called before the first frame update
    public string wwiseEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AkSoundEngine.PostEvent(wwiseEvent, gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AkSoundEngine.StopAll(gameObject);
        }
    }
}
