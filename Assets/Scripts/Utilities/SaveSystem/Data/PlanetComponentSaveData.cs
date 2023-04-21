using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class PlanetComponentSaveData
{
    // vectors
    [JsonProperty] private VectorSaveData _initialPosition, _initialVelocity, _currentPosition, _currentVelocity;
    // planet basic infos
    [JsonProperty] private float _radius, _mass;
    [JsonProperty] private ColorSaveData _planetColor;
    [JsonProperty] private string _planetName;
    [JsonProperty] private List<SliceData> _slices;
    [JsonProperty] private bool _isOriginal;
    [JsonProperty] private float _angularVelocity;

    [JsonConstructor]
    public PlanetComponentSaveData(VectorSaveData inPos, VectorSaveData inVel, VectorSaveData cPos, VectorSaveData cVel,
        float radius, float mass, ColorSaveData color, string name, List<SliceData> slices, bool isOriginal, float angularVelocity)
    {
        _initialPosition = inPos;
        _initialVelocity = inVel;
        _currentPosition = cPos;
        _currentVelocity = cVel;
        _radius = radius;
        _mass = mass;
        _planetColor = color;
        _planetName = name;
        _slices = slices;
        _isOriginal = isOriginal;
        _angularVelocity = angularVelocity;
    }
    
    public PlanetComponentSaveData(PlanetComponent component)
    {
        _initialPosition = component.InitialPosition;
        _initialVelocity = component.InitialVelocity;
        _currentPosition = component.CurrentPosition;
        _currentVelocity = component.CurrentVelocity;
        _radius = component.Radius;
        _mass = component.Mass;
        _planetName = component.Name;
        _planetColor = component.PlanetColor;
        _slices = component.Slices;
        _isOriginal = component.IsOriginalPlanet;
        _angularVelocity = component.PlanetRigidbody.angularVelocity;
    }

    public static implicit operator PlanetComponent(PlanetComponentSaveData data)
    {
        GameObject newPlanetGO = PlanetComponentsController.Instance.LoadPlanet();
        PlanetComponentHandler planetHandler = newPlanetGO.transform.GetChild(0).GetComponent<PlanetComponentHandler>();
        planetHandler.Initialize();

        SetValuesToComponent(planetHandler.MyComponent, data);
        return planetHandler.MyComponent;
    }
    
    public static implicit operator PlanetComponentSaveData(PlanetComponent component)
    {
        return new PlanetComponentSaveData(component);
    }

    public static void SetValuesToComponent(PlanetComponent receiver, PlanetComponentSaveData sender)
    {
        // vectors
        receiver.InitialPosition = sender._initialPosition;
        receiver.InitialVelocity = sender._initialVelocity;
        receiver.CurrentPosition = sender._currentPosition;
        receiver.CurrentVelocity = sender._currentVelocity;
        // basic info
        receiver.Mass = sender._mass;
        receiver.Radius = sender._radius;
        receiver.PlanetColor = sender._planetColor;
        receiver.Name = sender._planetName;
        receiver.Slices = sender._slices;
        receiver.IsOriginalPlanet = sender._isOriginal;
        receiver.PlanetRigidbody.angularVelocity = sender._angularVelocity;
    }
}
