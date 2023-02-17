using System.Collections.Generic;
using UnityEngine;

public class PlanetComponentsController : MonoBehaviour
{
    public static PlanetComponentsController Instance;
    private void Awake() => Instance = this;

    private List<PlanetComponent> _allGravityComponents = new ();

    public void AddNewGravityComponent(PlanetComponent gravityComponent)
    {
        if (_allGravityComponents.Contains(gravityComponent)) return;
        gravityComponent.OtherComponents = new List<PlanetComponent>(_allGravityComponents);
        foreach (var createdComponent in _allGravityComponents)
            createdComponent.AddGravityComponent(gravityComponent);

        _allGravityComponents.Add(gravityComponent);
    }

    public void RemovePlanet(PlanetComponent planetComponent) => _allGravityComponents.Remove(planetComponent);

    public void ResetLevel()
    {
        foreach (var component in _allGravityComponents)
        {
            component.Reset();
        }
    }
}
