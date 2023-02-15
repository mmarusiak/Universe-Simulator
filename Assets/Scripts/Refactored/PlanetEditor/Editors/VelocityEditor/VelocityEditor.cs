using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VelocityEditor : PlanetEditor
{
    public static VelocityEditor Instance;
    void Awake() => Instance = this;
    [SerializeField] private InputField[] axisComponents = new InputField[2];
    [SerializeField] private Text magComponent;

    [Space] [SerializeField] private UnityEvent onVelocityChanged;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeComponentsOnState(bool state)
    {
        axisComponents[0].interactable = !state;
        axisComponents[1].interactable = !state;
    }
    
    public void ChangeVelocity(Vector2 targetVel)
    {
        axisComponents[0].text = targetVel.x.ToString(CultureInfo.InvariantCulture);
        axisComponents[1].text = targetVel.y.ToString(CultureInfo.InvariantCulture);
        magComponent.text = targetVel.magnitude.ToString(CultureInfo.InvariantCulture);
        
        onVelocityChanged.Invoke();
    }
    
    public void InitializeVelocity() => ChangeVelocity(EditorBase.CurrentPlanet.CurrentVelocity);
    
    public void EnterNewVelocity(bool isAxisX)
    {
        if (isAxisX)
        {
            ChangeXAxis();
            onVelocityChanged.Invoke();
            return;
        }
        ChangeYAxis();
        onVelocityChanged.Invoke();
    }

    void ChangeYAxis() => EditorBase.CurrentPlanet.CurrentVelocity =
        new(EditorBase.CurrentPlanet.CurrentVelocity.x, UniverseMath.StringToFloat(axisComponents[1].text));
    void ChangeXAxis() => EditorBase.CurrentPlanet.CurrentVelocity =
        new(UniverseMath.StringToFloat(axisComponents[0].text), EditorBase.CurrentPlanet.CurrentVelocity.y);
}
