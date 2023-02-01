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
        _controller = GravityObjectsController.Instance;
        _planetNameBar = transform.GetChild(0).GetChild(2).GetComponent<Text>();
        (window = GetComponent<WindowController>()).Close(false);
    }

    public void ShowPanel(GameObject targetPlanet)
    {
        Planet = targetPlanet;
        GravityObject planetController = Planet.GetComponent<GravityObject>();

        window.Show(true);
        
        _planetNameBar.text = planetController.PlanetName;
        GameObject.Find("LookPlanetWindow").GetComponent<VisualWindowController>().Show(planetController.PlanetName, targetPlanet.transform.GetChild(0).GetComponent<SpriteRenderer>().color);
        transform.GetChild(0).Find("Components").GetComponent<ComponentEditor>().UpdateText(planetController.PlanetName, planetController.Mass, planetController.Radius);
        
        PreviewController previewController = transform.GetChild(0).GetChild(0).GetComponent<PreviewController>();
        previewController.LoadSpriteToPreview(targetPlanet.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite);
        previewController.LoadColorToPreview(targetPlanet.transform.GetChild(0).GetComponent<SpriteRenderer>().color);
        
        if (previewController.ImagesDropdown.options[previewController.ImagesDropdown.value].image !=
            targetPlanet.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite)
        {
            previewController.ImagesDropdown.SetValueWithoutNotify(0);
            // getting index of sprite
            for (int i = 0; i < previewController.ImagesDropdown.options.Count; i++)
            {
                if (previewController.ImagesDropdown.options[i].image == targetPlanet.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite)
                {
                    previewController.ImagesDropdown.SetValueWithoutNotify(i);
                    break;
                }
            }
        }
    }
}
