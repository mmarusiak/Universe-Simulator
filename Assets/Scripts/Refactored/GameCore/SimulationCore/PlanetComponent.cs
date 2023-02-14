using System.Collections.Generic;
using UnityEngine;

public class PlanetComponent
{
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
    // last saved components
    private Vector2 _currentVelocity;
    private Vector2 _currentPosition;
    // readonly fields
    private readonly SpriteRenderer _renderer;
    private readonly Rigidbody2D _rigidbody;
    private readonly Transform _planetTransform;
    // boolean that indicate if this planet is new created planet - only position is assigned in constructor
    // basically with this bool we assign not only current pos, but also initial pos if game is not reseted
    private bool _firstTouch = true;
    
    private List<PlanetComponent> _otherComponents;
    private static int _planetCount;
    private int _planetNum;

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
        get => _currentVelocity;
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
        set => _initialVelocity = value;
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

    public Transform PlanetTransform => _planetTransform;
    public Rigidbody2D PlanetRigidbody => _rigidbody;
    
    private readonly Color32[] _defaultColorPalette =
    {
        new Color32(107, 129, 140, 255), // #6B818C - slate gray
        new Color32(216, 228, 255, 255), // #D8E4FF - lavender (web)
        new Color32(49, 233, 129, 225), // #31E981 - spring green
        new Color32(193, 73, 83, 255), // #C14953 - bittersweet shimmer
    };
    public PlanetComponent(PlanetComponentHandler handler, Transform planetTransform, SpriteRenderer spriteRenderer, float radius, float mass,
        Vector2 spawnPos, string name, Color color = default, Vector2 currentVelocity = default)
    {
        Handler = handler;
        _planetTransform = planetTransform;
        _rigidbody = _planetTransform.GetComponent<Rigidbody2D>();
        _renderer = spriteRenderer;
        Radius = radius;
        Mass = mass;
        CurrentPosition = spawnPos;
        Name = name;
        PlanetColor = color;

        _planetNum = _planetCount;
        _planetCount++;

        // planets will have next colors: 1st planet - 1st color, 2nd planet - 2nd color, 5th planet - first color (only 4 colors)
        if (color == default) color = _defaultColorPalette[_planetNum % _defaultColorPalette.Length];
        PlanetColor = color;
        
        if(currentVelocity == default) currentVelocity = Vector2.zero;
        CurrentVelocity = currentVelocity;

        PlanetComponentsController.Instance.AddNewGravityComponent(this);
    }

    public void AddGravityComponent(PlanetComponent targetComponents)
    {
        if (targetComponents == this) return;
        _otherComponents.Add(targetComponents);
    }

    private float _temporaryMultiplier = 5;
    public void AddForce()
    {
        float gConstant = GlobalVariables.GravitationalConstant;
        Vector2 currentGravityForce = Vector2.zero;
        foreach (var otherComponent in _otherComponents)
        {
            float distance = Vector2.Distance(otherComponent.CurrentPosition, CurrentPosition);
            Vector2 distanceVector = otherComponent.CurrentPosition - CurrentPosition;
            
            float force = gConstant * otherComponent.Mass * Mass/ Mathf.Pow(distance, 2);
            float proportionScale = force / distance;
            Vector2 forceVector = new (proportionScale * distanceVector.x, proportionScale * distanceVector.y);
            
            currentGravityForce += forceVector * _temporaryMultiplier;
        }

        CurrentVelocity = _rigidbody.velocity;
        CurrentPosition = _planetTransform.position;
        _rigidbody.AddForce(currentGravityForce, ForceMode2D.Impulse);
    }

    // setters
    void SetPlanetSprite(Sprite newSprite)
    {
        _renderer.sprite = newSprite;
        _planetSprite = newSprite;
    }

    void SetPlanetColor(Color newColor)
    {
        _planetColor = newColor;
        _renderer.color = _planetColor;
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
        if (PlaybackController.Instance.Playback.IsReset) InitialVelocity = newVel;
        _currentVelocity = newVel;
        _rigidbody.velocity = _currentVelocity;
    }

    void SetPlanetCurrentPosition(Vector2 newPos)
    {
        if (PlaybackController.Instance.Playback.IsReset || _firstTouch) InitialPosition = newPos;
        _currentPosition = newPos;
        _planetTransform.position = _currentPosition;
        _firstTouch = false;
    }

    void SetPlanetName(string newName)
    {
        _name = newName;
        _planetTransform.gameObject.name = _name;
    }

    public void Reset()
    {
        CurrentPosition = InitialPosition;
        CurrentVelocity = InitialVelocity;
        _rigidbody.angularVelocity = 0;
    }
}
