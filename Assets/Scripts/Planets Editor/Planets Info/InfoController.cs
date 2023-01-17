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
        CurrentPlanet = GlobalVariables.Instance.CurrentGravityObject.gameObject;
        
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
            GameObject newWindow = Instantiate(InfoWindowPrefab, Input.mousePosition + new Vector3(345, -100), Quaternion.Euler(0, 0, 0),
                GameObject.Find("AllWindows").GetComponent<RectTransform>());
            newWindow.GetComponent<InfoHandler>().LoadInfoData(CurrentPlanet);
            OpenedWindows.Add(new InfoWindow(newWindow, CurrentPlanet.GetComponent<GravityObject>()));
        }
        else
        {
            Destroy(openedWindow.Window);
            OpenedWindows.Remove(openedWindow);
            Open();
        }
    }
}
