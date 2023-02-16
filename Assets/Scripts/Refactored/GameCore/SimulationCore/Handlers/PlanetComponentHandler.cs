using UnityEngine;

public class PlanetComponentHandler : MonoBehaviour
{
    [SerializeField] private float mass, radius;
    [SerializeField] private string name;
    [SerializeField] private Vector2 spawnPos;
    [SerializeField] private bool isDemoPlanet, loadedFromSave;
    
    private PlanetComponent _myComponent = null;
    
    public PlanetComponent MyComponent => _myComponent;

    private void Start()
    {
        if (!isDemoPlanet && !loadedFromSave) spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        else if (loadedFromSave) return;
        _myComponent = new PlanetComponent(this, transform, transform.GetChild(0).GetComponent<SpriteRenderer>(), radius, mass, spawnPos, name);
    }

    public void BeginDrag(Vector2 offset)
    {
        MyComponent.CurrentPosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset;
    }
    
    void Update()
    { 
        if(!PlaybackController.Instance.Playback.IsPaused && _myComponent != null) _myComponent.AddForce();
    }
}
