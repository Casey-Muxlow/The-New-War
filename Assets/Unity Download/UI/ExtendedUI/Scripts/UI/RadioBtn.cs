using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DIG.UIExpansion
{
    public class RadioBtn : MonoBehaviour
    {
        #region Properties
        public string LabelValue { get { return _valueLabel.text; } set { _valueLabel.text = value; } }
        public int Index { get; private set; }
        public bool IsActive { get { return _isActive; } private set { _isActive = value; } }
        #endregion

        public Image OnImage;
        private TMP_Text _valueLabel;
        public Button Button;
        [SerializeField] private bool _isActive;

        private void Awake()
        {
            _valueLabel = GetComponentInChildren<TMP_Text>();
        }

        internal void SetIndex(int index)
        {
            Index = index;
        }

        internal void SetActivStatus(bool status)
        {
            _isActive = status;
        }
    }

}
