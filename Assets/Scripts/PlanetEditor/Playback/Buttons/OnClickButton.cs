using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnClickButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UnityEvent _onClick;
    public void OnPointerClick(PointerEventData eventData) => _onClick.Invoke();
}
