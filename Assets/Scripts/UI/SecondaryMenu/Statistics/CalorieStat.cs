using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.App.UI.SecondaryMenu.Stats
{
    public class CalorieStat : MonoBehaviour
    {
        [SerializeField] private Image valueDisplay;
        [SerializeField] private Color baseColor;
        [SerializeField] private Color hoverColor;

        public void SetValue(float _value, bool _isCurrent)
        {
            Vector3 _scale = new Vector3(valueDisplay.rectTransform.localScale.x, _value, valueDisplay.rectTransform.localScale.z);
            valueDisplay.rectTransform.localScale = _scale;

            if (_isCurrent) valueDisplay.color = hoverColor;
            else valueDisplay.color = baseColor;
        }
    }
}

