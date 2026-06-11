using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.App.UI.SecondaryMenu.Stats
{
    public class CalorieStat : MonoBehaviour
    {
        [SerializeField] private Image valueDisplay;
        [SerializeField] private TextMeshProUGUI valueCount;
        [SerializeField] private Color baseColor;
        [SerializeField] private Color hoverColor;

        public void SetValue(float _value, float _ratio, bool _isCurrent)
        {
            Vector3 _scale = new Vector3(valueDisplay.rectTransform.localScale.x, _ratio, valueDisplay.rectTransform.localScale.z);
            valueDisplay.rectTransform.localScale = _scale;
            valueCount.text = _value + " kcal";

            if (_isCurrent) valueDisplay.color = hoverColor;
            else valueDisplay.color = baseColor;
        }
    }
}

