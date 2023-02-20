using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UISelector : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private UnityEvent onSelect, onDeselect;

    public void OnSelect(BaseEventData eventData) => onSelect.Invoke();
    
    public void OnDeselect(BaseEventData eventData) => onDeselect.Invoke();
}
