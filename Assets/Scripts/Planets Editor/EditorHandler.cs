using UnityEngine;
using UnityEngine.UI;

public class EditorHandler : MonoBehaviour
{
    public GameObject Planet;
    public GameObject Panel;
    
    public Sprite PlanetImage;
    public Color PlanetColor;
    
    private GravityObjectsController _controller;
    private Text _planetNameBar;
    public bool Shown = false;

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
        
        if(!Shown)
            gameObject.GetComponent<RectTransform>().position =
                new Vector3( Input.mousePosition.x - GetComponent<RectTransform>().sizeDelta.x/2,
                    Input.mousePosition.y - GetComponent<RectTransform>().sizeDelta.y/1.5f);
        Shown = true;
        
        _planetNameBar.text = planetController.PlanetName;
        transform.GetChild(0).Find("Components").GetComponent<ComponentEditor>().UpdateText(planetController.PlanetName, planetController.Mass, planetController.Radius);
        transform.GetChild(0).GetChild(2).GetComponent<PreviewController>().LoadSpriteToPlanet(targetPlanet.GetComponent<SpriteRenderer>().sprite, targetPlanet.GetComponent<SpriteRenderer>().color);
    }

    public void AddSpriteToPlanet(Sprite spriteToAdd)
    {
        PlanetImage = spriteToAdd;
        Planet.GetComponent<SpriteRenderer>().sprite = spriteToAdd;
    }
    
    public void Close()
    {
        Shown = false;
        GetComponent<RectTransform>().transform.position = new Vector3(6000, 6000);
    }
}
