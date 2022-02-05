using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ArrowControl : MonoBehaviour
{
    private const float DEFAULT_COLLIDER_RADIUS = 0.07f;
    private const float DEFAULT_OFFSET = 0.25f;
    private const int MAX_ARROW_COUNT = 311;
    private const int BASE_ARROW_COUNT = 1;
    private float xOffset = DEFAULT_OFFSET;
    private float yOffset = DEFAULT_OFFSET;

    private float _currentXradius = 0.25f;
    private float _currentYradius = 0.25f;
    //
    private int _ringCapacity = 10;
    private int _ring = 1;
    private float _angle = 0;
    private float _deltaAngle = 0;
    //
    private int _currentRing = 1;
    private int _currentArrowCount = 0;


    [Range(1, 1000)]
    [SerializeField]
    private int _arrowCount;

    [SerializeField]
    private GameObject _arrowPrefab;

    [SerializeField]
    private TMP_Text _arrowCountText;
    [SerializeField]
    private Transform _arrowPoint;

    private List<GameObject> _arrows;
    private float _offset = 0.25f;
    private int _activeRing = 0;

    private CapsuleCollider _collider;
    public int ArrowCount => _arrowCount;

    public void OnGameStart()
    {
        _arrows = new List<GameObject>();
        _collider = GetComponent<CapsuleCollider>();
        CreateArrows();//Create arrow buffer
        ShowArrows();
        SetColliderRadius();
        UpdateArrowCountText();
        //transform.position = new Vector3(transform.position.x, transform.parent.position.y + 3f, transform.position.z);
    }


    private void SetColliderRadius()
    {

        if (_arrowCount > 1)
        {
            _collider.radius = GetRing() * _offset;
        }
        else
        {
            _collider.radius = DEFAULT_COLLIDER_RADIUS;
        }

    }

    private int GetRing()
    {
        int currentCount = 10;
        int count = 10;
        int ring = 1;
        while (_arrowCount > currentCount && _arrowCount < MAX_ARROW_COUNT)
        {
            count *= 2;
            currentCount+=count;
            ring++;
        }
        return ring;
    }

    public void ShowArrows()
    {
        for (int i = 0; i < _arrowCount && i < MAX_ARROW_COUNT; i++)
        {
            _arrows[i].SetActive(true);
        }
    }

    public void ShowArrows(int arrowCount, int newArrowCount)
    {
        if (newArrowCount < MAX_ARROW_COUNT)
        {
            for (int i = arrowCount; i < newArrowCount && i < MAX_ARROW_COUNT; i++)
            {
                _arrows[i].SetActive(true);
            }
        }
    }

    private void HideArrows(int arrowCount, int newArrowCount)
    {
        if (newArrowCount < MAX_ARROW_COUNT)
        {
            for (int i = arrowCount > MAX_ARROW_COUNT ? MAX_ARROW_COUNT - 1 : arrowCount; i >= newArrowCount; i--)
            {
                _arrows[i].SetActive(false);
            }
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
        for (int i = 0; i <= MAX_ARROW_COUNT; i++)
        {

            var arrow = Instantiate(_arrowPrefab, Vector3.zero, Quaternion.identity);

            arrow.transform.SetParent(_arrowPoint);

            arrow.transform.localPosition = GetArrowPosition(i);

            arrow.SetActive(false);

            _arrows.Add(arrow);
        }
    }

    Vector3 GetArrowPosition(int i)
    {
        if (i == 0) return Vector3.zero;

        _currentArrowCount++;
        if (_currentArrowCount > _ringCapacity)
        {
            _ring++;
            _currentRing = _ring;
            _ringCapacity *= 2;
            _currentArrowCount = 0;
        }

        float angle = ((float)_currentArrowCount / (float)_ringCapacity) * 360f;

        float x = Mathf.Cos(Mathf.Deg2Rad * angle) * _ring * xOffset;
        float y = Mathf.Sin(Mathf.Deg2Rad * angle) * _ring * xOffset;

        return new Vector3(x, y, 0);
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

        return new Vector3(x, y, 0);
    }



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
        SetColliderRadius();
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
                if (newArrowCount < 1)
                {
                    //GameOver
                    GameManager.Instance.GameOver.Invoke();
                }
                else
                {
                    ChangeArrowCount(newArrowCount);
                    SetColliderRadius();
                    Invoke("ResetWallHasCollided", 1f);
                    UpdateArrowCountText();

                }

            }
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {

            if (_arrowCount > 3)
            {
                var enemy = other.GetComponent<Enemy>();
                enemy.Die();
                ChangeArrowCount(_arrowCount - 3);
                SetColliderRadius();
                UpdateArrowCountText();
            }
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

    public void OnGameReStart()
    {
        HideAllArrows();
        _arrows[0].SetActive(true);
        _arrowCount = 1;
        UpdateArrowCountText();
        ResetWallHasCollided();

    }

}
