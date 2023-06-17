using System;
using System.Collections.Generic;
using GameCore.SimulationCore;
using UnityEngine;

public class LogicAreaGate : MonoBehaviour
{
    [SerializeField] private Vector2 position;
    [SerializeField] private Vector2 size;
    [SerializeField] private float timeInZone;
    private readonly List<LogicAreaComponent> _planetsInZone = new();

    void Start()
    {
        LineRenderer renderer = GetComponent<LineRenderer>();
        renderer.positionCount = 5;
        Vector3[] pos = 
        {
            position,
            (position + new Vector2(size.x, 0)),
            (position + size),
            (position + new Vector2(0, size.y)),
            position
        };
        renderer.SetPositions(pos);
    }

    // helper void for logic area component list
    bool IsPlanetAlreadyInZone(PlanetComponent planet)
    {
        foreach (var p in _planetsInZone)
        {
            if (p.PlanetComponent == planet) return true;
        }
        return false;
    }

    void Update()
    {
        if (PlaybackController.Instance.Playback.IsPaused) return;
        
        List<LogicAreaComponent> copyOf_planetsInZone = new List<LogicAreaComponent>(_planetsInZone);
        foreach (var planet in copyOf_planetsInZone)
        {
            var lastPlanetInZone = planet.PlanetComponent;
            
            // check if last planet is still in zone
            if (lastPlanetInZone.CurrentPosition.x >= position.x &&
                lastPlanetInZone.CurrentPosition.x <= position.x + size.x &&
                lastPlanetInZone.CurrentPosition.y >= position.y &&
                lastPlanetInZone.CurrentPosition.y <= position.y + size.y)
            {
                planet.time += Time.deltaTime;
                CheckGate(planet);
            } 
            else _planetsInZone.Remove(planet);
        }
        
        // find if there are new planets in zone
        foreach (var planet in PlanetComponentsController.Instance.AllGravityComponents)
        {
            if (planet.CurrentPosition.x >= position.x && planet.CurrentPosition.x < position.x + size.x &&
                planet.CurrentPosition.y >= position.y && planet.CurrentPosition.y < position.y + size.y)
            {
                if (!IsPlanetAlreadyInZone(planet)) _planetsInZone.Add(planet);
                break;
            }
        }
    }

    void CheckGate(LogicAreaComponent detector)
    {
        if (detector.time >= timeInZone)
        {
            // gate achieved!
            Debug.Log("Gate achieved!");
        }
    }
}
