using UnityEngine;

public class PlanetNameHolder : MonoBehaviour
{
    public GameObject Planet;
    public GravityObject PlanetController;
    public float SmoothSpeed= 0.02f;
    public GravityObjectsController controller;
    private RectTransform _transform;
    private Vector2 textPos;

    // Start is called before the first frame update
    void Start()
    {
        Planet = PlanetController.gameObject;

        controller = GameObject.FindWithTag("GravityController").GetComponent<GravityObjectsController>();

        _transform = GetComponent<RectTransform>();
        _transform.sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x + 300,
            GetComponent<RectTransform>().sizeDelta.y + 100);
    }

    void LateUpdate()
    {
        Planet.transform.Find("TextPos").position = new Vector3(  Planet.transform.position.x - PlanetController.Radius - 4f,  Planet.transform.position.y - PlanetController.Radius - 2.1f);
        
        textPos = Camera.main.WorldToScreenPoint(Planet.transform.Find("TextPos").position);
        if (Time.timeScale > 0)
            _transform.position = new Vector2(textPos.x, textPos.y);
        else
            SmoothFollow();
    }

    void SmoothFollow()
    {
        Vector3 smoothFollow = Vector3.Lerp(_transform.position, textPos, SmoothSpeed);
        _transform.position = smoothFollow;
    }
}
