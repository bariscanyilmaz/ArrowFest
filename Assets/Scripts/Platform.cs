using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;

public class Platform : MonoBehaviour
{
    [SerializeField]
    private TMP_Text[] _platformTexts;

    public void SetText(int index)
    {
        string textVal = $"X{(1f + (index * 0.2f)).ToString("0.0", CultureInfo.InvariantCulture)}";

        _platformTexts[0].text = textVal;
        _platformTexts[1].text = textVal;
    }


}
