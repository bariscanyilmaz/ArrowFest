using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowControl : MonoBehaviour
{
    [Range(1, 1000)]
    [SerializeField]
    private int _arrowCount;

    [SerializeField]
    private GameObject _arrowPrefab;

    private List<GameObject> _arrows;
    private float _offset = 0.25f;
    private int _ring = 0;
    private float _angle = 0;

    private void Awake()
    {
        _arrows = new List<GameObject>();
        CreatArrows();
    }

    private void CreatArrows()
    {
        for (int i = 0; i < _arrowCount && i <= 340; i++)
        {
            var arrow = Instantiate(_arrowPrefab, GetPosition(i), Quaternion.identity);
            arrow.transform.SetParent(transform);
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

        float x = Mathf.Cos((_angle % 360) * Mathf.Deg2Rad) * _ring * _offset;
        float y = Mathf.Sin((_angle % 360) * Mathf.Deg2Rad) * _ring * _offset;

        return new Vector3(x, y, transform.position.z);


    }



}
