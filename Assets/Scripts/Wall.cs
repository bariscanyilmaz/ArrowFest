using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        _wallText.text = $"{GetOperatorChar(_operator)}{_value}";
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
            Operator.Div => "/",
            _ => string.Empty
        };
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         var arrow = other.gameObject.GetComponent<ArrowControl>();

    //         int newArrowCount = arrow.Calculate(_value, arrow.ArrowCount, _operator);
    //         arrow.Proceed(newArrowCount);
    //         Debug.Log(newArrowCount);
    //     }
    // }
}