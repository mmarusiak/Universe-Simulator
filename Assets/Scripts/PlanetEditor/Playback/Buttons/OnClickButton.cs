using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Script for button, when they are behind some other Game Object (even empty). Used f.e. in top buttons.
/// </summary>
public class OnClickButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UnityEvent _onClick;
    public void OnPointerClick(PointerEventData eventData) => _onClick.Invoke();
}
