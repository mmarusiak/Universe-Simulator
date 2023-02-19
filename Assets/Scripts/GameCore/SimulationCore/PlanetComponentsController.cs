using System.Collections.Generic;
using UnityEngine;

public class PlanetComponentsController : MonoBehaviour
{
    public static PlanetComponentsController Instance;
    private void Awake() => Instance = this;
    
    [SerializeField]
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

    public void RemovePlanet(PlanetComponent planetComponent) => _allGravityComponents.Remove(planetComponent);

    public void ResetLevel()
    {
        foreach (var component in _allGravityComponents)
        {
            component.Reset();
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
        RemovePlanet(handler.MyComponent);
        Destroy(handler.gameObject);
    }
}
