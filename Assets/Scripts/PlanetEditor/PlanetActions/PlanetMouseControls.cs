using UnityEngine;

public class PlanetMouseControls : MonoBehaviour
{
    // if mouse is down on a planet for less than > 0.3s then show editor
    private static float _timeToShowEditor = 0.09f;
    private UniverseTimer _timer = new ();
    private PlanetComponentHandler _myHandler;
    private Vector2 _planetOffset;

    private static bool _isOnTempPause;

    void Awake() => _myHandler = transform.GetChild(0).GetComponent<PlanetComponentHandler>();
    
    private void OnMouseDown()
    {
        Debug.Log("Aaaa");
        _planetOffset = OffsetPlanetDrag();
        EditorsController.Instance.LastEditedComponent = _myHandler.MyComponent;
        TimersController.Instance.StartTimer(_timer);
        
        // temp pause if needed
        if (!PlaybackController.Instance.Playback.IsPaused)
        {
            _isOnTempPause = true;
            PlaybackController.Instance.Playback.IsPaused = true;
        }
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
        return  Camera.main.ScreenToWorldPoint(Input.mousePosition) - _myHandler.MyComponent.PlanetTransform.position;
    }

    private void OnMouseUp()
    {
        TimersController.Instance.StopTimer(_timer);
        
        // unpause if was on temp pause
        if (_isOnTempPause)
        {
            _isOnTempPause = false;
            PlaybackController.Instance.Playback.IsPaused = false;
        }

        if (_timer.Time > _timeToShowEditor) return;
        
        BasicPlanetEditor.Instance.EditorBase.Shown = true;
    }
}
