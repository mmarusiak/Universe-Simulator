using UnityEngine;

public class PlanetNameHolder : MonoBehaviour
{
    public GameObject Planet;
    public GravityObject PlanetController;
    public float Smoothness = 0.125f;
    
    // Start is called before the first frame update
    void Start()
    {
        Planet = PlanetController.gameObject;
        
        Vector2 planetPos = Camera.main.WorldToScreenPoint(Planet.transform.position);
        gameObject.GetComponent<RectTransform>().position = new Vector2(planetPos.x - PlanetController.Radius*15, planetPos.y - PlanetController.Radius*15);

        GetComponent<RectTransform>().sizeDelta =
            new Vector2(GetComponent<RectTransform>().sizeDelta.x + 300, GetComponent<RectTransform>().sizeDelta.y + 100);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
    }
}
