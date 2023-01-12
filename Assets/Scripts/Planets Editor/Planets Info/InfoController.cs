#nullable enable
using System.Collections.Generic;
using UnityEngine;

public class InfoWindow
{
    public GameObject Window;
    public GravityObject DataContainer;

    public InfoWindow(GameObject window, GravityObject dataContainer)
    {
        Window = window;
        DataContainer = dataContainer;
    }
}

public class InfoController : MonoBehaviour
{
    public GameObject InfoWindowPrefab;
    public GameObject CurrentPlanet;

    public List<InfoWindow> OpenedWindows = new List<InfoWindow>();

    public void Open()
    {
        
        InfoWindow openedWindow = new InfoWindow(null, null);
        bool isWindowCreated = false;
        foreach (var window in OpenedWindows)
        {
            if (window.DataContainer == CurrentPlanet.GetComponent<GravityObject>())
            {
                isWindowCreated = true;
                openedWindow = window;
                break;
            }
        }
        
        if (!isWindowCreated)
        {
            GameObject newWindow = Instantiate(InfoWindowPrefab, Input.mousePosition, Quaternion.Euler(0, 0, 0),
                GameObject.Find("AllWindows").GetComponent<RectTransform>());
            newWindow.GetComponent<InfoHandler>().LoadInfoData(CurrentPlanet);
        }
        else
        {
            Destroy(openedWindow.DataContainer.gameObject);
            OpenedWindows.Remove(openedWindow);
            Open();
        }
    }
}
