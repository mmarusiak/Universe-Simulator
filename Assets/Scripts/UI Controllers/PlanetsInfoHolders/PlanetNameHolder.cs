using UnityEngine;

public class PlanetNameHolder : MonoBehaviour
{
    public GameObject Planet;
    public GravityObject PlanetController;
    public GravityObjectsController controller;
    public GameObject VelocityText;
    
    private RectTransform _nameTransform;
    private RectTransform _velocityTransform;
    
    private Vector2 textPos;
    public Vector2 velocityOffset = new (0, -30);
    
    public float SmoothSpeed= 0.02f;
    private bool firstTime = true;

    // Start is called before the first frame update
    void Start()
    {
        Planet = PlanetController.gameObject;
        SetUpVelocityHolder();
        
        controller = GravityObjectsController.Instance;

        _nameTransform = GetComponent<RectTransform>();
        _nameTransform.sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x + 300,
            GetComponent<RectTransform>().sizeDelta.y + 100);
    }

    void SetUpVelocityHolder()
    {
        VelocityText = Instantiate(GlobalVariables.Instance.VelocityPrefab, transform.parent);
        VelocityText.name = gameObject.name + " velocity";
        VelocityText.transform.GetChild(0).GetComponent<VelocityHolder>().SetRigid(Planet.GetComponent<Rigidbody2D>());
        _velocityTransform = VelocityText.GetComponent<RectTransform>();
    }
    
    void LateUpdate()
    {
        Planet.transform.GetChild(0).Find("TextPos").position = new Vector3(  Planet.transform.position.x - PlanetController.Radius - 4f,  Planet.transform.position.y - PlanetController.Radius - 2.1f);
        textPos = Camera.main.WorldToScreenPoint(Planet.transform.GetChild(0).Find("TextPos").position);

        if (Time.timeScale > 0 || firstTime)
        {
            _nameTransform.position = textPos;
            _velocityTransform.position = textPos + velocityOffset;
            firstTime = false;
        }
        else
            SmoothFollow();
    }

    void SmoothFollow()
    {
        Vector3 nameSmoothFollow = Vector3.Lerp(_nameTransform.position, textPos, SmoothSpeed);
        _nameTransform.position = nameSmoothFollow;
        
        Vector3 velSmoothFollow = Vector3.Lerp(_velocityTransform.position, textPos + velocityOffset, SmoothSpeed);
        _velocityTransform.position = velSmoothFollow;
    }
}
