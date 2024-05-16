using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MonoBehaviour
{
    #region Attributes
    [Header("Target")]
    [SerializeField]
    private Transform _player;

    [Header("Offset")]
    [SerializeField]
    private Vector3 _offset;
    #endregion

    private void Start()
    {
        _player = FindObjectOfType<Movement>(includeInactive: true).transform;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = _player.position + _offset;
    }
}
