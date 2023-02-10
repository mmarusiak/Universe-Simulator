using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GravityComponent
{
    // visuals
    private Image _planetImage;
    private Color _planetColor;
    // basic info
    private float _radius;
    private float _mass;
    private Vector2 _initialPosition;
    private Vector2 _initialVelocity;
    // last saved components
    private Vector2 _currentVelocity;
    private Vector2 _currentPosition;
    // static fields
    private static SpriteRenderer _renderer;
    private static Rigidbody2D _rigidbody;

    private List<GravityComponent> _otherComponents;
    
    // properties
    public Image PlanetImage
    {
        get => _planetImage;
        set => SetPlanetImage(value);
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

    public Vector2 InitialPosition
    {
        get => _initialPosition;
        set => SetPlanetInitialPosition(value);
    }

    public Vector2 InitialVelocity
    {
        get => _initialVelocity;
        set => SetPlanetInitialVelocity(value);
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

    public GravityComponent()
    {
        
    }

    public void AddComponents(GravityComponent targetComponents)
    {
        if (targetComponents == this) return;
        _otherComponents.Add(targetComponents);
    }

    // setters
    void SetPlanetImage(Image newImage)
    {
        
    }

    void SetPlanetColor(Color newColor)
    {
        
    }

    void SetPlanetRadius(float newRadius)
    {
        
    }

    void SetPlanetMass(float newMass)
    {
        
    }

    void SetPlanetInitialVelocity(Vector2 newVel)
    {
        
    }

    void SetPlanetInitialPosition(Vector2 newPos)
    {
        
    }

    void SetPlanetCurrentVelocity(Vector2 newVel)
    {
        
    }

    void SetPlanetCurrentPosition(Vector2 newPos)
    {
        
    }
}
