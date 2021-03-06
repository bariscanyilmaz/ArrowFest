using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float _maxWidth;
    [SerializeField]
    private float _swipeSpeed;
    [SerializeField]
    Transform _arrowController;

    [SerializeField]
    Vector3 _startPosition;

    private Vector3 _firstPosition, _lastPosition;
    private float _axisX;
    private Camera _cam;

    void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameState.Play)
        {


            if (Input.GetMouseButtonDown(0))
            {
                _firstPosition = GetMousePosition();

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
    }

    Vector3 GetMousePosition() => _cam.ScreenToViewportPoint(Input.mousePosition);
    public void OnWin()
    {

    }
    
    public void OnGameStart()
    {
        transform.position = _startPosition;
    }
    public void EndOfPath()
    {
        GameManager.Instance.SetState(GameState.Win);
        GameManager.Instance.Win.Invoke();
    }
}
