using UnityEngine;

public class PlanetNameHolder : MonoBehaviour
{
    public GameObject Planet;
    public GravityObject PlanetController;
    public float Smoothness = 0.125f;
    public GravityObjectsController controller;
    private RectTransform _transform;
    private float interval, startTime, offset;
    private Vector2 planetPos;

    // Start is called before the first frame update
    void Start()
    {
        Planet = PlanetController.gameObject;
        Vector2 planetPos = Camera.main.WorldToScreenPoint(Planet.transform.position);

        offset = PlanetController.Radius * 15;

        controller = GameObject.FindWithTag("GravityController").GetComponent<GravityObjectsController>();

        _transform = GetComponent<RectTransform>();
        _transform.position = new Vector2(planetPos.x - PlanetController.Radius*15, planetPos.y - PlanetController.Radius*15);
        _transform.sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x + 300,
            GetComponent<RectTransform>().sizeDelta.y + 100);
    }
}
