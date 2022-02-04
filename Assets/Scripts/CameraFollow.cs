using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    Transform _target;
    float _smoothSpeed = 2f;
    float _offset = 20;
    Vector3 _smoothedPosition;

    void Update()
    {
        transform.position = new Vector3(0, transform.position.y, transform.position.z);

    }
}
