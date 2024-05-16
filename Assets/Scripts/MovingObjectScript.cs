using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectScript : MonoBehaviour
{
    [SerializeField]
    private Vector3 _distanceToMove;

    [SerializeField]
    private float _velocityFactor;

    private Vector3 _startingPoint;
    private Vector3 _destinationPoint;
    private bool _increaseValue = true;

    private float _passedTimeForInterpolation = 0;

    // Start is called before the first frame update
    void Start()
    {
        _startingPoint = gameObject.transform.position;
        _destinationPoint = _startingPoint + _distanceToMove;
    }

    // Update is called once per frame
    void Update()
    {
        if (_increaseValue)

            _passedTimeForInterpolation += Time.deltaTime * _velocityFactor;
        else
            _passedTimeForInterpolation -= Time.deltaTime * _velocityFactor;

        if (_passedTimeForInterpolation > 1)
            _increaseValue = false;
        else if (_passedTimeForInterpolation < 0)
            _increaseValue = true;

        Vector3 result = Vector3.Lerp(_startingPoint, _destinationPoint, _passedTimeForInterpolation);
        gameObject.transform.position = result;
    }
}
