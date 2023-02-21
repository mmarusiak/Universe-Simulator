using UnityEngine;
using UnityEngine.Events;

public class PointerInZoneDetection : MonoBehaviour
{
    [SerializeField] private float zoneWidthPercentage, zoneHeightPercentage, zoneXPercentage, zoneYPercentage;
    [SerializeField] private UnityEvent onEnterZone, onQuitZone;
    private Rect zone;

    private bool _enteredZone = true;

    void OnValidate()
    {
        // Calculate the zone dimensions based on screen size and percentage values
        float zoneWidth = Screen.width * zoneWidthPercentage;
        float zoneHeight = Screen.height * zoneHeightPercentage;

        // Center the zone on the screen
        float zoneX = Screen.width * zoneXPercentage;
        float zoneY = Screen.height * zoneYPercentage;

        zone = new Rect(zoneX, zoneY, zoneWidth, zoneHeight);
    }
    
    void Update()
    {
        if (_enteredZone != zone.Contains(Input.mousePosition))
        {
            _enteredZone = !_enteredZone;
            if(_enteredZone) onEnterZone.Invoke();
            else onQuitZone.Invoke();
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(zone.x + zone.width / 2, zone.y + zone.height / 2, 0), new Vector3(zone.width, zone.height, 0));
    }
}
