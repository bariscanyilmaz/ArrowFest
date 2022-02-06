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
    private const int MAX_ALIGN_ARROW = 40;
    private float xOffset = DEFAULT_OFFSET;
    private float yOffset = DEFAULT_OFFSET;

    private float _currentXradius = 0.25f;
    private float _currentYradius = 0.25f;
    //
    private int _ringCapacity = 10;
    private int _ring = 1;
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

    private CapsuleCollider _collider;
    private BoxCollider _boxCollider;
    public int ArrowCount => _arrowCount;

    void Start()
    {
        _arrows = new List<GameObject>();
        _collider = GetComponent<CapsuleCollider>();
        _boxCollider = GetComponent<BoxCollider>();
    }

    public void OnGameStart()
    {

        _arrowCount = PlayerPrefs.GetInt("Arrow", 1);
        CreateArrows();//Create arrow buffer
        ResetArrowAlignments();
        AlignArrowsCircle();
        ShowAllArrows();
        SetColliderRadius();
        UpdateArrowCountText();
        _boxCollider.enabled = false;
        _collider.enabled = true;
        _arrowCountText.gameObject.SetActive(true);
    }

    void ResetArrowAlignments()
    {
        _ringCapacity = 10;
        _ring = 1;
        _currentRing = 1;
        _currentArrowCount = 1;
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
            currentCount += count;
            ring++;
        }
        return ring;
    }

    public void ShowAllArrows()
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
        if (_arrows.Count > 0) return;
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
            var enemy = other.GetComponent<Enemy>();
            int newArrowCount = enemy.TakeDamage(_arrowCount);
            if (newArrowCount < 1)
            {
                if (GameManager.Instance.GameState == GameState.FinishLine)
                {

                    GameManager.Instance.SetState(GameState.Win);
                    GameManager.Instance.Win.Invoke();
                }
                else if (GameManager.Instance.GameState == GameState.Play)
                {

                    GameManager.Instance.GameOver.Invoke();
                }
            }
            else
            {
                enemy.Die();
                ChangeArrowCount(newArrowCount);

                if (GameManager.Instance.GameState == GameState.Play)
                {
                    SetColliderRadius();
                    UpdateArrowCountText();

                    GameManager.Instance.IncreaseCoin();

                }
                else if (GameManager.Instance.GameState == GameState.FinishLine)
                {

                    GameManager.Instance.IncreaseCollectedCoin();
                }

            }

        }
        else if (other.CompareTag("FinishLine"))
        {
            GameManager.Instance.SetState(GameState.FinishLine);
            GameManager.Instance.FinishLine.Invoke();

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
        ResetArrowAlignments();
        HideAllArrows();
        _arrows[0].SetActive(true);
        _arrowCount = 1;
        UpdateArrowCountText();
        ResetWallHasCollided();
        SetColliderRadius();
    }

    public void OnFinishLine()
    {
        _arrowCountText.gameObject.SetActive(false);
        HideAllArrows();
        AlignArrowsOnXAxis();

        //
        _collider.enabled = false;
        _boxCollider.enabled = true;
        //
    }

    public void OnGameWin()
    {
        HideAllArrows();
    }

    public void AlignArrowsOnXAxis()
    {
        int n = ((_arrowCount < MAX_ALIGN_ARROW) ? _arrowCount : MAX_ALIGN_ARROW);
        float deltaX = (10f / (float)n);

        _arrows[0].transform.position = new Vector3(0, transform.position.y, transform.position.z);
        for (int i = 1; i < n; i += 2)
        {

            _arrows[i].SetActive(true);
            _arrows[i + 1].SetActive(true);
            _arrows[i].transform.position = new Vector3(0 - (deltaX * i), transform.position.y, transform.position.z);
            _arrows[i + 1].transform.position = new Vector3(0 + (deltaX * i), transform.position.y, transform.position.z);
        }

    }

    public void AlignArrowsCircle()
    {
        for (int i = 0; i <= MAX_ARROW_COUNT; i++)
        {
            _arrows[i].transform.localPosition = GetArrowPosition(i);
            _arrows[i].SetActive(false);
        }
    }
}
