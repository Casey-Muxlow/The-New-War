using DIG.UIExpansion;
using TMPro;
using UnityEngine;

namespace DIG.UIExpansion
{
    public class UIDemo : MonoBehaviour
    {
        [SerializeField] private ProgresBar _bar;
        [SerializeField] private ProgresBar _circularBar;
        [SerializeField] private Carrousel _arrowSelector;
        [SerializeField] private ToggleSlide _toggle;

        [Header("Texts")]
        [SerializeField] private TMP_Text _arrowBtnText;
        [SerializeField] private TMP_Text _toggleBtnText;
        [SerializeField] private TMP_Text _radioBtnText;

        public float TestValue = .2f;


        private float hp;

        private void Start()
        {
            hp = _bar.MaxFill;
            _bar.Value = _bar.MaxFill;
            _circularBar.Value = _circularBar.MaxFill;  
        }

        public void RadioBtnSelected(RadioBtn rb)
        {
            _radioBtnText.text = rb.LabelValue;
        }

        public void ArrownSelected(int arrowIndex)
        {
            _arrowBtnText.text = _arrowSelector.Options[arrowIndex];
        }

        public void ToggleSelected(bool toggleState)
        {
            if (toggleState)
                _toggleBtnText.text = "ON";
            else
                _toggleBtnText.text = "OFF";
        }

        public void AddHP()
        {
            if (hp < _bar.MaxFill)
            {
                hp++;
                _bar.Value = hp;
                _circularBar.Value = hp;
            }
        }

        public void ReduceHP()
        {
            if (hp > _bar.MinFill)
            {
                hp--;
                _bar.Value = hp;
                _circularBar.Value = hp;
            }
        }

    }

}
