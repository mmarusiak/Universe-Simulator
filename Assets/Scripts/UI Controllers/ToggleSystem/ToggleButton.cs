using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    [SerializeField] private Color baseColor = new Color32(248, 248, 248, 255);
    [SerializeField] private float pressedOpacity = 0.5f;
    public bool toggled;

    void Start()
    {
        Toggle();
    }

    public void Toggle()
    {
        toggled = !toggled;
        float opacity = toggled ? baseColor.a : pressedOpacity * baseColor.a;

        Color newColor = new Color(baseColor.r, baseColor.g, baseColor.b, opacity);

        try
        {
            transform.GetChild(0).GetComponent<Text>().color = newColor;
        }
        catch (Exception e) { // ignored if there is no text in child
        }
        
        GetComponent<Image>().color = newColor;
    }

    public void ToggleToState(bool targetState)
    {
        toggled = !targetState;
        Toggle();
    }
}