using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ArrowControl : MonoBehaviour
{
    //CONSTANTS
    private const int maxArrowCount = 340;


    [SerializeField]
    private float _maxWidth;

    [Range(1, 1000)]
    [SerializeField]
    private int _arrowCount;

    [SerializeField]
    private GameObject _arrowPrefab;

    [SerializeField]
    private TMP_Text _arrowCountText;

    [SerializeField]
    private float _swipeSpeed;
    //buffer
    private List<GameObject> _arrows;

    private float _offset = 0.25f;
    private int _ring = 0;
    private int _activeRing = 0;
    private float _angle = 0;
    private Rigidbody _rb;
    private bool _isPressing;

    private Vector3 _firstPosition, _lastPosition;
    private Camera _cam;
    private float _axisX;
    private CapsuleCollider _collider;

    private void Awake()
    {

        _arrows = new List<GameObject>();
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        _cam = Camera.main;
        CreateArrows();//Create arrow buffer
        ShowArrows();
        SetColliderRadius();
        _arrowCountText.text = (_arrowCount).ToString();
    }

    //TODO Better Swipe Control

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _firstPosition = GetMousePosition();
            _isPressing = true;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            _isPressing = false;

        }

        if (_isPressing)
        {
            _lastPosition = GetMousePosition();
            _axisX = (_firstPosition.x - _lastPosition.x)*Time.deltaTime*_swipeSpeed;
            Vector3 pos = new Vector3(Mathf.Clamp(_rb.position.x + _axisX, -_maxWidth, _maxWidth), transform.position.y, transform.position.z);
            _rb.position = pos;

        }
    }

    private void SetColliderRadius()
    {
        if (_arrowCount > 1)
        {
            _collider.radius = _activeRing * _offset;
        }

    }


    [ContextMenu("Create")]
    public void Create()
    {
        if (_arrows.Count == 0) CreateArrows();

        HideAllArrows();
        ShowArrows();
    }

    public void ShowArrows()
    {
        for (int i = 0; i < _arrowCount && i < maxArrowCount; i++)
        {
            _arrows[i].SetActive(true);
        }
    }

    public void HideAllArrows()
    {
        foreach (var item in _arrows)
        {
            item.SetActive(false);
        }
    }
    public void CreateArrows()
    {
        for (int i = 0; i <= maxArrowCount; i++)
        {
            var arrow = Instantiate(_arrowPrefab, GetPosition(i), Quaternion.identity);
            arrow.transform.SetParent(transform);
            arrow.SetActive(false);
            _arrows.Add(arrow);
        }
    }

    private Vector3 GetPosition(int index)
    {
        if (index == 0) return Vector3.zero;

        if (index <= 10)
        {//10
            _ring = 1;
            _angle += 36;
        }
        else if (index <= 30)
        {//20
            _ring = 2;
            _angle += 18f;
        }
        else if (index <= 70)
        {//40
            _ring = 3;
            _angle += 9f;
        }
        else if (index <= 130)
        {//60
            _ring = 4;
            _angle += 6f;
        }
        else if (index <= 220)
        {//90
            _ring = 5;
            _angle += 4f;
        }
        else if (index <= 340)
        {//120
            _ring = 6;
            _angle += 3f;
        }

        _activeRing = index <= _arrowCount ? _ring : _activeRing;

        float x = Mathf.Cos((_angle % 360) * Mathf.Deg2Rad) * _ring * _offset;
        float y = Mathf.Sin((_angle % 360) * Mathf.Deg2Rad) * _ring * _offset;

        return new Vector3(x, y, transform.position.z);
    }

    Vector3 GetMousePosition() => _cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
}
