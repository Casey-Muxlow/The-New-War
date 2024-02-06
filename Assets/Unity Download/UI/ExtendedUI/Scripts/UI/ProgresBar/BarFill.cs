using UnityEngine;
using UnityEngine.UI;

namespace DIG.UIExpansion
{
    [RequireComponent(typeof(Image))]
    public abstract class BarFill : MonoBehaviour
    {
        public Image FillImage;

        private void Reset()
        {
            FillImage = GetComponent<Image>();
            InitilizeSliderBar();
        }

        private void Awake()
        {
            FillImage = GetComponent<Image>();
            InitilizeSliderBar();
        }

        /// <summary>
        /// Here you sets corect image values and gets the image
        /// </summary>
        public abstract void InitilizeSliderBar();
    }
}
