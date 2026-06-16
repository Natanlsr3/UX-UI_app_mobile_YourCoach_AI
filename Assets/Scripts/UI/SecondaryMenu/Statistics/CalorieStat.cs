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

        /// <summary>
        /// Set the display based on calories value and ratio, and current day/week
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_ratio"></param>
        /// <param name="_isCurrent"></param>
        public void SetValue(float _value, float _ratio, bool _isCurrent)
        {
            // Set the scale based on the ratio between total calories and represented calories value
            Vector3 _scale = new Vector3(valueDisplay.rectTransform.localScale.x, _ratio, valueDisplay.rectTransform.localScale.z);
            valueDisplay.rectTransform.localScale = _scale;

            valueCount.text = _value + " kcal";

            // Set the color depending on if it represents current day/week or not
            if (_isCurrent) valueDisplay.color = hoverColor;
            else valueDisplay.color = baseColor;
        }
    }
}

