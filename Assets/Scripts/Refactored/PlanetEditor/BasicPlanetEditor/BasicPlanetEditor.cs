using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class BasicPlanetEditor : PlanetEditor
{
    public static BasicPlanetEditor Instance;

    // need to move it to some separate class/holder
    [SerializeField] private Sprite _defaultPlanetSprite;
    [SerializeField] private Image _planetImage;
    [SerializeField] private InputField _planetName, _planetMass, _planetRadius;

    private void Awake() => Instance = this;

    public void OnWindowShown() => OnPlanetChanged();

    public void OnPlanetChanged()
    {
        _planetImage.sprite = EditorBase.CurrentPlanet.PlanetSprite
            ? EditorBase.CurrentPlanet.PlanetSprite
            : _defaultPlanetSprite;
        _planetImage.color = EditorBase.CurrentPlanet.PlanetColor;
        _planetName.text = EditorBase.CurrentPlanet.Name;
        _planetMass.text = EditorBase.CurrentPlanet.Mass.ToString(CultureInfo.InvariantCulture);
        _planetRadius.text = EditorBase.CurrentPlanet.Radius.ToString(CultureInfo.InvariantCulture);

    }

    public void SetName(InputField nameField) => EditorBase.CurrentPlanet.Name = nameField.text;

    public void SetMass(InputField massField) => EditorBase.CurrentPlanet.Mass = UniverseMath.StringToFloat(massField.text);

    public void SetRadius(InputField radiusField) => EditorBase.CurrentPlanet.Radius = UniverseMath.StringToFloat(radiusField.text);
}
