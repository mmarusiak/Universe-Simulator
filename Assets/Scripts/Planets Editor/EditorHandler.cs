using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EditorHandler : MonoBehaviour
{
    public GameObject Planet;
    public GameObject Panel;
    
    private GravityObjectsController _controller;
    public Sprite PlanetImage;
    private Text _planetNameBar;

    void Start()
    {
        _controller = GameObject.FindWithTag("GravityController").GetComponent<GravityObjectsController>();
        _planetNameBar = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        Panel.SetActive(false);
    }

    public void ShowPanel(GameObject targetPlanet)
    {
        Planet = targetPlanet;
        GravityObject planetController = Planet.GetComponent<GravityObject>();
        
        Panel.GetComponent<RectTransform>().position =
            new Vector3( Input.mousePosition.x - Panel.GetComponent<RectTransform>().sizeDelta.x,
                Input.mousePosition.y + Panel.GetComponent<RectTransform>().sizeDelta.y/1.5f);
        
        _planetNameBar.text = planetController.PlanetName;
        transform.GetChild(0).Find("Components").GetComponent<ComponentEditor>().UpdateText(planetController.PlanetName, planetController.Mass, planetController.Radius);
        Panel.SetActive(true);
    }
    
    public void Close()
    {
        Panel.SetActive(false);
    }
}
