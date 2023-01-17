using UnityEngine;
using UnityEngine.UI;

public class VisualWindowController : MonoBehaviour
{
    void Start()
    {
        GetComponent<WindowController>().Close(true);
    }

    public void Show(string name, Color color)
    {
        transform.GetChild(0).Find("VisualWindowPlanetTXT").GetComponent<Text>().text = name + "'s look editor";
        transform.GetChild(3).Find("ColorWheel").GetComponent<ColorWheelControl>().PickColor(color);
    }
}
