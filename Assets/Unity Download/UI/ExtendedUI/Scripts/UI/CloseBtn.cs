using UnityEngine;
using UnityEngine.EventSystems;

namespace DIG.UIExpansion
{
    public class CloseBtn : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject _target;
        public void OnPointerClick(PointerEventData eventData)
        {
            _target.SetActive(false);
        }

    }
}
