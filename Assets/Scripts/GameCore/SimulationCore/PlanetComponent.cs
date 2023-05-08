using System.Collections.Generic;
using UnityEngine;

public class PlanetComponent
{
    public const float OriginalInertia = 125;
    private PlanetComponentHandler _handler;
    // visuals
    private Sprite _planetSprite;
    private Color _planetColor;
    // basic info
    private float _radius;
    private float _mass;
    private Vector2 _initialPosition;
    private Vector2 _initialVelocity;
    private string _name;
    private Vector2 _currentPosition;
    // readonly fields
    private SpriteRenderer _renderer;
    private SpriteMask _mask;
    private readonly Rigidbody2D _rigidbody;
    private UniverseTrail _universeTrail;
    private Transform _planetTransform;
    // boolean that indicate if this planet is new created planet - only position is assigned in constructor
    // basically with this bool we assign not only current pos, but also initial pos if game is not reseted
    private bool _firstTouch = true;

    private List<PlanetComponent> _otherComponents;
    private static int _planetCount;
    private int _planetNum;

    private bool _isOriginalPlanet = true;
    private List<SliceData> _slices = new ();

    public bool IsOriginalPlanet
    {
        get => _isOriginalPlanet;
        set => _isOriginalPlanet = value;
    }

    // properties
    public List<PlanetComponent> OtherComponents
    {
        get => _otherComponents;
        set => _otherComponents = value;
    }
    
    public Sprite PlanetSprite
    {
        get => _planetSprite;
        set => SetPlanetSprite(value);
    }

    public Color PlanetColor
    {
        get => _planetColor;
        set => SetPlanetColor(value);
    }

    public float Radius
    {
        get => _radius;
        set => SetPlanetRadius(value);
    }

    public float Mass
    {
        get => _mass;
        set => SetPlanetMass(value);
    }

    public Vector2 CurrentPosition
    {
        get => _currentPosition;
        set => SetPlanetCurrentPosition(value);
    }
    
    public Vector2 CurrentVelocity
    {
        get => _rigidbody.velocity;
        set => SetPlanetCurrentVelocity(value);
    }

    public Vector2 InitialPosition
    {
        get => _initialPosition;
        set
        {
            _initialPosition = value; 
            _planetTransform.position = _initialPosition; 
        }
    }

    public Vector2 InitialVelocity
    {
        get => _initialVelocity;
        set => SetInitialVelocity(value);
    }

    public string Name
    {
        get => _name;
        set => SetPlanetName(value);
    }

    public PlanetComponentHandler Handler
    {
        get => _handler;
        set => _handler = value;
    }

    public Transform PlanetTransform
    {
        get => _planetTransform;
        set => _planetTransform = value;
    }

    public SpriteRenderer Renderer
    {
        get => _renderer;
        set => _renderer = value;
    }

    public SpriteMask Mask
    {
        get => _mask;
        set => _mask = value;
    }

    public List<SliceData> Slices
    {
        get => _slices;
        set => _slices = value;
    }

    public float Inertia
    {
        get => _rigidbody.inertia;
        set => _rigidbody.inertia = value;
    }
    
    public UniverseTrail UniverseTrail => _universeTrail;

    public Rigidbody2D PlanetRigidbody => _rigidbody;
    
    private readonly Color32[] _defaultColorPalette =
    {
        new (107, 129, 140, 255), // #6B818C - slate gray
        new (216, 228, 255, 255), // #D8E4FF - lavender (web)
        new (49, 233, 129, 225), // #31E981 - spring green
        new (193, 73, 83, 255), // #C14953 - bittersweet shimmer
    };
    public PlanetComponent(PlanetComponentHandler handler, Transform planetTransform, SpriteRenderer spriteRenderer, float radius, float mass,
        Vector2 spawnPos, string name, Color color = default, Vector2 currentVelocity = default)
    {
        Handler = handler;
        _planetTransform = planetTransform;
        _universeTrail = _planetTransform.GetComponent<UniverseTrail>();
        _rigidbody = _planetTransform.GetComponent<Rigidbody2D>();
        _renderer = spriteRenderer;
        Mask = planetTransform.GetChild(0).GetComponent<SpriteMask>();
        Radius = radius;
        Mass = mass;
        CurrentPosition = spawnPos;
        Name = name;
        PlanetColor = color;
        Inertia = OriginalInertia;

        _planetNum = _planetCount;
        _planetCount++;

        // planets will have next colors: 1st planet - 1st color, 2nd planet - 2nd color, 5th planet - first color (only 4 colors)
        if (color == default) color = _defaultColorPalette[_planetNum % _defaultColorPalette.Length];
        PlanetColor = color;
        
        if(currentVelocity == default) currentVelocity = Vector2.zero;
        CurrentVelocity = currentVelocity;
    }
    
    public void AddGravityComponent(PlanetComponent targetComponents)
    {
        if (targetComponents == this) return;
        _otherComponents.Add(targetComponents);
    }

    private static float _temporaryMultiplier = 5;
    public void AddForce()
    {
        float gConstant = GlobalVariables.GravitationalConstant;
        SetPlanetCurrentVelocity(_rigidbody.velocity);
        Vector2 currentGravityForce = Vector2.zero;
        foreach (var otherComponent in _otherComponents)
        {
            float distance = Vector2.Distance(otherComponent.CurrentPosition, CurrentPosition);
            Vector2 distanceVector = otherComponent.CurrentPosition - CurrentPosition;
            
            float force = gConstant * otherComponent.Mass * Mass/ Mathf.Pow(distance, 2);
            float proportionScale = force / distance;
            Vector2 forceVector = new (proportionScale * distanceVector.x, proportionScale * distanceVector.y);
            
            if (float.IsNaN(forceVector.x) || float.IsNaN(forceVector.y))
            {
                // error message contains all infos about 2 planets
                Debug.LogWarning($"One of currentGravityForce values is NaN. " +
                                 $"\n\nvector values: {forceVector}" +
                                 $"\n distance: {distanceVector}" +
                                 $"\nplanet (receiver) info: " +
                                 $"\n- name = {_name}" +
                                 $"\n- mass = {_mass}" +
                                 $"\n- radius = {_radius}" +
                                 $"\nplanet (sender) info:" +
                                 $"\n- name = {otherComponent._name}" +
                                 $"\n- mass = {otherComponent._mass}" +
                                 $"\n- radius = {otherComponent._radius}");
                continue;
            }
            
            currentGravityForce += forceVector * _temporaryMultiplier;
        }
        
        GetPosFromTransform();
        _rigidbody.AddForce(currentGravityForce, ForceMode2D.Impulse);
    }

    public void GetPosFromTransform() => CurrentPosition = PlanetTransform.position;
    
    // setters
    void SetPlanetSprite(Sprite newSprite)
    {
        _planetSprite = newSprite;
        _renderer.sprite = _planetSprite;
        PlanetLookEditor.Instance.PlanetPreviewImage.sprite = _planetSprite;
    }

    void SetPlanetColor(Color newColor)
    {
        _planetColor = newColor;
        _renderer.color = _planetColor;
        _universeTrail.SetColor(_planetColor);
    }

    void SetPlanetRadius(float newRadius)
    {
        if (newRadius <= 0) return;
        _radius = newRadius;
        _planetTransform.localScale = new(_radius * 2, _radius * 2, _radius * 2);
    }

    void SetPlanetMass(float newMass)
    {
        if (newMass <= 0) return;
        _mass = newMass;
        _rigidbody.mass = _mass;
    }
    
    void SetPlanetCurrentVelocity(Vector2 newVel)
    {
        _rigidbody.velocity = newVel;

        if (PlaybackController.Instance.Playback.IsReset) InitialVelocity = CurrentVelocity;
        if(!PlaybackController.Instance.Playback.IsPaused && VelocityEditor.Instance.EditorBase.CurrentPlanet == this)  VelocityEditor.Instance.ChangeVelocity(CurrentVelocity);
        
        _handler.OnVelocityChanged.ChangeValue(UniverseTools.RoundOutput(CurrentVelocity.magnitude));
    }

    void SetPlanetCurrentPosition(Vector2 newPos)
    {
        if (PlaybackController.Instance.Playback.IsReset || _firstTouch) InitialPosition = newPos;
        _currentPosition = newPos;
        _planetTransform.position = _currentPosition;
        _firstTouch = false;
    }

    void SetInitialVelocity(Vector2 newVel) => _initialVelocity = newVel;
    
    void SetPlanetName(string newName)
    {
        _name = newName;
        _planetTransform.gameObject.name = _name;
        EditorsController.Instance.UpdateDisplayedPlanetNameInEditors(); 
        
        _handler.OnNameChanged.ChangeValue(_name);
    }

    public void Reset()
    {
        _universeTrail.Clear();
        CurrentPosition = InitialPosition;
        CurrentVelocity = InitialVelocity;
        Mask.sprite = BasicPlanetEditor.Instance.DefaultPlanetSprite;
        
        ClearPivot();
        _planetTransform.rotation = Quaternion.Euler(0,0,0);
        _rigidbody.angularVelocity = 0;
        
        int slicesCount = _slices.Count;
        for (int i = 0; i < slicesCount; i++)
        {
            if (!_slices[i].CreatedOnReset)
            {
                _slices.RemoveRange(i, slicesCount - i);
                break;
            }
        }
        if (_slices.Count > 0)
        {
            PlanetSlice.Instance.LoadSlices(_handler);
            return;
        }

        PlanetSlice.Instance.SliceCollider(Mask, _planetTransform.GetChild(0).GetComponent<PolygonCollider2D>());
    }
    

    void ClearPivot()
    {
        foreach (Transform child in _planetTransform)
            child.localPosition = Vector3.zero;
    }

    public void DestroySelf() => PlanetComponentsController.Instance.DestroyPlanet(_handler);
}
