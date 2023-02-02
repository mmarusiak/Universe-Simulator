using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WindowController : MonoBehaviour
{
    public bool Shown = false;
    public GameObject ToShow = null;
    public GameObject ToHide = null;
    [InspectorLabel("Offset of position on show")]
    public float offsetX = 300;
    public float offsetY = 700;

    public float TransitionTime = 0.5f;
    
    public void Show(bool instantShow)
    {
        if (!Shown)
        {
            Shown = true;
            if (ToShow == null || instantShow)
            {
                bool isXOnScreen = Input.mousePosition.x - offsetX > GetComponent<RectTransform>().rect.width/2 && Input.mousePosition.x - offsetX < Screen.currentResolution.width;
                bool isYOnScreen = Input.mousePosition.y - offsetY > GetComponent<RectTransform>().rect.height/2 && Input.mousePosition.y - offsetY < Screen.currentResolution.height;
                Vector3 newPos = new (Input.mousePosition.x + offsetX, Input.mousePosition.y + offsetY, 0);
                
                if(isXOnScreen && isYOnScreen) newPos = new (Input.mousePosition.x - offsetX, Input.mousePosition.y - offsetY, 0);
                else if(isXOnScreen) newPos = new (Input.mousePosition.x - offsetX, Input.mousePosition.y + offsetY, 0);
                else if(isYOnScreen) newPos = new (Input.mousePosition.x + offsetX, Input.mousePosition.y - offsetY, 0);

                gameObject.transform.position = newPos;
            }
            else ShowAll();
        }
    }
    
    public void OnButtonClose() => Close(false);

    public void Close(bool instantClose)
    {
        Shown = false;
        if(ToHide == null || instantClose || ToHide == gameObject)
            gameObject.transform.position = new Vector3(6000,6000,0);
        else
            CloseAll();
    }

    void ShowAll()
    {
        for (int i = 0; i < ToShow.transform.childCount; i++)
        {
            if (ToShow.transform.GetChild(i).TryGetComponent(out WindowController win))
            {
                win.Show(true);
            }
        }
    }

    void CloseAll()
    {
        for (int i = 0; i < ToHide.transform.childCount; i++)
        {
            if (ToHide.transform.GetChild(i).TryGetComponent(out WindowController win))
            {
                win.Close(true);
            }
        }
    }
}
