using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WindowController : MonoBehaviour
{
    public bool Shown = false;
    [InspectorLabel("Offset of position on show")]
    public float offsetX = 300;
    public float offsetY = 700;

    public void Show()
    {
        if (!Shown)
        {
            Shown = true;
            gameObject.transform.position =
                new Vector3(Input.mousePosition.x - offsetX, Input.mousePosition.y - offsetY, 0);
        }
    }

    public void Close()
    {
        Shown = false;
        gameObject.transform.position = new Vector3(6000,6000,0);
    }
}
