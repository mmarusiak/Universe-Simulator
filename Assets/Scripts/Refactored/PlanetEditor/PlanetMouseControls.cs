using UnityEngine;

[RequireComponent(typeof(PlanetComponentHandler))]
public class PlanetMouseControls : MonoBehaviour
{
    // if mouse is down on a planet for less than > 0.3s then show editor
    private static float _timeToShowEditor = 0.3f;
    private UniverseTimer _timer = new ();
    private PlanetComponentHandler _myHandler;
    private Vector2 _planetOffset;

    void Awake() => _myHandler = GetComponent<PlanetComponentHandler>();
    
    private void OnMouseDown()
    {
        _planetOffset = OffsetPlanetDrag();
        EditorsController.Instance.LastEditedComponent = _myHandler.MyComponent;
        TimersController.Instance.StartTimer(_timer); 
       // pause
    }

    private void OnMouseDrag()
    {
        if (_timer.Time > _timeToShowEditor)
        {
            _myHandler.BeginDrag(_planetOffset);
        }
    }

    Vector2 OffsetPlanetDrag()
    {
        return _myHandler.MyComponent.PlanetTransform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        TimersController.Instance.StopTimer(_timer);

        if (_timer.Time > _timeToShowEditor) return;
        
        BasicPlanetEditor.Instance.EditorBase.Shown = true;
    }
}
