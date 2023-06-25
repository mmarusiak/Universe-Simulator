using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Bar for every editor. Player can drag editors with this script.
/// </summary>
public class EditorBar : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    [SerializeField] private RectTransform _editorContainer;
    private Vector2 _offset;

    public void OnBeginDrag(PointerEventData data) => _offset = (Vector2)_editorContainer.position - data.position;
    
    public void OnDrag(PointerEventData data) =>  _editorContainer.position = _offset + data.position;
}
