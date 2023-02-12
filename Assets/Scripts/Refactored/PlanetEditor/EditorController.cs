using UnityEngine;

public class EditorController : MonoBehaviour
{
    public EditorController Instance;
    private void Awake() => Instance = this;

    private PlanetComponent _lastEditedComponent;
    
    public PlanetComponent LastEditedComponent
    {
        get => _lastEditedComponent;
        set
        {
            _lastEditedComponent = LastEditedComponent;
            ChangeComponentInWindows();
        }
    }

    // When last edited component is changed make sure that the new one is being edited, not the old one
    void ChangeComponentInWindows()
    {
        
    }
}
