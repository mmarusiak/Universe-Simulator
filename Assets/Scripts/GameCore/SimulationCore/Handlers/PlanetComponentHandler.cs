using System.Threading.Tasks;
using UnityEngine;

public class PlanetComponentHandler : MonoBehaviour
{
    [SerializeField] private float mass, radius;
    [SerializeField] private string name;
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
        else if (isCloned) return;
        else if (loadedFromSave) BeginLoad();
        if(!loadedFromSave) Initialize();

        await AddToController();
    }

    async Task AddToController()
    {
        while (PlanetComponentsController.Instance == null) await Task.Yield();
        // whole component loads from saving handler script
        PlanetComponentsController.Instance.AddNewGravityComponent(MyComponent);
    }

    public void LoadAsSlice(PlanetComponent src)
    {
        _myComponent = new PlanetComponent(this, transform.parent, transform.GetChild(0).GetComponent<SpriteRenderer>(), src.Radius, src.Mass, src.CurrentPosition, 
            "Slice of " + src.Name, src.PlanetColor, src.CurrentVelocity);
        _myComponent.IsOriginalPlanet = false;

        AddToController();
    }

    void BeginLoad()
    {
        _myComponent.Handler = this;
        _myComponent.PlanetTransform = transform.parent;
        _myComponent.Renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        Debug.Log(_myComponent.Radius + " - r");
        Debug.Log(_myComponent.Handler.transform.GetChild(0).lossyScale.x);
        PlanetSlice.Instance.LoadSlices(this);
    }
    
    public void Initialize() => _myComponent = new PlanetComponent(this, transform.parent, transform.GetChild(0).GetComponent<SpriteRenderer>(), radius, mass, spawnPos, name);

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
