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
    
    public void Show(bool instantShow)
    {
        if (!Shown)
        {
            Shown = true;
            if (ToShow == null || instantShow)
            {
                gameObject.transform.position =
                    new Vector3(Input.mousePosition.x - offsetX, Input.mousePosition.y - offsetY, 0);
            }
            else ShowAll();
        }
    }
    
    public void OnButtonClose() => Close(false);

    public void Close(bool instantClose)
    {
        Shown = false;
        if(ToHide == null || instantClose)
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
