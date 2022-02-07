using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public enum Operator { Sum, Sub, Div, Mul };

public class Wall : MonoBehaviour
{

    [SerializeField]
    private Operator _operator;

    [SerializeField]
    [Range(1, 100)]
    private int _value;

    [SerializeField]
    private TMP_Text _wallText;

    [SerializeField]
    private bool _isMove;

    [SerializeField]
    Vector3 _toPosition;

    [Range(0.01f, 10f)]
    [SerializeField]
    float _moveSpeed = 1f;



    void Start()
    {
        _wallText.text = $"{GetOperatorChar(_operator)}{_value}";

        if (_isMove)
        {
            //do infinite loop animation
            transform.parent.DOMove(_toPosition, _moveSpeed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }

    }

    public int Value => _value;
    public Operator Operator => _operator;

    private static string GetOperatorChar(Operator op)
    {
        return op switch
        {
            Operator.Mul => "x",
            Operator.Sum => "+",
            Operator.Sub => "-",
            Operator.Div => "\u00F7",
            _ => string.Empty
        };
    }

    public void KillTween()
    {
        transform.parent.DOKill(true);
    }

}