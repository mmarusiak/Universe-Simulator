using Newtonsoft.Json;
using UnityEngine;

public class PlanetSaveData
{
    [JsonProperty]
    private readonly VectorSaveData _initialPos;
    [JsonProperty]
    private readonly VectorSaveData _initialVel;
    [JsonProperty] 
    private readonly ColorSaveData _colorData;
    [JsonProperty]
    private readonly string _name;
    [JsonProperty]
    private readonly float _radius;
    [JsonProperty]
    private readonly float _mass;

    [JsonConstructor]
    private PlanetSaveData(VectorSaveData initialPos, VectorSaveData initialVel, ColorSaveData colorData , string name, float radius, float mass)
    {
        _initialPos = initialPos;
        _initialVel = initialVel;
        _colorData = colorData;
        _name = name;
        _radius = radius;
        _mass = mass;
    }

    /*public static implicit operator PlanetSaveData(GravityObject grav) => new(grav.InitialPos, grav.InitialVelocity, 
        grav.transform.GetChild(0).GetComponent<SpriteRenderer>().color, grav.PlanetName, grav.Radius, grav.Mass);

        
    public void InitializeData(GravityObject receiver)
    {
        receiver.InitialPos = _initialPos;
        receiver.InitialVelocity = _initialVel;
        receiver.Mass = _mass;
        receiver.Radius = _radius;
        receiver.PlanetName = _name;

        receiver.transform.GetChild(0).GetComponent<SpriteRenderer>().color = _colorData;
    }*/
}