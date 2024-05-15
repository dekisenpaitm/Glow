using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEnabler : MonoBehaviour
{
    private TrailRenderer _trail;

    private void Start()
    {
        _trail = GetComponent<TrailRenderer>();
    }

    public void EnableTrail()
    {
       _trail.emitting = !_trail.emitting;
    }
}
