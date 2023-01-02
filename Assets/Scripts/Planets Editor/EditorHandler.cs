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
        _planetNameBar = transform.GetChild(0).GetChild(1).GetComponent<Text>();
        Close();
    }

    public void ShowPanel(GameObject targetPlanet)
    {
        Planet = targetPlanet;
        GravityObject planetController = Planet.GetComponent<GravityObject>();
        
        gameObject.GetComponent<RectTransform>().position =
            new Vector3( Input.mousePosition.x - GetComponent<RectTransform>().sizeDelta.x/2,
                Input.mousePosition.y - GetComponent<RectTransform>().sizeDelta.y/1.5f);
        
       // _planetNameBar.text = planetController.PlanetName;
        transform.GetChild(0).Find("Components").GetComponent<ComponentEditor>().UpdateText(planetController.PlanetName, planetController.Mass, planetController.Radius);
    }
    
    public void Close()
    {
        GetComponent<RectTransform>().transform.position = new Vector3(6000, 6000);
    }
}
