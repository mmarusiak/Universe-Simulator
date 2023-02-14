using UnityEngine;
using UnityEngine.UI;

public class PlanetLookEditor : PlanetEditor
{
    [SerializeField] private Image _planetPreviewImage;
    [SerializeField] private ColorWheelControl _colorWheelControl;

    public void OnPlanetChanged() => _colorWheelControl.PickColor(EditorBase.CurrentPlanet.PlanetColor);

    public void UpdateColorToPreview() => _planetPreviewImage.color = _colorWheelControl.Selection;

    public void ApplyColorToPlanet() => EditorBase.CurrentPlanet.PlanetColor = _colorWheelControl.Selection;
}
