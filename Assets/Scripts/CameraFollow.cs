using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    Transform _target;
    float _smoothSpeed = 2f;
    Vector3 _offset;
    Vector3 _smoothedPosition;

    void Start()
    {
        _offset = _target.position - transform.position;
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position - _offset, Time.deltaTime * _smoothSpeed);
    }
}
