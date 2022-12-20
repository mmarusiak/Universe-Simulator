using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelHolder : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private float startDragX;
    private RectTransform _transform;
    private bool shown = true;

    private void Start()
    {
        _transform = GetComponent<RectTransform>().parent.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startDragX = eventData.position.x;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _transform.anchoredPosition = new Vector3(Math.Clamp(eventData.position.x - startDragX, -190, 0), 0, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (startDragX - eventData.position.x > 150)
        {
            HidePanel();
        }
        else if(startDragX - eventData.position.x < -150)
        {
            ShowPanel();
        }
        else if(shown)
            ShowPanel();
        else
            HidePanel();
    }

    public void HidePanel()
    {
        _transform.anchoredPosition = new Vector3(-190, 0, 0);
        shown = false;
    }

    public void ShowPanel()
    {
        _transform.anchoredPosition = new Vector3(0, 0, 0);
        shown = true;
    }
}
