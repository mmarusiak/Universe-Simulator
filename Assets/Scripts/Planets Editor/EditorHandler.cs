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
    private WindowController window;

    void Start()
    {
        _controller = GameObject.FindWithTag("GravityController").GetComponent<GravityObjectsController>();
        _planetNameBar = transform.GetChild(0).GetChild(2).GetComponent<Text>();
        (window = GetComponent<WindowController>()).Close();
    }

    public void ShowPanel(GameObject targetPlanet)
    {
        Planet = targetPlanet;
        GravityObject planetController = Planet.GetComponent<GravityObject>();

        window.Show();
        
        _planetNameBar.text = planetController.PlanetName;
        transform.GetChild(0).Find("Components").GetComponent<ComponentEditor>().UpdateText(planetController.PlanetName, planetController.Mass, planetController.Radius);
        transform.GetChild(0).GetChild(0).GetComponent<PreviewController>().LoadSpriteToPreview(targetPlanet.GetComponent<SpriteRenderer>().sprite, targetPlanet.GetComponent<SpriteRenderer>().color);
    }

    public void LoadSpriteToPlanet(Sprite spriteToAdd)
    {
        PlanetImage = spriteToAdd;
        Planet.GetComponent<SpriteRenderer>().sprite = spriteToAdd;
    }
}
