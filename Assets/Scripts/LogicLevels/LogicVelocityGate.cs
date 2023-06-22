using System;
using GameCore.SimulationCore;
using LogicLevels;
using UnityEngine;

public class LogicVelocityGate : MonoBehaviour
{
    [SerializeField] private float velocity;
    private LogicGate _myGate;
    
    void Start()
    {
        _myGate = LogicLevelController.Instance.AddNewGate(this);
    }

    void Update()
    {
        if (PlaybackController.Instance.Playback.IsPaused) return;
        
        foreach (var planet in PlanetComponentsController.Instance.AllGravityComponents)
        {
            if (planet.CurrentVelocity.magnitude >= velocity)
            {
                _myGate.Trigger();
                break;
            }
        }
    }
}
