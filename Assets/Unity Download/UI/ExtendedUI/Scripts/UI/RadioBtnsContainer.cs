using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DIG.UIExpansion
{
    [RequireComponent(typeof(VerticalLayoutGroup))]
    public class RadioBtnsContainer : MonoBehaviour
    {
        public RadioBtnContainerEvent OnValueChange { get { return _onValueChange; } set { _onValueChange = value; } }

        [SerializeField] private RadioBtn _radioBtnPrefab;
        [SerializeField] private List<string> _options;

        private List<RadioBtn> _radioButtons = new List<RadioBtn>();

        public RadioBtnContainerEvent _onValueChange;

        private VerticalLayoutGroup _VLayoutGroup;

        private void Awake()
        {
            _VLayoutGroup = GetComponent<VerticalLayoutGroup>();

            ClearChildren();
            CreateOptions();
        }


        private IEnumerator Start()
        {
            //Make sure we deactivate a layout group because we don't need it anymore
            yield return null;
            _VLayoutGroup.enabled = false;
        }


        /// <summary>
        /// Creates radio buttons from the prefab
        /// </summary>
        private void CreateOptions()
        {
            if (_radioBtnPrefab != null)
            {
                for (int i = 0; i < _options.Count; i++)
                {
                    RadioBtn rb = Instantiate(_radioBtnPrefab, transform);
                    rb.LabelValue = _options[i];
                    rb.SetIndex(i);
                    _radioButtons.Add(rb);
                    rb.Button.onClick.AddListener(() => SelectBtn(rb));
                }
            }
            else
            {
                Debug.LogError("Radio Btn Prefab missing!!");
            }
        }

        /// <summary>
        /// Clears Placeholder buttons
        /// </summary>
        private void ClearChildren()
        {
            List<RadioBtn> children = new List<RadioBtn>();

            children = transform.GetComponentsInChildren<RadioBtn>().ToList();

            children.ForEach(child =>
            {
                Destroy(child.gameObject);
            });

            children.Clear();
        }

        /// <summary>
        /// Methode that executes on radio button click
        /// </summary>
        /// <param name="rb"></param>
        public void SelectBtn(RadioBtn rb)
        {
            //rb.IsActive = !rb.IsActive;
            rb.SetActivStatus(!rb.IsActive);

            _radioButtons.ForEach(btn =>
            {
                if (btn != rb)
                {
                    btn.OnImage.gameObject.SetActive(false);
                    btn.SetActivStatus(false);
                }
            });

            if (rb.IsActive)
                rb.OnImage.gameObject.SetActive(true);
            else
                rb.OnImage.gameObject.SetActive(false);

            _onValueChange?.Invoke(rb);

        }
    }

    [System.Serializable]
    public class RadioBtnContainerEvent : UnityEvent<RadioBtn> { }
}
