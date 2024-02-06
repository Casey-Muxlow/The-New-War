using UnityEngine;
using UnityEngine.Events;
using DIG.Tools;
namespace DIG.UIExpansion
{
    public class ProgresBar : MonoBehaviour
    {
        public float Value
        {
            get
            {

                if (_fillValue < MinFill)
                    _fillValue = MinFill;
                else if (_fillValue > MaxFill)
                    _fillValue = MaxFill;

                return _fillValue;
            }
            set
            {
                _fillValue = value;

                float scaledValue;

                if (_fillValue == 0)
                    scaledValue = _fillValue / MaxFill;
                else
                    scaledValue = (_fillValue - MinFill) / (MaxFill - MinFill);

                _fill.FillImage.fillAmount = scaledValue;

                OnValueChange?.Invoke(value);
            }
        }
        public float MaxFill { get { return _maxFill; } private set { _maxFill = value; } }
        public float MinFill { get { return _minFill; } private set { _minFill = value; } }
        public SliderBarEvent OnValueChange { get { return _onValueChange; } set { _onValueChange = value; } }

        [Header("UI")]
        [SerializeField] private BarFill _fill;



        [Header("Settings")]
        [SerializeField] private float _maxFill = 1f;
        [SerializeField] private float _minFill = 0f;
        [CustomRange("_minFill", "_maxFill")]
        [SerializeField] private float _fillValue = 1;

        [Space(10)]
        [SerializeField] private SliderBarEvent _onValueChange;


        private void OnValidate()
        {
            UpdateBar(_fillValue);
        }

        /// <summary>
        /// Used to update bar value in editor 
        /// </summary>
        /// <param name="value"></param>
        private void UpdateBar(float value)
        {

            float scaledValue;

            if (_fillValue == 0)
                scaledValue = _fillValue / MaxFill;
            else
                scaledValue = (_fillValue - MinFill) / (MaxFill - MinFill);

            _fill.FillImage.fillAmount = scaledValue;

            OnValueChange?.Invoke(value);

        }

    }
    [System.Serializable]
    public class SliderBarEvent : UnityEvent<float> { }
}
