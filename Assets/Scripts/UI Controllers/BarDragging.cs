using UnityEngine;
using UnityEngine.EventSystems;

public class BarDragging : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public GameObject objectToDrag;
    private Vector2 moveVector;
    
    public void OnBeginDrag(PointerEventData data)
    {
        moveVector = new Vector2(objectToDrag.GetComponent<RectTransform>().position.x - Input.mousePosition.x, objectToDrag.GetComponent<RectTransform>().position.y - Input.mousePosition.y);
    }

    public void OnDrag(PointerEventData data)
    {
        objectToDrag.transform.position = new Vector3(Input.mousePosition.x + moveVector.x, Input.mousePosition.y+ moveVector.y);
    }
}
