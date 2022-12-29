using System;
using UnityEngine;

public class CreatorHandler : MonoBehaviour
{
    public GameObject PlanetPrefab;
    // planets' creator page args
    public string PlanetName;
    public Vector2 InitialVelocity;
    public float Mass, Radius;
    public Vector2 StartPos;
    private GravityObjectsController _controller;
    public GameObject Panel;

    void Start()
    {
        _controller = GameObject.FindWithTag("GravityController").GetComponent<GravityObjectsController>();
        Panel.SetActive(false);
    }

    public void ShowPanel()
    {
        Panel.SetActive(true);
    }
    
    public void CreatePlanet()
    {
        GameObject newPlanet = Instantiate(PlanetPrefab);
        var newPlanetController = newPlanet.AddComponent<GravityObject>();
        newPlanetController.Mass = Mass;
        newPlanetController.PlanetName = PlanetName;
        newPlanetController.Radius = Radius;
        newPlanetController.StartPos = StartPos;
        newPlanetController.InitialVelocity = InitialVelocity;
        
        newPlanetController.Init();
    }

    public void RemovePlanet() => _controller.RemovingPlanet = !_controller.RemovingPlanet;

    public void Close()
    {
        Panel.SetActive(false);
    }
}
