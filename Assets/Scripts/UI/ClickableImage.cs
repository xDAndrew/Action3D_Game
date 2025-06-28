using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UI
{
    public class ClickableImage : MonoBehaviour, IPointerClickHandler
    {
        [FormerlySerializedAs("OnLeftClick")]
        public UnityEvent onLeftClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;
            onLeftClick?.Invoke();
        }
    }
}