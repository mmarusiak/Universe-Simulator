using System.Collections.Generic;
using UnityEngine;

public class GravityComponentsController : MonoBehaviour
{
    public static GravityComponentsController Instance;
    private void Awake() => Instance = this;

    private List<GravityComponent> _allGravityComponents = new List<GravityComponent>();

    public void AddNewGravityComponent(GravityComponent gravityComponent)
    {
        if (_allGravityComponents.Contains(gravityComponent)) return;
        gravityComponent.OtherComponents = new List<GravityComponent>(_allGravityComponents);
        foreach (var createdComponent in _allGravityComponents)
            createdComponent.AddGravityComponent(gravityComponent);

        _allGravityComponents.Add(gravityComponent);
        Debug.Log(_allGravityComponents.Count);
    }
}
