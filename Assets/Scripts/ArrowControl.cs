using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowControl : MonoBehaviour
{
    [Range(1, 1000)]
    [SerializeField] int _arrowCount;

    [SerializeField] GameObject _arrowPrefab;

    List<GameObject> _arrows;
    float _offset = 0.25f;
    int _ring = 0;
    float _angle = 0;

    void Awake()
    {
        _arrows = new List<GameObject>();
        CreatArrows();

    }

    public void CreatArrows()
    {
        for (int i = 0; i < _arrowCount && i <= 340; i++)
        {
            var arrow = Instantiate(_arrowPrefab, GetPosition(i), Quaternion.identity);
            arrow.transform.SetParent(transform);
            _arrows.Add(arrow);
        }
    }

    public void OrderArrows2()
    {
        // Value influences distance between the spiral arms, usually kept 3.5 to 10+
        float angle = 0;
        float r;

        for (int i = 1; i < _arrowCount; i++)   // spirallength determines pointdist spaced points to plot
        {
            r = Mathf.Sqrt(i);//.Log10(i);
            angle += Mathf.Asin(1 / r);
            float x = Mathf.Cos(angle) * r * _offset;
            float y = Mathf.Sin(angle) * r * _offset;
            _arrows[i].transform.position = new Vector3(x, y, transform.position.z);
        }

    }

    public void OrderArrows(int i)
    {


    }

    Vector3 GetPosition(int index)
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
