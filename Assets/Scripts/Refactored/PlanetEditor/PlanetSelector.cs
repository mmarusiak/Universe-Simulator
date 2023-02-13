using UnityEngine;

[RequireComponent(typeof(PlanetComponentHandler))]
public class PlanetSelector : MonoBehaviour
{
    // if mouse is down on a planet for less than > 0.3s then show editor
    private static float _timeToShowEditor = 0.3f;
    private UniverseTimer _timer;
    private void OnMouseDown()
   {
       TimersController.Instance.BeginTimer(_timer);
   }

    private void OnMouseUp()
    {
        TimersController.Instance.EndTimer(_timer);
        Debug.Log(_timer.Time);
    }
}
