using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class PlanetComponentHandler : MonoBehaviour
{
    [SerializeField] private float mass, radius;
    [SerializeField] private string planetName;
    [SerializeField] private Vector2 spawnPos;
    [SerializeField] private bool isDemoPlanet, loadedFromSave, isCloned;
    
    private PlanetComponent _myComponent = null;

    [SerializeField] private PlanetTextInfo _onNameChanged, _onVelocityChanged;
    public PlanetTextInfo OnNameChanged => _onNameChanged;
    public PlanetTextInfo OnVelocityChanged => _onVelocityChanged;
    public PlanetComponent MyComponent => _myComponent;
    public bool IsCloned
    {
        get => isCloned;
        set => isCloned = value;
    }
    
    private async void Start()
    {
        if (!isDemoPlanet && !loadedFromSave && !isCloned) spawnPos =  UniverseCamera.Instance.ScreenToWorld(Input.mousePosition);
        // hide it when slicing
        else if (isCloned)
        {
            // we want to actually set new slice as new planet if it is sliced on reset
            isCloned = !PlaybackController.Instance.Playback.IsReset;
            _myComponent.IsOriginalPlanet = !isCloned;
            Debug.Log(isCloned);
            return;
        }
        else if (loadedFromSave) BeginLoad();
        if(!loadedFromSave) Initialize();

        await AddToController();
    }

    async Task AddToController()
    {
        while (PlanetComponentsController.Instance == null) await Task.Yield();
        // whole component loads from saving handler script
        PlanetComponentsController.Instance.AddNewGravityComponent(MyComponent);
        Debug.Log(_myComponent.CurrentVelocity);
    }

    public void LoadAsSlice(PlanetComponent src)
    {
        _myComponent = new PlanetComponent(this, transform.parent, transform.GetChild(0).GetComponent<SpriteRenderer>(), src.Radius, src.Mass, src.CurrentPosition, 
            "Slice of " + src.Name, src.PlanetColor, src.CurrentVelocity);

        AddToController();
    }

    async void BeginLoad()
    {
        _myComponent.Handler = this;
        _myComponent.PlanetTransform = transform.parent;
        _myComponent.Renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        var rot = _myComponent.PlanetTransform.rotation;
        _myComponent.PlanetTransform.rotation = quaternion.Euler(0, 0, 0);
        await PlanetSlice.Instance.LoadSlices(this);
        _myComponent.PlanetTransform.rotation = rot;
    }
    
    public void Initialize() => _myComponent = new PlanetComponent(this, transform.parent, transform.GetChild(0).GetComponent<SpriteRenderer>(), radius, mass, spawnPos, planetName);

    public void BeginDrag(Vector2 offset) => MyComponent.CurrentPosition = (Vector2)UniverseCamera.Instance.ScreenToWorld(Input.mousePosition) - offset;
    
    void Update()
    {
        if(!PlaybackController.Instance.Playback.IsPaused && _myComponent != null) _myComponent.AddForce();
    }

    public void NullTexts()
    {
        _onNameChanged.MakeNull();
        _onVelocityChanged.MakeNull();
    }
}
