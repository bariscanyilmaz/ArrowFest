using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _maxWidth;
    [SerializeField]
    private float _swipeSpeed, _moveSpeed;
    [SerializeField]
    Transform _arrowController;

    private Vector3 _firstPosition, _lastPosition;
    private float _axisX;
    private bool _isPressing;
    private Camera _cam;

    void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _firstPosition = GetMousePosition();
            _isPressing = true;
        }
        else if (Input.GetMouseButton(0))
        {
            _lastPosition = GetMousePosition();
            _axisX = _lastPosition.x - _firstPosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _axisX = 0;
            _lastPosition = _firstPosition;
        }


        float amount = Time.deltaTime * _swipeSpeed * _axisX;

        var pos = _arrowController.localPosition;
        pos.x += amount;

        pos.x = Mathf.Clamp(pos.x, -_maxWidth, _maxWidth);
        _arrowController.localPosition = pos;
    }

    Vector3 GetMousePosition() => _cam.ScreenToViewportPoint(Input.mousePosition);
}
