using System.Collections.Generic;
using UnityEngine;

public class EditorsController : MonoBehaviour
{
    public static EditorsController Instance;
    private void Awake() => Instance = this;

    private PlanetComponent _lastEditedComponent;
    [SerializeField]
    private List<PlanetEditor> _editors = new ();
    
    public PlanetComponent LastEditedComponent
    {
        get => _lastEditedComponent;
        set
        {
            _lastEditedComponent = value;
            ChangeComponentInWindows();
        }
    }

    // When last edited component is changed make sure that the new one is being edited, not the old one
    void ChangeComponentInWindows()
    {
        foreach (var editor in _editors)
        {
            editor.EditorBase.CurrentPlanet = _lastEditedComponent;
        }
    }
}
