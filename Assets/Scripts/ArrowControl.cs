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
    private float _swipeSpeed, _moveSpeed;
    //buffer
    private List<GameObject> _arrows;

    private float _offset = 0.25f;
    private int _ring = 0;
    private int _activeRing = 0;
    private float _angle = 0;
    private bool _isPressing;

    private Vector3 _firstPosition, _lastPosition;
    private Camera _cam;
    private float _axisX;
    private CapsuleCollider _collider;

    public int ArrowCount => _arrowCount;

    private void Awake()
    {

        _arrows = new List<GameObject>();
        _collider = GetComponent<CapsuleCollider>();
        _cam = Camera.main;
        CreateArrows();//Create arrow buffer
        ShowArrows();
        SetColliderRadius();
        _arrowCountText.text = (_arrowCount).ToString();
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
        }
    }

    void FixedUpdate()
    {
        float amount = Time.deltaTime * _swipeSpeed * _axisX;

        var pos = transform.localPosition;
        pos.x += amount;

        pos.x = Mathf.Clamp(pos.x, -_maxWidth, _maxWidth);
        transform.localPosition = pos;

        transform.position += Vector3.forward * Time.deltaTime * _moveSpeed;
    }

    private void SetColliderRadius()
    {
        if (_arrowCount > 1)
        {
            _collider.radius = _activeRing * _offset;
        }

    }

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

    public void ShowArrows(int arrowCount, int newArrowCount)
    {
        for (int i = arrowCount; i < newArrowCount && i < maxArrowCount; i++)
        {
            _arrows[i].SetActive(true);
        }

    }

    private void HideArrows(int arrowCount, int newArrowCount)
    {
        for (int i = arrowCount; i > newArrowCount; i--)
        {
            _arrows[i].SetActive(false);
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

    Vector3 GetMousePosition() => _cam.ScreenToViewportPoint(Input.mousePosition);

    public int Calculate(int val, int arrows, Operator op)
    {
        return op switch
        {
            Operator.Sum => arrows + val,
            Operator.Mul => arrows * val,
            Operator.Sub => arrows - val,
            Operator.Div => arrows / val,
            _ => arrows,
        };
    }
    public void Proceed(int newArrowCount)
    {
        ChangeArrowCount(newArrowCount);
    }
    private bool wallHasCollided = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            if (!wallHasCollided)
            {
                wallHasCollided = true;
                var wall = other.gameObject.GetComponent<Wall>();
                wall.gameObject.SetActive(false);
                int newArrowCount = Calculate(wall.Value, _arrowCount, wall.Operator);
                ChangeArrowCount(newArrowCount);
                Invoke("ResetWallHasCollided", 1f);
                UpdateArrowCountText();
            }
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            //check arrow count
            //
            //decrease arrow
            //
            //set die animation
            //
        }
    }

    private void ResetWallHasCollided()
    {
        wallHasCollided = false;
    }

    private void ChangeArrowCount(int newValue)
    {
        if (newValue > _arrowCount)
        {
            ShowArrows(_arrowCount, newValue);
        }
        else
        {
            HideArrows(_arrowCount, newValue);
        }

        _arrowCount = newValue;
    }

    private void UpdateArrowCountText()
    {
        _arrowCountText.text = (_arrowCount).ToString();
    }

}
