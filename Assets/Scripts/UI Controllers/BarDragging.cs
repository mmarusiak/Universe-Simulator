using UnityEngine;
using UnityEngine.EventSystems;

public class BarDragging : MonoBehaviour
{
    public GameObject objectToDrag;
    private Vector2 moveVector;
    
    public void OnBeginDrag(BaseEventData data)
    {
        moveVector = new Vector2(objectToDrag.GetComponent<RectTransform>().position.x - Input.mousePosition.x, objectToDrag.GetComponent<RectTransform>().position.y - Input.mousePosition.y);
    }

    public void DragHandler(BaseEventData data)
    {
        objectToDrag.transform.position = new Vector3(Input.mousePosition.x + moveVector.x, Input.mousePosition.y+ moveVector.y);
    }
}
