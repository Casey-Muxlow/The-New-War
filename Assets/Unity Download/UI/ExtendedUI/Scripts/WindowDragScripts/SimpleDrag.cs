using UnityEngine;
using UnityEngine.EventSystems;

namespace DIG.UIExpansion
{
    /// <summary>
    /// Used for draging objects works for UI and gameobjects
    /// </summary>
    public class SimpleDrag : MonoBehaviour, IDragHandler
    {
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

    }
}
