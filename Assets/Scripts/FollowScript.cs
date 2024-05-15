using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MonoBehaviour
{
    public Transform player;


    private void Start()
    {
        player = FindObjectOfType<Movement>(includeInactive: true).transform;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, 6, 10);
    }
}
