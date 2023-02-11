using UnityEngine;

public class PlanetNameHolder : MonoBehaviour
{
    public GameObject Planet;
    public GravityObject PlanetController;
    public GravityObjectsController controller;
    public GameObject VelocityText, DistText;
    
    private RectTransform _nameTransform;
    private RectTransform _velocityTransform;
    private RectTransform _distanceTransform;
    
    private Vector2 textPos;
    // pixels relative to full hd res
    public Vector2 velocityOffset = new (0, 32);
    
    public float SmoothSpeed= 0.02f;
    private bool firstTime = true;

    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_EDITOR
            velocityOffset = new( -velocityOffset.x * UnityEditor.Handles.GetMainGameViewSize().x / 1920, -velocityOffset.y * UnityEditor.Handles.GetMainGameViewSize().y / 1080);
        #else
             velocityOffset = new( -velocityOffset.x * Screen.currentResolution.width / 1920, -velocityOffset.y * Screen.currentResolution.height / 1080);
        #endif
        Planet = PlanetController.gameObject;
        
        SetUpVelocityHolder();
        SetUpDistanceHolder();

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

    void SetUpDistanceHolder()
    {
        DistText = Instantiate(GlobalVariables.Instance.DistancePrefab, transform.parent);
        DistText.name = gameObject.name + " distance";
        DistText.transform.GetChild(0).GetComponent<DistanceHolder>().SetTrans(Planet.transform);
        _distanceTransform = DistText.GetComponent<RectTransform>();
    }

    bool IsAttachedPlanetSelectedPlanet()
    {
        return PlanetController == GlobalVariables.Instance.CurrentGravityObject;
    }

    void LateUpdate()
    {
        Planet.transform.GetChild(0).Find("TextPos").position = new Vector3(  Planet.transform.position.x - PlanetController.Radius - 4f,  Planet.transform.position.y - PlanetController.Radius - 2.1f);
        textPos = Camera.main.WorldToScreenPoint(Planet.transform.GetChild(0).Find("TextPos").position);

        if (Time.timeScale > 0 || firstTime)
        {
            _nameTransform.position = textPos;
            _velocityTransform.position = textPos + velocityOffset;
            _distanceTransform.position = textPos + velocityOffset + velocityOffset;
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
        
        
        Vector3 distSmoothFollow = Vector3.Lerp(_distanceTransform.position, textPos + velocityOffset + velocityOffset, SmoothSpeed);
        _distanceTransform.position = distSmoothFollow;
    }
}
