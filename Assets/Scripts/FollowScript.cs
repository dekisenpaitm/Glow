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

    [Header("SmoothTime")]
    [SerializeField]
    private float _smoothTime;
    private Vector3 _velocity;
    #endregion

    private void Start()
    {
        _player = FindObjectOfType<Movement>(includeInactive: true).transform;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, _player.position + _offset, ref _velocity, _smoothTime);
    }
}
