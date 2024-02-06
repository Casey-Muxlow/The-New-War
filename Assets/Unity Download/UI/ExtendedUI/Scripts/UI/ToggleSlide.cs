using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DIG.Tweening;
using UnityEngine.UI;

namespace DIG.UIExpansion
{
    public class ToggleSlide : MonoBehaviour, IPointerDownHandler
    {
        #region Properties
        public bool IsActive { get { return _isActive; } set { _isActive = value; } }
        public float AnimationDuration { get { return _duration; } set { _duration = value; } }
        #endregion

        [SerializeField] private RectTransform _toggle;
        [Header("Settings")]
        [SerializeField] private bool _isActive;
        [SerializeField] private bool _snap = false;

        [Header("Possition settings")]
        [SerializeField] private float _activPosition;
        [SerializeField] private float _deactivatedPossition;

        [Header("Optional UI")]
        [SerializeField] private Sprite _activeSprite = null;
        [SerializeField] private Sprite _inactiveSprite = null;

        [Space(10)]
        public ToggleEvent OnValueChange;

        private float _duration = 0.2f;

        private Image _knobImage;
        private Sprite _defaultKnobSprite;

        private void Awake()
        {
            _knobImage = _toggle.GetComponent<Image>();
            _defaultKnobSprite = _knobImage.sprite;
        }

        private void Start()
        {
            SetStartState();
        }

        /// <summary>
        /// Sets starting state of a animation
        /// </summary>
        private void SetStartState()
        {
            if (IsActive)
                LerpPosition(_toggle, _deactivatedPossition, _duration);
            else
                LerpPosition(_toggle, _activPosition, _duration);

            SetSelectionSprites();
        }

        /// <summary>
        /// Sets optional selection sprites if there are any
        /// </summary>
        private void SetSelectionSprites()
        {
            if(IsActive)
            {
                if(_activeSprite != null)
                    _knobImage.sprite = _activeSprite;
                else
                    _knobImage.sprite = _defaultKnobSprite;
            }
            else
            {
                if (_inactiveSprite != null)
                    _knobImage.sprite = _inactiveSprite;
                else
                    _knobImage.sprite = _defaultKnobSprite;
            }
        }

        /// <summary>
        /// Handles toggle value, animation and invokes OnValueChange event
        /// </summary>
        private void AnimateToggle()
        {
            if (IsActive)
            {
                if (_snap)
                    LerpPosition(_toggle, _activPosition, 0);
                else
                    LerpPosition(_toggle, _activPosition, _duration);
            }
            else
            {
                if (_snap)
                    LerpPosition(_toggle, _deactivatedPossition, 0);
                else
                    LerpPosition(_toggle, _deactivatedPossition, _duration);
            }


            IsActive = !IsActive;
            SetSelectionSprites();
            OnValueChange?.Invoke(IsActive);
        }

        /// <summary>
        /// Refreshes toggle UI to current value (Useful if you chang value of the toggle somewhere else)
        /// DOESN'T INVOKE OnValueChange EVENT!!
        /// </summary>
        public void RefreshAndAnimate()
        {
            if (IsActive)
            {
                if (_snap)
                    LerpPosition(_toggle, _deactivatedPossition, 0);
                else
                    LerpPosition(_toggle, _deactivatedPossition, _duration);
            }
            else
            {
                if (_snap)
                    LerpPosition(_toggle, _activPosition, 0);
                else
                    LerpPosition(_toggle, _activPosition, _duration);
            }

            SetSelectionSprites();


        }
        public void OnPointerDown(PointerEventData eventData)
        {
            AnimateToggle();
        }

        /// <summary>
        /// Use for learping target position
        /// </summary>
        /// <param name="target"></param>
        /// <param name="endPosition"></param>
        /// <param name="duration"></param>
        private void LerpPosition(Transform target, float endPosition, float duration)
        {
            //StartCoroutine(LeerpPositionCorutine(target, endPosition, duration));
            target.LerpPositionX(endPosition, duration);
        }
    }
    [System.Serializable]
    public class ToggleEvent : UnityEvent<bool> { }
}
