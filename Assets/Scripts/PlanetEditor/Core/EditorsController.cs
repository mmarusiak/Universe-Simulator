using System.Collections.Generic;
using GameCore.SimulationCore;
using UnityEngine;

/// <summary>
/// Controller for every editor in the game. It updates planet's name, and basically manages all editors.
/// </summary>
public class EditorsController : MonoBehaviour
{
    public static EditorsController Instance;
    private void Awake() => Instance = this;

    private PlanetComponent _lastEditedComponent;
    [SerializeField] private List<PlanetEditor> _editors = new ();
    
    public PlanetComponent LastEditedComponent
    {
        get => _lastEditedComponent;
        set
        {
            _lastEditedComponent = value;
            ChangeComponentInWindows();
        }
    }

    public void PlanetDestroyed(PlanetComponent comp)
    {
        if (comp == _lastEditedComponent) LastEditedComponent = null;
    }
    
    // When last edited component is changed make sure that the new one is being edited, not the old one
    void ChangeComponentInWindows()
    {
        foreach (var editor in _editors) editor.EditorBase.CurrentPlanet = _lastEditedComponent;
    }

    public void UpdateDisplayedPlanetNameInEditors()
    {
        foreach (var editor in _editors) editor.EditorBase.UpdateDisplayedPlanetName();
    }
}
