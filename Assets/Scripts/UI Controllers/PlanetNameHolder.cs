using UnityEngine;

public class PlanetNameHolder : MonoBehaviour
{
    public GameObject Planet;
    public GravityObject PlanetController;
    public float Smoothness = 0.125f;
    public GravityObjectsController controller;
    private RectTransform _transform;
    private float interval, startTime, offset;
    private Vector2 textPos;

    // Start is called before the first frame update
    void Start()
    {
        Planet = PlanetController.gameObject;
        offset = PlanetController.Radius * 15;

        Planet.transform.Find("TextPos").localPosition = new Vector3(PlanetController.Radius * -1.4f, PlanetController.Radius / -5);
        
        controller = GameObject.FindWithTag("GravityController").GetComponent<GravityObjectsController>();

        _transform = GetComponent<RectTransform>();
        _transform.sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x + 300,
            GetComponent<RectTransform>().sizeDelta.y + 100);
    }

    void Update()
    {
        textPos = Camera.main.WorldToScreenPoint(Planet.transform.Find("TextPos").position);
        if (Time.timeScale > 0)
            _transform.position = new Vector2(textPos.x, textPos.y);
    }
    
}
