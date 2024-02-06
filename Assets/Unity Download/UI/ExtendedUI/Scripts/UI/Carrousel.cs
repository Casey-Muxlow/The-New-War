using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DIG.UIExpansion
{
    public class Carrousel : MonoBehaviour
    {
        #region Properties
        public int Value { get { return _value; } private set { _value = value; } }
        public int StartValue { get { return _startValue; } set { _startValue = value; } }
        public List<string> Options { get { return _options; } set { _options = value; } }
        public ArrowEvent OnValueChange { get { return _onValueChange; } set { _onValueChange = value; } }
        #endregion

        [Header("Settings")]
        [SerializeField] private int _startValue = 0;
        [SerializeField] private bool _canWrap = false;

        [Header("UI")]
        [SerializeField] private Button _nextArrow;
        [SerializeField] private Button _prevArrow;
        [SerializeField] private TMP_Text _valueText;

        [Space(10)]
        [SerializeField] private List<string> _options = new List<string>();

        [Space(15)]
        [SerializeField] private ArrowEvent _onValueChange;

        private int _value = 0;

        private void Awake()
        {
            _nextArrow.onClick.AddListener(() => NextItem());
            _prevArrow.onClick.AddListener(() => PrevItem());

        }

        private void Start()
        {
            RefreshValue(_startValue);
        }

        /// <summary>
        /// Clear options list
        /// </summary>
        public void ClearOptions()
        {
            Options.Clear();
        }

        /// <summary>
        /// Refreshes arrow value without invoking an event
        /// </summary>
        /// <param name="value">Value of an list element to set</param>
        public void RefreshValue(int value)
        {
            _value = value;
            _valueText.text = _options[_value].ToString();
        }

        #region Private Methodes
        private void NextItem()
        {
            if(_canWrap)
            {
                if (_value < _options.Count - 1)
                {
                    _value++;
                    ValueChange();
                }
                else
                {
                    _value = 0;
                    ValueChange();
                }
            }
            else
            {
                if (_value < _options.Count - 1)
                {
                    _value++;
                    ValueChange();
                }
            }
        }

        private void PrevItem()
        {
            if(_canWrap)
            {
                if (_value > 0)
                {
                    _value--;
                    ValueChange();
                }
                else
                {
                    _value = _options.Count - 1;
                    ValueChange();
                }
            }
            else
            {
                if (_value > 0)
                {
                    _value--;
                    ValueChange();
                }
            }
        }

        private void ValueChange()
        {
            _valueText.text = _options[_value];
            _onValueChange?.Invoke(_value);
        }
        #endregion
    }

    [System.Serializable]
    public class ArrowEvent : UnityEvent<int> { }
}