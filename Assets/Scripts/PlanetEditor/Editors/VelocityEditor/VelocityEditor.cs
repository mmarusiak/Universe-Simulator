using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities.UniverseLibraries;

/// <summary>
/// Velocity editor is editor where player can change planet's velocity.
/// </summary>
public class VelocityEditor : PlanetEditor
{
    public static VelocityEditor Instance;
    private void Awake() => Instance = this;
    [SerializeField] private InputField[] axisComponents = new InputField[2];
    [SerializeField] private Text magComponent;

    [Space] [SerializeField] private UnityEvent onVelocityChanged;
    public void ChangeComponentsOnState(bool state)
    {
        axisComponents[0].interactable = !state;
        axisComponents[1].interactable = !state;
    }
    
    public void ChangeVelocity(Vector2 targetVel)
    {
        axisComponents[0].text = UniverseTools.RoundOutput(targetVel.x);
        axisComponents[1].text = UniverseTools.RoundOutput(targetVel.y);
        magComponent.text = UniverseTools.RoundOutput(targetVel.magnitude);

        onVelocityChanged.Invoke();
    }

    public void InitializeVelocity()
    {
        if (EditorBase.CurrentPlanet == null) return;
        ChangeVelocity(EditorBase.CurrentPlanet.CurrentVelocity);
    }

    public void EnterNewVelocity(bool isAxisX)
    {
        if (isAxisX)
        {
            ChangeXAxis();
            onVelocityChanged.Invoke();
        }
        else
        {
            ChangeYAxis();
            onVelocityChanged.Invoke();
        }
        UpdateMagnitudeTxt();
    }

    private void UpdateMagnitudeTxt() => magComponent.text = UniverseTools.RoundOutput(EditorBase.CurrentPlanet.CurrentVelocity.magnitude);

    private void ChangeYAxis() => EditorBase.CurrentPlanet.CurrentVelocity =
        new(EditorBase.CurrentPlanet.CurrentVelocity.x, UniverseMath.StringToFloat(axisComponents[1].text));

    private void ChangeXAxis() => EditorBase.CurrentPlanet.CurrentVelocity =
        new(UniverseMath.StringToFloat(axisComponents[0].text), EditorBase.CurrentPlanet.CurrentVelocity.y);
}
