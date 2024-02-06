using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DIG.UIExpansion
{
    /// <summary>
    /// Used only on UI objects (note that SimpleDrag component will also work but this on is smoother)
    /// This component requers GraphicRaycaster component that in return requers Canvas component so all of them will be added
    /// </summary>
    [RequireComponent(typeof(GraphicRaycaster))]
    public class SmoothUIDrag : MonoBehaviour, IDragHandler
    {
        private RectTransform dragRect;
        private Canvas canvas;

        private void Awake()
        {
            if (dragRect == null)
            {
                dragRect = this.gameObject.GetComponent<RectTransform>();
            }
            if (canvas == null)
            {
                canvas = this.gameObject.GetComponent<Canvas>();
            }

        }

        public void OnDrag(PointerEventData eventData)
        {
            dragRect.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

    }
}
