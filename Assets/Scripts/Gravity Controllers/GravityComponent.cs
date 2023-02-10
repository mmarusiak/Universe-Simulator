using System.Collections.Generic;
using UnityEngine;

public class GravityComponent
{
    // visuals
    private Sprite _planetSprite;
    private Color _planetColor;
    // basic info
    private float _radius;
    private float _mass;
    private Vector2 _initialPosition;
    private Vector2 _initialVelocity = Vector2.zero;
    // last saved components
    private Vector2 _currentVelocity;
    private Vector2 _currentPosition;
    // static fields
    private static SpriteRenderer _renderer;
    private static Rigidbody2D _rigidbody;
    private static Transform _planetTransform;
    // boolean that indicate if this planet is new created planet - only position is assigned in constructor
    // basically with this bool we assign not only current pos, but also initial pos if game is not reseted
    private bool _firstTouch = true;
    
    private List<GravityComponent> _otherComponents;

    // properties
    public List<GravityComponent> OtherComponents
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
        set => SetPlanetCurrentPosition(value, true);
    }
    
    public Vector2 CurrentVelocity
    {
        get => _currentVelocity;
        set => SetPlanetCurrentVelocity(value, true);
    }

    public Vector2 InitialPosition
    {
        get => _initialPosition;
        set
        {
            _initialPosition = value; 
            _planetTransform.position = _initialPosition; }
    }
    
    public GravityComponent(Transform planetTransform, SpriteRenderer spriteRenderer, float radius, float mass,
        Vector2 spawnPos)
    {
        _planetTransform = planetTransform;
        _rigidbody = _planetTransform.GetComponent<Rigidbody2D>();
        _renderer = spriteRenderer;
        Radius = radius;
        Mass = mass;
        CurrentPosition = spawnPos;
        
        Debug.Log(_planetTransform.name + _rigidbody.name);
        
        GravityComponentsController.Instance.AddNewGravityComponent(this);
    }

    public void AddGravityComponent(GravityComponent targetComponents)
    {
        if (targetComponents == this) return;
        _otherComponents.Add(targetComponents);
    }

    public void AddForce()
    {
        float gConstant = GlobalVariables.GravitationalConstant;
        Vector2 currentGravityForce = Vector2.zero;
        foreach (var otherComponent in _otherComponents)
        {
            float distance = Vector2.Distance(otherComponent.CurrentPosition, CurrentPosition);
            Vector2 distanceVector = otherComponent.CurrentPosition - CurrentPosition;
            
            float force = gConstant * otherComponent.Mass * Mass / Mathf.Pow(distance, 2);
            float proportionScale = force / distance;
            Vector2 forceVector = new (proportionScale * distanceVector.x, proportionScale * distanceVector.y);
            
            currentGravityForce += forceVector;
        }
        //_rigidbody.AddForce(currentGravityForce * 1000000, ForceMode2D.Impulse);
        Debug.Log(Mass);
        var rg = _rigidbody;
        Debug.Log(rg.mass + _planetTransform.gameObject.name);
        _rigidbody.AddForce(new(100,100), ForceMode2D.Force);
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
    
    void SetPlanetCurrentVelocity(Vector2 newVel, bool reseted)
    {
        if (reseted) _initialVelocity = newVel;
        _currentVelocity = newVel;
    }

    void SetPlanetCurrentPosition(Vector2 newPos, bool reseted)
    {
        if (reseted || _firstTouch) InitialPosition = newPos;
        _currentPosition = newPos;
        _firstTouch = false;
    }
}
