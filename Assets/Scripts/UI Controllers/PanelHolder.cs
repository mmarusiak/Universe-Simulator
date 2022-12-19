using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelHolder : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private float startDragX;
    private RectTransform _transform;

    private void Start()
    {
        _transform = GetComponent<RectTransform>().parent.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startDragX = eventData.position.x;
        Debug.Log("start: " + eventData.position.x);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _transform.anchoredPosition = new Vector3(Math.Clamp(eventData.position.x - startDragX, -190, 0), 0, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("end: " + eventData.position.x);
        if (startDragX - eventData.position.x > 150)
        {
            HidePanel();
        }
        else if(startDragX - eventData.position.x < -150)
        {
            ShowPanel();
        }
        
        Debug.Log(name + " end drag");
    }

    public void HidePanel()
    {
        _transform.anchoredPosition = new Vector3(-190, 0, 0);
    }

    public void ShowPanel()
    {
        _transform.anchoredPosition = new Vector3(0, 0, 0);
    }
}
