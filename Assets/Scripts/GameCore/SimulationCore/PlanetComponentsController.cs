using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlanetComponentsController : MonoBehaviour
{
    public static PlanetComponentsController Instance;
    private void Awake() => Instance = this;
    
    private List<PlanetComponent> _allGravityComponents = new ();

    public List<PlanetComponent> AllGravityComponents
    {
        get => _allGravityComponents;
        set => _allGravityComponents = value;
    }

    [SerializeField] private Transform _planetsHolder;
    [SerializeField] private GameObject _planetPrefab, _loadPrefab;

    public void AddNewGravityComponent(PlanetComponent gravityComponent)
    {
        if (_allGravityComponents.Contains(gravityComponent)) return;
        gravityComponent.OtherComponents = new List<PlanetComponent>(_allGravityComponents);
        foreach (var createdComponent in _allGravityComponents)
            createdComponent.AddGravityComponent(gravityComponent);

        _allGravityComponents.Add(gravityComponent);
    }

    void RemovePlanet(PlanetComponent planetComponent) => _allGravityComponents.Remove(planetComponent);

    void RemovePlanetOnPlanets(PlanetComponent planetComponent)
    {
        foreach (var createdComponent in _allGravityComponents)
            createdComponent.OtherComponents.Remove(planetComponent);
    }
    public void ResetLevel()
    {
        DestroyClones();
        foreach (var component in _allGravityComponents) component.Reset();
    }

    void DestroyClones()
    {
        var copyList = new List<PlanetComponent>(_allGravityComponents);
        foreach (var component in copyList)
        {
            if (!component.IsOriginalPlanet)
            {
                RemovePlanetOnPlanets(component);
                RemovePlanet(component);
                component.DestroySelf();
            }
        }
    }

    public void ClearLevel()
    {
        foreach (var component in _allGravityComponents)
        {
            Destroy(component.Handler.gameObject);
        }
        _allGravityComponents = new();
    }

    public GameObject CreatePlanet()
    {
        return Instantiate(_planetPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition),
            Quaternion.Euler(0, 0, 0), _planetsHolder);
    }
    public GameObject LoadPlanet()
    {
        return Instantiate(_loadPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition),
            Quaternion.Euler(0, 0, 0), _planetsHolder);
    }

    public void DestroyPlanet(PlanetComponentHandler handler)
    {
        RemovePlanetOnPlanets(handler.MyComponent);
        RemovePlanet(handler.MyComponent);
        Destroy(handler.transform.parent.gameObject);
    }
}
